using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Resource;
using MotionFramework.Event;

public class UIRoot : IEnumerator
{
	private AssetReference _assetRef;
	private AssetOperationHandle _handle;

	private GameObject _uiRoot;
	public GameObject UIDesktop { private set; get; }
	public Camera UICamera { private set; get; }
	public bool IsPrepare { private set; get; }

	public void LoadAsync(string location)
	{
		if (_assetRef == null)
		{
			_assetRef = new AssetReference(location);
			_handle = _assetRef.LoadAssetAsync<GameObject>();
			_handle.Completed += Handle_Completed;
		}
	}
	public void Destroy()
	{
		if (_assetRef != null)
		{
			_assetRef.Release();
			_assetRef = null;
		}

		if (_uiRoot != null)
		{
			GameObject.Destroy(_uiRoot);
			_uiRoot = null;
		}
	}

	private void Handle_Completed(AssetOperationHandle obj)
	{
		_uiRoot = _handle.InstantiateObject;
		UIDesktop = _uiRoot.transform.BFSearch("UIDesktop").gameObject;
		UICamera = _uiRoot.transform.BFSearch("UICamera").GetComponent<Camera>();
		GameObject.DontDestroyOnLoad(_uiRoot);

		IsPrepare = true;
		_userCallback?.Invoke(this);
	}

	#region 异步相关
	private System.Action<UIRoot> _userCallback;

	/// <summary>
	/// 完成委托
	/// </summary>
	public event System.Action<UIRoot> Completed
	{
		add
		{
			if (IsPrepare)
				value.Invoke(this);
			else
				_userCallback += value;
		}
		remove
		{
			_userCallback -= value;
		}
	}

	bool IEnumerator.MoveNext()
	{
		return !IsPrepare;
	}
	void IEnumerator.Reset()
	{
	}
	object IEnumerator.Current
	{
		get { return null; }
	}
	#endregion
}