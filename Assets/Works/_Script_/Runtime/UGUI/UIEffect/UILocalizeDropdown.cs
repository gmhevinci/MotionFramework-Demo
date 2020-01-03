using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class UILocalizeDropdown : MonoBehaviour
{
	public List<string> Keywords = new List<string>();

	void Awake ()
	{
		Dropdown dropdown = GetComponent<Dropdown>();
		if (dropdown != null)
		{
			//当前文本
			string defaultKeyword = GetKeyword(dropdown.value);
			dropdown.captionText.text = ILRManager.Instance.UILanguage(defaultKeyword);

			//下拉列表
			for (int i = 0; i < dropdown.options.Count; i++)
			{
				string keyword = GetKeyword(i);
				dropdown.options[i].text = ILRManager.Instance.UILanguage(keyword);
			}
		}
	}

	private string GetKeyword(int index)
	{
		for(int i=0; i<Keywords.Count; i++)
		{
			if (i == index)
				return Keywords[i];
		}

		Debug.LogWarning($"UILocalizationDropdown : index is out range {index}");
		return string.Empty;
	}
}
