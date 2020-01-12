using System;
using System.Security;
using UnityEngine;
using Mono.Xml;
using MotionFramework.Resource;
using MotionFramework;

public abstract class AssetXml
{
	private AssetReference _assetRef;
	private AssetOperationHandle _handle;
	private System.Action _userCallback;
	protected SecurityElement _xml;

	/// <summary>
	/// 是否完成
	/// </summary>
	public bool IsDone
	{
		get
		{
			return _handle.IsDone;
		}
	}

	/// <summary>
	/// 资源地址
	/// </summary>
	public string Location { private set; get; }


	public void Init(string location)
	{
		Location = location;
		_assetRef = new AssetReference(location);
	}
	public void Load(System.Action callback)
	{
		if (_userCallback != null)
			return;

		_userCallback = callback;
		_handle = _assetRef.LoadAssetAsync<TextAsset>();
		_handle.Completed += Handle_Completed;
	}
	private void Handle_Completed(AssetOperationHandle obj)
	{
		TextAsset temp = _handle.AssetObject as TextAsset;
		if (temp != null)
		{
			try
			{
				SecurityParser sp = new SecurityParser();
				sp.LoadXml(temp.text);
				_xml = sp.ToXml();

				// 解析数据
				if (_xml != null)
					ParseData();
			}
			catch (Exception ex)
			{
				MotionLog.Log(ELogLevel.Error, $"Failed to parse xml {Location}. Exception : {ex.ToString()}");
			}
		}

		// 注意：为了节省内存这里立即释放了资源
		if (_assetRef != null)
		{
			_assetRef.Release();
			_assetRef = null;
		}

		_userCallback?.Invoke();
	}

	/// <summary>
	/// 解析数据
	/// </summary>
	protected abstract void ParseData();
}