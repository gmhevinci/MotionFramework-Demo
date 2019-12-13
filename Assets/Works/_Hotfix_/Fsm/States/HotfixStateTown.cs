using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Scene;

namespace Hotfix
{
	public class HotfixStateTown : HotfixFsmState
	{
		public HotfixStateTown(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
			string sceneName = "Scene/Town";		
			SceneManager.Instance.ChangeMainScene(sceneName, null);
			UIManager.Instance.OpenWindow(EWindowType.UILoading, sceneName);
		}

		public override void Execute()
		{

		}

		public override void Exit()
		{
		}
	}
}