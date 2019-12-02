using System;
using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Event;
using MotionGame;

namespace Hotfix
{
	public class HotfixEventManager
	{
		public static readonly HotfixEventManager Instance = new HotfixEventManager();

		/// <summary>
		/// 监听集合
		/// </summary>
		private readonly Dictionary<string, List<Action<IHotfixEventMessage>>> _handlers = new Dictionary<string, List<Action<IHotfixEventMessage>>>();


		/// <summary>
		/// 清空所有监听
		/// </summary>
		public void ClearListeners()
		{
			foreach (string type in _handlers.Keys)
			{
				_handlers[type].Clear();
			}
			_handlers.Clear();
		}

		/// <summary>
		/// 注册监听
		/// </summary>
		public void AddListener(string eventTag, Action<IHotfixEventMessage> listener)
		{
			if (_handlers.ContainsKey(eventTag) == false)
				_handlers.Add(eventTag, new List<Action<IHotfixEventMessage>>());

			if (_handlers[eventTag].Contains(listener) == false)
				_handlers[eventTag].Add(listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(string eventTag, Action<IHotfixEventMessage> listener)
		{
			if (_handlers.ContainsKey(eventTag))
			{
				if (_handlers[eventTag].Contains(listener))
					_handlers[eventTag].Remove(listener);
			}
		}

		/// <summary>
		/// 发送事件
		/// </summary>
		public void Send(string eventTag, IHotfixEventMessage msg)
		{
			if (_handlers.ContainsKey(eventTag) == false)
			{
				LogSystem.Log(ELogType.Warning, $"Not found message eventTag : {eventTag}");
				return;
			}

			List<Action<IHotfixEventMessage>> listeners = _handlers[eventTag];
			for (int i = 0; i < listeners.Count; i++)
			{
				listeners[i].Invoke(msg);
			}
		}
	}
}