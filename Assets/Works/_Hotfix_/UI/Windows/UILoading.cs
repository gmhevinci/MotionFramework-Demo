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
		private UISprite _progress;
		private Text _loadingTxt;

		private string _sceneName = string.Empty;
		private OnceTimer _hideTimer = new OnceTimer(0.5f);
		private RepeatTimer _repeater = new RepeatTimer(0, 0.1f);
		private int _count = 0;


		public UILoading()
		{
			FullScreen = true;
			DontDestroy = true;
		}
		public override void OnCreate()
		{
			_progress = GetUIComponent<UISprite>("UILoading/Loading/Fill");
			_loadingTxt = GetUIComponent<Text>("UILoading/Loading/Text");
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
			_sceneName = UserData as string;
			UpdateProgressSprite(0);
			UpdateLoadingText(0);
			_hideTimer.Reset();
			_repeater.Reset();
			_count = 0;
		}
		public override void OnUpdate()
		{
			if (_repeater.Update(Time.deltaTime))
			{
				int progress = SceneManager.Instance.GetSceneLoadProgress(_sceneName);
				UpdateProgressSprite(progress);

				_count++;
				if (_count > 3)
					_count = 0;
				UpdateLoadingText(_count);
			}

			if (SceneManager.Instance.CheckSceneIsDone(_sceneName))
			{
				// 延迟关闭加载窗口
				if (_hideTimer.Update(Time.deltaTime))
					UIManager.Instance.CloseWindow(EWindowType.UILoading);
			}
		}

		private void UpdateProgressSprite(int progress)
		{
			float normalize = progress / 100f;
			int index = (int)(normalize * 21); //一共21张图
			if (index < 10)
				_progress.SpriteName = $"Loading_Frame_000{index}";	
			else
				_progress.SpriteName = $"Loading_Frame_00{index}";
		}
		private void UpdateLoadingText(int count)
		{
			if(_count == 0)
				_loadingTxt.text = "Loading";
			else if(count == 1)
				_loadingTxt.text = "Loading.";
			else if (count == 2)
				_loadingTxt.text = "Loading..";
			else if (count == 3)
				_loadingTxt.text = "Loading...";
		}
	}
}