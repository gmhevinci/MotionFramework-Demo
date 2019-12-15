using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityPlayer : EntityCharacter
{
	private BhvCameraFlow _cameraFlow;

	protected override void OnCreate()
	{
		base.OnCreate();

		_cameraFlow = Camera.main.GetComponent<BhvCameraFlow>();
		if (_cameraFlow == null)
			_cameraFlow = Camera.main.gameObject.AddComponent<BhvCameraFlow>();

		_cameraFlow.FlowTarget = _trans;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	protected override void OnPrepare()
	{
		base.OnDestroy();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if (UIJoystick.Joystick != null)
		{
			if(UIJoystick.Joystick.IsDragging)
			{
				Vector2 moveDir = UIJoystick.Joystick.JoystickAxis;
				Vector3 speed = new Vector3(moveDir.x, _trans.position.y, moveDir.y);
				speed.Normalize();
				speed *= Time.deltaTime * -3f;
				_trans.Translate(speed, Space.Self);
			}
		}
	}
}