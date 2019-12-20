using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Pool;

namespace Hotfix
{
	public abstract class EntityObject
	{
		public int EntityID { private set; get; }
		public GameObject Root { private set; get; }
		public GameObject Avatar { private set; get; }
		private string _assetName = string.Empty;

		/// <summary>
		/// 美术资源是否准备完毕
		/// </summary>
		protected bool _isPrepareAvatar { private set; get; } = false;


		public EntityObject(int entityID)
		{
			EntityID = entityID;
		}
		public void Create(string assetName, Vector3 pos, Vector3 rot)
		{
			if (Root != null)
			{
				HotfixLogger.Error("Should never get here.");
				return;
			}

			_assetName = assetName;

			// 创建游戏对象
			Root = new GameObject("EntityCharacter");
			Root.transform.position = pos;
			Root.transform.rotation = Quaternion.Euler(rot);

			// 从对象池里获取游戏对象
			PoolManager.Instance.Spawn(assetName, OnAssetLoad);

			OnCreate();
		}
		public void Destroy()
		{
			if (Avatar != null)
			{
				PoolManager.Instance.Restore(_assetName, Avatar);
				Avatar = null;
			}

			if (Root != null)
			{
				GameObject.Destroy(Root);
				Root = null;
			}

			OnDestroy();
		}
		public void Update(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		/// 当美术资源加载完毕
		private void OnAssetLoad(GameObject go)
		{
			Avatar = go;
			Avatar.transform.SetParent(Root.transform, false);
			Avatar.transform.localPosition = Vector3.zero;
			Avatar.transform.localRotation = Quaternion.identity;

			OnPrepareAvatar();
			_isPrepareAvatar = true;
		}

		protected abstract void OnCreate();
		protected abstract void OnDestroy();
		protected abstract void OnPrepareAvatar();
		protected abstract void OnUpdate(float deltaTime);
	}
}