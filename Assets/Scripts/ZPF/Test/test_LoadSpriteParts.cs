using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyUtils;


public class test_LoadSpriteParts : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		List<Texture2D> partTexList = new List<Texture2D>();
		List<OpenCVForUnity.Rect> partBBList;

		for (var i = 0; i < 9; i++)
		{
			string path = "Sprites/Tests/" + (i + 1).ToString();
			partTexList.Add(ReadPicture.ReadAsTexture2D(path));
		}

		SetCordinate(out partBBList);




	}

	private void SetCordinate(out List<OpenCVForUnity.Rect> bbList)
	{
		bbList = new List<OpenCVForUnity.Rect>();

		bbList.Add(new OpenCVForUnity.Rect(84, 125, 59, 41));
		bbList.Add(new OpenCVForUnity.Rect(81, 75, 60, 58));
		bbList.Add(new OpenCVForUnity.Rect(149, 73, 60, 58));
		bbList.Add(new OpenCVForUnity.Rect(104, 187, 25, 31));
		bbList.Add(new OpenCVForUnity.Rect(70, 179, 23, 9));
		bbList.Add(new OpenCVForUnity.Rect(134, 176, 23, 9));
		bbList.Add(new OpenCVForUnity.Rect(91, 225, 15, 26));
		bbList.Add(new OpenCVForUnity.Rect(118, 225, 15, 26));
		bbList.Add(new OpenCVForUnity.Rect(226, 197, 109, 4));
	}
}

