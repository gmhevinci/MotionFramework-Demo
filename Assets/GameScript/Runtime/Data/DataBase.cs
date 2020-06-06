using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Event;

public abstract class DataBase
{
	protected readonly EventGroup _eventGroup = new EventGroup();

	public abstract void OnCreate();
	public abstract void OnUpdate();
	public abstract void OnDestroy();

	public void ClearEventGroup()
	{
		_eventGroup.RemoveAllListener();
	}
}