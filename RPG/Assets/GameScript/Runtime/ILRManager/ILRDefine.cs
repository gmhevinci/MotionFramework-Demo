
public class ILRDefine
{
	/// <summary>
	/// DLL编译的临时存储目录
	/// </summary>
	public const string AssemblyTemperDir = "Temp/Assembly";

	/// <summary>
	/// Unity编译的DLL存储目录
	/// </summary>
	public const string ScriptAssembliesDir = "Library/ScriptAssemblies";

	/// <summary>
	/// 我们设定的DLL存储目录
	/// </summary>
	public const string MyAssemblyDir = "Assets/GameRes/Assembly";

	/// <summary>
	/// 我们设定的热更DLL文件名称
	/// </summary>
	public const string GameDLLFileName = "GameDLL";
	
	/// <summary>
	/// 我们设定的热更PDB文件名称
	/// </summary>
	public const string GamePDBFileName = "GamePDB";

	/// <summary>
	/// 我们设定的自动生成的绑定脚本夹路径
	/// </summary>
	public const string MyBindingFolderPath = "Assets/GameScript/Runtime/ILRBinding";

	/// <summary>
	/// 我们设定的自动生成的适配脚本夹路径
	/// </summary>
	public const string MyAdapterFolderPath = "Assets/GameScript/Runtime/Temper";
}