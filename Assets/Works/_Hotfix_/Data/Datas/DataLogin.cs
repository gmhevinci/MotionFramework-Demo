using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;
using MotionFramework.Event;

namespace Hotfix
{
	public class DataLogin : DataBase
	{
		private string _account;
		private string _password;

		public override void Start()
		{
			AddEventListener(HotfixEventMessageTag.LoginEvent, OnHandleEventMessage);
			EventManager.Instance.AddListener<NetworkEventMessageDefine.ConnectSuccess>(OnHandleMonoEventMessage);
		}
		public override void Update()
		{
		}
		public override void Destroy()
		{
			EventManager.Instance.RemoveListener<NetworkEventMessageDefine.ConnectSuccess>(OnHandleMonoEventMessage);
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

		private void OnHandleEventMessage(IHotfixEventMessage msg)
		{
			if (msg is LoginEvent.ConnectServer)
			{
				LoginEvent.ConnectServer message = msg as LoginEvent.ConnectServer;
				_account = message.Account;
				_password = message.Password;

				// 连接到ET5.0服务器
				//NetworkManager.Instance.ConnectServer("127.0.0.1", 10002);

				// TODO 跳过服务器直接进入游戏
				HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Town);
			}
		}
		private void OnHandleMonoEventMessage(IEventMessage msg)
		{
			// 当服务器连接成功
			if(msg is NetworkEventMessageDefine.ConnectSuccess)
			{
				SendLoginMsg(_account, _password);
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