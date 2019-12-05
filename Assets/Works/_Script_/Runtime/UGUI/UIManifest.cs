using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using MotionEngine;
using MotionEngine.Patch;

namespace UnityEngine.UI
{
	[DisallowMultipleComponent]
	public class UIManifest : MonoBehaviour
	{
		[SerializeField]
		private List<string> _elementPath = new List<string>();
		[SerializeField]
		private List<Transform> _elementTrans = new List<Transform>();
		[SerializeField]
		private List<string> _cacheAtlasTags = new List<string>();

		/// <summary>
		/// 附加的预制体
		/// </summary>
		[SerializeField]
		public List<GameObject> AttachPrefabs = new List<GameObject>();

		private readonly Dictionary<string, Transform> _runtimeDic = new Dictionary<string, Transform>(200);


		private void Awake()
		{
			if (Application.isPlaying)
				ConvertListToDic();
		}

		/// <summary>
		/// 游戏运行时把List内容存在字典里方便查询
		/// </summary>
		private void ConvertListToDic()
		{
			_runtimeDic.Clear();

			if(_elementPath.Count == 0)
				throw new Exception($"Fatal error : {this.gameObject.name} elementPath list is empty.");
			if(_elementTrans.Count == 0)
				throw new Exception($"Fatal error : {this.gameObject.name} elementTrans list is empty.");
			if(_elementPath.Count != _elementTrans.Count)
				throw new Exception($"Fatal error : {this.gameObject.name} elementTrans list and elementPath list must has same count.");

			for (int i = 0; i < _elementPath.Count; i++)
			{
				string path = _elementPath[i];
				Transform trans = _elementTrans[i];
				_runtimeDic.Add(path, trans);
			}
		}

		/// <summary>
		/// 编辑器接口
		/// </summary>
		[ContextMenu("Refresh")]
		public void Refresh()
		{
#if UNITY_EDITOR
			CacheAllUIElement();
			UpdateUISpriteComponent();
#endif
		}


		/// <summary>
		/// 根据全路径获取UI元素
		/// </summary>
		public Transform GetElement(string path)
		{
			if (string.IsNullOrEmpty(path))
				return null;

			Transform result = null;
			if (_runtimeDic.TryGetValue(path, out result) == false)
			{
				LogSystem.Log(ELogType.Warning, $"Not found ui element : {path}");
			}
			return result;
		}

		/// <summary>
		/// 根据全路径获取UI组件
		/// </summary>
		public Component GetComponent(string path, string typeName)
		{
			Transform element = GetElement(path);
			if (element == null)
				return null;

			Component component = element.GetComponent(typeName);
			if (component == null)
				LogSystem.Log(ELogType.Warning, $"Not found ui component : {path}, {typeName}");
			return component;
		}

		/// <summary>
		/// 获取附加的预制体
		/// </summary>
		public GameObject CloneAttachPrefab(string name)
		{
			GameObject prefab = null;
			for (int i = 0; i < AttachPrefabs.Count; i++)
			{
				if (AttachPrefabs[i] == null)
					continue;
				if (AttachPrefabs[i].name == name)
					prefab = AttachPrefabs[i];
			}

			if (prefab == null)
				return null;

			return GameObject.Instantiate(prefab);
		}


#if UNITY_EDITOR
		/// <summary>
		/// 缓存所有UI元素
		/// </summary>
		private void CacheAllUIElement()
		{
			// 清空旧数据
			_elementPath.Clear();
			_elementTrans.Clear();

			Transform[] allTrans = this.transform.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < allTrans.Length; i++)
			{
				Transform trans = allTrans[i];
				string path = GetFullPath(trans);
				AddElementToList(path, trans);
			}

			Debug.Log($"Cache panel {this.transform.name} total {allTrans.Length} elements");
		}

		/// <summary>
		/// 获取到根节点的全路径
		/// </summary>
		private string GetFullPath(Transform trans)
		{
			string path = trans.name;
			while (trans.parent != null)
			{
				// 如果找到了根节点
				if (trans == this.transform)
				{
					break;
				}
				else
				{
					trans = trans.parent;
					if (trans != null)
						path = trans.name + "/" + path;
				}
			}
			return path;
		}

		/// <summary>
		/// 添加一个UI元素到列表
		/// </summary>
		private void AddElementToList(string path, Transform trans)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.Log("UI element is invalid. UI path is null or empty.");
				return;
			}
			if (trans == null)
			{
				Debug.Log($"UI element is invalid. UI path : {path}");
				return;
			}

			// 如果有重复路径的元素
			for (int i = 0; i < _elementPath.Count; i++)
			{
				if (_elementPath[i] == path)
				{
					Debug.LogError($"发现重复路径的元素 : {path}");
					return;
				}
			}

			_elementPath.Add(path);
			_elementTrans.Add(trans);
		}

		/// <summary>
		/// 更新组件
		/// </summary>
		private void UpdateUISpriteComponent()
		{
			// Clear cache
			_cacheAtlasTags.Clear();

			// 获取依赖的图集名称
			Image[] allImage = this.transform.GetComponentsInChildren<Image>(true);
			for (int i = 0; i < allImage.Length; i++)
			{
				Image img = allImage[i];

				// Clear
				UISprite uiSprite = img.GetComponent<UISprite>();
				if (uiSprite != null)
					uiSprite.Atlas = null;

				// 如果图片为空
				if (img.sprite == null)
					continue;

				// 文件路径
				string assetPath = UnityEditor.AssetDatabase.GetAssetPath(img.sprite);

				// 如果是系统内置资源
				if (assetPath.Contains("_builtin_"))
					continue;

				// 如果是图集资源
				if (assetPath.Contains(PatchDefine.StrMyUISpriteFolderPath))
				{
					if (uiSprite == null)
						uiSprite = img.gameObject.AddComponent<UISprite>();

					string[] splits = assetPath.Split('/');
					string atlasName = splits[4]; //注意：根据路径判断索引
					string atlasAssetPath = $"{PatchDefine.StrMyUIAtlasFolderPath}/{atlasName}.spriteatlas";
					SpriteAtlas spriteAtlas = UnityEditor.AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
					if (spriteAtlas == null)
					{
						throw new Exception($"Not found SpriteAtlas : {atlasAssetPath}");
					}
					else
					{
						uiSprite.Atlas = spriteAtlas;

						if (_cacheAtlasTags.Contains(atlasName) == false)
							_cacheAtlasTags.Add(atlasName);
					}
				}
			}
		}
#endif
	}
}