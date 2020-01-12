using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Pool;

namespace Hotfix
{
	public class EntityAvatar
	{
		private enum EAvatarState
		{
			None,
			Create,
			Destroy,
		}

		private readonly EntityObject _owner;
		private readonly CfgAvatarTab _avatarTable;
		private System.Action _callback;
		private EAvatarState _avatarState = EAvatarState.None;

		/// <summary>
		/// 游戏对象
		/// </summary>
		public GameObject GameObj { private set; get; }


		public EntityAvatar(EntityObject owner, int avatarID)
		{
			_owner = owner;
			_avatarTable = CfgAvatar.Instance.GetCfgTab(avatarID);
		}
		public void Create(System.Action callback)
		{
			if (_avatarState != EAvatarState.None)
			{
				HotfixLogger.Error($"Avatar is create yet : {_avatarTable.Id}");
				return;
			}

			_callback = callback;
			_avatarState = EAvatarState.Create;

			// 对象池
			GameObjectPoolManager.Instance.Spawn(GetModelName(), OnAssetLoad);
		}
		public void Destroy()
		{
			_callback = null;
			_avatarState = EAvatarState.Destroy;

			// 对象池
			if (GameObj != null)
			{
				GameObjectPoolManager.Instance.Restore(GetModelName(), GameObj);
				GameObj = null;
			}
		}

		// 当美术资源加载完毕
		private void OnAssetLoad(GameObject go)
		{
			// 注意：如果Avatar已经销毁，我们回收到对象池
			if(_avatarState == EAvatarState.Destroy)
			{
				GameObjectPoolManager.Instance.Restore(GetModelName(), go);
				return;
			}

			GameObj = go;
			GameObj.transform.SetParent(_owner.Root.transform, false);
			GameObj.transform.localPosition = Vector3.zero;
			GameObj.transform.localRotation = Quaternion.identity;
			GameObj.transform.localScale = Vector3.one * GetModelScale();

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
}