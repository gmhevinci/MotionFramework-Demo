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
    unsafe class MotionGame_NetMessageAttribute_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(MotionGame.NetMessageAttribute);

            field = type.GetField("MsgType", flag);
            app.RegisterCLRFieldGetter(field, get_MsgType_0);
            app.RegisterCLRFieldSetter(field, set_MsgType_0);


        }



        static object get_MsgType_0(ref object o)
        {
            return ((MotionGame.NetMessageAttribute)o).MsgType;
        }
        static void set_MsgType_0(ref object o, object v)
        {
            ((MotionGame.NetMessageAttribute)o).MsgType = (System.UInt16)v;
        }


    }
}
