using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Fall()
	{
		LevelThree._instance.mouseFall=true;

		Debug.Log("老鼠开始掉出屏幕");

	}
}
