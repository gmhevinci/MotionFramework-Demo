using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove
{
	private const float CompareEpsilon = 0.000001f;
	private const float RotateSpeed = 20f;

	private readonly EntityCharacter _owner;
	private readonly Transform _trans;

	// 上帧位置
	private Vector3 _lastFramePosition = Vector3.zero;

	// 旋转角度
	private Vector3 _lookDir = Vector3.zero;
	private float _currentAngle = 0f;
	private float _targetAngle = 0f;

	/// <summary>
	/// 是否在移动
	/// </summary>
	public bool IsMoving { private set; get; }


	public CharacterMove(EntityCharacter owner)
	{
		_owner = owner;
		_trans = owner.Root.transform;
	}
	public void Update(float deltaTime)
	{
		CheckMoving();
		UpdateRotate(deltaTime);
	}

	#region 角色旋转
	// NOTE：给客户端发送的注视方向，为了和服务器一致，客户端不要做限制，一定要差值完成。
	private void UpdateRotate(float deltaTime)
	{
		float deltaAngle = _currentAngle - _targetAngle;
		if (Mathf.Abs(deltaAngle) > 1f)
		{
			_trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(_lookDir), deltaTime * RotateSpeed);
			_currentAngle = _trans.rotation.eulerAngles.y;
		}
	}
	private void LookDirection(Vector3 lookDir)
	{
		_lookDir = CorrectDirection(lookDir);
		_targetAngle = Quaternion.LookRotation(_lookDir).eulerAngles.y;
	}
	private Vector3 CorrectDirection(Vector3 dir)
	{
		dir.y = 0;
		dir.Normalize();
		return dir;
	}
	#endregion

	#region 玩家控制逻辑
	public void BeginJoyMove(Vector3 joyDir)
	{
		joyDir.y = 0;
		joyDir.Normalize();

		if (IsZeroVector(joyDir))
			return;

		// 注视方向
		if (IsCanRotate())
			LookDirection(joyDir);

		if (IsCanMove() == false)
			return;

		// 位置移动
		joyDir *= _owner.CharData.MoveSpeed * Time.deltaTime;
		_trans.Translate(joyDir, Space.World);
	}
	#endregion

	private void CheckMoving()
	{
		Vector3 tempPos = _trans.position;
		IsMoving = !IsVectorEqual(tempPos, _lastFramePosition);
		_lastFramePosition = tempPos;
	}
	private bool IsCanRotate()
	{
		return _owner.CharData.IsCanRotate();
	}
	private bool IsCanMove()
	{
		return _owner.CharData.IsCanMove();
	}

	private bool IsVectorEqual(Vector3 l, Vector3 r)
	{
		float x = l.x - r.x;
		if (x > CompareEpsilon || x < -CompareEpsilon)
			return false;

		float z = l.z - r.z;
		if (z > CompareEpsilon || z < -CompareEpsilon)
			return false;

		return true;
	}
	private bool IsZeroVector(Vector3 v)
	{
		if (v.sqrMagnitude < CompareEpsilon)
			return true;
		else
			return false;
	}
	private bool IsZeroVector(Vector2 v)
	{
		if (v.sqrMagnitude < CompareEpsilon)
			return true;
		else
			return false;
	}
}