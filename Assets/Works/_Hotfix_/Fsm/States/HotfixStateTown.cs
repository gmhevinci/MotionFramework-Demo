using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Scene;
using MotionFramework.Audio;

namespace Hotfix
{
	public class HotfixStateTown : HotfixFsmState
	{
		private GameWorld _world = new GameWorld();
		private bool _initWorld = false;

		public HotfixStateTown(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
			string sceneName = "Scene/Town";		
			SceneManager.Instance.ChangeMainScene(sceneName, null);
			UIManager.Instance.OpenWindow(EWindowType.UILoading, sceneName);
			UIManager.Instance.OpenWindow(EWindowType.UIMain);
			AudioManager.Instance.PlayMusic("Music/town", true);
		}
		public override void Execute()
		{
			if(_initWorld == false)
			{
				string sceneName = "Scene/Town";
				if (SceneManager.Instance.CheckSceneIsDone(sceneName))
				{
					_initWorld = true;
					_world.Init();
				}
			}

			if(_initWorld)
				_world.Update();
		}
		public override void Exit()
		{
			_world.Destroy();
		}
	}
}