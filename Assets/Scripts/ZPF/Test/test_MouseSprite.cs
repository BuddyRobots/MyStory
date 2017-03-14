using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyStory;
using Anima2DRuntimeEngine;


public class test_MouseSprite : MonoBehaviour {

	[HideInInspector]
	Mouse mouse;

	public GameObject spriteRootGO;

	private class BasicCord
	{
		public Vector3 center;
		public Vector3 bottomLeft;
		public Vector3 topRight;
	}


	// Use this for initialization
	void Start () {
		mouse = Manager._instance.mouse;
		mouse.CreateSprite(spriteRootGO);

		// Change Sprite to SpritMesh
		GameObject mouseSpriteMeshRootGO = SpriteMeshUtils.CreateFromGameObjectRoot(spriteRootGO);
		mouseSpriteMeshRootGO.transform.position = new Vector3(0, 0, -1);

		// Create Bone Root
		GameObject oriMouseBoneRootGO = GameObject.Find("Hip");
		GameObject newMouseBoneRootGO = BoneUtils.CreateFromAnima2DBone2D(oriMouseBoneRootGO);

		SetupBone_Body(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_Head(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftEar(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightEar(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftArm(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightArm(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftLeg(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightLeg(mouseSpriteMeshRootGO, newMouseBoneRootGO);

		// Update BindInfo
		SpriteMeshUtils.BindBoneToInstance(mouseSpriteMeshRootGO);

		// Add to a parent GameObject
		GameObject newMouseAnimated = new GameObject("newMouseAnimated");
		mouseSpriteMeshRootGO.transform.parent = newMouseAnimated.transform;
		newMouseBoneRootGO.transform.parent = newMouseAnimated.transform;

		// Add Animator to Mouse
		Animator animator = newMouseAnimated.AddComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load("Animation/WJ/TestAnimations/TestController") as RuntimeAnimatorController;

		// Save to Manager
		Manager._instance.mouseGo = newMouseAnimated;
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

	// TODO Setup BoneList, can we simplify this routine?
	// TODO We have not considered the length of the bone yet. Maybe something with bone's child.
	// TODO Did not consider rotation problem.
	private void SetupBone_Head(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_head"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "Head");
		bone.transform.position = new Vector3(basicCord.center.x, basicCord.bottomLeft.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "Head"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_head"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_LeftEar(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftEar"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L ear");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.bottomLeft.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L ear"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_leftEar"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_RightEar(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightEar"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R ear");
		bone.transform.position = new Vector3(basicCord.bottomLeft.x, basicCord.bottomLeft.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R ear"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_rightEar"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_Body(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_body"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "Hip");
		bone.transform.position = new Vector3(basicCord.center.x, basicCord.bottomLeft.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "Torso");
		bone.transform.position = new Vector3(basicCord.center.x, basicCord.center.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "Hip"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "Torso"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_body"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_LeftArm(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftArm"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L arm");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L hand");
		bone.transform.position = new Vector3(basicCord.center.x, basicCord.center.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L arm"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L hand"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_leftArm"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_RightArm(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightArm"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R arm");
		bone.transform.position = new Vector3(basicCord.bottomLeft.x, basicCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R hand");
		bone.transform.position = new Vector3(basicCord.center.x, basicCord.center.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R arm"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R hand"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_rightArm"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_LeftLeg(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftLeg"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L leg");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L leg2");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.center.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L foot");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.bottomLeft.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L leg"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L leg2"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "L foot"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_leftLeg"), 
			boneList);		
		boneList.Clear();
	}

	private void SetupBone_RightLeg(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		BasicCord basicCord = GetBasicCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightLeg"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R leg");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R leg2");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.center.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R foot");
		bone.transform.position = new Vector3(basicCord.topRight.x, basicCord.bottomLeft.y, 0);

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R leg"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R leg2"));
		boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, "R foot"));
		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_rightLeg"), 
			boneList);		
		boneList.Clear();
	}

	private BasicCord GetBasicCord(GameObject go) 
	{
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		Vector3 pos = go.transform.position;
		BasicCord basicCord = new BasicCord();
		// Center
		basicCord.center = pos;
		// Bottom Left
		basicCord.bottomLeft = mr.bounds.min;
		// Top Right
		basicCord.topRight = mr.bounds.max;
		return basicCord;
	}

	private GameObject FindSpriteMeshGOInChild(GameObject spriteMeshRootGO, string name)
	{
		Transform[] children = spriteMeshRootGO.GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if(child.gameObject.name == name)
			{
				return child.gameObject;
			}
		}
		return new GameObject("sprite mesh go not found");
	}

	private GameObject FindBoneGOInChild(GameObject boneRootGo, string name)
	{
		Transform[] children = boneRootGo.GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if(child.gameObject.name == name)
			{
				return child.gameObject;
			}
		}
		return new GameObject("bone go not found");
	}		
}