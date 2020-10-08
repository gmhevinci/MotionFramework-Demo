using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Scene;

public class NodeLogin : IFsmNode
{
	public string Name { get; }

	public NodeLogin()
	{
		Name = nameof(NodeLogin);
	}
	void IFsmNode.OnEnter()
	{
		UITools.OpenWindow<UILogin>();

		string sceneName = "Scene/Login";
		SceneManager.Instance.ChangeMainScene(sceneName, true, null);
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
		UITools.CloseWindow<UILogin>();
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}
}