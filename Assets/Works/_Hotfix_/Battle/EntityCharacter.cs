using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Audio;

namespace Hotfix
{
	public abstract class EntityCharacter : EntityObject
	{
		public CharacterAnimation CharAnim { private set; get; }	
		public CharacterData CharData { private set; get; }		
		public CharacterSkill CharSkill { private set; get; }
		public CharacterMove CharMove { private set; get; }

		public EntityCharacter(int entityID) : base(entityID)
		{
		}
		protected override void OnCreate()
		{
			CharData = new CharacterData(this);
			CharSkill = new CharacterSkill(this);
			CharMove = new CharacterMove(this);
		}
		protected override void OnDestroy()
		{
		}	
		protected override void OnUpdate(float deltaTime)
		{
			CharData.Update(deltaTime);
			CharSkill.Update(deltaTime);
			CharMove.Update(deltaTime);
		}
		protected override void OnPrepareAvatar()
		{
			// 创建角色动画模块
			var anim = Avatar.GameObj.GetComponent<Animation>();
			CharAnim = new CharacterAnimation(anim);		
			CharAnim.InitAnimState("idle", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("run", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("getHit", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Once);
			CharAnim.InitAnimState("die", CharacterAnimation.EAnimationLayer.DeadLayer, WrapMode.ClampForever);
		}
		protected override void OnUpdateAvatar(float deltaTime)
		{
			if (CharSkill.IsAnyLife())
				return;

			if (CharMove.IsMoving)
			{
				float animSpeed = CharData.MoveSpeed / Avatar.GetRunAnimationSpeed();
				CharAnim.SetSpeed(CharData.CurrentRunAnimName, animSpeed);
				if (CharAnim.IsPlaying(CharData.CurrentRunAnimName) == false)
					CharAnim.PlayAnim(CharData.CurrentRunAnimName, 0.15f);
			}
			else
			{
				if (CharAnim.IsPlaying(CharData.CurrentIdleAnimName) == false)
					CharAnim.PlayAnim(CharData.CurrentIdleAnimName, 0.15f);
			}
		}
		protected override void OnHandleEvent(IHotfixEventMessage msg)
		{
			if (msg is BattleEvent.DamageHurt)
			{
				if (CharData.IsDead == false)
				{
					BattleEvent.DamageHurt message = msg as BattleEvent.DamageHurt;
					CharData.DamageHurt(message.Damage);
					CharAnim.PlayAnim("getHit");

					// 随机播放角色受击音效
					string soundName = Avatar.GetRandomGetHitSound();
					if (string.IsNullOrEmpty(soundName) == false)
						AudioManager.Instance.PlaySound(soundName);
				}
			}
			else if(msg is BattleEvent.CharacterDead)
			{
				CharSkill.ForbidAll();
				CharAnim.PlayAnim("die");

				// 播放角色死亡音效
				string soundName = Avatar.GetDeadSound();
				if (string.IsNullOrEmpty(soundName) == false)
					AudioManager.Instance.PlaySound(soundName);
			}
		}
	}
}