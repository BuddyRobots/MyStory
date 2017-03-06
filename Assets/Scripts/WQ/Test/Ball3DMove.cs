using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball3DMove : MonoBehaviour {


	private Rigidbody rig;

	public float speed;

	void Start () 
	{
		rig=gameObject.GetComponent<Rigidbody>();
	}
	


	void FixedUpdate () 
	{
		float moveHorizontal=Input.GetAxis("Horizontal");
		float moveVertical=Input.GetAxis("Vertical");

		Vector3 movement=new Vector3(moveHorizontal,0f,moveVertical);
		rig.AddForce(movement*speed*Time.deltaTime);


	}
}
