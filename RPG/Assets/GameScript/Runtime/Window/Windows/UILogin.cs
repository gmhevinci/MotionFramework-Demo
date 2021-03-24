using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;
using MotionFramework.Resource;

[Window((int)EWindowLayer.Panel, true)]
sealed class UILogin : CanvasWindow
{
	private UISprite _loginSprite;
	private InputField _account;
	private InputField _password;

	public override void OnCreate()
	{
		_loginSprite = GetUIComponent<UISprite>("UILogin/Window/Button (Login)");
		_account = GetUIComponent<InputField>("UILogin/Window/Content/Text Field (Username)");
		_password = GetUIComponent<InputField>("UILogin/Window/Content/Text Field (Password)");

		// 监听按钮点击事件
		AddButtonListener("UILogin/Window/Button (Login)", OnClickLogin);

		// 替换背景图片
		var bgImage = GetUIComponent<Image>("UILogin/Background");
		var handle = ResourceManager.Instance.LoadAssetSync<Sprite>("UITexture/bg1.png");
		bgImage.sprite = handle.AssetObject as Sprite;
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
		// 替换按钮图片
		if (_loginSprite.SpriteName == "Button_Rectangular_Large_Green_Background")
			_loginSprite.SpriteName = "Button_Rectangular_Large_Red_Background";
		else
			_loginSprite.SpriteName = "Button_Rectangular_Large_Green_Background";

		// 发送登录事件
		var message = new LoginEvent.ConnectServer
		{
			Account = _account.text,
			Password = _password.text
		};
		EventManager.Instance.SendMessage(message);
	}
}