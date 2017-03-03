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
		head.transform.position = GameObject.Find("Head").transform.position;

		GameObject body = new GameObject("Mosue Body");
		body.AddComponent<SpriteRenderer>();
		body.transform.position = GameObject.Find("Torso").transform.position;

		GameObject arm = new GameObject("Mosue Arm");
		arm.AddComponent<SpriteRenderer>();
		arm.transform.position = GameObject.Find("R hand").transform.position;

		GameObject imageTex = new GameObject("Image Texture");
		imageTex.AddComponent<SpriteRenderer>();
		imageTex.transform.position = GameObject.Find("Torso").transform.position - new Vector3(7, 0, 0);



		Texture2D tex = Manager._instance.texture;
		if (tex)
		{
			Mouse mouse = new Mouse(tex);

			head.GetComponent<SpriteRenderer>().sprite = mouse.head.sprite;
			body.GetComponent<SpriteRenderer>().sprite = mouse.body.sprite;
			arm.GetComponent<SpriteRenderer>().sprite = mouse.leftArm.sprite;


			Mat displayImage = mouse.image;
			Texture2D displayTex = new Texture2D(displayImage.width(), displayImage.height());
			Utils.matToTexture2D(mouse.image, displayTex);
			imageTex.GetComponent<SpriteRenderer>().sprite = Sprite.Create(displayTex, new UnityEngine.Rect(0.0f,0.0f,displayTex.width,displayTex.height), new Vector2(0.5f,0.5f), 100.0f);



			///
			Debug.Log("test_DisplaySprite.cs : flag 1");
			///



		}
		else
		{
			Mouse mouse = new Mouse();
			mouse.head.sprite = GameObject.Find("mouse-head").GetComponent<SpriteMeshInstance>().spriteMesh.sprite;	



			///
			Debug.Log("test_DisplaySprite.cs : flag 2");
			///



			head.GetComponent<SpriteRenderer>().sprite = mouse.head.sprite;
		}

		head.GetComponent<SpriteRenderer>().sortingOrder = 10;
		body.GetComponent<SpriteRenderer>().sortingOrder = 10;
		arm.GetComponent<SpriteRenderer>().sortingOrder = 10;
		imageTex.GetComponent<SpriteRenderer>().sortingOrder = 10;
	}
}