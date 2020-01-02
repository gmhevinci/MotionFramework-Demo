using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UILocalizeText : MonoBehaviour
{
	public string KeyWord = "";

	private void Start()
	{
		Text txt = GetComponent<Text>();
		if (ILRManager.Instance != null)
			txt.text = ILRManager.Instance.UILanguage(KeyWord);
	}
}