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
	private EPatchStates _currentStates = EPatchStates.None;

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

	/// <summary>
	/// 销毁窗体
	/// </summary>
	public void DestroyWindow()
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
	/// 事件
	/// </summary>
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;
			_currentStates = message.CurrentStates;

			if (_currentStates == EPatchStates.None)
				_tips.text = "正在准备游戏世界......";

			if (_currentStates == EPatchStates.RequestGameVersion)
				_tips.text = "正在请求最新游戏版本";

			if (_currentStates == EPatchStates.ParseWebPatchManifest)
				_tips.text = "正在分析新清单";

			if (_currentStates == EPatchStates.GetDonwloadList)
				_tips.text = "正在准备下载列表";

			if (_currentStates == EPatchStates.DownloadWebFiles)
				_tips.text = "正在下载更新文件";

			if (_currentStates == EPatchStates.DownloadWebPatchManifest)
				_tips.text = "正在替换最新的清单";

			if (_currentStates == EPatchStates.DownloadOver)
				_tips.text = "欢迎来到游戏世界";
		}

		else if (msg is PatchEventMessageDefine.FoundNewAPP)
		{
			var message = msg as PatchEventMessageDefine.FoundNewAPP;
			ShowMessageBox($"发现新版本 : {message.NewVersion}");
		}

		else if (msg is PatchEventMessageDefine.FoundUpdateFiles)
		{
			var message = msg as PatchEventMessageDefine.FoundUpdateFiles;
			ShowMessageBox($"发现更新文件需要下载 : 一共{message.TotalCount}个文件，总大小{message.TotalSizeKB / 1024f}MB");
		}

		else if (msg is PatchEventMessageDefine.DownloadFilesProgress)
		{
			var message = msg as PatchEventMessageDefine.DownloadFilesProgress;
			_slider.value = message.CurrentDownloadCount / message.TotalDownloadCount;
			_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {message.CurrentDownloadSizeKB / 1024f}MB/{message.TotalDownloadSizeKB / 1024f}MB";
		}

		else if (msg is PatchEventMessageDefine.GameVersionRequestFailed)
		{
			ShowMessageBox($"请求最新游戏版本失败");
		}

		else if (msg is PatchEventMessageDefine.WebFileDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileDownloadFailed;
			ShowMessageBox($"更新文件下载失败 : {message.FilePath}");
		}

		else if (msg is PatchEventMessageDefine.WebFileMD5VerifyFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileMD5VerifyFailed;
			ShowMessageBox($"更新文件验证失败 : {message.FilePath}");
		}

		else if (msg is PatchEventMessageDefine.WebPatchManifestDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileMD5VerifyFailed;
			ShowMessageBox($"清单文件下载失败 : {message.FilePath}");
		}

		else
		{
			throw new System.NotImplementedException($"{msg.GetType()}");
		}
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
		_messageBoxContent = _manifest.GetUIComponent<Text>("PatchWindow/UIWindow/MessgeBox/txt_content");
		_manifest.GetUIComponent<Button>("PatchWindow/UIWindow/MessgeBox/btn_yes").onClick.AddListener(OnClickMessageBoxOK);

		EventManager.Instance.AddListener(EPatchEventMessageTag.PatchSystemDispatchEvents.ToString(), OnHandleEvent);
	}
	private void OnWindowDestroy()
	{
		EventManager.Instance.RemoveListener(EPatchEventMessageTag.PatchSystemDispatchEvents.ToString(), OnHandleEvent);
	}

	// 消息框相关
	private void OnClickMessageBoxOK()
	{
	}
	private void ShowMessageBox(string content)
	{
		_messageBoxObj.SetActive(true);
		_messageBoxContent.text = content;
	}
	private void HideMessageBox()
	{
		_messageBoxObj.SetActive(false);
	}
}