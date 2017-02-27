using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{

	public Transform rotateCenter;


	void Update () 
	{
		transform.Rotate(new Vector3(1,1,0));
		transform.RotateAround(rotateCenter.position,rotateCenter.up,1f);
	
	}
}
