using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhvCameraFlow : MonoBehaviour
{
	private Transform _trans;

	/// <summary>
	/// 偏移
	/// </summary>
	public Vector3 Offset = new Vector3(0, 12, 10);

	/// <summary>
	/// 相机跟随目标
	/// </summary>
	public Transform FlowTarget;

	public Transform CameraTransform
	{
		get
		{
			return _trans;
		}
	}

	private void Awake()
	{
		_trans = this.transform;
	}
	void LateUpdate()
	{
		if(FlowTarget != null)
		{
			_trans.position = FlowTarget.position + Offset;
			_trans.rotation = Quaternion.LookRotation(FlowTarget.position - _trans.position, Vector3.up);
		}
	}
}
