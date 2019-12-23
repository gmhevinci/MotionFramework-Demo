using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
	public class EntityPlayer : EntityCharacter
	{
		private readonly EHeroType _heroType;
		private BhvCameraFlow _cameraFlow;

		public EntityPlayer(int entityID, EHeroType heroType) : base(entityID)
		{
			_heroType = heroType;
		}
		protected override void OnCreate()
		{
			base.OnCreate();

			// 初始化角色数据
			CfgPlayerTab table = CfgPlayer.Instance.GetCfgTab((int)_heroType);
			CharData.InitData(table.BodyRadius, table.MoveSpeed, table.Hp, table.Mp, table.Damage, table.Armor);

			// 跟随相机脚本
			_cameraFlow = Camera.main.GetComponent<BhvCameraFlow>();
			if (_cameraFlow == null)
				_cameraFlow = Camera.main.gameObject.AddComponent<BhvCameraFlow>();
			_cameraFlow.FlowTarget = Root.transform;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_cameraFlow != null)
				_cameraFlow.FlowTarget = null;
		}
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			if (UIJoystick.Joystick != null)
			{
				if (CharSkill.IsAnyLife())
					return;
				if (UIJoystick.Joystick.IsDragging)
				{
					Vector2 joyAxis = UIJoystick.Joystick.JoystickAxis;
					Vector3 joyDir = Camera.main.transform.forward * joyAxis.y + Camera.main.transform.right * joyAxis.x;
					CharMove.BeginJoyMove(joyDir);
				}
			}
		}
		protected override void OnPrepareAvatar()
		{
			base.OnPrepareAvatar();

			CharAnim.InitAnimState("idle02", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("idle03", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("walk", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
		}
		protected override void OnHandleEvent(IHotfixEventMessage msg)
		{
			base.OnHandleEvent(msg);

			if (msg is BattleEvent.PlayerSpell)
			{
				BattleEvent.PlayerSpell message = msg as BattleEvent.PlayerSpell;
				CharSkill.Spell(message.SkillID);
			}
		}
	}
}