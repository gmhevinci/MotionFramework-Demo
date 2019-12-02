using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using MotionGame;

public static class ILRuntimeCLRBinding
{
	[MenuItem("Tools/ILRuntime/Generate CLR Binding Code")]
	static void GenerateCLRBinding()
	{
		List<Type> types = new List<Type>();
		types.Add(typeof(int));
		types.Add(typeof(long));
		types.Add(typeof(float));
		types.Add(typeof(string));
		types.Add(typeof(object));
		types.Add(typeof(Array));
		types.Add(typeof(Vector2));
		types.Add(typeof(Vector3));
		types.Add(typeof(Quaternion));
		types.Add(typeof(GameObject));
		types.Add(typeof(UnityEngine.Object));
		types.Add(typeof(Transform));
		types.Add(typeof(RectTransform));
		types.Add(typeof(Time));
		types.Add(typeof(Debug));

		// 所有DLL内的类型的真实C#类型都是ILTypeInstance
		types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

		// 生成绑定脚本
		ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, ILRDefine.StrMyBindingFolderPath);
	}

	[MenuItem("Tools/ILRuntime/Generate CLR Binding Code by Analysis")]
	static void GenerateCLRBindingByAnalysis()
	{
		// 先删除旧代码
		AssetDatabase.DeleteAsset(ILRDefine.StrMyBindingFolderPath);
		Directory.CreateDirectory(ILRDefine.StrMyBindingFolderPath);

		// 分析热更DLL来生成绑定代码
		ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
		string dllFilePath = $"{ILRDefine.StrMyAssemblyFolderPath}/{ILRDefine.StrMyHotfixDLLFileName}.bytes";
		using (FileStream fs = new FileStream(dllFilePath, FileMode.Open, FileAccess.Read))
		{
			domain.LoadAssembly(fs);

			// Crossbind Adapter is needed to generate the correct binding code
			ILRHelper.Init(domain);

			// 生成所有绑定脚本
			ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, ILRDefine.StrMyBindingFolderPath);
		}
	}
}
