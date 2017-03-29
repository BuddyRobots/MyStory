using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColor3D_test : MonoBehaviour 
{

	public Camera cam;
	Texture2D newTex;
	Color colour=new Color(0,0,0,0);
	public GameObject net;
	private SpriteRenderer netRender;

	void Start()
	{
		cam = GetComponent<Camera>();
		newTex=null;

		netRender=net.GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
			{		

					Renderer rend = hit.transform.GetComponent<MeshRenderer>() as Renderer;
					MeshCollider meshCollider = hit.collider as MeshCollider;


					if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
						return;

					Texture2D tex = rend.material.mainTexture as Texture2D;
					Vector2 pixelUV = hit.textureCoord;
					pixelUV.x *= tex.width;
					pixelUV.y *= tex.height;


					//Replace texture  这里需要重新创建一个和Tex一样的纹理，直接操作Tex的话会把文件给修改了
					if (newTex==null) 
					{
						newTex = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, false);
						newTex.SetPixels32(tex.GetPixels32());
					}


					
					for (int i = -30; i <30; i++) 
					{
						for (int j = -30; j < 30; j++)
						{
							newTex.SetPixel((int) pixelUV.x+i, (int) pixelUV.y+j, colour);

						}
					}

					newTex.Apply();
					rend.material.mainTexture=newTex;
					netRender.sprite = Sprite.Create(newTex, netRender.sprite.rect, new Vector2(0.5f, 0.5f));

			}
	
		}
	
	}
}
