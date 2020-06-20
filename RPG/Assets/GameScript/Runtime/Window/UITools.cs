using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Window;

public static class UITools
{
	/// <summary>
	/// 预加载窗口
	/// </summary>
	public static UIWindow PreloadWindow<T>() where T : UIWindow
	{
		string location = $"UIPanel/{typeof(T).Name}";
		return UIManager.Instance.PreloadWindow<T>(location);
	}

	/// <summary>
	/// 打开窗口
	/// </summary>
	public static void OpenWindow<T>(object userData = null) where T : UIWindow
	{
		string location = $"UIPanel/{typeof(T).Name}";
		UIManager.Instance.OpenWindow<T>(location, userData);
	}

	/// <summary>
	/// 关闭窗口
	/// </summary>
	public static void CloseWindow<T>() where T : UIWindow
	{
		UIManager.Instance.CloseWindow<T>();
	}
}