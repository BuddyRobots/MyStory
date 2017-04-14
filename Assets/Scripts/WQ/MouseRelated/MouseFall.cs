using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFall : MonoBehaviour {

	public void Fall()
	{
		LevelThree._instance.mouseFall=true;

		Debug.Log("老鼠开始掉出屏幕");

	}
}
