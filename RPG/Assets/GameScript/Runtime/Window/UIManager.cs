using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;

public class UIManager : ModuleSingleton<UIManager>, IModule
{
	private readonly Dictionary<EWindowType, Type> _types = new Dictionary<EWindowType, Type>();
	private readonly List<UIWindow> _stack = new List<UIWindow>(100);
	private readonly UIRoot _root = new UIRoot();
	private readonly WindowCreater _creater = new WindowCreater();

	void IModule.OnCreate(object createParam)
	{
		_creater.Initialize(MotionFramework.Utility.AssemblyUtility.UnityDefaultAssemblyName);
	}
	void IModule.OnUpdate()
	{
		// 更新窗口
		int count = _stack.Count;
		for (int i = 0; i < _stack.Count; i++)
		{
			if (_stack.Count != count)
				break;
			UIWindow window = _stack[i];
			if (window.IsPrepare && window.IsOpen)
				window.InternalUpdate();
		}
	}
	void IModule.OnGUI()
	{
	}

	/// <summary>
	/// UI相机
	/// </summary>
	public Camera UICamera
	{
		get
		{
			return _root.UICamera;
		}	
	}

	/// <summary>
	/// 桌面对象
	/// </summary>
	public GameObject UIDesktop
	{
		get
		{
			return _root.UIDesktop;
		}
	}

	/// <summary>
	/// 创建UIRoot
	/// </summary>
	public IEnumerator CreateUIRoot(string location)
	{
		_root.LoadAsync(location);
		yield return _root;
	}

	/// <summary>
	/// 预加载窗口
	/// </summary>
	public UIWindow PreloadWindow(EWindowType type)
	{
		// 如果窗口已经存在
		if (IsContains(type))
			return null;

		GameLogger.Log($"Preload window {type}");
		UIWindow window = _creater.CreateInstance(type);
		Push(window);
		window.InternalClose();
		window.InternalLoad(OnWindowPrepare);	
		return window;
	}

	/// <summary>
	/// 是否有窗口正在加载
	/// </summary>
	public bool IsLoadingWindow()
	{
		for (int i = 0; i < _stack.Count; i++)
		{
			UIWindow window = _stack[i];
			if (window.IsPrepare == false)
				return true;
		}
		return false;
	}

	/// <summary>
	/// 打开窗口
	/// </summary>
	/// <param name="type">窗口类型</param>
	/// <param name="userData">用户数据</param>
	public void OpenWindow(EWindowType type, System.Object userData = null)
	{
		UIWindow window;

		// 如果窗口已经存在
		if (IsContains(type))
		{
			window = GetWindow(type);
			Pop(window); //弹出旧窗口
		}
		else
		{
			// 创建窗口类
			window = _creater.CreateInstance(type);
		}

		Push(window);
		window.InternalOpen(userData);
		window.InternalLoad(OnWindowPrepare);
	}

	/// <summary>
	/// 关闭窗口
	/// </summary>
	/// <param name="type">窗口类型</param>
	public void CloseWindow(EWindowType type)
	{
		UIWindow window = GetWindow(type);
		if (window != null)
		{
			if (window.DontDestroy)
			{
				window.InternalClose();
			}
			else
			{
				window.InternalDestroy();
				Pop(window);
				OnSortWindowDepth(window.WindowLayer);
				OnSetWindowVisible(window.WindowLayer);
			}
		}
	}

	/// <summary>
	/// 关闭所有窗口（常驻窗口除外）
	/// </summary>
	public void CloseAll()
	{
		List<UIWindow> tempList = new List<UIWindow>();
		for (int i = 0; i < _stack.Count; i++)
		{
			UIWindow window = _stack[i];
			if (window.DontDestroy == false)
				tempList.Add(window);
		}

		for (int i = 0; i < tempList.Count; i++)
		{
			UIWindow window = tempList[i];
			CloseWindow(window.WindowType);
		}
	}

	// 准备完毕
	private void OnWindowPrepare(UIWindow window)
	{
		window.OnRefresh();
		OnSortWindowDepth(window.WindowLayer);
		OnSetWindowVisible(window.WindowLayer);
	}
	// 排序深度
	private void OnSortWindowDepth(EWindowLayer layer)
	{
		int depth = (int)layer;
		for (int i = 0; i < _stack.Count; i++)
		{
			if (_stack[i].WindowLayer == layer)
			{
				_stack[i].Depth = depth;
				depth += 100; //注意：每次递增100深度
			}
		}
	}
	// 刷新可见性
	private void OnSetWindowVisible(EWindowLayer layer)
	{
		bool isHideNext = false;
		for (int i = _stack.Count - 1; i >= 0; i--)
		{
			UIWindow window = _stack[i];
			if (window.WindowLayer == layer)
			{
				if (isHideNext == false)
				{
					window.IsVisible = true;
					if (window.IsPrepare && window.IsOpen && window.FullScreen)
						isHideNext = true;
				}
				else
				{
					window.IsVisible = false;
				}
			}
		}
	}

	private UIWindow GetWindow(EWindowType type)
	{
		for (int i = 0; i < _stack.Count; i++)
		{
			UIWindow window = _stack[i];
			if (window.WindowType == type)
				return window;
		}
		return null;
	}
	private bool IsTopWindow(EWindowType type, EWindowLayer layer)
	{
		UIWindow lastOne = null;
		for (int i = 0; i < _stack.Count; i++)
		{
			if (_stack[i].WindowLayer == layer)
				lastOne = _stack[i];
		}

		if (lastOne == null)
			return false;

		return lastOne.WindowType == type;
	}
	private bool IsContains(EWindowType type)
	{
		for (int i = 0; i < _stack.Count; i++)
		{
			UIWindow window = _stack[i];
			if (window.WindowType == type)
				return true;
		}
		return false;
	}
	private void Push(UIWindow window)
	{
		// 如果已经存在
		if (IsContains(window.WindowType))
			throw new System.Exception($"Window {window.WindowType} is exist.");

		// 获取插入到所属层级的位置
		int insertIndex = -1;
		for (int i = 0; i < _stack.Count; i++)
		{
			if (window.WindowLayer == _stack[i].WindowLayer)
				insertIndex = i + 1;
		}

		// 如果没有所属层级，找到相邻层级
		if (insertIndex == -1)
		{
			for (int i = 0; i < _stack.Count; i++)
			{
				if (window.WindowLayer > _stack[i].WindowLayer)
					insertIndex = i + 1;
			}
		}

		// 如果是空栈或没有找到插入位置
		if (insertIndex == -1)
		{
			insertIndex = 0;
		}

		// 最后插入到堆栈
		_stack.Insert(insertIndex, window);
	}
	private void Pop(UIWindow window)
	{
		// 从堆栈里移除
		_stack.Remove(window);
	}
}