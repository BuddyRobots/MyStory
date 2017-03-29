using UnityEngine;


public class RemoveColor2D : MonoBehaviour 
{
	public GameObject netGO;
	public int brushRadius = 25;

	SpriteRenderer netSpriteRenderer;
	Sprite netSprite;
	Texture2D netTexture;
	Texture2D newTexture;
	int textureHalfWidth;
	int textureHalfHeight;


	void Awake()
	{
		netSpriteRenderer = netGO.GetComponent<SpriteRenderer>();
		netSprite = netSpriteRenderer.sprite;
		netTexture = netSprite.texture;
		newTexture = new Texture2D (netTexture.width, netTexture.height, TextureFormat.RGBA32, false);
		newTexture.SetPixels32(netTexture.GetPixels32());
		textureHalfWidth = netTexture.width/2;
		textureHalfHeight = netTexture.height/2;
	}		

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			// Get Mouse position - convert to global world position
			Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
			screenPos = new Vector2(screenPos.x, screenPos.y);

			// Check if we clicked on our object
			RaycastHit2D[] ray = Physics2D.RaycastAll(screenPos, Vector2.zero, 0.01f);
			for (int i = 0; i < ray.Length; i++)
			{
				// You will want to tag the image you want to lookup
				if (ray[i].collider.name == "net")
				{ 					
					// Set click position to the gameobject local area
					screenPos -= ray[i].collider.gameObject.transform.position;
					int x = (int)(screenPos.x * textureHalfWidth / netSprite.bounds.extents.x) + textureHalfWidth;
					int y = (int)(screenPos.y * textureHalfHeight / netSprite.bounds.extents.y) + textureHalfHeight;

					EraseCircle(newTexture, x, y, brushRadius);
					break;
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
}
