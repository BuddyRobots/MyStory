using UnityEngine;
using System.Collections.Generic;


public class RemoveColor2D : MonoBehaviour 
{
	public GameObject netGO;
	[Range (0, 100)]
	public int brushRadius = 40;
	private float worldRadius;

	SpriteRenderer netSpriteRenderer;
	Sprite netSprite;
	Texture2D netTexture;
	Texture2D newTexture;
	int textureHalfWidth;
	int textureHalfHeight;



	public List<Transform> keyPoints;
	public List<int> flags;


	void Awake()
	{
		netSpriteRenderer = netGO.GetComponent<SpriteRenderer>();
		netSprite = netSpriteRenderer.sprite;
		netTexture = netSprite.texture;
		newTexture = new Texture2D (netTexture.width, netTexture.height, TextureFormat.RGBA32, false);
		newTexture.SetPixels32(netTexture.GetPixels32());
		textureHalfWidth = netTexture.width/2;
		textureHalfHeight = netTexture.height/2;


		worldRadius = brushRadius / netSprite.bounds.extents.x * textureHalfWidth;
		Debug.Log("worldRadius--"+worldRadius);

		flags.Clear();
		for (int i = 0; i < keyPoints.Count; i++) {
			flags.Add(0);
		}
	}		

	void Update()
	{
		if(MouseDrag._instance.mouseDraging) //(Input.GetMouseButton(0))
		{

			// Get Mouse position - convert to global world position
			Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
			Vector3 localPosInNet = transform.InverseTransformPoint (screenPos);

			screenPos = new Vector2(screenPos.x, screenPos.y);

			// Check if we clicked on our object
			RaycastHit2D[] ray = Physics2D.RaycastAll(screenPos, Vector2.zero, 0.01f);
			for (int i = 0; i < ray.Length; i++)
			{

				// You will want to tag the image you want to lookup
				if (ray[i].collider.name == "net")
				{ 			
					
					MouseDrag._instance.isOnNet=true;

					// Set click position to the gameobject local area
					screenPos -= ray[i].collider.gameObject.transform.position;
					CheckIfNetWasErased(screenPos);

					int x = (int)(screenPos.x * textureHalfWidth / netSprite.bounds.extents.x) + textureHalfWidth;
					int y = (int)(screenPos.y * textureHalfHeight / netSprite.bounds.extents.y) + textureHalfHeight;

					EraseCircle(newTexture, x, y, brushRadius);
					break;

				}
				else
				{
					MouseDrag._instance.isOnNet=false;
				}
			} 
			newTexture.Apply();
			netSpriteRenderer.sprite = Sprite.Create(newTexture, netSpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));
		}
	}

	private void EraseCircle(Texture2D texture, int xCenter, int yCenter, int radius)
	{
		for (var x = xCenter - radius; x <= xCenter; x++)
			for (var y = yCenter - radius; y <= yCenter; y++)
				// we don't have to take the square root, it's slow
				if ((x - xCenter)*(x - xCenter) + (y - yCenter)*(y - yCenter) <= radius*radius) 
				{
					var xSym = xCenter - (x - xCenter);
					var ySym = yCenter - (y - yCenter);
					// (x, y), (x, ySym), (xSym , y), (xSym, ySym) are in the circle
					newTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
					newTexture.SetPixel(x, ySym, new Color(0, 0, 0, 0));
					newTexture.SetPixel(xSym, y, new Color(0, 0, 0, 0));
					newTexture.SetPixel(xSym, ySym, new Color(0, 0, 0, 0));
				}
	}



	void CheckIfNetWasErased(Vector3 center)
	{

		int index=0;


		for (int i = 0; i < keyPoints.Count; i++)
		{
			if (keyPoints[i].position.x>=center.x-worldRadius && keyPoints[i].position.x<=center.x+worldRadius
				&& keyPoints[i].position.y>=center.y-worldRadius && keyPoints[i].position.y<=center.y+worldRadius) 
			{
				flags[i]=1;
			}
		}
		for (int i = 0; i < flags.Count; i++) 
		{
			Debug.Log("flags["+i+"]:"+flags[i]);
			if (flags[i]==1) 
			{
				index++;
			}
			
		}

	}





}
