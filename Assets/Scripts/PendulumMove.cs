using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMove : MonoBehaviour 
{
	
	public GameObject target;
	public GameObject leftRegion;
	public GameObject rightRegion;


	private float speed;
	private float angle;
	private float angleOffset;

	private bool toRight;

	void Start () 
	{
		angle =30f;
		angleOffset=5f;
		toRight=false;
		speed=1f;
	}
	

	void Update () 
	{
		if (Input.GetMouseButton(0)) 
		{
			Ray ray=transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast (ray ,out hitInfo )) 
			{
				GameObject gameObj = hitInfo.collider.gameObject;

				if (gameObj==leftRegion) 
				{
					Debug.Log("----left--");
//					SwingRightToLeft();
					StartCoroutine(SwingRightToLeft());

				}
				if (gameObj==rightRegion) 
				{
					Debug.Log("----right--");
//					angle=Random.Range(-90f,-30f);
					transform.RotateAround(target.transform.position,Vector3.forward,-angle);
				}
			}
		}
	}


	IEnumerator SwingRightToLeft()
	{
		speed=1f;

//		angle=Random.Range(30f,90f);
		toRight=true;
		transform.RotateAround(target.transform.position,Vector3.forward,angle);
		yield return new WaitForSeconds(0.2f);  

		for (float i =angle; i >=-angle; i-=5)
		{
			Debug.Log("*****");
			toRight=!toRight;
			if (toRight) 
			{
				Debug.Log("--to right--");
				transform.RotateAround(target.transform.position,Vector3.forward,angle+i);

			}
			else
			{
				Debug.Log("--to left--");
				transform.RotateAround(target.transform.position,Vector3.forward,-(angle+i));

			}
			speed+=0.5f;
			yield return new WaitForSeconds(1f/speed);  
		}
	}

}
