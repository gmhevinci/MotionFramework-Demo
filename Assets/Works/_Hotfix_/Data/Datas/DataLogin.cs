using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;

namespace Hotfix
{
	public class DataLogin : DataBase
	{
		private string _account = string.Empty;
		private string _password = string.Empty;
		private bool _isSendLoginMsg = false;

		public override void Start()
		{
			AddEventListener(HotfixEventMessageTag.ConnectServer, OnHandleEventMessage);
		}
		public override void Update()
		{
			// 当网络连接成功之后发送登录消息
			if (_isSendLoginMsg == false && NetworkManager.Instance.State == ENetworkStates.Connected)
			{
				_isSendLoginMsg = true;
				SendLoginMsg(_account, _password);
			}
		}
		public override void Destroy()
		{
		}
		public override void OnHandleNetMessage(IHotfixNetMessage msg)
		{
			if (msg is R2C_Login)
			{
				R2C_Login message = msg as R2C_Login;
				HotfixLogger.Log($"登录成功：{message.Address} {message.Key}");

				HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Town);
			}
		}
		public void OnHandleEventMessage(IHotfixEventMessage msg)
		{
			if (msg is HotfixEvent.ConnectServer)
			{
				if (NetworkManager.Instance.State != ENetworkStates.Disconnect)
					return;

				HotfixEvent.ConnectServer message = msg as HotfixEvent.ConnectServer;
				_account = message.Account;
				_password = message.Password;

				// 连接到ET5.0服务器
				/*
				if (NetworkManager.Instance.State == ENetworkState.Disconnect)
					NetworkManager.Instance.ConnectServer("127.0.0.1", 10002, typeof(ProtoPackageParser));
				*/

				// TODO 跳过服务器直接进入游戏
				HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Town);
			}
		}

		private static void SendLoginMsg(string account, string password)
		{
			C2R_Login msg = new C2R_Login();
			msg.RpcId = 100;
			msg.Account = account;
			msg.Password = password;
			HotfixNetManager.Instance.SendHotfixMsg(msg);
		}
	}
}