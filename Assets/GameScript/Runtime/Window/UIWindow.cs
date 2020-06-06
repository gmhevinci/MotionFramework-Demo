using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Resource;
using MotionFramework.Event;

public abstract class UIWindow : IEnumerator
{
	public const int HIDE_LAYER = 2; // Ignore Raycast
	public const int SHOW_LAYER = 5; // UI

	protected readonly EventGroup _eventGroup = new EventGroup();
	private AssetReference _assetRef;
	private AssetOperationHandle _handle;
	private UIManifest _manifest;
	private Canvas _canvas;
	private Canvas[] _childCanvas;
	private GraphicRaycaster _raycaster;
	private GraphicRaycaster[] _childRaycaster;

	/// <summary>
	/// 窗口层级
	/// </summary>
	public EWindowLayer WindowLayer { set; get; }

	/// <summary>
	/// 窗口类型
	/// </summary>
	public EWindowType WindowType { set; get; }

	/// <summary>
	/// 是否为常驻窗口
	/// </summary>
	public bool DontDestroy { protected set; get; } = false;

	/// <summary>
	/// 是否是全屏窗口
	/// </summary>
	public bool FullScreen { protected set; get; } = false;

	/// <summary>
	/// 窗口是否准备完毕
	/// </summary>
	public bool IsPrepare { private set; get; } = false;

	/// <summary>
	/// 窗口是否打开
	/// </summary>
	public bool IsOpen { private set; get; } = true;

	/// <summary>
	/// GameObject对象
	/// </summary>
	public GameObject Go { private set; get; }

	/// <summary>
	/// 自定义数据
	/// </summary>
	public System.Object UserData { private set; get; }

	/// <summary>
	/// 窗口深度值
	/// </summary>
	public int Depth
	{
		get
		{
			if (_canvas != null)
				return _canvas.sortingOrder;
			else
				return 0;
		}

		set
		{
			if (_canvas != null)
			{
				// 设置父类
				_canvas.sortingOrder = value;

				// 设置子类
				int depth = value;
				for (int i = 0; i < _childCanvas.Length; i++)
				{
					depth += 5;
					var canvas = _childCanvas[i];
					canvas.sortingOrder = depth;
				}

				// 虚函数
				OnSortDepth();
			}
		}
	}

	/// <summary>
	/// 窗口是否可见
	/// </summary>
	public bool IsVisible
	{
		get
		{
			if (_canvas != null && _raycaster != null)
				return _canvas.gameObject.layer == SHOW_LAYER;
			else
				return false;
		}
		set
		{
			if (_canvas != null && _raycaster != null)
			{
				// 显示设置
				int setLayer = value ? SHOW_LAYER : HIDE_LAYER;
				_canvas.gameObject.layer = setLayer;
				for (int i = 0; i < _childCanvas.Length; i++)
				{
					_childCanvas[i].gameObject.layer = setLayer;
				}

				// 交互设置
				_raycaster.enabled = value;
				for (int i = 0; i < _childRaycaster.Length; i++)
				{
					_childRaycaster[i].enabled = value;
				}

				// 虚函数
				OnSetVisible();
			}
		}
	}


	public void Init(EWindowType type, EWindowLayer layer)
	{
		WindowType = type;
		WindowLayer = layer;
	}
	public abstract void OnCreate();
	public abstract void OnDestroy();
	public abstract void OnRefresh();
	public abstract void OnUpdate();
	public virtual void OnSortDepth() { }
	public virtual void OnSetVisible() { }

