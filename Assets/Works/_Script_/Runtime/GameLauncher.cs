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

public class GameLauncher : MonoBehaviour
{
	public static GameLauncher Instance = null;

	[Tooltip("是否启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("资源系统的加载模式")]
	public EAssetLoadMode AssetLoadMode = EAssetLoadMode.ResourceMode;

	void Awake()
	{
		Instance = this;

		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		LogSystem.RegisterCallback(HandleMotionEngineLog);

		// 设置协程脚本
		AppEngine.Instance.InitCoroutineBehaviour(this);

		// 初始化调试控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DebugConsole.Init();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		RegisterAndRunAllGameModule();
	}
	void Update()
	{
		AppEngine.Instance.Update();
	}
	void LateUpdate()
	{
		AppEngine.Instance.LateUpdate();
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
		// 设置资源系统加载模式
		AssetSystem.AssetLoadMode = AssetLoadMode;

		// 设置资源系统根路径
		AssetSystem.AssetRootPath = GameDefine.StrAssetRootPath;

		// 设置Bundle接口
		if(AssetLoadMode == EAssetLoadMode.BundleMode)
		{
			PatchBundleMethod method = new PatchBundleMethod();
			method.LoadManifestFile();
			AssetSystem.BundleMethod = method;
		}

		// 设置ILRuntime开关
		ILRManager.Instance.EnableILRuntime = EnableILRuntime;

		// 注册所有游戏模块
		AppEngine.Instance.RegisterModule(EventManager.Instance);
		AppEngine.Instance.RegisterModule(ResourceManager.Instance);
		AppEngine.Instance.RegisterModule(ConfigManager.Instance);
		AppEngine.Instance.RegisterModule(AudioManager.Instance);
		AppEngine.Instance.RegisterModule(NetworkManager.Instance);
		AppEngine.Instance.RegisterModule(ILRManager.Instance);
		AppEngine.Instance.RegisterModule(GameTest.Instance);
	}
}