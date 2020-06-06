using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

#if UNITY_2018_4_OR_NEWER
using UnityEditor.Experimental.SceneManagement;
#endif

public class UIPanelMonitor : Editor
{
	[InitializeOnLoadMethod]
	static void StartInitializeOnLoadMethod()
	{
		// 监听Inspector的Apply事件
		PrefabUtility.prefabInstanceUpdated = delegate (GameObject go)
		{
			UIManifest manifest = go.GetComponent<UIManifest>();
			if (manifest != null)
				manifest.Refresh();
		};

#if UNITY_2018_4_OR_NEWER
		// 监听新的Prefab系统
		PrefabStage.prefabSaving += OnPrefabSaving;
#endif
	}

#if UNITY_2018_4_OR_NEWER
	static void OnPrefabSaving(GameObject go)
	{
		PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
		if(stage != null)
		{
			UIManifest manifest = go.GetComponent<UIManifest>();
			if (manifest != null)
				manifest.Refresh();
		}
	}
#endif
}

/*
public class TestAssets : UnityEditor.AssetModificationProcessor
{
	static string[] OnWillSaveAssets(string[] paths)
	{
		foreach(var path in paths)
		{
			Debug.Log(path);
		}

		return paths;
	}
}
*/