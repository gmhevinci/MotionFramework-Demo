using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class HotfixStatePrepare : HotfixFsmState
	{
		private bool _isPreload = false;

		public HotfixStatePrepare(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
		}

		public override void Execute()
		{
			if (UIManager.Instance.IsPrepareUIRoot())
			{
				if (_isPreload == false)
				{
					_isPreload = true;
					UIManager.Instance.PreloadWindow(EWindowType.UILoading);
				}

				if (UIManager.Instance.IsLoadingWindow() == false)
				{
					HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Notice);
				}
			}
		}

		public override void Exit()
		{
		}
	}
}