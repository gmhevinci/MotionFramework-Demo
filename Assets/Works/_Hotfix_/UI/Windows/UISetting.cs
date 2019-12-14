using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
	[Window(EWindowType.UISetting, EWindowLayer.Panel)]
	public class UISetting : UIWindow
	{
		public override void OnCreate()
		{
			AddButtonListener("UISetting/Mask", OnClickClose);
			AddButtonListener("UISetting/Window/Button (Save)", OnClickClose);
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

		private void OnClickClose()
		{
			UIManager.Instance.CloseWindow(EWindowType.UISetting);
		}
	}
}