using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Res;
using UnityEngine;
using MotionGame;
using MotionEngine.Event;

public class GameTest : IModule
{
	public static readonly GameTest Instance = new GameTest();

	private GameTest()
	{
	}
	public void Awake()
	{
	}
	public void Start()
	{
		// 注册监听事件
		EventManager.Instance.AddListener(EventMessageTag.TestTag.ToString(), OnHandleEventMsg);

		// 优先加载多语言表
		CfgManager.Instance.Load(EConfigType.AutoGenerateLanguage.ToString(), OnLanguageConfigPrepare);
	}
	public void Update()
	{
	}
	public void LateUpdate()
	{
	}
	public void OnGUI()
	{
	}

	private void OnHandleEventMsg(IEventMessage msg)
	{
		if(msg is TestEventMsg)
		{
			TestEventMsg temp = msg as TestEventMsg;
			Debug.Log($"这里是MONO层： {temp.Value}");

			// 再给Hotfix层回复一条相同的测试消息
			TestEventMsg newMsg = new TestEventMsg()
			{
				Value = $"test event from mono {Time.frameCount}",
			};
			EventManager.Instance.Send(EventMessageTag.HotfixTag.ToString(), newMsg);
		}
	}
	private void OnLanguageConfigPrepare(Asset asset, EAssetResult result)
	{
		if (result != EAssetResult.OK)
			return;

		// 加载所有配表
		foreach (int value in System.Enum.GetValues(typeof(EConfigType)))
		{
			if (value == (int)EConfigType.AutoGenerateLanguage)
				continue;
			string cfgName = System.Enum.GetName(typeof(EConfigType), value);
			CfgManager.Instance.Load(cfgName, OnConfigPrepare);
		}
	}
	private void OnConfigPrepare(Asset asset, EAssetResult result)
	{
		if (result != EAssetResult.OK)
			return;

		// 测试打印表格数据
		if (asset is CfgHero)
		{
			CfgHeroTab tab1 = CfgHero.GetCfgTab(1001);
			Debug.Log($"非热更新表格数据：{tab1.Name}");

			CfgHeroTab tab2 = CfgHero.GetCfgTab(1002);
			Debug.Log($"非热更新表格数据：{tab2.Name}");
		}
	}
}