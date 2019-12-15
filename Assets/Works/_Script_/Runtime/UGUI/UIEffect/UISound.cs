using System.Collections;
using System.Collections.Generic;
using MotionFramework.Audio;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(Button))]
	public class UISound : MonoBehaviour
	{
		public string SoundName = string.Empty;

		void Start()
		{
			Button btn = GetComponent<Button>();
			if (btn != null)
				btn.onClick.AddListener(OnClickButton);
		}

		private void OnClickButton()
		{
			AudioManager.Instance.PlaySound($"UI/{SoundName}");
		}
	}
}