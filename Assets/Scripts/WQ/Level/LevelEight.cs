using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEight : MonoBehaviour 
{
	
	
	GameObject mouse;
	Animator mouseAnimator;
	Camera cam;

	public Transform originMousePos;

	void Start () 
	{
		//下一步按钮隐藏


		cam=GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	

	void Update ()
	{


		if (FormalScene._instance.storyBegin) 
		{



			if (Input.GetMouseButton(0))
			{

				RaycastHit hit;

				if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
				{
					Renderer rend = hit.transform.GetComponent<Renderer>();
					MeshCollider meshCollider = hit.collider as MeshCollider;

					if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
						return;

					Texture2D tex = rend.material.mainTexture as Texture2D;
					Vector2 pixelUV = hit.textureCoord;
					pixelUV.x *= tex.width;
					pixelUV.y *= tex.height;

					tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
					tex.Apply();

				}




			}
				









		}






	}



	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置

	}
	void ShowMouse()
	{
		if (mouse ==null) 
		{
			mouse=Manager._instance.mouseGo;
		}
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("idle",0);

		if (mouse.GetComponent<Rigidbody2D>()!=null) 
		{
			mouse.GetComponent<Rigidbody2D>().simulated=true;
		}

	}

	void OnDisable()
	{
		Manager._instance.Reset();

	}

}
