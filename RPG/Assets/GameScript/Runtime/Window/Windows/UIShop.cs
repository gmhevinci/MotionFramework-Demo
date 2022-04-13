using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Window;

[Window((int)EWindowLayer.Panel, true)]
sealed class UIShop : CanvasWindow
{
	public override void OnCreate()
	{
		AddButtonListener("UIShop/closeBtn", OnCloseWindow);
		AddButtonListener("UIShop/settingBtn", OnClickSetting);
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

	private void OnCloseWindow()
	{
		UITools.CloseWindow<UIShop>();
	}
	private void OnClickSetting()
	{
		UITools.OpenWindow<UISetting>();
	}
}
