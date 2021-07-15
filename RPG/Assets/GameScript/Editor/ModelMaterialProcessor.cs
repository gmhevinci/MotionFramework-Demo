using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ModelMaterialProcessor : AssetPostprocessor
{
	Material OnAssignMaterialModel(Material material, Renderer renderer)
	{
		string defaultPath = "Assets/MotionAssets/ModelDefaultMaterial.mat";
		Material defaultMaterial = AssetDatabase.LoadAssetAtPath<Material>(defaultPath);
		if (defaultMaterial == null)
			throw new System.Exception("Not found model default material.");
		return defaultMaterial;
	}
}