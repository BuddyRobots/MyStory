using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColor2D : MonoBehaviour 
{
	public GameObject net;
	public GameObject mouse;

	void Start () {
		
	}
	

	void Update () 
	{
		if (Input.GetMouseButton (0))
		{
//			CheckPointToEraseColor (Input.mousePosition);
		}
	}

//	bool CheckPointToEraseColor (Vector3 pScreenPos)
//	{
//		Vector3 worldPos = Camera.main.ScreenToWorldPoint (pScreenPos);//鼠标的屏幕坐标--->世界坐标
//		Vector3 localPosInNet=net.transform.InverseTransformPoint(worldPos);//鼠标的世界坐标 在 网 的本地坐标
//		Vector3 localPosInMouse=mouse.transform.InverseTransformPoint(worldPos);//鼠标的世界坐标 在 老鼠 的本地坐标
//
//
//
//	}
}
