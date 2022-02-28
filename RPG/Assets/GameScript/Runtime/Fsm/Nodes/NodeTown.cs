using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Scene;
using MotionFramework.Audio;
using MotionFramework.Resource;
using MotionFramework.AI;
using YooAsset;

public class NodeTown : IFsmNode
{
	public string Name { get; }
	private GameWorld _gameWorld = new GameWorld();
	private bool _initWorld = false;

	public NodeTown()
	{
		Name = nameof(NodeTown);
	}
	void IFsmNode.OnEnter()
	{
		string sceneName = "Scene/Town";
		SceneManager.Instance.ChangeMainScene(sceneName, OnSceneLoad);
		UITools.OpenWindow<UILoading>(sceneName);
		UITools.OpenWindow<UIMain>();
		AudioManager.Instance.PlayMusic("Audio/Music/town", true);
	}
	void IFsmNode.OnUpdate()
	{
		if (_initWorld)
			_gameWorld.Update();
	}
	void IFsmNode.OnExit()
	{
		_gameWorld.Destroy();
		UITools.CloseWindow<UIMain>();
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}

	private void OnSceneLoad(SceneInstance instance)
	{
		if (instance == null)
			return;

		_initWorld = true;
		_gameWorld.Init();
	}
}