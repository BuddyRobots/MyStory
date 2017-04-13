using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Fall()
	{
		LevelThree._instance.ballFall=true;
		Debug.Log("球开始掉出屏幕");
	}
}
