using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MotionGame;

namespace Hotfix
{
	public class HotfixNetManager
	{
		public static readonly HotfixNetManager Instance = new HotfixNetManager();

		/// <summary>
		/// 所有热更新协议的类型集合
		/// </summary>
		private readonly DoubleMap<ushort, Type> _msgTypes = new DoubleMap<ushort, Type>();


		public void Start()
		{
			// 收集所有网络协议的类型
			List<Type> types = ILRManager.Instance.HotfixAssemblyTypes;
			for (int i = 0; i < types.Count; i++)
			{
				System.Type type = types[i];

				// 判断属性标签
				if (Attribute.IsDefined(type, typeof(NetMessageAttribute)))
				{
					var attributeArray = type.GetCustomAttributes(typeof(NetMessageAttribute), false);
					NetMessageAttribute attribute = attributeArray[0] as NetMessageAttribute;

					// 判断是否重复
					if (_msgTypes.ContainsKey(attribute.MsgType))
						throw new Exception($"Message {type} has same value : {attribute.MsgType}");

					// 添加到集合
					_msgTypes.Add(attribute.MsgType, type);
				}
			}

			// 注册消息接收回调
			NetManager.Instance.HotfixPackageCallback += OnHandleHotfixMsg;
		}
		public void Update()
		{
		}

		/// <summary>
		/// 接收消息回调函数
		/// </summary>
		private void OnHandleHotfixMsg(INetPackage package)
		{
			Type msgID = _msgTypes.GetValueByKey(package.MsgID);
			object instance = Activator.CreateInstance(msgID);
			var message = ProtobufHelper.Decode(instance, package.MsgBytes);

			Debug.Log($"Handle net message : {msgID}");

			// 注意：可以在这里分发消息到逻辑层
			R2C_Login loginMsg = message as R2C_Login;
			if(loginMsg != null)
			{
				Debug.Log($"R2C_Login = {loginMsg.Address}");
				Debug.Log($"R2C_Login = {loginMsg.Key}");		
			}
		}

		/// <summary>
		/// 发送网络消息
		/// </summary>
		public void SendMsg(IHotfixMessage msg)
		{
			ushort msgID = _msgTypes.GetKeyByValue(msg.GetType());
			NetSendPackage package = new NetSendPackage();
			package.MsgID = msgID;
			package.MsgObj = msg;
			NetManager.Instance.SendMsg(package);
		}
	}
}