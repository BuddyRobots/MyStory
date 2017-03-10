using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveTest : MonoBehaviour 
{
	float speed=5f;


	void Start () {
		
	}
	

	void Update () 
	{
		if (Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector3.left*Time.deltaTime*speed);

		}
	}
}
