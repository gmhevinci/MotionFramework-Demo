using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Window;

public static class UITools
{
	/// <summary>
	/// 打开窗口
	/// </summary>
	public static UIWindow OpenWindow<T>(object userData = null) where T : UIWindow
	{
		string location = $"UIPanel/{typeof(T).Name}";
		//GameLog.Log($"Open : {typeof(T).Name}");
		return WindowManager.Instance.OpenWindow<T>(location, userData);
	}

	/// <summary>
	/// 关闭窗口
	/// </summary>
	public static void CloseWindow<T>() where T : UIWindow
	{
		//GameLog.Log($"Close : {typeof(T).Name}");
		WindowManager.Instance.CloseWindow<T>();
	}
}