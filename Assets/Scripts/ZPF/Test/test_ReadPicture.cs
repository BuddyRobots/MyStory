using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtils;


public class test_ReadPicture : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Texture2D texture = MyUtils.ReadPicture.ReadAsTexture2D("Pictures/Mouses/1487573118");

		gameObject.GetComponent<Renderer>().material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
