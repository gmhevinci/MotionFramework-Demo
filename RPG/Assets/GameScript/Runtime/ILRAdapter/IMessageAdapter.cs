using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace Google.Protobuf
{
	public class IMessageAdapter : CrossBindingAdaptor
	{
		public override Type BaseCLRType
		{
			get
			{
				return typeof(Google.Protobuf.IMessage);
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

		public class Adapter : Google.Protobuf.IMessage, CrossBindingAdaptorType
		{
			ILTypeInstance instance;
			ILRuntime.Runtime.Enviorment.AppDomain appdomain;

			CrossBindingMethodInfo<Google.Protobuf.CodedInputStream> mMergeFrom_0 = new CrossBindingMethodInfo<Google.Protobuf.CodedInputStream>("MergeFrom");
			CrossBindingMethodInfo<Google.Protobuf.CodedOutputStream> mWriteTo_1 = new CrossBindingMethodInfo<Google.Protobuf.CodedOutputStream>("WriteTo");
			CrossBindingFunctionInfo<System.Int32> mCalculateSize_2 = new CrossBindingFunctionInfo<System.Int32>("CalculateSize");

			public Adapter()
			{

			}

			public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
			{
				this.appdomain = appdomain;
				this.instance = instance;
			}

			public ILTypeInstance ILInstance { get { return instance; } }

			public void MergeFrom(Google.Protobuf.CodedInputStream input)
			{
				mMergeFrom_0.Invoke(this.instance, input);
			}

			public void WriteTo(Google.Protobuf.CodedOutputStream output)
			{
				mWriteTo_1.Invoke(this.instance, output);
			}

			public System.Int32 CalculateSize()
			{
				return mCalculateSize_2.Invoke(this.instance);
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
