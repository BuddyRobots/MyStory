using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour {


	bool stopAni=false;

	void Start () 
	{

		GetComponent<Animation>().Play("GrassScale");
		
	}
	

	void Update () 
	{
		ClickTheGrass2D();
		if (stopAni) {
			GetComponent<Animation>().Stop("GrassScale");
		}
	}

	void ClickTheGrass2D()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					Debug.Log("点击了草----");
					stopAni=true;
				}


			}
		}

	}
}
