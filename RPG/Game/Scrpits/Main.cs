using System;
using MotionFramework.Window;

public class Main
{
	public static void Start()
	{
		// 缓存所有的特性
		GameLog.Log("收集所有热更类的属性并缓存");
		{
			Attribute attribute1 = TypeHelper.GetAttribute<WindowAttribute>(typeof(UILoading));
			ILRManager.Instance.CacheHotfixAttribute(typeof(UILoading), attribute1);

			Attribute attribute2 = TypeHelper.GetAttribute<WindowAttribute>(typeof(UILogin));
			ILRManager.Instance.CacheHotfixAttribute(typeof(UILogin), attribute2);

			Attribute attribute3 = TypeHelper.GetAttribute<WindowAttribute>(typeof(UIMain));
			ILRManager.Instance.CacheHotfixAttribute(typeof(UIMain), attribute3);

			Attribute attribute4 = TypeHelper.GetAttribute<WindowAttribute>(typeof(UISetting));
			ILRManager.Instance.CacheHotfixAttribute(typeof(UISetting), attribute4);
		}

		DataManager.Instance.Init();
		FsmManager.Instance.Init();

		GameLog.Log("Start RPG GAME");
		FsmManager.Instance.StartGame();
	}
	public static void Update()
	{
		DataManager.Instance.Update();
		FsmManager.Instance.Update();
	}
	public static void FixedUpdate()
	{
		FsmManager.Instance.FixedUpdate();
	}
}