using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Scene;
using MotionFramework.Audio;
using MotionFramework.Resource;

namespace Hotfix
{
	public class HotfixStateTown : HotfixFsmState
	{
		private GameWorld _gameWorld = new GameWorld();
		private bool _initWorld = false;

		public HotfixStateTown(EHotfixStateType type) : base(type)
		{
		}

		public override void Enter()
		{
			string sceneName = "Scene/Town";
			SceneManager.Instance.ChangeMainScene(sceneName, true, OnSceneLoad);
			UIManager.Instance.OpenWindow(EWindowType.UILoading, sceneName);
			UIManager.Instance.OpenWindow(EWindowType.UIMain);
			AudioManager.Instance.PlayMusic("Audio/Music/town", true);
		}
		public override void Execute()
		{
			if(_initWorld)
				_gameWorld.Update();
		}
		public override void Exit()
		{
			_gameWorld.Destroy();
		}
		
		private void OnSceneLoad(SceneInstance instance)
		{
			if (instance == null)
				return;

			_initWorld = true;
			_gameWorld.Init();
		}
	}
}