using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;

public class NodeLogin : IFsmNode
{
	public string Name { get; }

	public NodeLogin()
	{
		Name = nameof(NodeLogin);
	}
	void IFsmNode.OnEnter()
	{
		UIManager.Instance.OpenWindow(EWindowType.UILogin);
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
		UIManager.Instance.CloseWindow(EWindowType.UILogin);
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}
}