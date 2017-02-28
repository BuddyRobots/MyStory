using UnityEngine;


namespace Anima2DRuntimeEngine
{
	public class SpriteMesh : ScriptableObject
	{
		Sprite m_Sprite;
		Mesh m_SharedMesh;
		Material[] m_SharedMaterials = new Material[0];

		public Sprite sprite
		{ 
			get { return m_Sprite; } 
			set { m_Sprite = value; }
		}

		public Mesh sharedMesh 
		{ 
			get { return m_SharedMesh; }
			set { m_SharedMesh = value; }
		}

		public Material[] sharedMaterials 
		{ 
			get { return m_SharedMaterials; }
			set { m_SharedMaterials = value; }
		}
	}
}
