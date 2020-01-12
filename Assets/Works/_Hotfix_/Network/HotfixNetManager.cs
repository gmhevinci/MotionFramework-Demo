using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MotionFramework.Network;

namespace Hotfix
{
	public class HotfixNetManager
	{
		public static readonly HotfixNetManager Instance = new HotfixNetManager();

		/// <summary>
		/// 所有热更新协议的类型集合
		/// </summary>
		private readonly DoubleMap<int, Type> _types = new DoubleMap<int, Type>();


		public void Start()
		{
			// 收集所有网络协议的类型
			CollectTypes();

			// 注册消息接收回调
			NetworkManager.Instance.HotfixPackageCallback += OnHandleHotfixMsg;
		}
		public void Update()
		{
		}

		/// <summary>
		/// 接收消息回调函数
		/// </summary>
		private void OnHandleHotfixMsg(INetworkPackage pack)
		{
			NetworkPackage package = pack as NetworkPackage;
			Type msgType = _types.GetValueByKey(package.MsgID);
			HotfixLogger.Log($"Handle hotfix net message : {msgType}");
			object instance = Activator.CreateInstance(msgType);
			var message = ProtobufHelper.Decode(instance, package.BodyBytes);			
			DataManager.Instance.HandleNetMessage(message as IHotfixNetMessage);
		}

		/// <summary>
		/// 发送网络消息
		/// </summary>
		public void SendHotfixMsg(IHotfixNetMessage msg)
		{
			HotfixLogger.Log($"Send hotfix net message : {msg.GetType()}");
			int msgID = _types.GetKeyByValue(msg.GetType());
			NetworkPackage package = new NetworkPackage();
			package.IsHotfixPackage = true;
			package.MsgID = msgID;
			package.BodyBytes = ProtobufHelper.Encode(msg);
			NetworkManager.Instance.SendMessage(package);
		}

		// 收集所有网络协议的类型
		private void CollectTypes()
		{
			List<Type> types = ILRManager.Instance.HotfixAssemblyTypes;
			for (int i = 0; i < types.Count; i++)
			{
				System.Type type = types[i];

				// 判断属性标签
				if (Attribute.IsDefined(type, typeof(NetworkMessageAttribute)))
				{
					// 判断是否重复
					NetworkMessageAttribute attribute = HotfixTypeHelper.GetAttribute<NetworkMessageAttribute>(type);
					if (_types.ContainsKey(attribute.MsgID))
						throw new Exception($"Message {type} has same value : {attribute.MsgID}");

					// 添加到集合
					_types.Add(attribute.MsgID, type);
				}
			}
		}
	}
}