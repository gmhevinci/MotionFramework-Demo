using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhvCameraFlow : MonoBehaviour
{
	private Vector3 _pos;

	public Vector3 Offset = new Vector3(0, 12, 10);
	public float Speed = 10;
	public Transform FlowTarget;

	void LateUpdate()
	{
		if(FlowTarget != null)
		{
			_pos = FlowTarget.position + Offset;
			this.transform.position = Vector3.Lerp(this.transform.position, _pos, Speed * Time.deltaTime);//调整相机与玩家之间的距离
			Quaternion angel = Quaternion.LookRotation(FlowTarget.position - this.transform.position);//获取旋转角度
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angel, Speed * Time.deltaTime);
		}
	}
}
