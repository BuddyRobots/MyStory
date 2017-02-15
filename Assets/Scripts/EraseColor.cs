using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EraseColor : MonoBehaviour
{
	
	public Transform keyPos;

	public GameObject activateBtn;
	public GameObject net;
	public GameObject mouse;

	public int brushSize;




	private UITexture netUITex;
	private UITexture mouseUITex;

	private Texture2D netTex2D;
	private Texture2D mouseTex2D;

	private Color[] colors;

	private int netTexWidth;
	private int netTexHeight;
	private int  mouseTexWidth;
	private int mouseTexHeight;

	void Start ()
	{
		if (net != null) 
		{
			netUITex = net.GetComponent<UITexture> ();
		}
		if (mouse!=null) 
		{
			mouseUITex=mouse.GetComponent<UITexture>();
		}
		netTex2D = (Texture2D)netUITex.mainTexture;
		mouseTex2D=(Texture2D)mouseUITex.mainTexture;

		netTexWidth = netTex2D.width;
		netTexHeight = netTex2D.height;
		mouseTexWidth=mouseTex2D.width;
		mouseTexHeight=mouseTex2D.height;

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
		Vector3 worldPos = UICamera.mainCamera.ScreenToWorldPoint (pScreenPos);
		Vector3 localPosInNet = netUITex.gameObject.transform.InverseTransformPoint (worldPos);
		Vector3 localPosInMouse = mouseUITex.gameObject.transform.InverseTransformPoint (worldPos);

//		Debug.Log("localPos-----"+localPos);
//		Debug.LogWarning (localPos);


		if (localPosInMouse.x > -mouseTexWidth / 2 && localPosInMouse.x < mouseTexWidth / 2 && localPosInMouse.y > -mouseTexHeight / 2 && localPosInMouse.y < mouseTexHeight / 2) //如果坐标在老鼠图片范围内
		{
			if (localPosInNet.x > -netTexWidth / 2 && localPosInNet.x < netTexWidth / 2 && localPosInNet.y > -netTexHeight / 2 && localPosInNet.y < netTexHeight / 2) //如果坐标在网的图片范围内
			{

				for(int i = (int)localPosInNet.x - brushSize; i < (int)localPosInNet.x + brushSize ; i++)
				{
					for(int j = (int)localPosInNet.y - brushSize ; j < (int)localPosInNet.y + brushSize ; j++)
					{
						if(Mathf.Pow(i-localPosInNet.x,2)+Mathf.Pow(j-localPosInNet.y,2) > Mathf.Pow(brushSize,2))
							continue;

						netTex2D.SetPixel(i+(int)netTexWidth/2,j+(int)netTexHeight/2,new Color(0, 0, 0, 0));
					}
				}
				netTex2D.Apply ();
				CheckIfNetIsErased();

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
		if (Mathf.Pow(Mathf.Abs(mouse.transform.localPosition.x-keyPos.localPosition.x),2f)+Mathf.Pow(Mathf.Abs(mouse.transform.localPosition.y-keyPos.localPosition.y),2f)<=50f) 
		{
			activateBtn.SetActive(true);
		}

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



