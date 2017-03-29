using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMouse :MonoBehaviour 
{
	private GameObject target;
	private bool isMouseDrag;
	private Vector3 screenPosition;
	private Vector3 offset;

	public GameObject net;
	public GameObject mouse;

	private int netTexWidth;
	private int netTexHeight;
	public int brushSize=30;
	Color colour=new Color(0,0,0,0);

	private Texture2D netTex2D;

	Texture2D newTex;

	void Start () 
	{
		netTexWidth = net.GetComponent<SpriteRenderer>().sprite.texture.width;
		netTexHeight= net.GetComponent<SpriteRenderer>().sprite.texture.height;
		netTex2D = net.GetComponent<SpriteRenderer>().sprite.texture as Texture2D;
	}
	
	void Update () 
	{
		GameObjectDragAndDrog2D();

	}

	//返回老鼠
	private GameObject ReturnGameObjectDrag2D()
	{
		target = null;

		RaycastHit2D hit2D =Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),transform.position);

		if (hit2D.collider!=null) 
		{
			if (hit2D.collider.tag=="Player") 
			{
				target = hit2D.collider.gameObject;

			}
		}
		return target;
	}

	private void GameObjectDragAndDrog2D()
	{
		if (Input.GetMouseButtonDown (0))
		{
			target = ReturnGameObjectDrag2D();
			if (target != null)
			{

				isMouseDrag = true;
				screenPosition = Camera.main.WorldToScreenPoint(target.transform.parent.transform.position);
				//offset=老鼠的世界坐标-鼠标的世界坐标（由屏幕坐标转化而来）
				offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
			}      
		}

		if (Input.GetMouseButtonUp(0))
		{
			isMouseDrag = false;
			target.transform.localPosition=new Vector3(5.8f,-4.4f,0);
		}

		if (isMouseDrag)
		{
			Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
//			Debug.Log("鼠标的屏幕坐标 currentScreenSpace----"+currentScreenSpace);
			Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
//			Debug.Log("鼠标的世界坐标加上偏移值 currentPosition----"+currentPosition);
			target.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
//			Debug.Log("target.transform.localPosition----"+target.transform.localPosition);

		}

	}


	bool CheckPointToEraseColor (Vector3 pScreenPos)
	{
		if (isMouseDrag) 
		{
			Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (currentScreenSpace);//鼠标的屏幕坐标--->世界坐标
			Vector3 localPosInNet = net.transform.InverseTransformPoint (worldPos);;//鼠标的世界坐标 在 网 的本地坐标

			if (localPosInNet.x > -netTexWidth / 2 && localPosInNet.x < netTexWidth / 2 && localPosInNet.y > -netTexHeight / 2 && localPosInNet.y < netTexHeight / 2) //如果坐标在网的图片范围内
			{
				Debug.Log("坐标在网的范围内");

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
				return true;
			}
		}
		return false;
	}

}
