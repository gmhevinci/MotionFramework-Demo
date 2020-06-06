using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Pool;

public class EntityAvatar
{
	private readonly EntityObject _owner;
	private readonly CfgAvatarTable _avatarTable;
	private SpawnGameObject _spawnGo;
	private System.Action _callback;

	/// <summary>
	/// 游戏对象
	/// </summary>
	public GameObject GameObj
	{
		get
		{
			return _spawnGo.Go;
		}
	}


	public EntityAvatar(EntityObject owner, int avatarID)
	{
		_owner = owner;
		_avatarTable = CfgAvatar.Instance.GetConfigTable(avatarID);
	}
	public void Create(System.Action callback)
	{
		if (_spawnGo != null)
		{
			GameLogger.Error($"Avatar is create yet : {_avatarTable.Id}");
			return;
		}

		_callback = callback;

		// 对象池
		_spawnGo = GameObjectPoolManager.Instance.Spawn(GetModelName());
		_spawnGo.Completed += SpawnGo_Completed;
	}
	public void Destroy()
	{
		_callback = null;
		if (_spawnGo != null)
		{
			_spawnGo.Restore();
			_spawnGo = null;
		}
	}

	private void SpawnGo_Completed(GameObject go)
	{
		go.transform.SetParent(_owner.Root.transform, false);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one * GetModelScale();

		if (_callback != null)
			_callback.Invoke();
	}

	#region 配置表数据相关
	// 注意：在配表数据查找失败的情况下，也不会影响到正常的游戏逻辑
	private string GetModelName()
	{
		if (_avatarTable == null)
			return string.Empty;
		return _avatarTable.Model;
	}
	private float GetModelScale()
	{
		if (_avatarTable == null)
			return 1f;
		return _avatarTable.ModelScale;
	}
	public float GetBuffScale()
	{
		if (_avatarTable == null)
			return 1f;
		return _avatarTable.BuffScale;
	}
	public float GetRunAnimationSpeed()
	{
		if (_avatarTable == null)
			return 1f;
		return _avatarTable.RunAnimationSpeed;
	}
	public string GetRandomAttackSound()
	{
		if (_avatarTable == null)
			return string.Empty;
		if (_avatarTable.AttackSounds.Count == 0)
			return string.Empty;
		int index = Random.Range(0, _avatarTable.AttackSounds.Count);
		return _avatarTable.AttackSounds[index];
	}
	public string GetRandomGetHitSound()
	{
		if (_avatarTable == null)
			return string.Empty;
		if (_avatarTable.GetHitSounds.Count == 0)
			return string.Empty;
		int index = Random.Range(0, _avatarTable.GetHitSounds.Count);
		return _avatarTable.GetHitSounds[index];
	}
	public string GetDeadSound()
	{
		if (_avatarTable == null)
			return string.Empty;
		return _avatarTable.DeadSound;
	}
	#endregion
}