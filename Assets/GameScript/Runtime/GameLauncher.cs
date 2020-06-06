using System;
using System.Collections;
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
	[Tooltip("在编辑器下模拟运行")]
	public bool SimulationOnEditor = true;

	void Awake()
	{
#if !UNITY_EDITOR
		SimulationOnEditor = false;
#endif

		// 初始化应用
		InitAppliaction();

		// 初始化框架
		bool showConsole = Application.isEditor || Debug.isDebugBuild;
		MotionEngine.Initialize(this, showConsole, HandleMotionFrameworkLog);
	}
	void Start()
	{
		// 创建游戏模块
		CreateGameModules();
	}
	void Update()
	{
		// 更新框架
		MotionEngine.Update();
	}
	void OnGUI()
	{
		// 绘制控制台
		MotionEngine.DrawConsole();
	}

	/// <summary>
	/// 初始化应用
	/// </summary>
	private void InitAppliaction()
	{
		Application.runInBackground = true;
		Application.backgroundLoadingPriority = ThreadPriority.High;

		// 设置最大帧数
		Application.targetFrameRate = 60;

		// 屏幕不休眠
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	/// <summary>
	/// 监听框架日志
	/// </summary>
	private void HandleMotionFrameworkLog(ELogLevel logLevel, string log)
	{
		if (logLevel == ELogLevel.Log)
		{
			UnityEngine.Debug.Log(log);
		}
		else if (logLevel == ELogLevel.Error)
		{
			UnityEngine.Debug.LogError(log);
		}
		else if (logLevel == ELogLevel.Warning)
		{
			UnityEngine.Debug.LogWarning(log);
		}
		else if (logLevel == ELogLevel.Exception)
		{
			UnityEngine.Debug.LogError(log);
		}
		else
		{
			throw new NotImplementedException($"{logLevel}");
		}
	}

	/// <summary>
	/// 创建游戏模块
	/// </summary>
	private void CreateGameModules()
	{
		// 创建事件管理器
		MotionEngine.CreateModule<EventManager>();

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
		resourceCreateParam.SimulationOnEditor = SimulationOnEditor;
		resourceCreateParam.BundleServices = new LocalBundleServices();
		resourceCreateParam.DecryptServices = null;
		resourceCreateParam.AutoReleaseInterval = 10f;
		MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);

		// 创建音频管理器
		MotionEngine.CreateModule<AudioManager>();

		// 创建场景管理器
		MotionEngine.CreateModule<SceneManager>();

		// 创建对象池管理器
		MotionEngine.CreateModule<GameObjectPoolManager>();

		// 最后创建游戏业务逻辑模块
		MotionEngine.CreateModule<DataManager>();
		MotionEngine.CreateModule<UIManager>();
		MotionEngine.CreateModule<FsmManager>();

		// 开始游戏逻辑
		FsmManager.Instance.StartGame();
	}
}