using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Console;

public class ILRManager : ModuleSingleton<ILRManager>, IMotionModule
{
	/// <summary>
	/// 游戏模块创建参数
	/// </summary>
	public class CreateParameters
	{
		/// <summary>
		/// 是否启用ILRuntime，否则使用mono模式运行
		/// </summary>
		public bool IsEnableILRuntime;
	}

	private MemoryStream _dllStream;
	private MemoryStream _pdbStream;
	private Assembly _monoAssembly;
	private bool _isEnableILRuntime;

	// 热更新层相关函数
	private IStaticMethod _startFun;
	private IStaticMethod _updateFun;
	private IStaticMethod _lateUpdateFun;
	private IStaticMethod _uiLanguageFun;

	/// <summary>
	/// 热更新的程序域
	/// </summary>
	public ILRuntime.Runtime.Enviorment.AppDomain ILRDomain { private set; get; }

	/// <summary>
	/// 热更新所有类型集合
	/// </summary>
	public List<Type> HotfixAssemblyTypes { private set; get; }


	void IMotionModule.OnCreate(System.Object param)
	{
		CreateParameters createParam = param as CreateParameters;
		if (createParam == null)
			throw new Exception($"{nameof(ILRManager)} create param is invalid.");

		_isEnableILRuntime = createParam.IsEnableILRuntime;
	}
	void IMotionModule.OnStart()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			LoadHotfixAssemblyWithPDB();
		else
			LoadHotfixAssembly();

		InitHotfixProgram();

		if (_startFun != null)
			_startFun.Invoke();
	}
	void IMotionModule.OnUpdate()
	{
		if (_updateFun != null)
			_updateFun.Invoke();
	}
	void IMotionModule.OnGUI()
	{
		AppConsole.GUILable($"[{nameof(ILRManager)}] EnableILRuntime : {_isEnableILRuntime}");
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

	/// <summary>
	/// 从热更配备里获取界面多语言
	/// </summary>
	public string UILanguage(string key)
	{
		return (string)_uiLanguageFun.Invoke(key);
	}

	// 加载热更的动态库文件
	private void LoadHotfixAssembly()
	{
		TextAsset dllAsset = LoadDLL();

		if (_isEnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, null, null);
		}
		else
		{
			_monoAssembly = Assembly.Load(dllAsset.bytes, null);
		}
	}
	private void LoadHotfixAssemblyWithPDB()
	{
		TextAsset dllAsset = LoadDLL();
		TextAsset pdbAsset = LoadPDB();

		if (_isEnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			_pdbStream = new MemoryStream(pdbAsset.bytes);
			var symbolReader = new Mono.Cecil.Pdb.PdbReaderProvider();
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, _pdbStream, symbolReader);
		}
		else
		{
			_monoAssembly = Assembly.Load(dllAsset.bytes, pdbAsset.bytes);
		}
	}
	private TextAsset LoadDLL()
	{
		string location = $"Assembly/{ILRDefine.StrMyHotfixDLLFileName}";
		return ResourceManager.Instance.SyncLoad<TextAsset>(location);
	}
	private TextAsset LoadPDB()
	{
		string location = $"Assembly/{ILRDefine.StrMyHotfixPDBFileName}";
		return ResourceManager.Instance.SyncLoad<TextAsset>(location);
	}

	// 初始化热更程序
	private void InitHotfixProgram()
	{
		string typeName = "Hotfix.HotfixMain";
		string startFunName = "Start";
		string updateFunName = "Update";
		string lateUpdateFunName = "LateUpdate";
		string uiLanguageFunName = "UILanguage";

		if (_isEnableILRuntime)
		{
			ILRHelper.Init(ILRDomain);
			_startFun = new ILRStaticMethod(ILRDomain, typeName, startFunName, 0);
			_updateFun = new ILRStaticMethod(ILRDomain, typeName, updateFunName, 0);
			_lateUpdateFun = new ILRStaticMethod(ILRDomain, typeName, lateUpdateFunName, 0);
			_uiLanguageFun = new ILRStaticMethod(ILRDomain, typeName, uiLanguageFunName, 1);
			HotfixAssemblyTypes = ILRDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
		}
		else
		{
			Type type = _monoAssembly.GetType(typeName);
			_startFun = new MonoStaticMethod(type, startFunName);
			_updateFun = new MonoStaticMethod(type, updateFunName);
			_lateUpdateFun = new MonoStaticMethod(type, lateUpdateFunName);
			_uiLanguageFun = new MonoStaticMethod(type, uiLanguageFunName);
			HotfixAssemblyTypes = _monoAssembly.GetTypes().ToList<Type>();
		}
	}
}