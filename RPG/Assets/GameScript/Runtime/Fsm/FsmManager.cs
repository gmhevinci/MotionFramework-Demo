using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.AI;
using MotionFramework;

public class FsmManager : ModuleSingleton<FsmManager>, IModule
{
	private FiniteStateMachine _fsm = new FiniteStateMachine();

	void IModule.OnCreate(object createParam)
	{
		_fsm.AddNode(new NodeInit());
		_fsm.AddNode(new NodeLogin());
		_fsm.AddNode(new NodeTown());
	}
	void IModule.OnUpdate()
	{
		_fsm.Update();
	}
	void IModule.OnGUI()
	{
	}

	public void FixedUpdate()
	{
		_fsm.FixedUpdate();
	}

	public void StartGame()
	{
		_fsm.Run(nameof(NodeInit));
	}

	public void Transition(string nodeName)
	{
		_fsm.Transition(nodeName);
	}
}