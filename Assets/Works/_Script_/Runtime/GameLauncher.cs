using System;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.Debug;
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
	private IEngine _engine;

	[Tooltip("是否启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("是否跳过CDN服务器")]
	public bool SkipCDN = true;

	[Tooltip("资源系统的加载模式")]
	public EAssetSystemMode AssetSystemMode = EAssetSystemMode.ResourcesMode;

	void Awake()
	{
		_engine = AppEngine.Instance;

		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		LogHelper.RegisterCallback(HandleMotionEngineLog);

		// 设置协程脚本
		AppEngine.Instance.InitCoroutineBehaviour(this);

		// 初始化调试控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DebugConsole.Initialize();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		RegisterAndRunAllGameModule();
	}
	void Update()
	{
		_engine.OnUpdate();
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
			DebugConsole.DrawGUI();
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

	// 日志回调
	private void HandleMotionEngineLog(ELogType logType, string log)
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
		// 模块创建参数
		var patchCreateParam = new PatchManager.CreateParameters();
		patchCreateParam.StrCDNServerIP = "127.0.0.1/CDN";
		patchCreateParam.StrWebServerIP = "127.0.0.1/WEB";
		patchCreateParam.SkipCDN = SkipCDN;

		// 模块创建参数
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.AssetRootPath = GameDefine.StrAssetRootPath;
		resourceCreateParam.AssetSystemMode = AssetSystemMode;

		// 模块创建参数
		var configCreateParam = new ConfigManager.CreateParameters();
		configCreateParam.BaseFolderPath = "Config";
		
		// 创建游戏模块
		AppEngine.Instance.CreateModule<EventManager>();
		AppEngine.Instance.CreateModule<NetworkManager>();
		AppEngine.Instance.CreateModule<ResourceManager>(resourceCreateParam);
		AppEngine.Instance.CreateModule<ConfigManager>(configCreateParam);
		AppEngine.Instance.CreateModule<AudioManager>();
		AppEngine.Instance.CreateModule<SceneManager>();
		AppEngine.Instance.CreateModule<PoolManager>();

		// 最后创建补丁管理器
		AppEngine.Instance.CreateModule<PatchManager>(patchCreateParam);

		// 设置AssetBundle服务器接口
		AssetSystem.Instance.BundleServices = PatchManager.Instance;

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