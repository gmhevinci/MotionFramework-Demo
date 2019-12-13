using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public abstract class HotfixFsmState
	{
		/// <summary>
		/// 状态类型
		/// </summary>
		public EHotfixStateType Type { get; private set; }

		public HotfixFsmState(EHotfixStateType type)
		{
			Type = type;
		}
		public abstract void Enter();
		public abstract void Execute();
		public abstract void Exit();
		public virtual void OnMessage(object msg) { }
	}
}