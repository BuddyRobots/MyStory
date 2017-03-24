using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Serialization;

namespace Anima2DRuntimeEngine
{
	public class Bone2D : MonoBehaviour
	{		
		Color m_Color = Color.white;
		public Color color {
			get {
				return m_Color;
			}
			set {
				m_Color = color;
			}
		}
			
		[SerializeField]
		float m_Length = 1f;

		[SerializeField]
		Transform m_ChildTransform;				
		Bone2D m_CachedChild;
		public Bone2D child
		{
			get {
				if(m_CachedChild && m_ChildTransform != m_CachedChild.transform)
				{
					m_CachedChild = null;
				}

				if(m_ChildTransform && m_ChildTransform.parent != transform)
				{
					m_CachedChild = null;
				}

				if(!m_CachedChild && m_ChildTransform && m_ChildTransform.parent == transform)
				{
					m_CachedChild = m_ChildTransform.GetComponent<Bone2D>();
				}

				return m_CachedChild;
			}
			set {
				m_CachedChild = value;
				if (m_CachedChild)
				{
					m_ChildTransform = m_CachedChild.transform;
				}
				else
					m_ChildTransform = null;
			}
		}

		public Vector3 localEndPosition
		{
			get {
				return Vector3.right*localLength;
			}
		}

		public Vector3 endPosition
		{
			get {
				return transform.TransformPoint(localEndPosition);
			}
		}

		public float localLength {
			get {
				if(child)
				{
					Vector3 childPosition = transform.InverseTransformPoint(child.transform.position);
					m_Length = Mathf.Clamp(childPosition.x,0f,childPosition.x);
				}

				return m_Length;
			}
			set {
				if(!child)
				{
					m_Length = value;
				}
			}
		}			

		public float length {
			get {
				return transform.TransformVector(localEndPosition).magnitude;
			}
		}

		Bone2D mParentBone = null;
		public Bone2D parentBone
		{
			get {
				Transform parentTransform = transform.parent;

				if(!mParentBone)
				{
					if(parentTransform)
					{
						mParentBone = parentTransform.GetComponent<Bone2D>();
					}
				}else if(parentTransform != mParentBone.transform)
				{
					if(parentTransform)
					{
						mParentBone = parentTransform.GetComponent<Bone2D>();
					}else{
						mParentBone = null;
					}
				}
				
				return mParentBone;
			}
		}

		public Bone2D linkedParentBone
		{
			get {
				if(parentBone && parentBone.child == this)
				{
					return parentBone;
				}
				
				return null;
			}
		}
		
		public Bone2D root
		{
			get {
				Bone2D rootBone = this;
				
				while(rootBone.parentBone)
				{
					rootBone = rootBone.parentBone;
				}
				
				return rootBone;
			}
		}

		public Bone2D chainRoot
		{
			get {
				Bone2D chainRoot = this;
				
				while(chainRoot.parentBone && chainRoot.parentBone.child == chainRoot)
				{
					chainRoot = chainRoot.parentBone;
				}
				
				return chainRoot;
			}
		}

		public int chainLength
		{
			get {
				Bone2D chainRoot = this;

				int length = 1;

				while(chainRoot.parentBone && chainRoot.parentBone.child == chainRoot)
				{
					++length;
					chainRoot = chainRoot.parentBone;
				}
				
				return length;
			}
		}

		public static Bone2D GetChainBoneByIndex(Bone2D chainTip, int index)
		{
			if(!chainTip) return null;
			
			Bone2D bone = chainTip;
			
			int chainLength = bone.chainLength;
			
			for(int i = 0; i < chainLength && bone; ++i)
			{
				if(i == index)
				{
					return bone;
				}
				
				if(bone.linkedParentBone)
				{
					bone = bone.parentBone;
				}else{
					return null;
				}
			}
			
			return null;
		}
	}
}