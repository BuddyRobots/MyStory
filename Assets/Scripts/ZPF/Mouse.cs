using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;


namespace MyStory
{
	public class Mouse
	{
		public BodyPart head;
		public BodyPart leftEar;
		public BodyPart rightEar;
		public BodyPart body;
		public BodyPart leftArm;
		public BodyPart rightArm;
		public BodyPart leftLeg;
		public BodyPart rightLeg;
		public BodyPart tail;

		// Test : For test.
		public Mat modelSizeMat;

		private List<BodyPart> bodyPartList;
		private List<GameObject> oriSpriteGOList;

		private const float pixelsPerUnit = 100.0f;

		private GameObject spriteMeshRootGO;


		// TODO Need to complete this. It's for test now.
		public Mouse(Mat inputMat/*, GameObject oriSpriteRootGO*/)
		{
			List<Texture2D> partList = new List<Texture2D>();
			List<OpenCVForUnity.Rect> bbList = new List<OpenCVForUnity.Rect>();
			modelSizeMat = Segmentation.Segment(inputMat, out partList, out bbList);

			head     = new BodyPart(partList[0], bbList[0], "head", 9);
			leftEar  = new BodyPart(partList[1], bbList[1], "leftEar", 7);
			rightEar = new BodyPart(partList[2], bbList[2], "rightEar", 8);
			body     = new BodyPart(partList[3], bbList[3], "body", 4);
			leftArm  = new BodyPart(partList[4], bbList[4], "leftArm", 3);
			rightArm = new BodyPart(partList[5], bbList[5], "rightArm", 20);
			leftLeg  = new BodyPart(partList[6], bbList[6], "leftLeg", 2);
			rightLeg = new BodyPart(partList[7], bbList[7], "rightLeg", 5);
			tail     = new BodyPart(partList[8], bbList[8], "tail", 2);

			AddPartsToList();
		}

		// Test : Constructor For Test.
		public Mouse(List<Texture2D> partList, List<OpenCVForUnity.Rect> bbList)
		{
			head     = new BodyPart(partList[0], bbList[0], "head", 9);
			leftEar  = new BodyPart(partList[1], bbList[1], "leftEar", 7);
			rightEar = new BodyPart(partList[2], bbList[2], "rightEar", 8);
			body     = new BodyPart(partList[3], bbList[3], "body", 4);
			leftArm  = new BodyPart(partList[4], bbList[4], "leftArm", 3);
			rightArm = new BodyPart(partList[5], bbList[5], "rightArm", 20);
			leftLeg  = new BodyPart(partList[6], bbList[6], "leftLeg", 2);
			rightLeg = new BodyPart(partList[7], bbList[7], "rightLeg", 5);
			tail     = new BodyPart(partList[8], bbList[8], "tail", 2);

			AddPartsToList();
		}

		public class BodyPart
		{
			public Texture2D texture;
			public OpenCVForUnity.Rect bb;
			public string name;
			public int sortingOrder;
				
			public BodyPart(Texture2D tex, OpenCVForUnity.Rect bb, string name, int sortingOrder = 0)
			{
				this.texture = tex;
				this.bb = bb;
				this.name = name;
				this.sortingOrder = sortingOrder;
			}
		}
	
		private void AddPartsToList()
		{
			bodyPartList = new List<BodyPart>();
			bodyPartList.Add(head);
			bodyPartList.Add(leftEar);
			bodyPartList.Add(rightEar);
			bodyPartList.Add(body);
			bodyPartList.Add(leftArm);
			bodyPartList.Add(rightArm);
			bodyPartList.Add(leftLeg);
			bodyPartList.Add(rightLeg);
			bodyPartList.Add(tail);
		}
	
		public void CreateSprite(GameObject oriSpriteRootGO)
		{
			CreateSpriteStructure(oriSpriteRootGO);

			for (var i = 0; i < oriSpriteGOList.Count; i++)
				SetSpriteTexture(oriSpriteGOList[i], bodyPartList[i].texture);			

			TransformCordinate();
		}

		private void CreateSpriteStructure(GameObject root)
		{
			oriSpriteGOList = new List<GameObject>();
			for (var i = 0; i < bodyPartList.Count; i++)
			{
				oriSpriteGOList.Add(new GameObject(bodyPartList[i].name));
				oriSpriteGOList[i].AddComponent<SpriteRenderer>();
				oriSpriteGOList[i].transform.SetParent(root.transform);
				oriSpriteGOList[i].GetComponent<SpriteRenderer>().sortingOrder = bodyPartList[i].sortingOrder;
			}				
		}

		private void SetSpriteTexture(GameObject spriteGO, Texture2D texture)
		{
			spriteGO.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new UnityEngine.Rect(0.0f,0.0f,texture.width,texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
		}

		private void TransformCordinate()
		{
			List<Vector3> matCordList = new List<Vector3>();
			for (var i = 0; i < oriSpriteGOList.Count; i++)
			{
				matCordList.Add(new Vector3((bodyPartList[i].bb.x + bodyPartList[i].bb.width/2)/pixelsPerUnit, (bodyPartList[i].bb.y + bodyPartList[i].bb.height/2)/pixelsPerUnit, 0.0f));
			}
			Vector3 matOrigin = new Vector3((float)(matCordList[6].x + matCordList[7].x)/2.0f, 
				Mathf.Max((float)leftLeg.bb.br().y, (float)rightLeg.bb.br().y)/pixelsPerUnit, 0.0f);

			for (var i = 0 ; i < oriSpriteGOList.Count; i++)
			{
				matCordList[i] -= matOrigin;

				float y = -matCordList[i].y;
				float x = matCordList[i].x;
				matCordList[i] = new Vector3(x, y, 0.0f);
			}

			for (var i = 0; i < oriSpriteGOList.Count; i++)
			{
				oriSpriteGOList[i].transform.localPosition = matCordList[i];
			}
		}
	}
}