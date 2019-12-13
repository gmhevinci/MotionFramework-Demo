using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Network;
using MotionFramework.Audio;

namespace Hotfix
{
	[Window(EWindowType.UILogin, EWindowLayer.Panel)]
	public class UILogin : UIWindow
	{
		private UISprite _loginSprite;
		private InputField _account;
		private InputField _password;

		public override void OnCreate()
		{
			_loginSprite = GetUIComponent<UISprite>("UILogin/box_login/btn_login") ;
			_account = GetUIComponent<InputField>("UILogin/box_login/box_id/InputField");
			_password = GetUIComponent<InputField>("UILogin/box_login/box_pw/InputField");

			// 监听按钮点击事件
			Button btnLogin = GetUIComponent<Button>("UILogin/box_login/btn_login");
			btnLogin.onClick.AddListener(OnClickLogin);
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
		}
		public override void OnUpdate()
		{
		}

		private void OnClickLogin()
		{
			// 播放点击音效
			AudioManager.Instance.PlaySound("click");

			// 替换按钮图片
			if (_loginSprite.SpriteName == "button_login")
				_loginSprite.SpriteName = "button_start";
			else
				_loginSprite.SpriteName = "button_login";

			// 发送登录事件
			var message = new HotfixEvent.ConnectServer();
			message.Account = _account.text;
			message.Password = _password.text;
			HotfixEventManager.Instance.Send(HotfixEventMessageTag.ConnectServer, message);
		}
	}
}