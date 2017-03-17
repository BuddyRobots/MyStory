using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {
	
	public Transform center;

	private float radius;

	void Start()
	{
		StartCoroutine (BeginPendulum ());
	}

	IEnumerator BeginPendulum()
	{
		radius = (transform.position - center.position).magnitude;
		float angle = Vector3.Angle (Vector3.down, transform.position - center.position);
		bool isLeft = JudgeLeft ();
		angle *= isLeft ? -1 : 1;
		Place (angle);
		float temp_Angle = angle;
		while (true)
		{
			float tempData = Mathf.Abs(angle/temp_Angle);
			tempData = tempData > 2 ? 2 : tempData;
			if (Mathf.Abs(temp_Angle) + tempData<= angle+2) 
			{ 
				// 如果是左，往右走，就是加；如果是右，往左走，就是减
				if (isLeft) 
				{
					temp_Angle += tempData;
				} 
				else 
				{
					temp_Angle -= tempData;
				}
				Place (temp_Angle);
			} 
			else 
			{
				isLeft ^= true;
				temp_Angle += isLeft ? tempData : - tempData;
				angle-=1;//缩小摆动的角度
				if (angle <0) 
				{
					angle=0;
				}
			}
			print (isLeft);
			yield return new WaitForFixedUpdate ();
		}
	}

	void Place(float _angle)
	{
		//  限制在-90到90度
		//  左为负，右为正
		_angle = _angle < -90?-90 : _angle;
		_angle = _angle > 90 ? 90 : _angle;
		float x = radius * Mathf.Sin(_angle * 3.1415926f / 180);
		float y = radius * Mathf.Cos (_angle * 3.1415926f / 180);
		Vector3 tempPos = new Vector3 (center.position.x + x, center.position.y - y, transform.position.z);
		transform.position = tempPos;
	}

	bool JudgeLeft()//判断是在左边还是在右边，左边为TRUE
	{
		return Vector3.Dot (Vector3.forward, Vector3.Cross (Vector3.down, transform.position - center.position)) < 0;
	}
}

