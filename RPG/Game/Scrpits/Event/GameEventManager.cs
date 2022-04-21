using System.Collections;
using System.Collections.Generic;
using System;
using MotionFramework.Event;

public class GameEventManager
{
	public static readonly GameEventManager Instance = new GameEventManager();

	/// <summary>
	/// 添加监听
	/// </summary>
	public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
	{
		int eventId = typeof(TEvent).GetHashCode();
		EventManager.Instance.AddListener(eventId, listener);
	}

	/// <summary>
	/// 移除监听
	/// </summary>
	public void RemoveListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
	{
		int eventId = typeof(TEvent).GetHashCode();
		EventManager.Instance.RemoveListener(eventId, listener);
	}

	/// <summary>
	/// 广播事件
	/// </summary>
	public void SendMessage(IEventMessage message)
	{
		int eventId = message.GetType().GetHashCode();
		EventManager.Instance.SendMessage(eventId, message);
	}
}