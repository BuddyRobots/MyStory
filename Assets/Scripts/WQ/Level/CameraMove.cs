using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour 
{
	
	[HideInInspector]
	public bool move;
	private Vector3 destPos;

	void Start () {
		destPos=new Vector3 (-9.8f,0,-10);
	}
	

	void Update () 
	{
		if (move) 
		{
			transform.Translate(Vector3.left*Time.deltaTime);

		}
	}
}
