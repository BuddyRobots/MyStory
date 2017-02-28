using UnityEngine;
using System.Collections;
using Anima2D;
using MyStory;


public class test_DisplaySprite : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GameObject head = new GameObject("Mosue Head");
		head.AddComponent<SpriteRenderer>();
		head.transform.position = GameObject.Find("shenti").transform.position;

		if (Manager._instance.texture)
		{
			Mouse mouse = new Mouse(Manager._instance.texture);

			head.GetComponent<SpriteRenderer>().sprite = mouse.head.sprite;



			///
			Debug.Log("test_DisplaySprite.cs : flag 1");
			///



		}
		else
		{
			Mouse mouse = new Mouse();
			mouse.head.sprite = GameObject.Find("tou").GetComponent<SpriteMeshInstance>().spriteMesh.sprite;	



			///
			Debug.Log("test_DisplaySprite.cs : flag 2");
			///



			head.GetComponent<SpriteRenderer>().sprite = mouse.head.sprite;
		}

		head.GetComponent<SpriteRenderer>().sortingOrder = 10;
	}
}