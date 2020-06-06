using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(Text))]
	public class UILocalizeText : MonoBehaviour
	{
		public string KeyWord = "";

		private void Start()
		{
			Text txt = GetComponent<Text>();
			txt.text = UILocalization.GetLanguage(KeyWord);
		}
	}
}