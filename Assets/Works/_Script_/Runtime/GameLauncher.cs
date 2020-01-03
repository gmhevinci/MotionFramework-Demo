using System;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.Console;
using MotionFramework.Resource;
using MotionFramework.Event;
using MotionFramework.Config;
using MotionFramework.Audio;
using MotionFramework.Network;
using MotionFramework.Patch;
using MotionFramework.Scene;
using MotionFramework.Pool;

public class GameLauncher : MonoBehaviour
{
	private IMotionEngine _motionEngine;

	[Tooltip("是否启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("是否跳过CDN服务器")]
	public bool SkipCDN = true;

	[Tooltip("资源系统的加载模式")]
	public EAssetSystemMode AssetSystemMode = EAssetSystemMode.ResourcesMode;

	void Awake()
	{
		_motionEngine = AppEngine.Instance;
		_motionEngine.Initialize(this);

		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		AppLog.RegisterCallback(HandleMotionFrameworkLog);

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			AppConsole.Initialize();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		RegisterAndRunAllGameModule();
	}
	void Update()
	{
		_motionEngine.OnUpdate();
	}
	void OnApplicationQuit()
	{
	}
	void OnApplicationFocus(bool focusStatus)
	{
	}
	void OnApplicationPause(bool pauseStatus)
	{
	}
	void OnGUI()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			AppConsole.DrawGUI();
	}

	private void InitAppliaction()
	{
		UnityEngine.Debug.Log($"Game run platform : {Application.platform}");
		UnityEngine.Debug.Log($"Version of the runtime : {Application.unityVersion}");

		Application.runInBackground = true;
		Application.backgroundLoadingPriority = ThreadPriority.High;

		// 设置最大帧数
		Application.targetFrameRate = 60;

		// 屏幕不休眠
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	/// <summary>
	/// 框架日志监听
	/// </summary>
	private void HandleMotionFrameworkLog(ELogType logType, string log)
	{
		if (logType == ELogType.Log)
		{
			UnityEngine.Debug.Log(log);
		}
		else if (logType == ELogType.Error)
		{
			UnityEngine.Debug.LogError(log);
		}
		else if (logType == ELogType.Warning)
		{
			UnityEngine.Debug.LogWarning(log);
		}
		else if (logType == ELogType.Exception)
		{
			UnityEngine.Debug.LogError(log);
		}
		else
		{
			throw new NotImplementedException($"{logType}");
		}
	}

	/// <summary>
	/// 注册所有游戏模块
	/// </summary>
	private void RegisterAndRunAllGameModule()
	{
		// 创建事件管理器
		AppEngine.Instance.CreateModule<EventManager>(201);

		// 创建网络管理器
		AppEngine.Instance.CreateModule<NetworkManager>(200);

		// 创建补丁管理器
		var patchCreateParam = new PatchManager.CreateParameters();
		patchCreateParam.CDNServerIP = "127.0.0.1/CDN";
		patchCreateParam.WebServerIP = "127.0.0.1/WEB";
		patchCreateParam.SkipCDN = SkipCDN;
		AppEngine.Instance.CreateModule<PatchManager>(patchCreateParam);

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.AssetRootPath = GameDefine.StrAssetRootPath;
		resourceCreateParam.AssetSystemMode = AssetSystemMode;
		resourceCreateParam.BundleServices = PatchManager.Instance;
		AppEngine.Instance.CreateModule<ResourceManager>(resourceCreateParam, 104);

		// 创建配表管理器
		var configCreateParam = new ConfigManager.CreateParameters();
		configCreateParam.BaseFolderPath = "Config";
		AppEngine.Instance.CreateModule<ConfigManager>(configCreateParam, 103);

		// 创建音频管理器
		AppEngine.Instance.CreateModule<AudioManager>(102);

		// 创建场景管理器
		AppEngine.Instance.CreateModule<SceneManager>(101);

		// 创建对象池管理器
		AppEngine.Instance.CreateModule<PoolManager>(100);

		// 注册补丁更新结束事件
		EventManager.Instance.AddListener(EPatchEventMessageTag.PatchManagerEvent.ToString(), OnHandleEvent);
	}

	private void OnHandleEvent(IEventMessage msg)
	{
		if(msg is PatchEventMessageDefine.PatchOver)
		{
			AppEngine.Instance.CreateModule<ILRManager>(new ILRManager.CreateParameters() { IsEnableILRuntime = EnableILRuntime });
		}
	}
}