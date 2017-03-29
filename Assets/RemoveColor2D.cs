using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColor2D : MonoBehaviour 
{
	public Camera cam;
	Texture2D newTex;

	public GameObject mouse;
	public GameObject net;


	bool mouseClicked;
	bool netClicked;


	Color colour=new Color(0,0,0,0);

	private int netTexWidth;
	private int netTexHeight;
	int brushSize=100;

	private Texture2D netTex2D;
	SpriteRenderer netRender;



	void Start()
	{
		cam = GetComponent<Camera>();
		newTex=null;

		netRender=net.GetComponent<SpriteRenderer>();
		netTex2D=net.GetComponent<SpriteRenderer>().sprite.texture;
		netTexWidth=net.GetComponent<SpriteRenderer>().sprite.texture.width;
		netTexHeight=net.GetComponent<SpriteRenderer>().sprite.texture.height;

	}


	void Update()
	{


		if (Input.GetMouseButton(0))
		{
			Ray myRay=Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit2D hit=Physics2D.Raycast(new Vector2(myRay.origin.x,myRay.origin.y),Vector2.down);
			if (hit.collider) 
			{
				if (hit.collider.name=="net") 
				{

					Debug.Log("hit point--"+hit.point);
					Debug.Log("hit point in net --"+net.transform.InverseTransformPoint(hit.point));




					if (newTex==null) 
					{
						newTex = new Texture2D (netTex2D.width, netTex2D.height, TextureFormat.ARGB32, false);
						newTex.SetPixels32(netTex2D.GetPixels32());
					}

//					for (int k = -brushSize; k <brushSize; k++) 
//					{
//						for (int j = -brushSize; j < brushSize; j++)
//						{
//							
//							newTex.SetPixel((int)net.transform.InverseTransformPoint(hit.point).x+k, (int)  net.transform.InverseTransformPoint(hit.point).y+j, colour);
////							newTex.SetPixel((int)  Input.mousePosition.x+k, (int)  Input.mousePosition.y+j, colour);
//						}
//					}
					newTex.SetPixel((int)(net.transform.InverseTransformPoint(hit.point).x), (int) (net.transform.InverseTransformPoint(hit.point).y), colour);
					Debug.Log("x:"+(int)net.transform.InverseTransformPoint(hit.point).x+" , y:"+(int)net.transform.InverseTransformPoint(hit.point).y);


					newTex.Apply();
					netRender.sprite = Sprite.Create(newTex, netRender.sprite.rect, new Vector2(0.5f, 0.5f));


				}
			}
		}
















		/*
		if (Input.GetMouseButton(0))
		{
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 0;

			Debug.Log("worldPos--"+ worldPos);
			Debug.Log("Input.mousePosition--"+ Input.mousePosition);

			Collider2D[] col = Physics2D.OverlapPointAll(worldPos);
			if(col.Length > 0)
			{
				for (int i = 0; i < col.Length; i++) 
				{

					if (col[i].tag=="Net") 
					{
						Vector3 localPosInNet = net.transform.InverseTransformPoint (worldPos);
						//Replace texture  这里需要重新创建一个和Tex一样的纹理，直接操作Tex的话会把文件给修改了
						if (newTex==null) 
						{
							newTex = new Texture2D (netTex2D.width, netTex2D.height, TextureFormat.ARGB32, false);
							newTex.SetPixels32(netTex2D.GetPixels32());
						}
							
						for (int k = -brushSize; k <brushSize; k++) 
						{
							for (int j = -brushSize; j < brushSize; j++)
							{

								newTex.SetPixel((int)  Input.mousePosition.x+k, (int)  Input.mousePosition.y+j, colour);

							}
						}

						newTex.Apply();
						netRender.sprite = Sprite.Create(newTex, netRender.sprite.rect, new Vector2(0.5f, 0.5f));

					}
				}
			}


		}
		*/

	}
}
