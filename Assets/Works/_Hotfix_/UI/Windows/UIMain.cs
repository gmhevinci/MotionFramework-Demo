using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
	[Window(EWindowType.UIMain, EWindowLayer.Panel)]
	public class UIMain : UIWindow
	{

		public override void OnCreate()
		{
			AddButtonListener("UIMain/Meum/Button (Settings)", OnClickSetting);
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

		private void OnClickSetting()
		{
			UIManager.Instance.OpenWindow(EWindowType.UISetting);
		}
	}
}