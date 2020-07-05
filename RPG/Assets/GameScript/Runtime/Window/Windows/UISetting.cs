using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Audio;
using MotionFramework.Window;
using MotionFramework.Utility;
using MotionFramework.Flow;

[Window((int)EWindowLayer.Panel, true)]
sealed class UISetting : CanvasWindow
{
	private Slider _volumeSlider;
	private Toggle _musicToggle;
	private Toggle _soundToggle;

	private Transform _animTrans;
	private float _animTimer = 0;

	public override void OnCreate()
	{
		WindowOpenAnimationTime = 0.25f;

		AddButtonListener("UISetting/Mask", OnClickClose);
		AddButtonListener("UISetting/Window/Button (Save)", OnClickClose);

		_volumeSlider = GetUIComponent<Slider>("UISetting/Window/Content/Control Area (Slider)/Slider");
		_volumeSlider.onValueChanged.AddListener(OnSliderValueChange);
		_musicToggle = GetUIComponent<Toggle>("UISetting/Window/Content/Control Area (Toggles)/Toggle 1");
		_musicToggle.onValueChanged.AddListener(OnMusicToggleValueChange);
		_soundToggle = GetUIComponent<Toggle>("UISetting/Window/Content/Control Area (Toggles)/Toggle 2");
		_soundToggle.onValueChanged.AddListener(OnSoundToggleValueChange);

		_animTrans = GetUIElement("UISetting/Window");
		_animTrans.transform.localScale = Vector3.zero;
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
		if(_animTimer < 0.01f)
		{
			IFlowNode rootNode = TimerNode.AllocateDuration(0, 0.25f, () => { _animTrans.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animTimer / 0.25f); });
			FlowGrouper.AddNode(rootNode);
		}
	}
	public override void OnUpdate()
	{
		_animTimer += Time.deltaTime;
	}

	private void OnClickClose()
	{
		// 窗口关闭动画
		_animTimer = 0;
		IFlowNode rootNode = SequenceNode.Allocate(
			TimerNode.AllocateDuration(0, 0.25f, () => { _animTrans.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, _animTimer / 0.25f); }),
			ExecuteNode.Allocate(() => { UITools.CloseWindow<UISetting>(); })
			);
		FlowGrouper.AddNode(rootNode);
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
}