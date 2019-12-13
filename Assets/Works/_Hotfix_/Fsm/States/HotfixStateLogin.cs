using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class HotfixStateLogin : HotfixFsmState
	{
		public HotfixStateLogin(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
			UIManager.Instance.OpenWindow(EWindowType.UILogin);
		}

		public override void Execute()
		{
		}

		public override void Exit()
		{
			UIManager.Instance.CloseWindow(EWindowType.UILogin);
		}
	}
}