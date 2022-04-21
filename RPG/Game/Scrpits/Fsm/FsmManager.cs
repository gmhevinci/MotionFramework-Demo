using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.AI;
using MotionFramework;

public class FsmManager 
{
	public static readonly FsmManager Instance = new FsmManager();

	private FiniteStateMachine _fsm = new FiniteStateMachine();

	public void Init()
	{
		_fsm.AddNode(new NodeInit());
		_fsm.AddNode(new NodeLogin());
		_fsm.AddNode(new NodeTown());
	}
	public void Update()
	{
		_fsm.Update();
	}
	public void FixedUpdate()
	{
		_fsm.FixedUpdate();
	}

	public void StartGame()
	{
		_fsm.Run(nameof(NodeInit));
	}
	
	public void Change(string nodeName)
	{
		_fsm.Transition(nodeName);
	}
}