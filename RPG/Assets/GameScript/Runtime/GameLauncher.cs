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
	public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

	private bool _gameStart = false;

	void Awake()
	{
#if !UNITY_EDITOR
		PlayMode = EPlayMode.OfflinePlayMode;
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
	void FixedUpdate()
	{
		if (_gameStart)
			ILRManager.Instance.FixedUpdate();
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
		string locationRoot = "Assets/GameRes/";
		if (PlayMode == EPlayMode.EditorSimulateMode)
		{
			var resourceCreateParam = new EditorSimulateModeParameters();
			resourceCreateParam.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
			MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);
			var operation = ResourceManager.Instance.InitializeAsync(locationRoot);
			yield return operation;
		}
		else if (PlayMode == EPlayMode.OfflinePlayMode)
		{
			var resourceCreateParam = new OfflinePlayModeParameters();
			MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);
			var operation = ResourceManager.Instance.InitializeAsync(locationRoot);
			yield return operation;
		}
		else
		{
			throw new System.NotImplementedException();
		}

		// 创建对象池管理器
		var poolCreateParam = new GameObjectPoolManager.CreateParameters();
		poolCreateParam.DefaultDestroyTime = 5f;
		MotionEngine.CreateModule<GameObjectPoolManager>(poolCreateParam);

		// 创建音频管理器
		MotionEngine.CreateModule<AudioManager>();

		// 创建配置表管理器
		MotionEngine.CreateModule<ConfigManager>();

		// 创建场景管理器
		MotionEngine.CreateModule<SceneManager>();

		// 创建窗口管理器
		MotionEngine.CreateModule<WindowManager>();

		// 创建ILRuntime管理器
		MotionEngine.CreateModule<ILRManager>();

		// 注册反射服务接口
		ConfigManager.Instance.ActivatorServices = ILRManager.Instance;
		WindowManager.Instance.ActivatorServices = ILRManager.Instance;

		// 开始游戏
		_gameStart = true;
		ILRManager.Instance.StartGame();
	}
}