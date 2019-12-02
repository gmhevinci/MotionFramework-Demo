using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class MotionEngine_Res_Asset_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(MotionEngine.Res.Asset);
            args = new Type[]{typeof(System.String), typeof(System.Action<MotionEngine.Res.Asset, MotionEngine.Res.EAssetResult>)};
            method = type.GetMethod("Load", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Load_0);


        }


        static StackObject* Load_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<MotionEngine.Res.Asset, MotionEngine.Res.EAssetResult> @prepareCallbcak = (System.Action<MotionEngine.Res.Asset, MotionEngine.Res.EAssetResult>)typeof(System.Action<MotionEngine.Res.Asset, MotionEngine.Res.EAssetResult>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @resName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            MotionEngine.Res.Asset instance_of_this_method = (MotionEngine.Res.Asset)typeof(MotionEngine.Res.Asset).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Load(@resName, @prepareCallbcak);

            return __ret;
        }



    }
}
