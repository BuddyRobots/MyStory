using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//包括小手的显示
public class BussinessManager : MonoBehaviour 
{

	public static BussinessManager _instance;
	[HideInInspector]
	public GameObject finger;
	private GameObject fingerPrefab;
	private Vector3 offSet = new Vector3 (1.3f, -0.7f, 0);//这个值表示小手距离需要点击的对象的距离（对象Position+offset=小手Position）
	[HideInInspector]
	public Vector3 prePos= Vector3.zero; // 记录当前指向的位置，如果没发生变化，不做任何操作


	private GameObject sceneParent;

	void Awake()
	{
		_instance=this;

		fingerPrefab=Resources.Load("Prefab/Finger",typeof(GameObject)) as GameObject;
		sceneParent=GameObject.Find("SceneParent");
	}

	void Start () 
	{

	}
	

	public void ShowFinger(Vector3 pos)
	{
//		Debug.Log("----出现小手");
		if (finger) 
		{
			Destroy (finger);
			finger = null;
		}
		finger = Instantiate (fingerPrefab) as GameObject;
		finger.name="finger";
		finger.transform.parent = sceneParent.transform;
		finger.transform.localScale = Vector3.one;
		finger.GetComponent<FingerCtrl> ().FingerShow (pos + offSet);
	}

}
