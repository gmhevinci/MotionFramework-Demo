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
	private IMotionEngine _motionEngine;

	[Tooltip("是否启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("是否跳过CDN服务器")]
	public bool SkipCDN = true;

	[Tooltip("资源系统的加载模式")]
	public EAssetSystemMode AssetSystemMode = EAssetSystemMode.ResourcesMode;

	void Awake()
	{
		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		AppLog.RegisterCallback(HandleMotionFrameworkLog);

		// 初始化框架
		_motionEngine = AppEngine.Instance;
		_motionEngine.Initialize(this);

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			AppConsole.Initialize();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		CreateGameModules();
	}
	void Update()
	{
		_motionEngine.OnUpdate();
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
	/// 监听框架日志
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

	private void CreateGameModules()
	{
		// 创建事件管理器
		AppEngine.Instance.CreateModule<EventManager>();

		// 创建网络管理器
		AppEngine.Instance.CreateModule<NetworkManager>();

		// 创建补丁管理器
		IBundleServices bundleServices = null;
		if (AssetSystemMode == EAssetSystemMode.BundleMode)
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
			bundleServices = AppEngine.Instance.CreateModule<PatchManager>(patchCreateParam);

			EventManager.Instance.AddListener(EPatchEventMessageTag.PatchSystemDispatchEvents.ToString(), OnHandleEvent);
			EventManager.Instance.AddListener(EPatchEventMessageTag.PatchWindowDispatchEvents.ToString(), OnHandleEvent);
		}

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.AssetRootPath = GameDefine.AssetRootPath;
		resourceCreateParam.AssetSystemMode = AssetSystemMode;
		resourceCreateParam.BundleServices = bundleServices;
		AppEngine.Instance.CreateModule<ResourceManager>(resourceCreateParam);

		// 创建音频管理器
		AppEngine.Instance.CreateModule<AudioManager>();

		// 创建场景管理器
		AppEngine.Instance.CreateModule<SceneManager>();

		// 创建对象池管理器
		AppEngine.Instance.CreateModule<PoolManager>();

		// 直接进入游戏
		if (bundleServices == null)
			CreateILRManager();
	}
	private void CreateILRManager()
	{
		ILRManager.CreateParameters createParameters = new ILRManager.CreateParameters();
		createParameters.IsEnableILRuntime = EnableILRuntime;
		AppEngine.Instance.CreateModule<ILRManager>(createParameters);
	}
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;

			// 初始化结束
			if (message.CurrentStates == EPatchStates.InitiationOver)
			{
				if (SkipCDN)
					CreateILRManager();
				else
					AppEngine.Instance.CreateModule<PatchWindow>();
			}

			// 补丁下载完毕
			// 注意：在补丁下载结束之后，一定要强制释放资源管理器里所有的资源，还有重新载入Unity清单。
			if (message.CurrentStates == EPatchStates.DownloadOver)
			{
				PatchWindow.Instance.Shutdown();
				ResourceManager.Instance.ForceReleaseAll();
				PatchManager.Instance.ReloadUnityManifest();
				CreateILRManager();
			}
		}

		if(msg is PatchEventMessageDefine.OperationEvent)
		{
			PatchManager.Instance.HandleEventMessage(msg);
		}
	}
}