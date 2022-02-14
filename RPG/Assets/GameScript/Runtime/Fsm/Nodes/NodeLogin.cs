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
		var uiwindow = UITools.OpenWindow<UILogin>();
		uiwindow.Completed += Uiwindow_Completed;

		string sceneName = "Scene/Login";
		SceneManager.Instance.ChangeMainScene(sceneName, null);
	}

	private void Uiwindow_Completed(MotionFramework.Window.UIWindow obj)
	{
		UnityEngine.Debug.Log("Login window load complete.");
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