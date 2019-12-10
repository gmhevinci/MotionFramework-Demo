using System;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using MotionFramework;

public static class ILRHelper
{
	public static void Init(ILRuntime.Runtime.Enviorment.AppDomain appDomain)
	{
		// 注册委托
		appDomain.DelegateManager.RegisterMethodDelegate<List<object>>();
		appDomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
		appDomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Network.INetPackage>();
		appDomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Event.IEventMessage>();
		appDomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Resource.Asset>();
		appDomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapter_IMessage.Adaptor>();
		appDomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapter_IMessage.Adaptor>();

		// 注册委托转换器
		appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
		{
			return new UnityEngine.Events.UnityAction(() =>
			{
				((Action)act)();
			});
		});

		// 注册值类型绑定器
		appDomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector2), new Vector2Binder());
		appDomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector3), new Vector3Binder());
		appDomain.RegisterValueTypeBinder(typeof(UnityEngine.Quaternion), new QuaternionBinder());

		// 执行CLR绑定
		//ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
		Type classCLRBinding = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
		if (classCLRBinding != null)
		{
			var method = classCLRBinding.GetMethod("Initialize");
			method.Invoke(null, new object[] { appDomain });
		}
		else
		{
			LogSystem.Log(ELogType.Warning, "ILRuntime not generated binding scripts.");
		}

		// 注册适配器
		Google.Protobuf.Adapter_IMessage adaptor = new Google.Protobuf.Adapter_IMessage();
		appDomain.RegisterCrossBindingAdaptor(adaptor);

		// 注册LitJson
		LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
	}
}