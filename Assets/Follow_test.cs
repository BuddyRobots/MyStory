using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_test : MonoBehaviour 
{

	public Transform target;
	public Vector3 offset;

	void LateUpdate()
	{
		if(target)
		{
			transform.position =new Vector3(target.position.x,0,target.position.z) + offset;
		}
	}
}