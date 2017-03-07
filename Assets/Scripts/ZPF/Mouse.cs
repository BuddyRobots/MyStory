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

		public Mat modelSizeMat;

		GameObject spriteMeshRootGO;

		// TODO Need to complete this. It's for test now.
		public Mouse(Mat inputMat/*, GameObject oriSpriteRootGO*/)
		{
			List<Texture2D> partList = new List<Texture2D>();
			List<OpenCVForUnity.Rect> bbList = new List<OpenCVForUnity.Rect>();
			modelSizeMat = Segmentation.Segment(inputMat, out partList, out bbList);

			head     = new BodyPart(partList[0], bbList[0]);
			leftEar  = new BodyPart(partList[1], bbList[1]);
			rightEar = new BodyPart(partList[2], bbList[2]);
			body     = new BodyPart(partList[3], bbList[3]);
			leftArm  = new BodyPart(partList[4], bbList[4]);
			rightArm = new BodyPart(partList[5], bbList[5]);
			leftLeg  = new BodyPart(partList[6], bbList[6]);
			rightLeg = new BodyPart(partList[7], bbList[7]);
			tail     = new BodyPart(partList[8], bbList[8]);
		}

		public class BodyPart
		{
			Texture2D m_texture;
			public Texture2D texture {
				get {
					return m_texture;
				}
				set {
					m_texture = value;
				}
			}

			OpenCVForUnity.Rect m_bb;
			public OpenCVForUnity.Rect bb {
				get {
					return m_bb;
				}
				set {
					m_bb = value;
				}
			}

			Sprite m_sprite;
			public Sprite sprite {
				get {
					if (!m_sprite.texture)
						m_sprite = Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
					return m_sprite;
				}
			}
				
			public BodyPart(Texture2D tex, OpenCVForUnity.Rect bb)
			{
				this.texture = tex;
				this.bb = bb;

				this.m_sprite = Sprite.Create(tex, new UnityEngine.Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			}
		}
	}
}