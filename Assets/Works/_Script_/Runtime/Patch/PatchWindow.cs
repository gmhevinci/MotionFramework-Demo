using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Patch;
using MotionFramework.Resource;
using MotionFramework.Event;

public class PatchWindow : ModuleSingleton<PatchWindow>, IMotionModule
{
	private AssetReference _assetRef;
	private System.Action _clickYes;
	private System.Action _clickNo;
	
	// UGUI相关
	private GameObject _uiRoot;
	private UIManifest _manifest;
	private Slider _slider;
	private Text _tips;
	private GameObject _messageBoxObj;
	private Text _messageBoxContent;


	void IMotionModule.OnCreate(object createParam)
	{
		_assetRef = new AssetReference("UIPanel/PatchWindow");
		_assetRef.LoadAssetAsync<GameObject>().Completed += Handle_Completed;
	}
	void IMotionModule.OnStart()
	{
	}
	void IMotionModule.OnUpdate()
	{
	}
	void IMotionModule.OnGUI()
	{
	}

	private void Handle_Completed(AssetOperationHandle obj)
	{
		if (obj.AssetObject == null)
			return;

		_uiRoot = obj.InstantiateObject;
		_manifest = _uiRoot.GetComponent<UIManifest>();
		OnWindowCreate();
	}
	private void OnWindowCreate()
	{
		_slider = _manifest.GetUIComponent<Slider>("PatchWindow/UIWindow/Slider");
		_tips = _manifest.GetUIComponent<Text>("PatchWindow/UIWindow/Slider/txt_tips");
		_messageBoxObj = _manifest.GetUIElement("PatchWindow/UIWindow/MessgeBox").gameObject;
		_messageBoxObj.SetActive(false);
		_messageBoxContent = _manifest.GetUIComponent<Text>("PatchWindow/UIWindow/MessgeBox/txt_content");
		_manifest.GetUIComponent<Button>("PatchWindow/UIWindow/MessgeBox/btn_yes").onClick.AddListener(OnClickMessageBoxOK);

		EventManager.Instance.AddListener(EPatchEventMessageTag.PatchSystemDispatchEvents.ToString(), OnHandleEvent);

		SendOperationEvent(EPatchOperation.BeginingRequestGameVersion);
	}
	private void OnWindowDestroy()
	{
		EventManager.Instance.RemoveListener(EPatchEventMessageTag.PatchSystemDispatchEvents.ToString(), OnHandleEvent);
	}

	/// <summary>
	/// 关闭
	/// </summary>
	public void Shutdown()
	{
		OnWindowDestroy();

		if (_uiRoot != null)
		{
			GameObject.Destroy(_uiRoot);
			_uiRoot = null;
		}

		if (_assetRef != null)
		{
			_assetRef.Release();
			_assetRef = null;
		}
	}

	/// <summary>
	/// 接收事件
	/// </summary>
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;
			if (message.CurrentStates == EPatchStates.None)
				_tips.text = "正在准备游戏世界......";
			else if (message.CurrentStates == EPatchStates.RequestGameVersion)
				_tips.text = "正在请求最新游戏版本";
			else if(message.CurrentStates == EPatchStates.ParseWebPatchManifest)
				_tips.text = "正在分析新清单";
			else if(message.CurrentStates == EPatchStates.GetDonwloadList)
				_tips.text = "正在准备下载列表";
			else if(message.CurrentStates == EPatchStates.DownloadWebFiles)
				_tips.text = "正在下载更新文件";
			else if(message.CurrentStates == EPatchStates.DownloadWebPatchManifest)
				_tips.text = "正在替换最新的清单";
			else if(message.CurrentStates == EPatchStates.DownloadOver)
				_tips.text = "欢迎来到游戏世界";
		}

		else if (msg is PatchEventMessageDefine.FoundForceInstallAPP)
		{
			var message = msg as PatchEventMessageDefine.FoundForceInstallAPP;
			System.Action callback = () => 
			{
				Application.OpenURL(message.InstallURL);
			};
			ShowMessageBox($"发现强制更新安装包 : {message.NewVersion}，请重新下载游戏", callback);
		}

		else if (msg is PatchEventMessageDefine.FoundUpdateFiles)
		{
			var message = msg as PatchEventMessageDefine.FoundUpdateFiles;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.BeginingDownloadWebFiles);
			};
			string totalSize = (message.TotalSizeKB / 1024f).ToString("f3");
			ShowMessageBox($"发现新版本需要更新 : 一共{message.TotalCount}个文件，总大小{totalSize}MB", callback);
		}

		else if (msg is PatchEventMessageDefine.DownloadFilesProgress)
		{
			var message = msg as PatchEventMessageDefine.DownloadFilesProgress;
			_slider.value = message.CurrentDownloadCount / message.TotalDownloadCount;
			string currentSize = (message.CurrentDownloadSizeKB / 1024f).ToString("f3");
			string totalSize = (message.TotalDownloadSizeKB / 1024f).ToString("f3");
			_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {currentSize}MB/{totalSize}MB";
		}

		else if (msg is PatchEventMessageDefine.GameVersionRequestFailed)
		{
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryRequestGameVersion);
			};
			ShowMessageBox($"请求最新版本失败，请检查网络状况", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileDownloadFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件下载失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileMD5VerifyFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileMD5VerifyFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件验证失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebPatchManifestDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileMD5VerifyFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebPatchManifest);
			};
			ShowMessageBox($"清单下载失败 : {message.Name}", callback);
		}

		else
		{
			throw new System.NotImplementedException($"{msg.GetType()}");
		}
	}

	// 消息框相关
	private void OnClickMessageBoxOK()
	{
		_clickYes?.Invoke();
		HideMessageBox();
	}
	private void ShowMessageBox(string content, System.Action clickYes)
	{
		_clickYes = clickYes;
		_messageBoxObj.SetActive(true);
		_messageBoxContent.text = content;
	}
	private void HideMessageBox()
	{
		_messageBoxObj.SetActive(false);
	}

	// 事件相关
	private void SendOperationEvent(EPatchOperation operation)
	{
		PatchEventMessageDefine.OperationEvent msg = new PatchEventMessageDefine.OperationEvent();
		msg.operation = operation;
		EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchWindowDispatchEvents.ToString(), msg);
	}
}