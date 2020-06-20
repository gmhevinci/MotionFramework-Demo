using MotionFramework.AI;
using MotionFramework.Config;
using MotionFramework;
using System;
using System.Collections.Generic;
using System.Collections;
using MotionFramework.Window;

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
	void IFsmNode.OnExit()
	{
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}

	private IEnumerator Init()
	{
		// 加载UIRoot
		yield return UIManager.Instance.CreateUIRoot<CanvasRoot>("UIPanel/UIRoot");

		// 加载常驻面板
		var loadingWindow = UITools.PreloadWindow<UILoading>();
		yield return loadingWindow;

		// 进入到登录流程
		FsmManager.Instance.Transition(nameof(NodeLogin));
	}
}