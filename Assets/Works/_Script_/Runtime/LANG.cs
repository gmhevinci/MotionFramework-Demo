using System.Collections;
using System.Collections.Generic;
using MotionGame;

/// <summary>
/// 多语言接口类
/// </summary>
public static class LANG
{
	public static string Convert(int hashCode)
	{
		CfgAutoGenerateLanguage cfg = CfgManager.Instance.GetConfig(EConfigType.AutoGenerateLanguage.ToString()) as CfgAutoGenerateLanguage;
		CfgAutoGenerateLanguageTab tab = cfg.GetTab(hashCode) as CfgAutoGenerateLanguageTab;
		if (tab != null)
			return tab.Lang;
		else
			return string.Empty;
	}

	public static List<string> Convert(List<int> hashCodes)
	{
		CfgAutoGenerateLanguage cfg = CfgManager.Instance.GetConfig(EConfigType.AutoGenerateLanguage.ToString()) as CfgAutoGenerateLanguage;

		List<string> result = new List<string>();
		for (int i = 0; i < hashCodes.Count; i++)
		{
			int hashCode = hashCodes[i];
			CfgAutoGenerateLanguageTab tab = cfg.GetTab(hashCode) as CfgAutoGenerateLanguageTab;
			result.Add(tab.Lang);
		}
		return result;
	}
}