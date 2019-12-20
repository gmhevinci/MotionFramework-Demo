using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public class EntityCharacter : EntityObject
	{
		public CharacterAnimation CharAnim { private set; get; }
		public CharacterMove CharMove { private set; get; }
		public CharacterData CharData { private set; get; }

		public EntityCharacter(int entityID) : base(entityID)
		{
		}
		protected override void OnCreate()
		{
			// 创建角色数据模块
			CharData = new CharacterData();

			// 创建角色移动模块
			CharMove = new CharacterMove(this);
		}
		protected override void OnDestroy()
		{
		}
		protected override void OnPrepareAvatar()
		{
			// 创建角色动画模块
			var anim = Avatar.GetComponent<Animation>();
			CharAnim = new CharacterAnimation(anim);
			CharAnim.InitAnimState("idle", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("run", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("die", CharacterAnimation.EAnimationLayer.DeadLayer, WrapMode.ClampForever);
		}
		protected override void OnUpdate(float deltaTime)
		{
			CharMove.Update(deltaTime);

			if(_isPrepareAvatar)
			{
				UpdateAnim();
			}	
		}

		private void UpdateAnim()
		{
			if (CharMove.IsMoving)
			{
				CharAnim.SetSpeed(CharData.CurrentRunAnimName, CharData.RunAnimSpeed);
				if (CharAnim.IsPlaying(CharData.CurrentRunAnimName) == false)
					CharAnim.PlayAnim(CharData.CurrentRunAnimName, 0.15f);
			}
			else
			{
				if (CharAnim.IsPlaying(CharData.CurrentIdleAnimName) == false)
					CharAnim.PlayAnim(CharData.CurrentIdleAnimName, 0.15f);

				// 注意：在闪避技能时会有动作问题，需要下面逻辑代码
				if (CharAnim.IsPlaying(CharData.CurrentRunAnimName))
					CharAnim.PlayAnim(CharData.CurrentIdleAnimName, 0.15f);
			}
		}
	}
}