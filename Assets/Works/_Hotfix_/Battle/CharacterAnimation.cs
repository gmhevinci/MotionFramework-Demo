using UnityEngine;

namespace Hotfix
{
	public class CharacterAnimation
	{
		/// <summary>
		/// 动作层级
		/// </summary>
		public enum EAnimationLayer
		{
			DefaultLayer = 1, //默认层级
			SkillLayer = 1, //技能层级
			StateLayer = 3, //状态层级
			BornLayer = 8, //出生层级
			DeadLayer = 9, //死亡层级
			TopLayer = 10, //最高层级
		}

		private Animation _animation = null;
		private bool _isPauseAll = false;


		public CharacterAnimation(Animation anim)
		{
			if (anim == null)
			{
				HotfixLogger.Warning("Animation is null.");
				return;
			}
			_animation = anim;
		}

		/// <summary>
		/// 初始化动画状态
		/// </summary>
		/// <param name="name">动画名称</param>
		/// <param name="layer">动画层级</param>
		/// <param name="wrapMode">模式</param>
		public void InitAnimState(string name, EAnimationLayer layer, WrapMode wrapMode)
		{
			if (_animation == null)
				return;
			AnimationState state = _animation[name];
			if (state != null)
			{
				state.layer = (int)layer;
				state.wrapMode = wrapMode;
			}
		}

		/// <summary>
		/// 模拟动画
		/// </summary>
		public void SampleAnim(string name, float progress)
		{
			if (_animation == null)
				return;
			AnimationState state = _animation[name];
			if (state != null && state.length > 0)
			{
				state.normalizedTime = Mathf.Clamp01(progress / state.length);
				_animation.Sample();
			}
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		public void PlayAnim(string name, float normalizedTime = 0, float fadeLength = 0.1f, PlayMode mode = PlayMode.StopSameLayer)
		{
			if (_animation == null)
				return;
			if (_isPauseAll)
				return;
			AnimationState state = _animation[name];
			if (state != null)
			{
				state.normalizedTime = normalizedTime;
				_animation.CrossFade(name, fadeLength, mode);
			}
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		public void CrossFadeQueued(string animation, float fadeLength = 0.3f, QueueMode queue = QueueMode.CompleteOthers, PlayMode mode = PlayMode.StopSameLayer)
		{
			if (_animation == null)
				return;
			if (_isPauseAll)
				return;
			AnimationState state = _animation[animation];
			if (state != null)
			{
				_animation.CrossFadeQueued(animation, fadeLength, queue, mode);
			}
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
			AnimationState state = _animation[name];
			return state != null;
		}
		public void StopAnim(string name)
		{
			if (_animation == null)
				return;
			if (_animation != null)
				_animation.Stop(name);
		}
		public void StopAllAnim(EAnimationLayer layer)
		{
			if (_animation == null)
				return;
			foreach (AnimationState state in _animation)
			{
				if (state.layer == (int)layer)
					state.enabled = false;
			}
		}
		public void StopAllAnim()
		{
			if (_animation == null)
				return;
			if (_animation != null)
				_animation.Stop();
		}
		
		public void PauseAllAnim()
		{
			_isPauseAll = true;

			// 暂停所有动作
			StopAllAnim();
		}
		public void ResumeAllAnim()
		{
			_isPauseAll = false;
		}

		public void SetSpeed(string name, float speed)
		{
			if (_animation == null)
				return;
			if (_isPauseAll)
				return;
			AnimationState state = _animation[name];
			if (state != null)
				state.speed = speed;
		}
		public void SetNormalizedTime(string name, float normalizedTime)
		{
			if (_animation == null)
				return;
			if (_isPauseAll)
				return;
			AnimationState state = _animation[name];
			if (state != null)
				state.normalizedTime = normalizedTime;
		}
		
		private void AddMixing(string name, Transform mix)
		{
			if (_animation == null)
				return;
			AnimationState state = _animation[name];
			if (state != null)
			{
				state.AddMixingTransform(mix);
			}
		}
	}
}