using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Audio;
using MotionFramework.Window;
using MotionFramework.Utility;
using MotionFramework.Tween;

[Window((int)EWindowLayer.Panel, true)]
sealed class UISetting : CanvasWindow
{
	private Slider _volumeSlider;
	private Toggle _musicToggle;
	private Toggle _soundToggle;
	private Transform _animTrans;
	private RectTransform _ainmRectTrans;
	private CanvasGroup _canvasGroup;
	private bool _isPlayOpenAnimation = false;

	public override void OnCreate()
	{
		WindowOpenAnimationTime = 0.5f;

		AddButtonListener("UISetting/Mask", OnClickClose);
		AddButtonListener("UISetting/Window/Button (Save)", OnClickClose);

		_volumeSlider = GetUIComponent<Slider>("UISetting/Window/Content/Control Area (Slider)/Slider");
		_volumeSlider.onValueChanged.AddListener(OnSliderValueChange);
		_musicToggle = GetUIComponent<Toggle>("UISetting/Window/Content/Control Area (Toggles)/Toggle 1");
		_musicToggle.onValueChanged.AddListener(OnMusicToggleValueChange);
		_soundToggle = GetUIComponent<Toggle>("UISetting/Window/Content/Control Area (Toggles)/Toggle 2");
		_soundToggle.onValueChanged.AddListener(OnSoundToggleValueChange);
		_canvasGroup = GetUIComponent<CanvasGroup>("UISetting/Window");

		_animTrans = GetUIElement("UISetting/Window");
		_animTrans.transform.localScale = Vector3.zero;
		_ainmRectTrans = _animTrans as RectTransform;
	}
	public override void OnDestroy()
	{
	}
	public override void OnRefresh()
	{
		_volumeSlider.value = AudioPlayerSetting.AudioVolume;
		_musicToggle.isOn = !AudioPlayerSetting.MusicMute;
		_soundToggle.isOn = !AudioPlayerSetting.SoundMute;

		// 窗口打开动画
		if(_isPlayOpenAnimation == false)
		{
			_isPlayOpenAnimation = true;
			ITweenNode rootNode = ParallelNode.Allocate(
				_canvasGroup.TweenAlpha(0.4f, 0f, 1f),
				_animTrans.TweenScaleTo(0.8f, Vector3.one).SetEase(TweenEase.Bounce.EaseOut),
				_animTrans.TweenAnglesTo(0.4f, new Vector3(0, 0, 720))
				);
			TweenGrouper.AddNode(rootNode);
		}
	}
	public override void OnUpdate()
	{
	}

	private void OnClickClose()
	{
		// 窗口关闭动画
		ITweenNode rootNode = SequenceNode.Allocate(
			_ainmRectTrans.TweenAnchoredPositionTo(0.5f, new Vector2(800, 0)).SetLerp(LerpFun), 
			_animTrans.TweenScaleTo(0.5f, Vector3.zero).SetEase(TweenEase.Bounce.EaseOut),
			ExecuteNode.Allocate(() => { UITools.CloseWindow<UISetting>(); })
			);
		TweenGrouper.AddNode(rootNode);
	}
	private void OnSliderValueChange(float value)
	{
		AudioPlayerSetting.AudioVolume = value;
	}
	private void OnMusicToggleValueChange(bool value)
	{
		AudioPlayerSetting.MusicMute = !value;
	}
	private void OnSoundToggleValueChange(bool value)
	{
		AudioPlayerSetting.SoundMute = !value;
	}

	// 贝塞尔路径
	private Vector2 LerpFun(Vector2 from, Vector2 to, float progress)
	{
		Vector3 control1 = Vector3.one * 500;
		Vector3 control2 = Vector3.one * -500;
		Vector3[] nodes = new Vector3[4] { from, control1, control2, to};

		float t = progress;
		float d = 1f - t;
		return d * d * d * nodes[0] + 3f * d * d * t * nodes[1] + 3f * d * t * t * nodes[2] + t * t * t * nodes[3];
	}
}