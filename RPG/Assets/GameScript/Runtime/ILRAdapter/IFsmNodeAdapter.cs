using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace MotionFramework.AI
{
	public class IFsmNodeAdapter : CrossBindingAdaptor
	{
		public override Type BaseCLRType
		{
			get
			{
				return typeof(MotionFramework.AI.IFsmNode);
			}
		}

		public override Type AdaptorType
		{
			get
			{
				return typeof(Adapter);
			}
		}

		public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
		{
			return new Adapter(appdomain, instance);
		}

		public class Adapter : MotionFramework.AI.IFsmNode, CrossBindingAdaptorType
		{
			ILTypeInstance instance;
			ILRuntime.Runtime.Enviorment.AppDomain appdomain;

			CrossBindingFunctionInfo<System.String> mget_Name_0 = new CrossBindingFunctionInfo<System.String>("get_Name");
			CrossBindingMethodInfo mOnEnter_1 = new CrossBindingMethodInfo("OnEnter");
			CrossBindingMethodInfo mOnUpdate_2 = new CrossBindingMethodInfo("OnUpdate");
			CrossBindingMethodInfo mOnFixedUpdate_3 = new CrossBindingMethodInfo("OnFixedUpdate");
			CrossBindingMethodInfo mOnExit_4 = new CrossBindingMethodInfo("OnExit");
			CrossBindingMethodInfo<System.Object> mOnHandleMessage_5 = new CrossBindingMethodInfo<System.Object>("OnHandleMessage");

			public Adapter()
			{

			}

			public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
			{
				this.appdomain = appdomain;
				this.instance = instance;
			}

			public ILTypeInstance ILInstance { get { return instance; } }

			public void OnEnter()
			{
				mOnEnter_1.Invoke(this.instance);
			}

			public void OnUpdate()
			{
				mOnUpdate_2.Invoke(this.instance);
			}

			public void OnFixedUpdate()
			{
				mOnFixedUpdate_3.Invoke(this.instance);
			}

			public void OnExit()
			{
				mOnExit_4.Invoke(this.instance);
			}

			public void OnHandleMessage(System.Object msg)
			{
				mOnHandleMessage_5.Invoke(this.instance, msg);
			}

			public System.String Name
			{
				get
				{
					return mget_Name_0.Invoke(this.instance);

				}
			}

			public override string ToString()
			{
				IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
				m = instance.Type.GetVirtualMethod(m);
				if (m == null || m is ILMethod)
				{
					return instance.ToString();
				}
				else
					return instance.Type.FullName;
			}
		}
	}
}
