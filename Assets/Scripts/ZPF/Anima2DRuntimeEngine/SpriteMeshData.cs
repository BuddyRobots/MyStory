using UnityEngine;


namespace Anima2DRuntimeEngine
{
	public class SpriteMeshData : ScriptableObject
	{
		Vector2 m_PivotPoint;
		Vector2[] m_Vertices = new Vector2[0];
		BoneWeight[] m_BoneWeights = new BoneWeight[0];
		IndexedEdge[] m_Edges = new IndexedEdge[0];
		Vector2[] m_Holes = new Vector2[0];
		int[] m_Indices = new int[0];
		BindInfo[] m_BindPoses = new BindInfo[0];


		public Vector2 pivotPoint
		{
			get {
				return m_PivotPoint;
			}
			set {
				m_PivotPoint = value;
			}
		}

		public Vector2[] vertices
		{
			get {
				return m_Vertices;
			}
			set {
				m_Vertices = value;
			}
		}

		public BoneWeight[] boneWeights
		{
			get {
				return m_BoneWeights;
			}
			set {
				m_BoneWeights = value;
			}
		}

		public IndexedEdge[] edges
		{
			get {
				return m_Edges;
			}
			set {
				m_Edges = value;
			}
		}

		public int[] indices
		{
			get {
				return m_Indices;
			}
			set {
				m_Indices = value;
			}
		}

		public Vector2[] holes
		{
			get {
				return m_Holes;
			}
			set {
				m_Holes = value;
			}
		}

		public BindInfo[] bindPoses
		{
			get {
				return m_BindPoses;
			}
			set {
				m_BindPoses = value;
			}
		}
	}
}