	internal void InternalOpen(System.Object userData)
	{
		UserData = userData;

		IsOpen = true;
		if (Go != null && Go.activeSelf == false)
			Go.SetActive(true);
	}
	internal void InternalClose()
	{
		IsOpen = false;
		if (Go != null && Go.activeSelf)
			Go.SetActive(false);
	}
	internal void InternalLoad(System.Action<UIWindow> callback)
	{
		if (IsPrepare)
			callback?.Invoke(this);

		if (_assetRef == null)
		{
			_userCallback = callback;
			_assetRef = new AssetReference($"UIPanel/{WindowType}");
			_handle = _assetRef.LoadAssetAsync<GameObject>();
			_handle.Completed += Handle_Completed;
		}
	}
	internal void InternalDestroy()
	{
		// 虚函数
		if (IsPrepare)
		{
			IsPrepare = false;
			OnDestroy();
		}

		// 注销回调函数
		_userCallback = null;

		// 销毁面板对象
		if (Go != null)
		{
			GameObject.Destroy(Go);
			Go = null;
		}

		// 卸载面板资源
		if (_assetRef != null)
		{
			_assetRef.Release();
			_assetRef = null;
		}

		// 移除所有缓存的事件监听
		_eventGroup.RemoveAllListener();
	}
	internal void InternalUpdate()
	{
		OnUpdate();
	}
	private void Handle_Completed(AssetOperationHandle obj)
	{
		if (_handle.AssetObject == null)
			return;

		Go = _handle.InstantiateObject;

		// 设置父类
		GameObject uiDesktop = UIManager.Instance.UIDesktop;
		Go.transform.SetParent(uiDesktop.transform, false);

		// 获取组件
		_manifest = Go.GetComponent<UIManifest>();
		if (_manifest == null)
		{
			GameLogger.Error($"Not found {nameof(UIManifest)} in window {WindowType}");
			return;
		}

		// 获取组件
		_canvas = Go.GetComponent<Canvas>();
		if (_canvas == null)
		{
			GameLogger.Error($"Not found {nameof(Canvas)} in window {WindowType}");
			return;
		}
		_canvas.overrideSorting = true;

		// 获取组件
		_raycaster = Go.GetComponent<GraphicRaycaster>();
		if (_raycaster == null)
		{
			GameLogger.Error($"Not found {nameof(GraphicRaycaster)} in window {WindowType}");
			return;
		}

		// 获取组件
		_childCanvas = Go.GetComponentsInChildren<Canvas>(true);
		_childRaycaster = Go.GetComponentsInChildren<GraphicRaycaster>(true);

		// 虚函数
		if (IsPrepare == false)
		{
			IsPrepare = true;
			OnCreate();
		}

		// 最后设置是否激活
		Go.SetActive(IsOpen);

		// 通知UI管理器
		_userCallback?.Invoke(this);
	}

	#region UI组件相关
	/// <summary>
	/// 获取窗口里缓存的元素对象
	/// </summary>
	/// <param name="path">对象路径</param>
	protected Transform GetUIElement(string path)
	{
		if (_manifest == null)
			return null;
		return _manifest.GetUIElement(path);
	}

	/// <summary>
	/// 获取窗口里缓存的组件对象
	/// </summary>
	/// <typeparam name="T">组件类型</typeparam>
	/// <param name="path">组件路径</param>
	protected T GetUIComponent<T>(string path) where T : UnityEngine.Component
	{
		if (_manifest == null)
			return null;
		return _manifest.GetUIComponent<T>(path);
	}

	/// <summary>
	/// 获取窗口里缓存的组件对象
	/// </summary>
	/// <param name="path">组件路径</param>
	/// <param name="typeName">组件类型名称</param>
	protected UnityEngine.Component GetUIComponent(string path, string typeName)
	{
		if (_manifest == null)
			return null;
		return _manifest.GetUIComponent(path, typeName);
	}

	/// <summary>
	/// 监听按钮点击事件
	/// </summary>
	protected void AddButtonListener(string path, UnityEngine.Events.UnityAction call)
	{
		Button btn = GetUIComponent<Button>(path);
		if (btn != null)
			btn.onClick.AddListener(call);
	}
	#endregion

	#region 异步相关
	private System.Action<UIWindow> _userCallback;

	/// <summary>
	/// 完成委托
	/// </summary>
	public event System.Action<UIWindow> Completed
	{
		add
		{
			if (IsPrepare)
				value.Invoke(this);
			else
				_userCallback += value;
		}
		remove
		{
			_userCallback -= value;
		}
	}

	bool IEnumerator.MoveNext()
	{
		return !IsPrepare;
	}
	void IEnumerator.Reset()
	{
	}
	object IEnumerator.Current
	{
		get { return null; }
	}
	#endregion
}