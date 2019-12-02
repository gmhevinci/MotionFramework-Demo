using System;
using System.Collections.Generic;
using UnityEngine;
using MotionEngine;
using MotionEngine.Res;
using MotionEngine.Debug;
using MotionEngine.Patch;
using MotionGame;

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

		// 注册MotionEngine日志系统
		LogSystem.RegisterCallback(HandleMotionEngineLog);

		// 设置协程脚本
		Engine.Instance.InitCoroutineBehaviour(this);

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
		Engine.Instance.Update();
	}
	void LateUpdate()
	{
		Engine.Instance.LateUpdate();
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
		AssetSystem.AssetRootPath = PatchDefine.StrMyPackRootPath;

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
		Engine.Instance.RegisterModule(EventManager.Instance);
		Engine.Instance.RegisterModule(ResManager.Instance);
		Engine.Instance.RegisterModule(CfgManager.Instance);
		Engine.Instance.RegisterModule(AudioManager.Instance);
		Engine.Instance.RegisterModule(NetManager.Instance);
		Engine.Instance.RegisterModule(ILRManager.Instance);
		Engine.Instance.RegisterModule(GameTest.Instance);
	}
}