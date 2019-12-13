using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class HotfixFsmManager
	{
		public readonly static HotfixFsmManager Instance = new HotfixFsmManager();

		/// <summary>
		/// 状态集合
		/// </summary>
		private readonly List<HotfixFsmState> _states = new List<HotfixFsmState>();

		/// <summary>
		/// 当前运行状态
		/// </summary>
		private HotfixFsmState _runState;

		/// <summary>
		/// 之前运行状态
		/// </summary>
		private HotfixFsmState _preState;


		/// <summary>
		/// 当前运行的状态类型
		/// </summary>
		public EHotfixStateType RunStateType
		{
			get { return _runState != null ? _runState.Type : 0; }
		}

		/// <summary>
		/// 之前运行的状态类型
		/// </summary>
		public EHotfixStateType PreStateType
		{
			get { return _preState != null ? _preState.Type : 0; }
		}


		private HotfixFsmManager()
		{
		}

		/// <summary>
		/// 启动状态机
		/// </summary>
		/// <param name="runStateType">初始状态类型</param>
		public void Run(EHotfixStateType runStateType)
		{
			_runState = GetState(runStateType);
			_preState = GetState(runStateType);

			if (_runState != null)
				_runState.Enter();
			else
				HotfixLogger.Error($"Not found run state : {runStateType}");
		}

		/// <summary>
		/// 更新状态机
		/// </summary>
		public void Update()
		{
			if (_runState != null)
				_runState.Execute();
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			if (_runState != null)
				_runState.OnMessage(msg);
		}

		/// <summary>
		/// 添加一个状态节点
		/// </summary>
		public void AddState(HotfixFsmState state)
		{
			if (state == null)
				throw new ArgumentNullException();

			if (_states.Contains(state) == false)
			{
				_states.Add(state);
			}
			else
			{
				HotfixLogger.Warning($"State {state.Type} already existed");
			}
		}

		/// <summary>
		/// 改变状态
		/// </summary>
		public void ChangeState(EHotfixStateType stateType)
		{
			HotfixFsmState state = GetState(stateType);
			if (state == null)
			{
				HotfixLogger.Error($"Can not found state {stateType}");
				return;
			}

			HotfixLogger.Log($"Change state {_runState} to {state}");
			_preState = _runState;
			_runState.Exit();
			_runState = state;
			_runState.Enter();
		}

		/// <summary>
		/// 返回之前状态
		/// </summary>
		public void RevertToPreState()
		{
			EHotfixStateType stateType = _preState != null ? _preState.Type : 0;
			ChangeState(stateType);
		}

		/// <summary>
		/// 查询是否包含状态类型
		/// </summary>
		private bool IsContains(EHotfixStateType stateType)
		{
			for (int i = 0; i < _states.Count; i++)
			{
				if (_states[i].Type == stateType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 获取状态类
		/// </summary>
		private HotfixFsmState GetState(EHotfixStateType stateType)
		{
			for (int i = 0; i < _states.Count; i++)
			{
				if (_states[i].Type == stateType)
					return _states[i];
			}
			return null;
		}
	}
}
