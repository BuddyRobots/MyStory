using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColor3D_test : MonoBehaviour {

	public Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
			{
				Debug.Log("-----");

				Renderer rend = hit.transform.GetComponent<MeshRenderer>() as Renderer;
				MeshCollider meshCollider = hit.collider as MeshCollider;


//
//				Debug.Log("rend--"+rend);
//				Debug.Log("rend.material--"+rend.sharedMaterial);
//				Debug.Log("rend.materials[0].mainTexture--"+rend.materials[0].mainTexture);
//				Debug.Log("boxCol--"+meshCollider);


				if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
					return;

				Texture2D tex = rend.material.mainTexture as Texture2D;
				Vector2 pixelUV = hit.textureCoord;
				pixelUV.x *= tex.width;
				pixelUV.y *= tex.height;



				Color colour=new Color(0,0,0,0);
				for (int i = 0; i <10; i++) 
				{
					for (int j = 0; j < 10; j++)
					{
						tex.SetPixel((int)pixelUV.x+i, (int)pixelUV.y+j, colour);
						tex.Apply();

					}
				}

			}
				



		}
			



//		tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
//		tex.Apply();
	}
}
