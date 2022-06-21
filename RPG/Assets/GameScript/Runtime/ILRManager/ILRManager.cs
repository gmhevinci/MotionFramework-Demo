using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MotionFramework;
using MotionFramework.Resource;

public class ILRManager : ModuleSingleton<ILRManager>, IModule, IActivatorServices
{
	private bool EnableILRuntime = true;

	private MemoryStream _dllStream;
	private MemoryStream _pdbStream;
	private Assembly _monoAssembly;

	// 热更新层相关函数
	private IStaticMethod _startFun;
	private IStaticMethod _updateFun;
	private IStaticMethod _fixedUpdateFun;

	/// <summary>
	/// 热更新的程序域
	/// </summary>
	public ILRuntime.Runtime.Enviorment.AppDomain ILRDomain { private set; get; }

	/// <summary>
	/// 热更新所有类型集合
	/// </summary>
	public List<Type> HotfixAssemblyTypes { private set; get; }


	void IModule.OnCreate(System.Object param)
	{
	}
	void IModule.OnUpdate()
	{
		_updateFun?.Invoke();
	}
	void IModule.OnDestroy()
	{
		DestroySingleton();
	}
	void IModule.OnGUI()
	{
	}
	public void FixedUpdate()
	{
		_fixedUpdateFun?.Invoke();
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			LoadHotfixAssemblyWithPDB();
		else
			LoadHotfixAssembly();

		InitHotfixProgram();

		if (_startFun != null)
			_startFun.Invoke();
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	public void ReleaseILRuntime()
	{
		if (_dllStream != null)
		{
			_dllStream.Close();
			_dllStream = null;
		}
		if (_pdbStream != null)
		{
			_pdbStream.Close();
			_pdbStream = null;
		}
	}

	// 加载热更的动态库文件
	private void LoadHotfixAssembly()
	{
		TextAsset dllAsset = LoadDLL();

		if (EnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, null, null);
		}
		else
		{
#if ENABLE_IL2CPP
			throw new NotImplementedException("You must enable ILRuntime when with IL2CPP mode.");
#endif
			_monoAssembly = Assembly.Load(dllAsset.bytes, null);
		}
	}
	private void LoadHotfixAssemblyWithPDB()
	{
		TextAsset dllAsset = LoadDLL();
		TextAsset pdbAsset = LoadPDB();

		if (EnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			_pdbStream = new MemoryStream(pdbAsset.bytes);
			var symbolReader = new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider();
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, _pdbStream, symbolReader);
		}
		else
		{
#if ENABLE_IL2CPP
			throw new NotImplementedException("You must enable ILRuntime when with IL2CPP mode.");
#endif
			_monoAssembly = Assembly.Load(dllAsset.bytes, pdbAsset.bytes);
		}
	}
	private TextAsset LoadDLL()
	{
		string location = $"Assembly/{ILRDefine.GameDLLFileName}";
		return LoadAsset(location);
	}
	private TextAsset LoadPDB()
	{
		string location = $"Assembly/{ILRDefine.GamePDBFileName}";
		return LoadAsset(location);
	}

	// 初始化热更程序
	private void InitHotfixProgram()
	{
		string typeName = "Main";
		string startFunName = "Start";
		string updateFunName = "Update";
		string lateUpdateFunName = "FixedUpdate";

		if (EnableILRuntime)
		{
			ILRRegister.Register(ILRDomain);
			_startFun = new ILRStaticMethod(ILRDomain, typeName, startFunName, 0);
			_updateFun = new ILRStaticMethod(ILRDomain, typeName, updateFunName, 0);
			_fixedUpdateFun = new ILRStaticMethod(ILRDomain, typeName, lateUpdateFunName, 0);
			HotfixAssemblyTypes = ILRDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
		}
		else
		{
			Type type = _monoAssembly.GetType(typeName);
			_startFun = new MonoStaticMethod(type, startFunName);
			_updateFun = new MonoStaticMethod(type, updateFunName);
			_fixedUpdateFun = new MonoStaticMethod(type, lateUpdateFunName);
			HotfixAssemblyTypes = _monoAssembly.GetTypes().ToList<Type>();
		}
	}

	/// <summary>
	/// 同步加载程序集文件
	/// </summary>
	private TextAsset LoadAsset(string location)
	{
		var assetOperation = ResourceManager.Instance.LoadAssetSync<TextAsset>(location);
		return assetOperation.AssetObject as TextAsset;
	}

	#region 反射服务接口
	private readonly Dictionary<Type, Attribute> _cacheAttributes = new Dictionary<Type, Attribute>();

	/// <summary>
	/// 缓存热更新特性
	/// </summary>
	public void CacheHotfixAttribute(Type type, Attribute attribute)
	{
		_cacheAttributes.Add(type, attribute);
	}

	object IActivatorServices.CreateInstance(Type type)
	{
		if (EnableILRuntime)
			return ILRDomain.Instantiate(type.FullName).CLRInstance;
		else
			return Activator.CreateInstance(type);
	}
	Attribute IActivatorServices.GetAttribute(Type type)
	{
		_cacheAttributes.TryGetValue(type, out Attribute result);
		return result;
	}
	#endregion
}