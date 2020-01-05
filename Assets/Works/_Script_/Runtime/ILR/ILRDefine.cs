
public class ILRDefine
{
	/// <summary>
	/// Unity编译的DLL存储目录
	/// </summary>
	public const string StrScriptAssembliesDir = "Library/ScriptAssemblies";

	/// <summary>
	/// 我们设定的DLL存储目录
	/// </summary>
	public const string StrMyAssemblyFolderPath = GameDefine.AssetRootPath + "/Assembly";

	/// <summary>
	/// 热更程序集的名称
	/// </summary>
	public const string StrHotfixAssemblyName = "Hotfix";

	/// <summary>
	/// 我们设定的热更DLL文件名称
	/// </summary>
	public const string StrMyHotfixDLLFileName = "HotfixDLL";

	/// <summary>
	/// 我们设定的热更PDB文件名称
	/// </summary>
	public const string StrMyHotfixPDBFileName = "HotfixPDB";

	/// <summary>
	/// 我们设定的自动生成的绑定脚本文件夹路径
	/// </summary>
	public const string StrMyBindingFolderPath = "Assets/Works/_Script_/Runtime/ILBinding";
}