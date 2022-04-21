using System.IO;
using UnityEditor;
using UnityEngine;

/*
public class ILRuntimeInitialize
{
	[InitializeOnLoadMethod]
	static void InitializeCopyAssemblyFiles()
	{
		// 创建目录
		if (Directory.Exists(ILRDefine.MyAssemblyDir) == false)
			Directory.CreateDirectory(ILRDefine.MyAssemblyDir);

		// Copy DLL
		string dllSource = Path.Combine(ILRDefine.ScriptAssembliesDir, $"{ILRDefine.GameDLLFileName}.dll");
		string dllDest = Path.Combine(ILRDefine.MyAssemblyDir, $"{ILRDefine.GameDLLFileName}.bytes");
		if (File.Exists(dllDest))
			AssetDatabase.DeleteAsset(dllDest);
		if (File.Exists(dllSource))
			File.Copy(dllSource, dllDest, true);

		// Copy PDB
		string pdbSource = Path.Combine(ILRDefine.ScriptAssembliesDir, $"{ILRDefine.GamePDBFileName}.pdb");
		string pdbDest = Path.Combine(ILRDefine.MyAssemblyDir, $"{ILRDefine.GamePDBFileName}.bytes");
		if (File.Exists(pdbDest))
			AssetDatabase.DeleteAsset(pdbDest);
		if (File.Exists(pdbSource))
			File.Copy(pdbSource, pdbDest, true);

		Debug.Log("Copy hotfix assembly files done.");
		AssetDatabase.Refresh();
	}
}
*/