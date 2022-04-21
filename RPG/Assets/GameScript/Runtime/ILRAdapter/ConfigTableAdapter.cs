using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace MotionFramework.Config
{
	public class ConfigTableAdapter : CrossBindingAdaptor
	{
		public override Type BaseCLRType
		{
			get
			{
				return typeof(MotionFramework.Config.ConfigTable);
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

		public class Adapter : MotionFramework.Config.ConfigTable, CrossBindingAdaptorType
		{
			ILTypeInstance instance;
			ILRuntime.Runtime.Enviorment.AppDomain appdomain;

			CrossBindingMethodInfo<MotionFramework.IO.ByteBuffer> mReadByte_0 = new CrossBindingMethodInfo<MotionFramework.IO.ByteBuffer>("ReadByte");

			public Adapter()
			{

			}

			public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
			{
				this.appdomain = appdomain;
				this.instance = instance;
			}

			public ILTypeInstance ILInstance { get { return instance; } }

			public override void ReadByte(MotionFramework.IO.ByteBuffer byteBuf)
			{
				mReadByte_0.Invoke(this.instance, byteBuf);
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
