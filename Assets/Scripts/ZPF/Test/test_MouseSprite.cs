using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyStory;
using Anima2DRuntimeEngine;


public class test_MouseSprite : MonoBehaviour {

	[HideInInspector]
	Mouse mouse;

	public GameObject spriteRootGO;


	// Use this for initialization
	void Start () {
		mouse = Manager._instance.mouse;
		mouse.CreateSprite(spriteRootGO);


		GameObject spriteMeshRootGO = SpriteMeshUtils.CreateFromGameObjectRoot(spriteRootGO);

	}
}