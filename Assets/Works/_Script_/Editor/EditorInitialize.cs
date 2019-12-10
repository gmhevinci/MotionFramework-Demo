using System.IO;
using UnityEditor;
using UnityEngine;

class EditorInitialize
{
	/// <summary>
	/// 检测Unity编辑器版本
	/// </summary>
	[InitializeOnLoadMethod]
	static void CheckEditorVersion()
	{
		// TODO：这里设置你团队当前使用的Unity版本
		string version = "2018.4.6f1";
		Debug.Log($"当前使用Unity版本：{Application.unityVersion}");
		if (Application.unityVersion != version)
		{
			if(EditorUtility.DisplayDialog("错误", $"请安装Unity{version}，详情咨询客户端！", "OK"))
			{
				EditorApplication.Exit(0);
			}
		}
	}

	/// <summary>
	/// 创建MotionEngien依赖文件夹
	/// </summary>
	[InitializeOnLoadMethod]
	static void CreateMotionEngineDependFolder()
	{
		if (Directory.Exists(UIDefine.StrMyUIAtlasFolderPath) == false)
			Directory.CreateDirectory(UIDefine.StrMyUIAtlasFolderPath);

		if (Directory.Exists(UIDefine.StrMyUISpriteFolderPath) == false)
			Directory.CreateDirectory(UIDefine.StrMyUISpriteFolderPath);
	}
}