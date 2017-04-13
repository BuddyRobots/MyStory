using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectToMove : MonoBehaviour {


	IEnumerator OnMouseDown()  
	{  

		Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);  


		Vector3 offset = transform.position-Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z));  

		Debug.Log("down");  

		//当鼠标左键按下时  
		while(Input.GetMouseButton(0))  
		{  
			//得到现在鼠标的2维坐标系位置  
			Vector3 curScreenSpace =  new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z);     
			//将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动量  
			Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace)+offset;          
			//CurPosition就是物体应该的移动向量赋给transform的position属性        
			transform.position = CurPosition;  
			//这个很主要  
			yield return new WaitForFixedUpdate();  
		}  


	}
}
