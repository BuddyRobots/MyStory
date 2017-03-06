using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnDrag(Vector2 delta)
	{
		Debug .Log ("//////");
		Ray ray = UICamera.current.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(transform.forward, transform.position);
		float dist;
		if (plane.Raycast(ray,out dist))
		{
			Vector3 worldPos = ray.GetPoint(dist);
			transform.position = worldPos;
		}


	}
}
