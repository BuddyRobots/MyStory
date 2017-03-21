using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EraseNetColor : MonoBehaviour
{
	public GameObject net;
	public GameObject mouse;

	public int brushSize;

	private Sprite netSprit;
	private Sprite mouseSprite;

	private Texture2D netTex2D;
	private Texture2D mouseTex2D;

	private Color[] colors;

	private int netImageWidth;
	private int netImageHeight;
	private int  mouseImageWidth;
	private int mouseImageHeight;

	void Start ()
	{
		if (net != null) 
		{
			netSprit = net.GetComponent<SpriteRenderer> ().sprite;
		}
		if (mouse!=null) 
		{
			mouseSprite=mouse.GetComponent<SpriteRenderer>().sprite;
		}
			
		netTex2D = (Texture2D)netSprit.texture;
		mouseTex2D=(Texture2D)mouseSprite.texture;

		netImageWidth = netTex2D.width;
		netImageHeight = netTex2D.height;
		mouseImageWidth=mouseTex2D.width;
		mouseImageHeight=mouseTex2D.height;

		brushSize =20;

		colors = netTex2D.GetPixels();
	}


	void Update ()
	{
		if (Input.GetMouseButton (0))
		{
			CheckPointToEraseColor (Input.mousePosition);
		}

	}


	bool CheckPointToEraseColor (Vector3 pScreenPos)
	{
		Vector3 worldPos =Camera.main.ScreenToWorldPoint (pScreenPos);
		Vector3 localPosInNet = net.transform.InverseTransformPoint (worldPos);
		Vector3 localPosInMouse = mouse.transform.InverseTransformPoint (worldPos);

		//		Debug.Log("localPos-----"+localPos);
		//		Debug.LogWarning (localPos);


		if (localPosInMouse.x > -mouseImageWidth / 2 && localPosInMouse.x < mouseImageWidth / 2 && localPosInMouse.y > -mouseImageHeight / 2 && localPosInMouse.y < mouseImageHeight / 2) //如果坐标在老鼠图片范围内
		{
			if (localPosInNet.x > -netImageWidth / 2 && localPosInNet.x < netImageWidth / 2 && localPosInNet.y > -netImageHeight / 2 && localPosInNet.y < netImageHeight / 2) //如果坐标在网的图片范围内
			{

				for(int i = (int)localPosInNet.x - brushSize; i < (int)localPosInNet.x + brushSize ; i++)
				{
					for(int j = (int)localPosInNet.y - brushSize ; j < (int)localPosInNet.y + brushSize ; j++)
					{
						if(Mathf.Pow(i-localPosInNet.x,2)+Mathf.Pow(j-localPosInNet.y,2) > Mathf.Pow(brushSize,2))
							continue;

						netTex2D.SetPixel(i+(int)netImageWidth/2,j+(int)netImageHeight/2,new Color(0, 0, 0, 0));
					}
				}
				netTex2D.Apply ();
//				CheckIfNetIsErased();

				return true;
			}
			return false;
		}
		return false;
	}

	//判断网是否被擦除------判断网上设定的几个关键点有没有被擦除，也就是老鼠有没有移动到以关键点为中心的半径为5的圆的范围内
	void CheckIfNetIsErased()
	{
		Debug.Log("------CheckIfNetIsErased");


	}


	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, 100, 50), "Rest Color"))
		{
			RecoverNet();
		}
	}

	void OnDisable()
	{
		RecoverNet();
	}

	//复原网图片
	void RecoverNet()
	{
		netTex2D.SetPixels(colors);
		netTex2D.Apply();
	}
}




