using UnityEngine;
using MotionFramework.Experimental.Animation;

public class CharacterAnimation
{
	private AnimBehaviour _animation = null;

	public CharacterAnimation(AnimBehaviour anim)
	{
		if (anim == null)
		{
			GameLog.Warning("Animation is null.");
			return;
		}
		_animation = anim;
	}

	/// <summary>
	/// 模拟动画
	/// </summary>
	public void Sample(string name, float progress)
	{
		if (_animation == null)
			return;
		AnimState state = _animation.GetState(name);
		if (state != null && state.ClipLength > 0)
		{
			state.NormalizedTime = Mathf.Clamp01(progress / state.ClipLength);
		}
	}

	/// <summary>
	/// 播放动画
	/// </summary>
	public void Play(string name, float fadeLength = 0.15f, float normalizedTime = 0)
	{
		if (_animation == null)
			return;
		AnimState state = _animation.GetState(name);
		if (state != null)
		{
			state.NormalizedTime = normalizedTime;
			_animation.Play(name, fadeLength);
		}
	}

	/// <summary>
	/// 停止动画
	/// </summary>
	public void Stop(string name)
	{
		if (_animation == null)
			return;
		if (_animation != null)
			_animation.Stop(name);
	}

	public bool IsPlaying(string name)
	{
		if (_animation == null)
			return false;
		return _animation.IsPlaying(name);
	}
	public bool IsContains(string name)
	{
		if (_animation == null)
			return false;
		return _animation.IsContains(name);
	}
	public void SetSpeed(string name, float speed)
	{
		if (_animation == null)
			return;
		AnimState state = _animation.GetState(name);
		if (state != null)
			state.Speed = speed;
	}
	public void SetNormalizedTime(string name, float normalizedTime)
	{
		if (_animation == null)
			return;
		AnimState state = _animation.GetState(name);
		if (state != null)
			state.NormalizedTime = normalizedTime;
	}
}