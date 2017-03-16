using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using MyUtils;
using Anima2DRuntimeEngine;


public class test_TextureOutline : MonoBehaviour {

	public GameObject spriteGO;


	// Use this for initialization
	void Start () {
		Vector2[][] paths;
		Texture2D texture = ReadPicture.ReadAsTexture2D("Sprites/Tests/2");
		Mat image = SpriteMeshUtils.GetTexture2DOutline(texture, 1.0f, out paths);

		Texture2D displayTex = new Texture2D(image.cols(), image.rows());
		Utils.matToTexture2D(image, displayTex);

		Sprite displaySprite = Sprite.Create(displayTex, new UnityEngine.Rect(0.0f,0.0f,displayTex.width,displayTex.height), new Vector2(0.5f,0.5f), 100);
		spriteGO.GetComponent<SpriteRenderer>().sprite = displaySprite;
	}
}
