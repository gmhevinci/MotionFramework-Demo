using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Resource;

public abstract class EntityObject
{
	protected GameObject _go;
	protected Transform _trans;
	protected GameObject _avatar;
	private AssetObject _asset;
	
	public void Create(Vector3 pos, Vector3 rot)
	{
		if (_go != null)
			return;

		// 创建游戏对象
		_go = new GameObject("EntityCharacter");
		_trans = _go.transform;
		_trans.position = pos;
		_trans.rotation = Quaternion.Euler(rot);

		// 加载模型
		_asset = new AssetObject();
		_asset.Load("Model/Character/footman_Blue", OnAssetPrepare);

		OnCreate();
	}
	public void Destroy()
	{
		if(_avatar != null)
		{
			GameObject.Destroy(_avatar);
			_avatar = null;
		}

		if (_asset != null)
		{
			_asset.UnLoad();
			_asset = null;
		}

		if (_go != null)
		{
			GameObject.Destroy(_go);
			_go = null;
		}

		OnDestroy();
	}
	public void Update()
	{
		OnUpdate();
	}

	private void OnAssetPrepare(Asset asset)
	{
		if (asset.Result != EAssetResult.OK)
			return;

		_avatar = _asset.GetMainAsset<GameObject>();
		_avatar.transform.SetParent(_trans, false);
		_avatar.transform.localPosition = Vector3.zero;
		_avatar.transform.localRotation = Quaternion.identity;

		OnPrepare();
	}

	protected abstract void OnCreate();
	protected abstract void OnDestroy();
	protected abstract void OnPrepare();
	protected abstract void OnUpdate();
}