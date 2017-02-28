using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Anima2DRuntimeEngine
{
	public class SpriteMeshCache : ScriptableObject
	{
		public SpriteMesh spriteMesh;
		public SpriteMeshData spriteMeshData;
		public SpriteMeshInstance spriteMeshInstance;
		public List<BindInfo> bindPoses = new List<BindInfo>();

		public Rect rect;
		public List<Node> nodes = new List<Node>();
		public List<Hole> holes = new List<Hole>();
		public List<Edge> edges = new List<Edge>();
		List<Vector2> m_TexVertices = new List<Vector2>();
		public List<BoneWeight> boneWeights = new List<BoneWeight>();
		public List<int> indices = new List<int>();

		List<IndexedEdge> indexedEdges {
			get {
				return edges.ConvertAll( e => new IndexedEdge(nodes.IndexOf(e.node1), nodes.IndexOf(e.node2)) );
			}
		}

		public Vector2 pivotPoint = Vector2.zero;	

		public bool isBound { get { return bindPoses.Count > 0f; } }


		public void SetSpriteMesh(SpriteMesh _spriteMesh, SpriteMeshData _spriteMeshData, SpriteMeshInstance _spriteMeshInstance)
		{
			spriteMesh = _spriteMesh;
			spriteMeshInstance = _spriteMeshInstance;
			spriteMeshData = _spriteMeshData;

			SetSpriteMeshData();

			pivotPoint = spriteMeshData.pivotPoint;
		}
			
		public void SetSpriteMeshData()
		{
			Clear();

			if(spriteMesh && spriteMeshData)
			{
				pivotPoint = spriteMeshData.pivotPoint;
				rect = SpriteMeshUtils.GetRect(spriteMesh.sprite);

				m_TexVertices = spriteMeshData.vertices.ToList();

				nodes = m_TexVertices.ConvertAll( v => Node.Create(m_TexVertices.IndexOf(v)) );
				boneWeights = spriteMeshData.boneWeights.ToList();
				edges = spriteMeshData.edges.ToList().ConvertAll( e => Edge.Create(nodes[e.index1],nodes[e.index2]) );
				holes = spriteMeshData.holes.ToList().ConvertAll( h => new Hole(h) );
				indices = spriteMeshData.indices.ToList();
				bindPoses = spriteMeshData.bindPoses.ToList().ConvertAll( b => b.Clone() as BindInfo );

				if(boneWeights.Count != nodes.Count)
				{
					boneWeights = nodes.ConvertAll( n => BoneWeight.Create() );
				}
			}
		}

		public void Clear()
		{
			foreach(Edge edge in edges)
			{
				DestroyImmediate(edge);
			}

			foreach(Node node in nodes)
			{
				DestroyImmediate(node);
			}

			nodes.Clear();
			edges.Clear();
			indices.Clear();
			boneWeights.Clear();
		}			

		public void BindBones()
		{
			if(spriteMeshInstance)
			{
				bindPoses.Clear();

				foreach(Bone2D bone in spriteMeshInstance.bones)
				{
					BindBone(bone);
				}
			}
		}

		public void BindBone(Bone2D bone)
		{
			if(spriteMeshInstance && bone)
			{
				BindInfo bindInfo = new BindInfo();
				bindInfo.bindPose = bone.transform.worldToLocalMatrix * spriteMeshInstance.transform.localToWorldMatrix;
				bindInfo.boneLength = bone.localLength;
				bindInfo.name = bone.name;

				if(!bindPoses.Contains(bindInfo))
				{
					bindPoses.Add(bindInfo);
				}
			}
		}

		public void CalculateAutomaticWeights()
		{
			CalculateAutomaticWeights(nodes);
		}

		public void CalculateAutomaticWeights(List<Node> targetNodes)
		{
			float pixelsPerUnit = SpriteMeshUtils.GetSpritePixelsPerUnit(spriteMesh.sprite);

			if(nodes.Count <= 0)
			{
				Debug.Log("Cannot calculate automatic weights from a SpriteMesh with no vertices.");
				return;
			}

			if(bindPoses.Count <= 0)
			{
				Debug.Log("Cannot calculate automatic weights. Specify bones to the SpriteMeshInstance.");
				return;
			}

			if(spriteMesh && bindPoses.Count > 1)
			{
				List<Vector2> controlPoints = new List<Vector2>(bindPoses.Count*2);
				List<IndexedEdge> controlPointEdges = new List<IndexedEdge>(bindPoses.Count);

				foreach(BindInfo bindInfo in bindPoses)
				{
					Vector2 tip = SpriteMeshUtils.VertexToTexCoord(spriteMesh,pivotPoint,bindInfo.position,pixelsPerUnit);
					Vector2 tail = SpriteMeshUtils.VertexToTexCoord(spriteMesh,pivotPoint,bindInfo.endPoint,pixelsPerUnit);

					int index1 = -1;

					if(!ContainsVector(tip, controlPoints, 0.01f, out index1))
					{
						index1 = controlPoints.Count;
						controlPoints.Add(tip);
					}

					int index2 = -1;

					if(!ContainsVector(tail, controlPoints, 0.01f, out index2))
					{
						index2 = controlPoints.Count;
						controlPoints.Add(tail);
					}

					IndexedEdge edge = new IndexedEdge(index1, index2);
					controlPointEdges.Add(edge);	
				}
					
				float[,] weightArray;

				/*BbwPlugin.CalculateBbw(
				    m_TexVertices.ToArray(),
					indexedEdges.ToArray(),
					controlPoints.ToArray(),
					controlPointEdges.ToArray(),
					out weightArray
				);*/
				BbwPlugin.SimplifiedCalculateBbw(
					m_TexVertices.ToArray(),
					controlPoints.ToArray(),
					controlPointEdges.ToArray(),
					out weightArray
				);

				FillBoneWeights(targetNodes, weightArray);

			} else {

				BoneWeight boneWeight = BoneWeight.Create();
				boneWeight.boneIndex0 = 0;
				boneWeight.weight0 = 1f;

				foreach(Node node in targetNodes)
				{
					SetBoneWeight(node, boneWeight);
				}
			}
		}

		bool ContainsVector(Vector2 vectorToFind, List<Vector2> list, float epsilon, out int index)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Vector2 v = list [i];
				if ((v - vectorToFind).sqrMagnitude < epsilon)
				{
					index = i;
					return true;
				}
			}

			index = -1;
			return false;
		}

		void FillBoneWeights(List<Node> targetNodes, float[,] weights)
		{
			List<float> l_weights = new List<float>();

			foreach(Node node in targetNodes)
			{
				l_weights.Clear();

				for(int i = 0; i < bindPoses.Count; ++i)
				{
					l_weights.Add(weights[i,node.index]);
				}

				SetBoneWeight(node,CreateBoneWeightFromWeights(l_weights));
			}
		}

		public void SetBoneWeight(Node node, BoneWeight boneWeight)
		{
			boneWeights[node.index] = boneWeight;
		}

		BoneWeight CreateBoneWeightFromWeights(List<float> weights)
		{
			BoneWeight boneWeight = new BoneWeight();

			float weight = 0f;
			int index = -1;

			weight = weights.Max();
			if(weight < 0.01f) weight = 0f;
			index = weight > 0f? weights.IndexOf(weight) : -1;

			boneWeight.weight0 = weight;
			boneWeight.boneIndex0 = index;

			if(index >= 0) weights[index] = 0f;

			weight = weights.Max();
			if(weight < 0.01f) weight = 0f;
			index = weight > 0f? weights.IndexOf(weight) : -1;

			boneWeight.weight1 = weight;
			boneWeight.boneIndex1 = index;

			if(index >= 0) weights[index] = 0f;

			weight = weights.Max();
			if(weight < 0.01f) weight = 0f;
			index = weight > 0f? weights.IndexOf(weight) : -1;

			boneWeight.weight2 = weight;
			boneWeight.boneIndex2 = index;

			if(index >= 0) weights[index] = 0f;

			weight = weights.Max();
			if(weight < 0.01f) weight = 0f;
			index = weight > 0f? weights.IndexOf(weight) : -1;

			boneWeight.weight3 = weight;
			boneWeight.boneIndex3 = index;

			float sum = boneWeight.weight0 + 
				boneWeight.weight1 +
				boneWeight.weight2 +
				boneWeight.weight3;

			if(sum > 0f)
			{
				boneWeight.weight0 /= sum;
				boneWeight.weight1 /= sum;
				boneWeight.weight2 /= sum;
				boneWeight.weight3 /= sum;
			}

			return boneWeight;
		}

		public void ApplyChanges()
		{
			if(spriteMeshData)
			{
				spriteMeshData.vertices = m_TexVertices.ToArray();
				spriteMeshData.boneWeights = boneWeights.ToArray();
				spriteMeshData.edges = indexedEdges.ToArray();
				spriteMeshData.holes = holes.ConvertAll( h => h.vertex ).ToArray();
				spriteMeshData.indices = indices.ToArray();
				spriteMeshData.bindPoses = bindPoses.ToArray();
				spriteMeshData.pivotPoint = pivotPoint;

				SpriteMeshUtils.UpdateAssets(spriteMesh, spriteMeshData);
			}

			if(spriteMeshInstance)
			{
				SpriteMeshUtils.UpdateRenderer(spriteMeshInstance);
			}
		}			
	}
}