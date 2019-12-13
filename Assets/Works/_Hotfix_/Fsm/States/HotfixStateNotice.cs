using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class HotfixStateNotice : HotfixFsmState
	{
		public HotfixStateNotice(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
		}

		public override void Execute()
		{
			HotfixFsmManager.Instance.ChangeState(EHotfixStateType.Login);
		}

		public override void Exit()
		{
		}
	}
}