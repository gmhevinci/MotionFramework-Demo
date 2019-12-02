using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2> s_UnityEngine_Vector2_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3> s_UnityEngine_Vector3_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion> s_UnityEngine_Quaternion_Binding_Binder = null;

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_String_Binding.Register(app);
            MotionEngine_LogSystem_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int32_ILTypeInstance_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_List_1_Int64_Binding.Register(app);
            System_Collections_Generic_List_1_Single_Binding.Register(app);
            System_Collections_Generic_List_1_Double_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            MotionEngine_IO_StringConvert_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_Action_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_Action_1_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_Action_1_ILTypeInstance_Binding_KeyCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Action_1_ILTypeInstance_Binding.Register(app);
            System_Action_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            MotionEngine_Res_AssetObject_Binding.Register(app);
            MotionEngine_Res_Asset_Binding.Register(app);
            MotionGame_EventManager_Binding.Register(app);
            MotionGame_NetManager_Binding.Register(app);
            TestEventMsg_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_TransformExtension_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            UnityEngine_UI_UIManifest_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            MotionGame_AudioManager_Binding.Register(app);
            UnityEngine_UI_UISprite_Binding.Register(app);
            System_Type_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            MotionGame_ILRManager_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Attribute_Binding.Register(app);
            MotionGame_NetMessageAttribute_Binding.Register(app);
            MotionGame_DoubleMap_2_UInt16_Type_Binding.Register(app);
            System_Exception_Binding.Register(app);
            MotionGame_INetPackage_Binding.Register(app);
            System_Activator_Binding.Register(app);
            MotionGame_ProtobufHelper_Binding.Register(app);
            MotionGame_NetSendPackage_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Quaternion));
            s_UnityEngine_Quaternion_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector2_Binding_Binder = null;
            s_UnityEngine_Vector3_Binding_Binder = null;
            s_UnityEngine_Quaternion_Binding_Binder = null;
        }
    }
}
