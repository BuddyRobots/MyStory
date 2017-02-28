using UnityEngine;
using System.Collections;
using Anima2D;
using MyStory;
using OpenCVForUnity;


public class test_DisplaySprite : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GameObject head = new GameObject("Mosue Head");
		head.AddComponent<SpriteRenderer>();
		head.transform.position = GameObject.Find("tou2").transform.position;

		GameObject body = new GameObject("Mosue Body");
		head.AddComponent<SpriteRenderer>();
		head.transform.position = GameObject.Find("shenti").transform.position;

		GameObject arm = new GameObject("Mosue Arm");
		head.AddComponent<SpriteRenderer>();
		head.transform.position = GameObject.Find("shenti").transform.position;



		Texture2D tex = Manager._instance.texture;
		if (tex)
		{
			Mouse mouse = new Mouse(tex);

			//head.GetComponent<SpriteRenderer>().sprite = mouse.head.sprite;

			Mat displayImage = mouse.image;
			Texture2D displayTex = new Texture2D(displayImage.width(), displayImage.height());
			Utils.matToTexture2D(mouse.image, displayTex);

			head.GetComponent<SpriteRenderer>().sprite = Sprite.Create(displayTex, new UnityEngine.Rect(0.0f,0.0f,displayTex.width,displayTex.height), new Vector2(0.5f,0.5f), 100.0f);



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