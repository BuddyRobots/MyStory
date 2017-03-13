using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Anima2DRuntimeEngine
{
	public static class BoneUtils
	{	
		public static GameObject CreateFromAnima2DBone2D(GameObject boneRootGO)
		{
			GameObject newBoneRootGO = new GameObject(boneRootGO.name);
			newBoneRootGO.transform.position = boneRootGO.transform.position;
			newBoneRootGO.transform.rotation = boneRootGO.transform.rotation;

			Anima2D.Bone2D boneRoot = boneRootGO.GetComponent<Anima2D.Bone2D>();
			if (boneRoot)
			{				
				Bone2D newBoneRoot = newBoneRootGO.AddComponent<Bone2D>();
				InitFromAnima2DBone2D(boneRoot, newBoneRoot);

				PreOrderTraversalCreate(boneRootGO.transform, newBoneRootGO.transform);
			}
			return newBoneRootGO;
		}

		private static void PreOrderTraversalCreate(Transform oriRoot, Transform newRoot)
		{
			foreach (Transform child in oriRoot)
			{
				GameObject newChildGO = new GameObject(child.gameObject.name);
				newChildGO.transform.position = child.position;
				newChildGO.transform.rotation = child.rotation;
				newChildGO.transform.parent = newRoot;
				Anima2D.Bone2D childBone = child.gameObject.GetComponent<Anima2D.Bone2D>();
				if (childBone)
				{
					Bone2D newChildBone = newChildGO.AddComponent<Bone2D>();
					InitFromAnima2DBone2D(childBone, newChildBone);
					if (!newRoot.gameObject.GetComponent<Bone2D>().child)
						newRoot.gameObject.GetComponent<Bone2D>().child = newChildBone;

					PreOrderTraversalCreate(child, newChildGO.transform);
				}				
			}		
		}

		private static void InitFromAnima2DBone2D(Anima2D.Bone2D oldBone, Bone2D newBone)
		{
			newBone.color = oldBone.color;
			newBone.localLength = oldBone.localLength;
			newBone.name = oldBone.name;
		}
			
		public static Bone2D FindBoneWithName(GameObject boneRootGO, string name)
		{
			Component[] bone2Ds;
			bone2Ds = boneRootGO.GetComponentsInChildren<Bone2D>();

			foreach (Bone2D bone2D in bone2Ds)
				if (bone2D.name == name)
					return bone2D;

			Debug.Log("BoneUtils.cs FindBoneWithName() : Bone " + name + " not found!");
			return new Bone2D();
		}

		public static Bone2D ReconstructHierarchy(List<Bone2D> bones, List<string> paths)
		{
			Bone2D rootBone = null;

			for (int i = 0; i < bones.Count; i++)
			{
				Bone2D bone = bones[i];
				string path = paths[i];

				for (int j = 0; j < bones.Count; j++)
				{
					Bone2D other = bones [j];
					string otherPath = paths[j];

					if(bone != other && !path.Equals(otherPath) && otherPath.Contains(path))
					{
						other.transform.parent = bone.transform;
						other.transform.localScale = Vector3.one;
					}
				}
			}

			for (int i = 0; i < bones.Count; i++)
			{
				Bone2D bone = bones[i];

				if(bone.parentBone)
				{
					if((bone.transform.position - bone.parentBone.endPosition).sqrMagnitude < 0.00001f)
					{
						bone.parentBone.child = bone;
					}
				}else{
					rootBone = bone;
				}
			}

			return rootBone;
		}
	}
}
