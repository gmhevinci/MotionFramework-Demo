using UnityEngine;
using MotionFramework.Resource;
using MotionFramework.Patch;

public class BundleServices : IBundleServices
{
	private AssetBundleManifest _manifest;

	/// <summary>
	/// 加载Manifest文件
	/// </summary>
	public void LoadManifestFile()
	{
		// 注意：可能从沙盒内加载或者从流文件夹内加载
		string loadPath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.StrBuildManifestFileName);
		AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
		if (bundle == null)
			throw new System.Exception($"AssetBundleManifest file load failed : {loadPath}");

		_manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		if (_manifest == null)
			throw new System.Exception("AssetBundleManifest object load failed.");

		// 最后卸载AssetBundle
		bundle.Unload(false);
	}

	public string GetAssetBundleLoadPath(string manifestPath)
	{
		// 注意：可能从沙盒内加载或者从流文件夹内加载
		// 范例代码统一从流文件夹内加载
		return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
	}
	public string[] GetDirectDependencies(string assetBundleName)
	{
		return _manifest.GetDirectDependencies(assetBundleName);
	}
	public string[] GetAllDependencies(string assetBundleName)
	{
		return _manifest.GetAllDependencies(assetBundleName);
	}
}