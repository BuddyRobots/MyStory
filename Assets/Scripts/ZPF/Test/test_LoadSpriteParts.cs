using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyUtils;
using MyStory;
using Anima2DRuntimeEngine;


public class test_LoadSpriteParts : MonoBehaviour
{
	public GameObject mouseSpriteRootGO;


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

		SetCordinate(partTexList, out partBBList);

		Mouse mouse = new Mouse(partTexList, partBBList);
		mouse.CreateSprite(mouseSpriteRootGO);

		// Change Sprite to SpritMesh
		GameObject mouseSpriteMeshRootGO = SpriteMeshUtils.CreateFromGameObjectRoot(mouseSpriteRootGO);
		mouseSpriteMeshRootGO.transform.position = new Vector3(0, 0, -1);

		// Create Bone Root
		GameObject oriMouseBoneRootGO = GameObject.Find("Hip");
		GameObject newMouseBoneRootGO = BoneUtils.CreateFromAnima2DBone2D(oriMouseBoneRootGO);

		// Find Key Points
		Vector3[] new_leftArmCord = SpriteLocalToWorld(GameObject.Find("new_leftArm"));
		GameObject center = new GameObject("center");
		center.transform.position = new_leftArmCord[0];
		GameObject bottomLeft = new GameObject("bottomLeft");
		bottomLeft.transform.position = new_leftArmCord[1];
		GameObject topRight = new GameObject("topRight");
		topRight.transform.position = new_leftArmCord[2];




	}

	private void SetCordinate(List<Texture2D> partTexList, out List<OpenCVForUnity.Rect> bbList)
	{
		bbList = new List<OpenCVForUnity.Rect>();

		List<List<int>> values = new List<List<int>>();
		values.Add(new List<int>(new int[] { -18, 122, 117, 81 }));
		values.Add(new List<int>(new int[] { -22, 172, 119, 116 }));
		values.Add(new List<int>(new int[] { 47, 174, 119, 116 }));
		values.Add(new List<int>(new int[] { 2, 60, 50, 61 }));
		values.Add(new List<int>(new int[] { -32, 68, 46, 17 }));
		values.Add(new List<int>(new int[] { 32, 71, 46, 17 }));
		values.Add(new List<int>(new int[] { -12, 22, 29, 51 }));
		values.Add(new List<int>(new int[] { 16, 22, 29, 51 }));
		values.Add(new List<int>(new int[] { 124, 50, 217, 8 }));

		for (var i = 0; i < values.Count; i++)
		{
			values[i][0] = values[i][0] + 102 - values[i][2]/2;
			values[i][1] = -(values[i][1] - 247 + values[i][3]/2);
		}

		for (var i = 0; i < values.Count; i++)
			bbList.Add(new OpenCVForUnity.Rect(values[i][0], values[i][1], values[i][2], values[i][3]));
	}

	private Vector3[] SpriteLocalToWorld(GameObject go) 
	{
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		Vector3 pos = go.transform.position;
		Vector3 [] array = new Vector3[3];
		// Center
		array[0] = pos;
		// Bottom Left
		array[1] = mr.bounds.min;
		// Top Right
		array[2] = mr.bounds.max;
		return array;
	}
}