using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Audio;

public static class AudioPlayerSetting
{
	private const string StrAudioVolumeKey = "StrAudioVolumeKey";
	private const string StrMusicMuteKey = "StrMusicMuteKey";
	private const string StrSoundMuteKey = "StrSoundMuteKey";


	public static void InitAudioSetting()
	{
		MusicMute = GameUtility.PlayerGetBool(StrMusicMuteKey, false);
		SoundMute = GameUtility.PlayerGetBool(StrSoundMuteKey, false);
		AudioVolume = PlayerPrefs.GetFloat(StrAudioVolumeKey, 1f);
	}
	public static float AudioVolume
	{
		get
		{
			return PlayerPrefs.GetFloat(StrAudioVolumeKey, 1f);
		}
		set
		{
			PlayerPrefs.SetFloat(StrAudioVolumeKey, value);
			AudioManager.Instance.Volume(value);
		}
	}
	public static bool MusicMute
	{
		get
		{
			return GameUtility.PlayerGetBool(StrMusicMuteKey, false);
		}
		set
		{
			GameUtility.PlayerSetBool(StrMusicMuteKey, value);
			AudioManager.Instance.Mute(EAudioLayer.Music, value);
			AudioManager.Instance.Mute(EAudioLayer.Ambient, value);
			AudioManager.Instance.Mute(EAudioLayer.Voice, value);
		}
	}
	public static bool SoundMute
	{
		get
		{
			return GameUtility.PlayerGetBool(StrSoundMuteKey, false);
		}
		set
		{
			GameUtility.PlayerSetBool(StrSoundMuteKey, value);
			AudioManager.Instance.Mute(EAudioLayer.Sound, value);
		}
	}
}