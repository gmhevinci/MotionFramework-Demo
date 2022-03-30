using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework;
using MotionFramework.AI;
using MotionFramework.Window;
using MotionFramework.Pool;
using UnityEngine;

public class NodeInit : IFsmNode
{
	public string Name { get; }

	public NodeInit()
	{
		Name = nameof(NodeInit);
	}
	void IFsmNode.OnEnter()
	{
		AudioPlayerSetting.InitAudioSetting();

		// 使用协程初始化
		MotionEngine.StartCoroutine(Init());
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnFixedUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}

	private IEnumerator Init()
	{
		// 加载UIRoot
		var uiRoot = WindowManager.Instance.CreateUIRoot<CanvasRoot>("UIPanel/UIRoot");
		yield return uiRoot;

		// 加载常驻面板
		yield return GameObjectPoolManager.Instance.CreatePool("UIPanel/UILoading", true);

		// 进入到登录流程
		FsmManager.Instance.Transition(nameof(NodeLogin));
	}
}