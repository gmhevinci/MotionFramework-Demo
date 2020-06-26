using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UILocalization
{
	public static string GetLanguage(string key)
	{
		var table = CfgUILanguage.Instance.GetConfigTable(key.GetHashCode());
		if (table != null)
		{
			return table.Lang;
		}
		else
		{
			GameLog.Warning($"Not found UILanguage {key}");
			return key;
		}
	}
}