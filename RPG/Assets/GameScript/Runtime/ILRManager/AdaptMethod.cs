using System;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class AdaptMethod
{
	private readonly AppDomain _appDomain;
	private readonly ILTypeInstance _instance;
	private readonly string _name;
	private readonly int _paramCount;
	private readonly bool _checkInvokeBase;
	private IMethod _method = null;
	private bool _isFind = false;

	public AdaptMethod(AppDomain appDomain, ILTypeInstance instance, string name, int paramCount, bool checkInvokeBase)
	{
		_appDomain = appDomain;
		_instance = instance;
		_name = name;
		_paramCount = paramCount;
		_checkInvokeBase = checkInvokeBase;
	}

	/// <summary>
	/// 调用热更方法
	/// </summary>
	public object Invoke(params object[] paramList)
	{
		// 获取热更新里的方法
		if(_isFind == false)
		{
			_isFind = true;
			_method = _instance.Type.GetMethod(_name, _paramCount);

			// 注意：热更类里可能没有实现该重载接口
			if (_method == null && _checkInvokeBase == false)
			{
				string baseClass = "";
				if (_instance.Type.FirstCLRBaseType != null)
				{
					baseClass = _instance.Type.FirstCLRBaseType.FullName;
				}
				else if (_instance.Type.FirstCLRInterface != null)
				{
					baseClass = _instance.Type.FirstCLRInterface.FullName;
				}
				throw new Exception($"Can't find the hotfix method: {_instance.Type.FullName}.{_name}:{baseClass}, paramCount={_paramCount}");
			}
		}

		if (_method != null)
			return _appDomain.Invoke(_method, _instance, paramList);
		else
			return null;
	}

	/// <summary>
	/// 在热更方法不存在的清空下，我们直接调用基类方法
	/// </summary>
	public bool ShouldInvokeBase()
	{
		if (_checkInvokeBase && _method == null)
			return true;
		else
			return false;
	}
}