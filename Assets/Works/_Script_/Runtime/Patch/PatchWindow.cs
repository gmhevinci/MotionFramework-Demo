using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Patch;
using MotionFramework.Resource;
using MotionFramework.Event;

public class PatchWindow : ModuleSingleton<PatchWindow>, IModule
{
	/// <summary>
	/// 对话框封装类
	/// </summary>
	private class MessageBox
	{
		private GameObject _cloneObject;
		private Text _content;
		private System.Action _clickYes;

		public void Create(GameObject cloneObject)
		{
			_cloneObject = cloneObject;
			_content = cloneObject.transform.BFSearch("txt_content").GetComponent<Text>();
			var btnYes = cloneObject.transform.BFSearch("btn_yes").GetComponent<Button>();
			btnYes.onClick.AddListener(OnClickYes);
		}
		public void Show(string content, System.Action clickYes)
		{
			_content.text = content;
			_clickYes = clickYes;
			_cloneObject.SetActive(true);
		}
		public void Hide()
		{
			_content.text = string.Empty;
			_clickYes = null;
			_cloneObject.SetActive(false);
		}
		public bool ActiveSelf
		{
			get
			{
				return _cloneObject.activeSelf;
			}
		}

		private void OnClickYes()
		{
			_clickYes?.Invoke();
			Hide();
		}
	}

	private AssetReference _assetRef;

	// 事件组
	private EventGroup _eventGroup = new EventGroup();

	// 对话框列表
	private List<MessageBox> _msgBoxList = new List<MessageBox>();

	// UGUI相关
	private GameObject _uiRoot;
	private UIManifest _manifest;
	private Slider _slider;
	private Text _tips;
	private GameObject _messageBoxObj;

	void IModule.OnCreate(object createParam)
	{
		_assetRef = new AssetReference("UIPanel/PatchWindow");
		_assetRef.LoadAssetAsync<GameObject>().Completed += Handle_Completed;
	}
	void IModule.OnUpdate()
	{
	}
	void IModule.OnGUI()
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

		_eventGroup.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundForceInstallAPP>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundUpdateFiles>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.DownloadFilesProgress>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.GameVersionRequestFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebPatchManifestDownloadFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebFileDownloadFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebFileCheckFailed>(OnHandleEvent);

		SendOperationEvent(EPatchOperation.BeginingRequestGameVersion);
	}
	private void OnWindowDestroy()
	{
		_eventGroup.RemoveAllListener();
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

	// 接收事件
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;
			if (message.CurrentStates == EPatchStates.None)
				_tips.text = "正在准备游戏世界......";
			else if (message.CurrentStates == EPatchStates.RequestGameVersion)
				_tips.text = "正在请求最新游戏版本";
			else if (message.CurrentStates == EPatchStates.ParseWebPatchManifest)
				_tips.text = "正在分析新清单";
			else if (message.CurrentStates == EPatchStates.GetDonwloadList)
				_tips.text = "正在准备下载列表";
			else if (message.CurrentStates == EPatchStates.DownloadWebFiles)
				_tips.text = "正在下载更新文件";
			else if (message.CurrentStates == EPatchStates.DownloadWebPatchManifest)
				_tips.text = "正在替换最新的清单";
			else if (message.CurrentStates == EPatchStates.DownloadOver)
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
			string totalSizeMB = (message.TotalSizeBytes / 1048576f).ToString("f1");
			ShowMessageBox($"发现新版本需要更新 : 一共{message.TotalCount}个文件，总大小{totalSizeMB}MB", callback);
		}

		else if (msg is PatchEventMessageDefine.DownloadFilesProgress)
		{
			var message = msg as PatchEventMessageDefine.DownloadFilesProgress;
			_slider.value = message.CurrentDownloadCount / message.TotalDownloadCount;
			string currentSizeMB = (message.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
			string totalSizeMB = (message.TotalDownloadSizeBytes / 1048576f).ToString("f1");
			_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
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

		else if (msg is PatchEventMessageDefine.WebFileCheckFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileCheckFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件验证失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebPatchManifestDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebPatchManifestDownloadFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebPatchManifest);
			};
			ShowMessageBox($"清单下载失败", callback);
		}

		else
		{
			throw new System.NotImplementedException($"{msg.GetType()}");
		}
	}

	// 消息框相关
	private void ShowMessageBox(string content, System.Action clickYes)
	{
		// 尝试获取一个可用的对话框
		MessageBox msgBox = null;
		for (int i = 0; i < _msgBoxList.Count; i++)
		{
			var item = _msgBoxList[i];
			if(item.ActiveSelf == false)
			{
				msgBox = item;
				break;
			}
		}

		// 如果没有可用的对话框则创建一个新的对话框
		if(msgBox == null)
		{
			msgBox = new MessageBox();
			var cloneObject = GameObject.Instantiate(_messageBoxObj, _messageBoxObj.transform.parent);
			msgBox.Create(cloneObject);
			_msgBoxList.Add(msgBox);
		}

		// 显示对话框
		msgBox.Show(content, clickYes);
	}

	// 事件相关
	private void SendOperationEvent(EPatchOperation operation)
	{
		PatchEventMessageDefine.OperationEvent msg = new PatchEventMessageDefine.OperationEvent();
		msg.operation = operation;
		EventManager.Instance.SendMessage(msg);
	}
}