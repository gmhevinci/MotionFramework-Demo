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
	[Tooltip("启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("跳过CDN服务器")]
	public bool SkipCDNServer = true;

	[Tooltip("在编辑器下模拟运行")]
	public bool SimulationOnEditor = true;

	void Awake()
	{
		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		MotionLog.RegisterCallback(HandleMotionFrameworkLog);

		// 初始化框架
		MotionEngine.Initialize(this);

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Initialize();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		CreateGameModules();
	}
	void Update()
	{
		MotionEngine.Update();
	}
	void OnGUI()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.DrawGUI();
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

		// 创建网络管理器
		var networkCreateParam = new NetworkManager.CreateParameters();
		networkCreateParam.PackageCoderType = typeof(ProtoPackageCoder);
		MotionEngine.CreateModule<NetworkManager>(networkCreateParam);

		// 创建补丁管理器
		IBundleServices bundleServices = null;
		if (SimulationOnEditor == false)
		{
			var patchCreateParam = new PatchManager.CreateParameters();
			patchCreateParam.ServerID = PlayerPrefs.GetInt("SERVER_ID_KEY", 0);
			patchCreateParam.ChannelID = 0;
			patchCreateParam.DeviceID = 0;
			patchCreateParam.TestFlag = PlayerPrefs.GetInt("TEST_FLAG_KEY", 0);

			patchCreateParam.WebServers = new Dictionary<RuntimePlatform, string>();
			patchCreateParam.WebServers.Add(RuntimePlatform.Android, "127.0.0.1/WEB/Android/GameVersion.php");
			patchCreateParam.WebServers.Add(RuntimePlatform.IPhonePlayer, "127.0.0.1/WEB/Iphone/GameVersion.php");

			patchCreateParam.CDNServers = new Dictionary<RuntimePlatform, string>();
			patchCreateParam.CDNServers.Add(RuntimePlatform.Android, "127.0.0.1/CDN/Android");
			patchCreateParam.CDNServers.Add(RuntimePlatform.IPhonePlayer, "127.0.0.1/CDN/Iphone");

			patchCreateParam.DefaultWebServerIP = "127.0.0.1/WEB/PC/GameVersion.php";
			patchCreateParam.DefaultCDNServerIP = "127.0.0.1/CDN/PC";

			var variantRule1 = new PatchManager.CreateParameters.VariantRule();
			variantRule1.VariantGroup = new List<string>() { "CN", "EN", "KR" };
			variantRule1.TargetVariant = "EN";
			patchCreateParam.VariantRules = new List<PatchManager.CreateParameters.VariantRule>() { variantRule1 };
			bundleServices = MotionEngine.CreateModule<PatchManager>(patchCreateParam);

			EventManager.Instance.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEvent);
			EventManager.Instance.AddListener<PatchEventMessageDefine.OperationEvent>(OnHandleEvent);
		}

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
		resourceCreateParam.SimulationOnEditor = SimulationOnEditor;
		resourceCreateParam.BundleServices = bundleServices;
		resourceCreateParam.DecryptServices = null;
		resourceCreateParam.AutoReleaseInterval = 10f;
		MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);

		// 创建音频管理器
		MotionEngine.CreateModule<AudioManager>();

		// 创建场景管理器
		MotionEngine.CreateModule<SceneManager>();

		// 创建对象池管理器
		MotionEngine.CreateModule<GameObjectPoolManager>();

		// 创建ILRuntime管理器
		ILRManager.CreateParameters createParameters = new ILRManager.CreateParameters();
		createParameters.IsEnableILRuntime = EnableILRuntime;
		MotionEngine.CreateModule<ILRManager>(createParameters);

		// 直接进入游戏
		if (bundleServices == null)
			ILRManager.Instance.StartGame();
		else
			PatchManager.Instance.Run();
	}
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;

			// 初始化结束
			if (message.CurrentStates == EPatchStates.InitiationOver)
			{
				if (SkipCDNServer)
					ILRManager.Instance.StartGame();
				else
					MotionEngine.CreateModule<PatchWindow>();
			}

			// 补丁下载完毕
			// 注意：在补丁下载结束之后，一定要强制释放资源管理器里所有的资源，还有重新载入Unity清单。
			if (message.CurrentStates == EPatchStates.DownloadOver)
			{
				PatchWindow.Instance.Shutdown();
				ResourceManager.Instance.ForceReleaseAll();
				PatchManager.Instance.ReloadUnityManifest();
				ILRManager.Instance.StartGame();
			}
		}

		if (msg is PatchEventMessageDefine.OperationEvent)
		{
			PatchManager.Instance.HandleEventMessage(msg);
		}
	}
}