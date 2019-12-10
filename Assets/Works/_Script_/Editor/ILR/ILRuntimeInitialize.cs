using System.IO;
using UnityEditor;
using UnityEngine;

public class ILRuntimeInitialize
{
	[InitializeOnLoadMethod]
	static void CopyAssemblyFiles()
	{
		// 创建目录
		if (Directory.Exists(ILRDefine.StrMyAssemblyFolderPath) == false)
			Directory.CreateDirectory(ILRDefine.StrMyAssemblyFolderPath);

		// Copy DLL
		string dllSource = Path.Combine(ILRDefine.StrScriptAssembliesDir, $"{ILRDefine.StrHotfixAssemblyName}.dll");
		string dllDest = Path.Combine(ILRDefine.StrMyAssemblyFolderPath, $"{ILRDefine.StrMyHotfixDLLFileName}.bytes");
		AssetDatabase.DeleteAsset(dllDest);
		if (File.Exists(dllSource))
			File.Copy(dllSource, dllDest, true);

		// Copy PDB
		string pdbSource = Path.Combine(ILRDefine.StrScriptAssembliesDir, $"{ILRDefine.StrHotfixAssemblyName}.pdb");
		string pdbDest = Path.Combine(ILRDefine.StrMyAssemblyFolderPath, $"{ILRDefine.StrMyHotfixPDBFileName}.bytes");
		AssetDatabase.DeleteAsset(pdbDest);
		if (File.Exists(pdbSource))
			File.Copy(pdbSource, pdbDest, true);

		Debug.Log("Copy hotfix assembly files done.");
		AssetDatabase.Refresh();
	}
}