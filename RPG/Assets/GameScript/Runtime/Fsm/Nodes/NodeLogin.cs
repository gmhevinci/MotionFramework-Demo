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
		UITools.OpenWindow<UILogin>();
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