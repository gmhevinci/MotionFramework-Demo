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
    unsafe class MotionGame_NetManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(MotionGame.NetManager);
            args = new Type[]{};
            method = type.GetMethod("get_State", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_State_0);
            args = new Type[]{typeof(System.String), typeof(System.Int32), typeof(System.Type)};
            method = type.GetMethod("ConnectServer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConnectServer_1);
            args = new Type[]{typeof(MotionGame.INetPackage)};
            method = type.GetMethod("SendMsg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SendMsg_2);

            field = type.GetField("Instance", flag);
            app.RegisterCLRFieldGetter(field, get_Instance_0);
            field = type.GetField("HotfixPackageCallback", flag);
            app.RegisterCLRFieldGetter(field, get_HotfixPackageCallback_1);
            app.RegisterCLRFieldSetter(field, set_HotfixPackageCallback_1);


        }


        static StackObject* get_State_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionGame.NetManager instance_of_this_method = (MotionGame.NetManager)typeof(MotionGame.NetManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.State;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ConnectServer_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Type @packageParseType = (System.Type)typeof(System.Type).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @port = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @host = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            MotionGame.NetManager instance_of_this_method = (MotionGame.NetManager)typeof(MotionGame.NetManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ConnectServer(@host, @port, @packageParseType);

            return __ret;
        }

        static StackObject* SendMsg_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionGame.INetPackage @package = (MotionGame.INetPackage)typeof(MotionGame.INetPackage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionGame.NetManager instance_of_this_method = (MotionGame.NetManager)typeof(MotionGame.NetManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SendMsg(@package);

            return __ret;
        }


        static object get_Instance_0(ref object o)
        {
            return MotionGame.NetManager.Instance;
        }
        static object get_HotfixPackageCallback_1(ref object o)
        {
            return ((MotionGame.NetManager)o).HotfixPackageCallback;
        }
        static void set_HotfixPackageCallback_1(ref object o, object v)
        {
            ((MotionGame.NetManager)o).HotfixPackageCallback = (System.Action<MotionGame.INetPackage>)v;
        }


    }
}
