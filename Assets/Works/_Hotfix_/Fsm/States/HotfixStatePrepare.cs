using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class HotfixStatePrepare : HotfixFsmState
	{
		public HotfixStatePrepare(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
		}

		public override void Execute()
		{
			if (UIManager.Instance.IsPrepareUIRoot())
				HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Notice);
		}

		public override void Exit()
		{
		}
	}
}