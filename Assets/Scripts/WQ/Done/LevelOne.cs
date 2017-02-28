using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour 
{
	private Animation ani;

	void Start () 
	{
		ani=transform.Find("Grass").GetComponent<Animation>();
	}
	

	void Update () 
	{
		ClickTheGrass();
	}


	void ClickTheGrass()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					if (BussinessManager._instance.finger!=null) 
					{
						Destroy(BussinessManager._instance.finger);
						Debug.Log("销毁了小手");
						PlayAnimation();
					}
				}
			}
		}
	}

	void PlayAnimation()
	{
		ani.Play("GrassScale");
	}




}
