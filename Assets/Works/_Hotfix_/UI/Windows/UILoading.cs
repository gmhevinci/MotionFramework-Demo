using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Scene;
using MotionFramework;

namespace Hotfix
{
	[Window(EWindowType.UILoading, EWindowLayer.Loading)]
	public class UILoading : UIWindow
	{
		private Slider _slider;
		private Text _txtValue;
		private string _sceneName = string.Empty;
		private OnceTimer _timer = new OnceTimer(0.5f);

		public UILoading()
		{
			FullScreen = true;
			DontDestroy = true;
		}
		public override void OnCreate()
		{
			_slider = GetUIComponent<Slider>("UILoading/Slider");
			_txtValue = GetUIComponent<Text>("UILoading/Slider/Text");
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
			_sceneName = UserData as string;
			_slider.value = 0;
			_txtValue.text = "0%";
			_timer.Reset();
		}
		public override void OnUpdate()
		{
			int progress = SceneManager.Instance.GetSceneLoadProgress(_sceneName);
			_slider.value = progress / 100f;
			_txtValue.text = $"{progress}%";

			if (SceneManager.Instance.CheckSceneIsDone(_sceneName))
			{
				// 延迟关闭加载窗口
				if (_timer.Update(Time.deltaTime))
					UIManager.Instance.CloseWindow(EWindowType.UILoading);
			}
		}
	}
}