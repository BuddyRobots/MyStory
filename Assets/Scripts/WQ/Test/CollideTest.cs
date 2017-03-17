using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Debug.Log(".....");
	}
	

	void Update () 
	{
		transform.Translate(Vector3.left*Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log(col.gameObject.name);
		if (col.gameObject.tag=="ClickObj") {
			Debug.Log("collide the ball");
		}
	}


}
