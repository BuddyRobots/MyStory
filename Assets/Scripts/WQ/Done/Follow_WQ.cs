using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Follow_WQ : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;

	void LateUpdate()
	{
		if(target)
		{
			transform.position = new Vector3((target.position + offset).x,0,(target.position + offset).z);
		}
	}
}
