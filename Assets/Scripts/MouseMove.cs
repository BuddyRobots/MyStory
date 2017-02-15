using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{

	public Transform net;

	void Update () 
	 {
		 if(Input.GetMouseButtonDown(0))
		 {
			
			Ray ray = transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);//Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
	        RaycastHit hitInfo;
	        if(Physics.Raycast(ray,out hitInfo))
	        {
	             GameObject gameObj = hitInfo.collider.gameObject;
	             if(gameObj.tag == "Mouse")//当射线碰撞目标是老鼠
		         {
					StartCoroutine(OnMouseDown());
		         }
	        }
		 }
	}
		
	IEnumerator OnMouseDown()
	{
		while (Input.GetMouseButton(0)) 
		{
			Vector3 worldPos = UICamera.mainCamera.ScreenToWorldPoint (Input.mousePosition);
			Vector3 localPos = net.InverseTransformPoint (worldPos);

			transform.localPosition = localPos;
			yield return new WaitForFixedUpdate();  
		}
	}





}
