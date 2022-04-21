using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;
using MotionFramework.Event;

public class DataLogin : DataBase
{
	//private string _account;
	//private string _password;

	public override void OnCreate()
	{
		_eventGroup.AddListener<LoginEvent.ConnectServer>(OnHandleEvent);
	}
	public override void OnUpdate()
	{
	}
	public override void OnDestroy()
	{
	}

	private void OnHandleEvent(IEventMessage msg)
	{
		if(msg is LoginEvent.ConnectServer)
		{
			FsmManager.Instance.Change(nameof(NodeTown));
		}
	}
}