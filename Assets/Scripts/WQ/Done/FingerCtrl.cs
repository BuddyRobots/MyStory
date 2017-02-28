using UnityEngine;
using System.Collections;

public class FingerCtrl : MonoBehaviour 
{
	private float speed = 4;

	//posTo，posFrom这两个值的意义在于用来计算手指需要移动的距离---moveOffset
	Vector3 posTo=new Vector3(2.2f,-0.7f,0);
	Vector3 posFrom=new Vector3(3.0f,-1.3f,0);

	private Vector3 dest;
	private Vector3 startPos;
	private Vector3 moveOffset;//手指需要移动的距离----posFrom-posTo  
	private Vector3 dir;

	void OnEnable()
	{
		moveOffset=new Vector3((posFrom-posTo).x,(posFrom-posTo).y,0);
	} 


	void Start () 
	{
		speed = 4f;
	}



	//出现手指，需要传入坐标
	public void FingerShow(Vector3 fingerPos)
	{
		transform.localPosition = fingerPos;
		startPos=fingerPos;
		dest =startPos - moveOffset;
		StartCoroutine (FingerMove(startPos, dest));
	}


	IEnumerator FingerMove(Vector3 start, Vector3 end) 
	{
		
		while (Vector3.Distance(transform.localPosition , end) > 0.1f) //这里小手不放在UI层，距离判断取值为0.1f,小手放在UI层的话距离取值为1合适
		{
			

			transform.localPosition = Vector3.Lerp (transform.localPosition, end, speed * Time.deltaTime);
			yield return new WaitForFixedUpdate ();
		}
		yield return new WaitForSeconds (0.1f);

		while (Vector3.Distance (transform.localPosition, start) >0.1f) 
		{


			transform.localPosition = Vector3.Lerp (transform.localPosition, start, speed * Time.deltaTime);
			yield return new WaitForFixedUpdate ();
		}
		StartCoroutine (FingerMove (start, end));//协同递归，保证一直移动
	}

}
