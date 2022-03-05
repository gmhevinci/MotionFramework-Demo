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
using MotionFramework.Scene;
using MotionFramework.Pool;
using MotionFramework.Window;
using MotionFramework.Tween;
using YooAsset;

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

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Initialize();

		// 初始化框架
		MotionEngine.Initialize(this, HandleMotionFrameworkLog);
	}
	void Start()
	{
		// 创建游戏模块
		StartCoroutine(CreateGameModules());
	}
	void Update()
	{
		// 更新框架
		MotionEngine.Update();
	}
	void OnGUI()
	{
		// 绘制控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Draw();
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
	private IEnumerator CreateGameModules()
	{
		// 创建事件管理器
		MotionEngine.CreateModule<EventManager>();

		// 创建补间管理器
		MotionEngine.CreateModule<TweenManager>();

		// 创建资源管理器
		if(SimulationOnEditor)
		{
			var resourceCreateParam = new YooAssets.EditorPlayModeParameters();
			resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
			MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);
			var operation = ResourceManager.Instance.InitializeAsync();
			yield return operation;
		}
		else
		{
			var resourceCreateParam = new YooAssets.OfflinePlayModeParameters();
			resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
			resourceCreateParam.AutoReleaseInterval = 5f;
			MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);
			var operation = ResourceManager.Instance.InitializeAsync();
			yield return operation;
		}

		// 创建音频管理器
		MotionEngine.CreateModule<AudioManager>();

		// 创建场景管理器
		MotionEngine.CreateModule<SceneManager>();

		// 创建对象池管理器
		var poolCreateParam = new GameObjectPoolManager.CreateParameters();
		poolCreateParam.DefaultDestroyTime = 5f;
		MotionEngine.CreateModule<GameObjectPoolManager>(poolCreateParam);

		// 最后创建游戏业务逻辑模块
		MotionEngine.CreateModule<DataManager>();
		MotionEngine.CreateModule<WindowManager>();
		MotionEngine.CreateModule<FsmManager>();

		// 开始游戏逻辑
		FsmManager.Instance.StartGame();
	}
}