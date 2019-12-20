using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public class CharacterData
	{
		public string CurrentIdleAnimName = "idle"; //角色当前的待机动作名称
		public string CurrentRunAnimName = "run"; //角色当前的跑步动作名称
		

		/// <summary>
		/// 跑步动画的固定速度（美术相关）
		/// </summary>
		public float RunAnimSpeed { private set; get; } = 1f;

		/// <summary>
		/// 移动速度
		/// </summary>
		public float MoveSpeed { private set; get; } = 5f;

		public bool IsCanRotate()
		{
			return true;
		}
		public bool IsCanMove()
		{
			return true;
		}
	}
}