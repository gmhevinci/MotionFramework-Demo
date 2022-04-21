using System;
using System.Collections;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace MotionFramework.Config
{   
    public class AssetConfigAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(MotionFramework.Config.AssetConfig);
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

        public class Adapter : MotionFramework.Config.AssetConfig, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            CrossBindingFunctionInfo<MotionFramework.IO.ByteBuffer, MotionFramework.Config.ConfigTable> mReadTable_0 = new CrossBindingFunctionInfo<MotionFramework.IO.ByteBuffer, MotionFramework.Config.ConfigTable>("ReadTable");

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override MotionFramework.Config.ConfigTable ReadTable(MotionFramework.IO.ByteBuffer byteBuffer)
            {
                return mReadTable_0.Invoke(this.instance, byteBuffer);
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
