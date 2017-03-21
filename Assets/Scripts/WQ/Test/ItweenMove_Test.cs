using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItweenMove_Test : MonoBehaviour {


public 	bool move;
	bool arrived;

	void Start () {

//		gameObject.GetComponent<Animator>().SetTrigger("walkIn");
//		iTween.MoveTo(gameObject, new Vector3(transform.position.x-7,transform.position.y,transform.position.z),4.1f);
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (move) 
		{
			if (!arrived) 
			{
				gameObject.GetComponent<Animator>().SetTrigger("walkIn");
				iTween.MoveTo(gameObject, new Vector3(transform.position.x-7,transform.position.y,transform.position.z),4.1f);

				arrived=true;
			}


		}



	}
}
