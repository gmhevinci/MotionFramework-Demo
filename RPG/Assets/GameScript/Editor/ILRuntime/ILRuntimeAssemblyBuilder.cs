using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.Collections.Generic;

/*
public class ILRuntimeAssemblyBuilder
{
	/// <summary>
	/// DLL编译的临时存储目录
	/// </summary>
	private const string UnityAssemblyTemperDir = "Temp/Assembly";

	[MenuItem("Tools/ILRuntime/编译并拷贝程序集")]
	public static void BuildAssembly()
	{
		// 创建目录
		if (Directory.Exists(UnityAssemblyTemperDir) == false)
			Directory.CreateDirectory(UnityAssemblyTemperDir);

		string csprojPath = $"Game/Game_Project.csproj";
		string outputPath = $"{UnityAssemblyTemperDir}/{ILRDefine.GameDLLFileName}.dll";

		string[] scripts = GetScripts(csprojPath);
		var assemblyBuilder = new AssemblyBuilder(outputPath, scripts);
		assemblyBuilder.compilerOptions = new ScriptCompilerOptions();
		assemblyBuilder.compilerOptions.AllowUnsafeCode = false;
		assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;
		assemblyBuilder.flags = AssemblyBuilderFlags.None;
		assemblyBuilder.additionalReferences = GetRefrences(csprojPath);

		// Started
		assemblyBuilder.buildStarted += delegate (string assemblyPath)
		{
			Debug.Log($"Assembly build started for {assemblyPath}");
		};

		// Finished
		assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
		{
			var errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
			var warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);
			Debug.Log($"Assembly build finished for {assemblyPath}");
			Debug.Log($"Warnings: {warningCount} - Errors: {errorCount}");
			foreach (var cm in compilerMessages)
			{
				if (cm.type == CompilerMessageType.Warning)
					Debug.LogWarning(cm.message);
				if (cm.type == CompilerMessageType.Error)
					Debug.LogError(cm.message);
			}

			if (errorCount == 0)
			{
				CopyAssemblyFiles();
			}
		};

		// 开始构建程序集
		if (!assemblyBuilder.Build())
		{
			Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
			return;
		}

		// 等待编译结束
		if (true)
		{
			while (assemblyBuilder.status != AssemblyBuilderStatus.Finished)
				System.Threading.Thread.Sleep(10);
		}
	}
	private static void CopyAssemblyFiles()
	{
		// 创建目录
		if (Directory.Exists(ILRDefine.GameAssemblyDir) == false)
			Directory.CreateDirectory(ILRDefine.GameAssemblyDir);

		// Copy DLL
		string dllSource = Path.Combine(UnityAssemblyTemperDir, $"{ILRDefine.GameDLLFileName}.dll");
		string dllDest = Path.Combine(ILRDefine.GameAssemblyDir, $"{ILRDefine.GameDLLFileName}.bytes");
		AssetDatabase.DeleteAsset(dllDest);
		if (File.Exists(dllSource))
			File.Copy(dllSource, dllDest, true);

		// Copy PDB
		string pdbSource = Path.Combine(UnityAssemblyTemperDir, $"{ILRDefine.GameDLLFileName}.pdb");
		string pdbDest = Path.Combine(ILRDefine.GameAssemblyDir, $"{ILRDefine.GamePDBFileName}.bytes");
		AssetDatabase.DeleteAsset(pdbDest);
		if (File.Exists(pdbSource))
			File.Copy(pdbSource, pdbDest, true);

		Debug.Log("Copy assembly files done.");
	}

	/// <summary>
	/// 获取编译脚本集合
	/// </summary>
	private static string[] GetScripts(string csprojPath)
	{
		List<string> results = new List<string>();
		XmlDocument doc = new XmlDocument();
		doc.Load(csprojPath);

		XmlNodeList nodeList = doc.GetElementsByTagName("Compile");
		foreach (XmlNode itemNode in nodeList)
		{
			string scriptPath = "Game/" + itemNode.Attributes.GetNamedItem("Include").InnerText;
			results.Add(scriptPath);
		}

		return results.ToArray();
	}

	/// <summary>
	/// 获取引用程序集集合
	/// </summary>
	private static string[] GetRefrences(string csprojPath)
	{
		List<string> results = new List<string>();
		XmlDocument doc = new XmlDocument();
		doc.Load(csprojPath);

		XmlNodeList nodeList = doc.GetElementsByTagName("Reference");
		foreach (XmlNode itemNode in nodeList)
		{
			foreach (XmlNode hintNode in itemNode.ChildNodes)
			{
				if (hintNode.Name == "HintPath")
				{
					results.Add(hintNode.InnerText);
				}
			}
		}

		return results.ToArray();
	}
}
*/