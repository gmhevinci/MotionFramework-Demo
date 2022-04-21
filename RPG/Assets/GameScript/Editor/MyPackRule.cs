using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset.Editor;

public class PackShaderVariants : IPackRule
{
	public string GetBundleName(PackRuleData data)
	{
		return "myshaders";
	}
}
