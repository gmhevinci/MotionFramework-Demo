using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
	public class EntityPlayer : EntityCharacter
	{
		private BhvCameraFlow _cameraFlow;

		public EntityPlayer(int entityID) : base(entityID)
		{
		}
		protected override void OnCreate()
		{
			base.OnCreate();

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
		protected override void OnPrepareAvatar()
		{
			base.OnPrepareAvatar();

			CharAnim.InitAnimState("idle02", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("idle03", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
			CharAnim.InitAnimState("walk", CharacterAnimation.EAnimationLayer.DefaultLayer, WrapMode.Loop);
		}
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			if (UIJoystick.Joystick != null)
			{
				if (UIJoystick.Joystick.IsDragging)
				{
					CharMove.BeginJoyMove(UIJoystick.Joystick.JoystickAxis);
				}
			}
		}
	}
}