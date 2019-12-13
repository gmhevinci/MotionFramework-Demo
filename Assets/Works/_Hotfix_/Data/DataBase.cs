using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public abstract class DataBase
	{
		public abstract void Start();
		public abstract void Update();
		public abstract void Destroy();
		public abstract void OnHandleNetMessage(IHotfixNetMessage msg);

		#region 事件相关
		private readonly Dictionary<HotfixEventMessageTag, List<Action<IHotfixEventMessage>>> _cachedListener = new Dictionary<HotfixEventMessageTag, List<Action<IHotfixEventMessage>>>();

		/// <summary>
		/// 添加一个事件监听
		/// </summary>
		protected void AddEventListener(HotfixEventMessageTag eventTag, System.Action<IHotfixEventMessage> listener)
		{
			if (_cachedListener.ContainsKey(eventTag) == false)
				_cachedListener.Add(eventTag, new List<Action<IHotfixEventMessage>>());

			if (_cachedListener[eventTag].Contains(listener) == false)
			{
				_cachedListener[eventTag].Add(listener);
				HotfixEventManager.Instance.AddListener(eventTag, listener);
			}
			else
			{
				HotfixLogger.Warning($"Event listener is exist : {eventTag}");
			}
		}

		/// <summary>
		/// 移除所有缓存的事件监听
		/// </summary>
		public void RemoveAllListener()
		{
			foreach (var pair in _cachedListener)
			{
				HotfixEventMessageTag eventTag = pair.Key;
				for (int i = 0; i < pair.Value.Count; i++)
				{
					HotfixEventManager.Instance.RemoveListener(eventTag, pair.Value[i]);
				}
				pair.Value.Clear();
			}
			_cachedListener.Clear();
		}
		#endregion
	}
}