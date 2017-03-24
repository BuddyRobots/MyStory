using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyUtils;
using MyStory;
using Anima2DRuntimeEngine;
using OpenCVForUnity;


public class test_LoadSpriteParts : MonoBehaviour
{
	public GameObject mouseSpriteRootGO;
	public RuntimeAnimatorController animationController;

	private class WorldCord
	{
		public Vector3 center;
		public Vector3 bottomLeft;
		public Vector3 topRight;
		public Vector3 topleft;
		public Vector3 bottomRight;
		public float width;
		public float height;
	}


	// Use this for initialization
	void Start ()
	{
		#if UNITY_EDITOR
		List<Texture2D> partTexList = new List<Texture2D>();
		List<OpenCVForUnity.Rect> partBBList;

		for (var i = 0; i < 9; i++)
		{
			string path = "Sprites/Tests/" + (i + 1).ToString();
			partTexList.Add(ReadPicture.ReadAsTexture2D(path));
		}

		SetCordinate(partTexList, out partBBList);

		Mouse mouse = new Mouse(partTexList, partBBList);
		#elif !UNITY_EDITOR
		Mouse mouse = Manager._instance.mouse;
		#endif

		mouse.CreateSprite(mouseSpriteRootGO);

		// Change Sprite to SpritMesh
		GameObject mouseSpriteMeshRootGO = SpriteMeshUtils.CreateFromGameObjectRoot(mouseSpriteRootGO);
		mouseSpriteMeshRootGO.transform.position = new Vector3(0, 0, -1);

		// Create Bone Root
		GameObject oriMouseBoneRootGO = GameObject.Find("Mouse Stand Pose Animated/Hip");
		GameObject newMouseBoneRootGO = BoneUtils.CreateFromAnima2DBone2D(oriMouseBoneRootGO);

		SetupBone_Body(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_Head(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftEar(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightEar(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftArm(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightArm(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_LeftLeg(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_RightLeg(mouseSpriteMeshRootGO, newMouseBoneRootGO);
		SetupBone_Tail(mouseSpriteMeshRootGO, newMouseBoneRootGO);

		// Update BindInfo
		SpriteMeshUtils.BindBoneToInstance(mouseSpriteMeshRootGO);

		// Add Box Collider
		AddCollider(newMouseBoneRootGO);

		// Add to a parent GameObject
		GameObject newMouseAnimatedGO = new GameObject("Mouse");
		mouseSpriteMeshRootGO.transform.parent = newMouseAnimatedGO.transform;
		newMouseBoneRootGO.transform.parent = newMouseAnimatedGO.transform;
		Rigidbody2D rigidBody = newMouseAnimatedGO.AddComponent<Rigidbody2D>();
		rigidBody.gravityScale = 0;
		GameObject garLandGO = GetPositionOfGarland(newMouseBoneRootGO);
		garLandGO.transform.parent = newMouseAnimatedGO.transform;

		// Add Animator to Mouse
		Animator animator = newMouseAnimatedGO.AddComponent<Animator>();
		//animator.runtimeAnimatorController = Resources.Load("Animation/WJ/StandPoseAnimations/MouseStandPoseController") as RuntimeAnimatorController;
		animator.runtimeAnimatorController = animationController;

		// Save to Manager
		Destroy(Manager._instance.mouseGo);
		Manager._instance.mouseGo = newMouseAnimatedGO;
		GameObject.DontDestroyOnLoad(Manager._instance.mouseGo);
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
	// TODO Scan line method to make sure bone key points are in the texture. 
	private void SetupBone_Head(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{		
		// Find sprite key point (center, bottom left, top right).
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_head"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "Head");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.bottomLeft.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftEar"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L ear");
		bone.transform.position = new Vector3(worldCord.topRight.x, worldCord.bottomLeft.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightEar"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R ear");
		bone.transform.position = new Vector3(worldCord.bottomLeft.x, worldCord.bottomLeft.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_body"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "Hip");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.bottomLeft.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "Torso");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.center.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftArm"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L arm");
		bone.transform.position = new Vector3(worldCord.topRight.x, worldCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L hand");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.center.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightArm"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R arm");
		bone.transform.position = new Vector3(worldCord.bottomLeft.x, worldCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R hand");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.center.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_leftLeg"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "L leg");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L leg2");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.center.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "L foot");
		bone.transform.position = new Vector3(worldCord.topRight.x, worldCord.bottomLeft.y, 0);

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
		WorldCord worldCord = GetWorldCord(FindSpriteMeshGOInChild(spriteMeshRootGO, "new_rightLeg"));

		// Set bone position
		GameObject bone = FindBoneGOInChild(boneRootGO, "R leg");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.topRight.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R leg2");
		bone.transform.position = new Vector3(worldCord.center.x, worldCord.center.y, 0);
		bone = FindBoneGOInChild(boneRootGO, "R foot");
		bone.transform.position = new Vector3(worldCord.topRight.x, worldCord.bottomLeft.y, 0);

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

	// TODO Need to simplify this.
	private void SetupBone_Tail(GameObject spriteMeshRootGO, GameObject boneRootGO)
	{
		GameObject tailGO = FindSpriteMeshGOInChild(spriteMeshRootGO, "new_tail");
		Texture2D texture = tailGO.GetComponent<SpriteMeshInstance>().spriteMesh.sprite.texture;

		Mat sourceImage = new Mat(texture.height, texture.width, CvType.CV_8UC4);
		Utils.texture2DToMat(texture, sourceImage);

		List<Mat> mv = new List<Mat>();
		Core.split(sourceImage, mv);
		Mat binaryImage = new Mat(mv[3].rows(), mv[3].cols(), CvType.CV_8UC1);
		Imgproc.threshold(mv[3], binaryImage, 1, 255, Imgproc.THRESH_BINARY);

		List<List<Vector2>> scannedPoints = VerticalScanLine(binaryImage, 0, binaryImage.cols(), 14);
		List<Vector2> keyPoints = GetKeyPoints(scannedPoints, true); 
		WorldCord worldCord = GetWorldCord(tailGO);
		List<Vector3> worldKeyPoints = CordMatToWorld(keyPoints, binaryImage.cols(), binaryImage.rows(), worldCord);

		// Set bone position
		GameObject bone;

		for (var i = 0; i < 14; i++)
		{
			string name = "Tail" + (i + 1).ToString();
			bone = FindBoneGOInChild(boneRootGO, name);
			bone.transform.position = new Vector3(worldKeyPoints[i].x, worldKeyPoints[i].y, 0);
		}

		// Bind bone
		List<Bone2D> boneList = new List<Bone2D>();

		for (var i = 1; i <= 14; i++)
		{
			string name = "Tail" + i.ToString();
			boneList.Add(BoneUtils.FindBoneWithName(boneRootGO, name));
		}

		SpriteMeshInstanceEditor.SetupBoneList(
			SpriteMeshUtils.FindInstanceWithName(spriteMeshRootGO, "new_tail"), 
			boneList);		
		boneList.Clear();
	}

	private WorldCord GetWorldCord(GameObject go) 
	{
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		Vector3 pos = go.transform.position;
		WorldCord worldCord = new WorldCord();
		// Center
		worldCord.center = pos;
		// Bottom Left
		worldCord.bottomLeft = mr.bounds.min;
		// Top Right
		worldCord.topRight = mr.bounds.max;
		// Top Left
		worldCord.topleft = new Vector3(worldCord.bottomLeft.x, worldCord.topRight.y, worldCord.center.z);
		// Bottom Right
		worldCord.bottomRight = new Vector3(worldCord.topRight.x, worldCord.bottomLeft.y, worldCord.center.z);
		// Width
		worldCord.width = mr.bounds.extents.x*2;
		// Height
		worldCord.height = mr.bounds.extents.y*2;

		return worldCord;
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
		
	private List<List<Vector2>> VerticalScanLine(Mat binaryImage, int startCol, int endCol, int amount)
	{
		int width = binaryImage.cols();
		int height = binaryImage.rows();
		byte[] imageData = new byte[width*height];
		binaryImage.get(0, 0, imageData);

		// scannedPoints[amount][pointsEachLine]
		List<List<Vector2>> scannedPoints = new List<List<Vector2>>();
		int step = Mathf.FloorToInt((float)(endCol - startCol)/(float)amount);

		List<int> columnList = new List<int>();
		for (var i = 0; i < amount; i++)
		{
			scannedPoints.Add(new List<Vector2>());
			columnList.Add(startCol + step*i);
		}
			
		for (var j = 0; j < amount;)
		{
			bool scanned = false;
			for (var i = 0; i < height; i++)
				if (imageData[i*width + columnList[j]] != 0)
				{
					scannedPoints[j].Add(new Vector2(columnList[j], i));
					scanned = true;
				}
			if (!scanned)
				columnList[j]++;
			else
				j++;
		}			
		return scannedPoints;
	}

	private List<List<Vector2>> HorizontalScanLine(Mat binaryImage, int startRow, int endRow, int amount)
	{
		int width = binaryImage.cols();
		int height = binaryImage.rows();
		byte[] imageData = new byte[width*height];
		binaryImage.get(0, 0, imageData);

		// scannedPoints[amount][pointsEachLine]
		List<List<Vector2>> scannedPoints = new List<List<Vector2>>();
		int step = Mathf.FloorToInt((float)(endRow - startRow)/(float)amount);

		List<int> rowList = new List<int>();
		for (var i = 0; i < amount; i++)
		{
			scannedPoints.Add(new List<Vector2>());
			rowList.Add(startRow + step*i);
		}

		for (var i = 0; i < amount;)
		{
			bool scanned = false;
			for (var j = 0; j < width; j++)
				if (imageData[rowList[i]*width + j] != 0)
				{
					scannedPoints[i].Add(new Vector2(j, rowList[i]));
					scanned = true;
				}
			if (!scanned)
				rowList[i]++;
			else
				i++;
		}			
		return scannedPoints;
	}

	private List<Vector2> GetKeyPoints(List<List<Vector2>> scannedPoints, bool isVertical)
	{
		List<Vector2> keyPoints = new List<Vector2>();

		for (var i = 0; i < scannedPoints.Count; i++)
		{
			float avgX = 0.0f;
			float avgY = 0.0f;
			if (isVertical)
			{
				// TODO need to fix this : when scannedPoints[i] do not have [0]th element.
				avgX = scannedPoints[i].Average(item => item.x);
				avgY = scannedPoints[i].Average(item => item.y);
			}
			else
			{
				avgX = scannedPoints[i].Average(item => item.x);
				avgY = scannedPoints[i].Average(item => item.y);
			}
			keyPoints.Add(new Vector2(avgX, avgY));
		}
		return keyPoints;
	}

	private List<Vector3> CordMatToWorld(List<Vector2> keyPoints, int matWidth, int matHeight, WorldCord worldCord)
	{
		float xScale = (float)matWidth/worldCord.width;
		float yScale = (float)matHeight/worldCord.height;

		List<Vector3> worldKeyPoints = new List<Vector3>();

		for (var i = 0; i < keyPoints.Count; i++)
		{
			float x = worldCord.topleft.x + keyPoints[i].x / xScale;
			float y = worldCord.topleft.y - keyPoints[i].y / yScale;

			worldKeyPoints.Add(new Vector3(x, y, worldCord.center.z));
		}
		return worldKeyPoints;
	}

	private void AddCollider(GameObject boneRootGO)
	{
		GameObject hipBoneGO = FindBoneGOInChild(boneRootGO, "Hip");
		GameObject rEarBoneGO = FindBoneGOInChild(boneRootGO, "R ear");
		GameObject lHandBoneGO = FindBoneGOInChild(boneRootGO, "L hand");
		GameObject lFootBoneGO = FindBoneGOInChild(boneRootGO, "L foot");
		GameObject rHandBoneGO = FindBoneGOInChild(boneRootGO, "R hand");

		Vector2 tr = new Vector2(
			rEarBoneGO.transform.position.y + rEarBoneGO.GetComponent<Bone2D>().length,
			rHandBoneGO.transform.position.x + rHandBoneGO.GetComponent<Bone2D>().length
		);
		Vector2 bl = new Vector2(
			lFootBoneGO.transform.position.y,
			lHandBoneGO.transform.position.x - lHandBoneGO.GetComponent<Bone2D>().length
		);

		BoxCollider2D boxCollider = hipBoneGO.AddComponent<BoxCollider2D>();
		boxCollider.offset = (tr + bl)/2;
		boxCollider.offset -= new Vector2(hipBoneGO.transform.position.y, 0.0f);
		boxCollider.size = tr - bl;

		hipBoneGO.tag = "Player";
	}

	private GameObject GetPositionOfGarland(GameObject boneRootGO)
	{
		GameObject headBoneGO = FindBoneGOInChild(boneRootGO, "Head");

		GameObject garlandGO = new GameObject("GarlandDest");
		garlandGO.transform.position = headBoneGO.GetComponent<Bone2D>().endPosition;

		return garlandGO;
	}
}