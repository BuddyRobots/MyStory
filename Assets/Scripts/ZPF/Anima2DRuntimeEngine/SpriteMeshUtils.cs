using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TriangleNet.Geometry;


namespace Anima2DRuntimeEngine
{
	public class SpriteMeshUtils
	{
		static Material m_DefaultMaterial = null;
		public static Material defaultMaterial {
			get {
				if(!m_DefaultMaterial)
				{
					GameObject go = new GameObject();
					SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
					m_DefaultMaterial = sr.sharedMaterial;
					GameObject.DestroyImmediate(go);
				}
				return m_DefaultMaterial;
			}
		}

		public class WeightedTriangle
		{
			int m_P1;
			int m_P2;
			int m_P3;
			float m_W1;
			float m_W2;
			float m_W3;
			float m_Weight;

			public int p1 { get { return m_P1; } }
			public int p2 { get { return m_P2; } }
			public int p3 { get { return m_P3; } }
			public float w1 { get { return m_W1; } }
			public float w2 { get { return m_W2; } }
			public float w3 { get { return m_W3; } }
			public float weight { get { return m_Weight; } }

			public WeightedTriangle(int _p1, int _p2, int _p3,
				float _w1, float _w2, float _w3)
			{
				m_P1 = _p1;
				m_P2 = _p2;
				m_P3 = _p3;
				m_W1 = _w1;
				m_W2 = _w2;
				m_W3 = _w3;
				m_Weight = (w1 + w2 + w3) / 3f;
			}
		}

		public static GameObject CreateFromGameObjectRoot(GameObject oriRootGO)
		{
			GameObject newRootGO = new GameObject("new_" + oriRootGO.name);

			PreOrderTraversalCreate(oriRootGO.transform, newRootGO.transform);

			return newRootGO;
		}

		private static void PreOrderTraversalCreate(Transform oriRoot, Transform newRoot)
		{
			GameObject oriRootGO = oriRoot.gameObject;
			GameObject newRootGO = newRoot.gameObject;
			newRoot.position = oriRoot.position;
			newRoot.rotation = oriRoot.rotation;

			SpriteRenderer oriRootGOSpriteRenderer = oriRootGO.GetComponent<SpriteRenderer>();
			if (oriRootGOSpriteRenderer)
			{
				Sprite oriRootGOSprite = oriRootGOSpriteRenderer.sprite;
				SpriteMesh newRootGOSpriteMesh = null;
				SpriteMeshData newRootGOSpriteMeshData = null;

				SpriteMeshUtils.CreateSpriteMesh(oriRootGOSprite as Sprite, out newRootGOSpriteMesh, out newRootGOSpriteMeshData);

				SpriteMeshInstance newRootGOSpriteMeshInstance = SpriteMeshUtils.CreateSpriteMeshInstance(newRootGOSpriteMesh, newRootGOSpriteMeshData, newRootGO);
				newRootGOSpriteMeshInstance.UpdateCurrentMesh();
				newRootGOSpriteMeshInstance.sortingOrder = oriRootGOSpriteRenderer.sortingOrder;
			}

			foreach (Transform child in oriRoot)
			{
				GameObject newChildGO = new GameObject("new_" + child.gameObject.name);
				newChildGO.transform.parent = newRootGO.transform;

				PreOrderTraversalCreate(child, newChildGO.transform);						
			}		
		}

		public static SpriteMeshInstance FindInstanceWithName(GameObject spriteRootGO, string name)
		{
			Component[] instances;
			instances = spriteRootGO.GetComponentsInChildren<SpriteMeshInstance>();

			foreach (SpriteMeshInstance instance in instances)
				if (instance.name == name)
					return instance;
			return new SpriteMeshInstance();
		}
			
		public static void BindBoneToInstance(GameObject spriteRootGO)
		{
			Component[] instances;
			instances = spriteRootGO.GetComponentsInChildren<SpriteMeshInstance>();

			foreach (SpriteMeshInstance instance in instances)
			{
				SpriteMeshEditorWindow spriteMeshEditorWindow = new SpriteMeshEditorWindow();
				spriteMeshEditorWindow.UpdateFromSelection(instance.gameObject, instance.spriteMeshData);
				spriteMeshEditorWindow.HandleAddBindPose();
				instance.UpdateCurrentMesh();
			}
		}

		// TODO need to complete this
		public static SpriteMesh CreateSpriteMesh(Texture2D texture)
		{
			return new SpriteMesh();
		}
			
		public static void CreateSpriteMesh(Sprite sprite, out SpriteMesh spriteMesh, out SpriteMeshData spriteMeshData)
		{
			spriteMesh = null;
			spriteMeshData = null;

			if(sprite)
			{
				spriteMesh = ScriptableObject.CreateInstance<SpriteMesh>();
				InitFromSprite(spriteMesh, sprite);

				spriteMeshData = ScriptableObject.CreateInstance<SpriteMeshData>();
				spriteMeshData.name = spriteMesh.name + "_Data";
				InitFromSprite(spriteMeshData, sprite);

				UpdateAssets(spriteMesh, spriteMeshData);
			}

			return;
		}
			
		private static void InitFromSprite(SpriteMesh spriteMesh, Sprite sprite)
		{
			if (sprite)
			{
				spriteMesh.name = sprite.name;
				spriteMesh.sprite = sprite;
			}
		}
			
		private static void InitFromSprite(SpriteMeshData spriteMeshData, Sprite sprite)
		{
			Vector2[] vertices;
			IndexedEdge[] edges;
			int[] indices;
			Vector2 pivotPoint;

			if(sprite)
			{
				GetSpriteData(sprite, out vertices, out edges, out indices, out pivotPoint);

				spriteMeshData.vertices = vertices;
				spriteMeshData.edges = edges;
				spriteMeshData.indices = indices;
				spriteMeshData.pivotPoint = pivotPoint;
			}
		}
			
		public static void GetSpriteData(Sprite sprite, out Vector2[] vertices, out IndexedEdge[] edges, out int[] indices, out Vector2 pivotPoint)
		{
			int width = 0;
			int height = 0;

			GetSpriteTextureSize(sprite,ref width, ref height);

			pivotPoint = Vector2.zero;

			Vector2[] uvs = sprite.uv;

			vertices = new Vector2[uvs.Length];

			for(int i = 0; i < uvs.Length; ++i)
			{
				vertices[i] = new Vector2(uvs[i].x * width, uvs[i].y * height);
			}

			ushort[] l_indices = sprite.triangles;

			indices = new int[l_indices.Length];

			for(int i = 0; i < l_indices.Length; ++i)
			{	
				indices[i] = (int)l_indices[i];
			}

			HashSet<IndexedEdge> edgesSet = new HashSet<IndexedEdge>();

			for(int i = 0; i < indices.Length; i += 3)
			{
				int index1 = indices[i];
				int index2 = indices[i+1];
				int index3 = indices[i+2];

				IndexedEdge edge1 = new IndexedEdge(index1,index2);
				IndexedEdge edge2 = new IndexedEdge(index2,index3);
				IndexedEdge edge3 = new IndexedEdge(index1,index3);

				if(edgesSet.Contains(edge1))
				{
					edgesSet.Remove(edge1);
				}else{
					edgesSet.Add(edge1);
				}

				if(edgesSet.Contains(edge2))
				{
					edgesSet.Remove(edge2);
				}else{
					edgesSet.Add(edge2);
				}

				if(edgesSet.Contains(edge3))
				{
					edgesSet.Remove(edge3);
				}else{
					edgesSet.Add(edge3);
				}
			}

			edges = new IndexedEdge[edgesSet.Count];
			int edgeIndex = 0;
			foreach(IndexedEdge edge in edgesSet)
			{
				edges[edgeIndex] = edge;
				++edgeIndex;
			}

			pivotPoint = GetPivotPoint(sprite);
		}
			
		public static void GetSpriteTextureSize(Sprite sprite, ref int width, ref int height)
		{
			if(sprite)
			{
				Texture2D texture = sprite.texture;

				width = texture.width;
				height = texture.height;
			}
		}

		public static Vector2 GetPivotPoint(Sprite sprite)
		{			
			float pixelsPerUnit = GetSpritePixelsPerUnit(sprite);
			return (sprite.pivot + sprite.rect.position) * pixelsPerUnit / sprite.pixelsPerUnit;
		}
			
		public static float GetSpritePixelsPerUnit(Sprite sprite)
		{
			return sprite.pixelsPerUnit;
		}
			
		public static void UpdateAssets(SpriteMesh spriteMesh, SpriteMeshData spriteMeshData)
		{
			if(spriteMesh && spriteMeshData)
			{
				// Intialize spriteMash.sharedMesh
				if(!spriteMesh.sharedMesh)
				{
					spriteMesh.sharedMesh = new Mesh();
				}				
				spriteMesh.sharedMesh.name = spriteMesh.name;

				// Initialize spriteMesh.sharedMaterials
				if(spriteMesh.sharedMaterials.Length == 0)
				{
					Material material = new Material(Shader.Find("Sprites/Default"));
					material.mainTexture = spriteMesh.sprite.texture;

					spriteMesh.sharedMaterials = new Material[1];
					spriteMesh.sharedMaterials[0] = material;
				}

				for (int i = 0; i < spriteMesh.sharedMaterials.Length; i++)
				{
					Material material = spriteMesh.sharedMaterials[i];

					if(material)
					{
						if(spriteMesh.sprite)
						{
							material.mainTexture = spriteMesh.sprite.texture;
						}

						material.name = spriteMesh.name + "_" + i;
					}
				}

				// Initialize triangles, uvs, normals, boneWeights
				int width = 0;
				int height = 0;

				GetSpriteTextureSize(spriteMesh.sprite,ref width,ref height);

				Vector3[] vertices = GetMeshVertices(spriteMesh.sprite, spriteMeshData);

				Vector2 textureWidthHeightInv = new Vector2(1f/width, 1f/height);

				Vector2[] uvs = (new List<Vector2>(spriteMeshData.vertices)).ConvertAll( v => Vector2.Scale(v,textureWidthHeightInv)).ToArray();

				Vector3[] normals = (new List<Vector3>(vertices)).ConvertAll( v => Vector3.back ).ToArray();

				BoneWeight[] boneWeightsData = spriteMeshData.boneWeights;

				if(boneWeightsData.Length != spriteMeshData.vertices.Length)
				{
					boneWeightsData = new BoneWeight[spriteMeshData.vertices.Length];
				}

				List<UnityEngine.BoneWeight> boneWeights = new List<UnityEngine.BoneWeight>(boneWeightsData.Length);

				List<float> verticesOrder = new List<float>(spriteMeshData.vertices.Length);

				for (int i = 0; i < boneWeightsData.Length; i++)
				{
					BoneWeight boneWeight = boneWeightsData[i];

					List< KeyValuePair<int,float> > pairs = new List<KeyValuePair<int, float>>();
					pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex0, boneWeight.weight0));
					pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex1, boneWeight.weight1));
					pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex2, boneWeight.weight2));
					pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex3, boneWeight.weight3));

					pairs = pairs.OrderByDescending( s => s.Value ).ToList();

					UnityEngine.BoneWeight boneWeight2 = new UnityEngine.BoneWeight();
					boneWeight2.boneIndex0 = Mathf.Max(0, pairs[0].Key);
					boneWeight2.boneIndex1 = Mathf.Max(0, pairs[1].Key);
					boneWeight2.boneIndex2 = Mathf.Max(0, pairs[2].Key);
					boneWeight2.boneIndex3 = Mathf.Max(0, pairs[3].Key);
					boneWeight2.weight0 = pairs[0].Value;
					boneWeight2.weight1 = pairs[1].Value;
					boneWeight2.weight2 = pairs[2].Value;
					boneWeight2.weight3 = pairs[3].Value;

					boneWeights.Add(boneWeight2);

					float vertexOrder = i;

					if(spriteMeshData.bindPoses.Length > 0)
					{
						vertexOrder = spriteMeshData.bindPoses[boneWeight2.boneIndex0].zOrder * boneWeight2.weight0 +
							spriteMeshData.bindPoses[boneWeight2.boneIndex1].zOrder * boneWeight2.weight1 +
							spriteMeshData.bindPoses[boneWeight2.boneIndex2].zOrder * boneWeight2.weight2 +
							spriteMeshData.bindPoses[boneWeight2.boneIndex3].zOrder * boneWeight2.weight3;
					}

					verticesOrder.Add(vertexOrder);
				}

				List<WeightedTriangle> weightedTriangles = new List<WeightedTriangle>(spriteMeshData.indices.Length/3);

				for(int i = 0; i < spriteMeshData.indices.Length; i += 3)
				{
					int p1 = spriteMeshData.indices[i];
					int p2 = spriteMeshData.indices[i+1];
					int p3 = spriteMeshData.indices[i+2];

					weightedTriangles.Add(new WeightedTriangle(p1,p2,p3,
						verticesOrder[p1],
						verticesOrder[p2],
						verticesOrder[p3]));
				}

				weightedTriangles = weightedTriangles.OrderBy( t => t.weight ).ToList();

				List<int> indices = new List<int>(spriteMeshData.indices.Length);

				for(int i = 0; i < weightedTriangles.Count; ++i)
				{
					WeightedTriangle t = weightedTriangles[i];
					indices.Add(t.p1);
					indices.Add(t.p2);
					indices.Add(t.p3);
				}

				List<Matrix4x4> bindposes = (new List<BindInfo>(spriteMeshData.bindPoses)).ConvertAll( p => p.bindPose );

				for (int i = 0; i < bindposes.Count; i++)
				{
					Matrix4x4 bindpose = bindposes[i];

					bindpose.m23 = 0f;

					bindposes[i] = bindpose;
				}	

				spriteMesh.sharedMesh.Clear();
				spriteMesh.sharedMesh.vertices = vertices;
				spriteMesh.sharedMesh.uv = uvs;
				spriteMesh.sharedMesh.triangles = indices.ToArray();
				spriteMesh.sharedMesh.normals = normals;
				spriteMesh.sharedMesh.boneWeights = boneWeights.ToArray();
				spriteMesh.sharedMesh.bindposes = bindposes.ToArray();
				spriteMesh.sharedMesh.RecalculateBounds();
			}				
		}

		public static Vector3[] GetMeshVertices(Sprite sprite, SpriteMeshData spriteMeshData)
		{
			float pixelsPerUnit = GetSpritePixelsPerUnit(sprite);

			return (new List<Vector2>(spriteMeshData.vertices)).ConvertAll( v => TexCoordToVertex(spriteMeshData.pivotPoint,v,pixelsPerUnit)).ToArray();
		}

		public static Vector3 TexCoordToVertex(Vector2 pivotPoint, Vector2 vertex, float pixelsPerUnit)
		{
			return (Vector3)(vertex - pivotPoint) / pixelsPerUnit;
		}			

		static Vector3[] GetDeltaVertices(Vector3[] from, Vector3[] to)
		{
			Vector3[] result = new Vector3[from.Length];

			for (int i = 0; i < to.Length; i++)
			{
				result[i] = to[i] - from[i];
			}

			return result;
		}
			
		public static SpriteMeshInstance CreateSpriteMeshInstance(SpriteMesh spriteMesh, SpriteMeshData spriteMeshData)
		{
			if(spriteMesh)
			{
				GameObject gameObject = new GameObject(spriteMesh.name);

				return CreateSpriteMeshInstance(spriteMesh, spriteMeshData, gameObject);
			}

			return null;
		}

		public static SpriteMeshInstance CreateSpriteMeshInstance(SpriteMesh spriteMesh, SpriteMeshData spriteMeshData, GameObject gameObject)
		{
			SpriteMeshInstance spriteMeshInstance = null;

			if(spriteMesh && gameObject)
			{
				spriteMeshInstance = gameObject.AddComponent<SpriteMeshInstance>();

				spriteMeshInstance.spriteMesh = spriteMesh;
				spriteMeshInstance.spriteMeshData = spriteMeshData;
				spriteMeshInstance.sharedMaterial = defaultMaterial;

				List<Bone2D> bones = new List<Bone2D>();
				List<string> paths = new List<string>();

				Vector4 zero = new Vector4 (0f, 0f, 0f, 1f);

				// TODO maybe we don't need to do this yet, because we don't have any bindinfo right now.
				foreach(BindInfo bindInfo in spriteMeshData.bindPoses)
				{
					Matrix4x4 m =  spriteMeshInstance.transform.localToWorldMatrix * bindInfo.bindPose.inverse;

					GameObject bone = new GameObject(bindInfo.name);

					Bone2D boneComponent = bone.AddComponent<Bone2D>();

					boneComponent.localLength = bindInfo.boneLength;
					bone.transform.position = m * zero;
					bone.transform.rotation = m.GetRotation();
					bone.transform.parent = gameObject.transform;

					bones.Add(boneComponent);
					paths.Add(bindInfo.path);
				}

				// TODO why do we need to reconstruct hierarchy?
				BoneUtils.ReconstructHierarchy(bones,paths);

				spriteMeshInstance.bones = bones;

				SpriteMeshUtils.UpdateRenderer(spriteMeshInstance);
			}

			return spriteMeshInstance;
		}

		public static void UpdateRenderer(SpriteMeshInstance spriteMeshInstance)
		{
			if(!spriteMeshInstance)
			{
				return;
			}				

			SpriteMesh spriteMesh = spriteMeshInstance.spriteMesh;

			if(spriteMesh)
			{
				Material[] materials = spriteMesh.sharedMaterials;
				Mesh sharedMesh = spriteMesh.sharedMesh;

				if(sharedMesh.bindposes.Length > 0 && spriteMeshInstance.bones.Count > sharedMesh.bindposes.Length)
				{
					spriteMeshInstance.bones = spriteMeshInstance.bones.GetRange(0,sharedMesh.bindposes.Length);
				}

				if(CanEnableSkinning(spriteMeshInstance))
				{
					MeshFilter meshFilter = spriteMeshInstance.cachedMeshFilter;
					MeshRenderer meshRenderer = spriteMeshInstance.cachedRenderer as MeshRenderer;

					if(meshFilter)
					{
						GameObject.DestroyImmediate(meshFilter);
					}
					if(meshRenderer)
					{
						GameObject.DestroyImmediate(meshRenderer);
					}

					SkinnedMeshRenderer skinnedMeshRenderer = spriteMeshInstance.cachedSkinnedRenderer;

					if(!skinnedMeshRenderer)
					{
						skinnedMeshRenderer = spriteMeshInstance.gameObject.AddComponent<SkinnedMeshRenderer>();
					}

					skinnedMeshRenderer.bones = spriteMeshInstance.bones.ConvertAll( bone => bone.transform ).ToArray();

					if(spriteMeshInstance.bones.Count > 0)
					{
						skinnedMeshRenderer.rootBone = spriteMeshInstance.bones[0].transform;
					}

					skinnedMeshRenderer.sharedMaterials = materials;

				} else {
					
					SkinnedMeshRenderer skinnedMeshRenderer = spriteMeshInstance.cachedSkinnedRenderer;
					MeshFilter meshFilter = spriteMeshInstance.cachedMeshFilter;
					MeshRenderer meshRenderer = spriteMeshInstance.cachedRenderer as MeshRenderer;

					if(skinnedMeshRenderer)
					{
						GameObject.DestroyImmediate(skinnedMeshRenderer);
					}

					if(!meshFilter)
					{
						meshFilter = spriteMeshInstance.gameObject.AddComponent<MeshFilter>();
					}

					if(!meshRenderer)
					{
						meshRenderer = spriteMeshInstance.gameObject.AddComponent<MeshRenderer>();

						meshRenderer.sharedMaterials = materials;
					}
				}
			}
		}

		public static bool CanEnableSkinning(SpriteMeshInstance spriteMeshInstance)
		{
			return spriteMeshInstance.spriteMesh && !HasNullBones(spriteMeshInstance) && spriteMeshInstance.bones.Count > 0 && (spriteMeshInstance.spriteMesh.sharedMesh.bindposes.Length == spriteMeshInstance.bones.Count);
		}

		public static bool HasNullBones(SpriteMeshInstance spriteMeshInstance)
		{
			if(spriteMeshInstance)
			{
				return spriteMeshInstance.bones.Contains(null);
			}
			return false;
		}

		public static Rect GetRect(Sprite sprite)
		{
			float pixelsPerUnit = GetSpritePixelsPerUnit(sprite);
			float factor = pixelsPerUnit / sprite.pixelsPerUnit;
			Vector2 position = sprite.rect.position * factor;
			Vector2 size = sprite.rect.size * factor;

			return new Rect(position.x,position.y,size.x,size.y);
		}

		/*public static void InitFromOutline(Texture2D texture, Rect rect, float detail, float alphaTolerance, bool holeDetection,
			out List<Vector2> vertices, out List<IndexedEdge> indexedEdges, out List<int> indices)
		{
			vertices = new List<Vector2>();
			indexedEdges = new List<IndexedEdge>();
			indices = new List<int>();

			if(texture)
			{
				Vector2[][] paths = GenerateOutline(texture,rect,detail,(byte)(alphaTolerance * 255f),holeDetection);

				int startIndex = 0;
				for (int i = 0; i < paths.Length; i++)
				{
					Vector2[] path = paths [i];
					for (int j = 0; j < path.Length; j++)
					{
						vertices.Add(path[j] + rect.center);
						indexedEdges.Add(new IndexedEdge(startIndex + j,startIndex + ((j+1) % path.Length)));
					}
					startIndex += path.Length;
				}
				List<Hole> holes = new List<Hole>();
				Triangulate(vertices,indexedEdges,holes,ref indices);
			}
		}

		static Vector2[][] GenerateOutline(Texture2D texture, Rect rect, float detail, byte alphaTolerance, bool holeDetection)
		{
			Vector2[][] paths = null;

			MethodInfo methodInfo = typeof(SpriteUtility).GetMethod("GenerateOutline", BindingFlags.Static | BindingFlags.NonPublic);

			if(methodInfo != null)
			{
				object[] parameters = new object[] { texture,rect,detail,alphaTolerance,holeDetection,null };
				methodInfo.Invoke(null,parameters);

				paths = (Vector2[][]) parameters[5];
			}

			return paths;
		}*/

		public static void Triangulate(List<Vector2> vertices, List<IndexedEdge> edges, List<Hole> holes,ref List<int> indices)
		{
			indices.Clear();

			if(vertices.Count >= 3)
			{
				InputGeometry inputGeometry = new InputGeometry(vertices.Count);

				for(int i = 0; i < vertices.Count; ++i)
				{
					Vector2 position = vertices[i];
					inputGeometry.AddPoint(position.x,position.y);
				}

				for(int i = 0; i < edges.Count; ++i)
				{
					IndexedEdge edge = edges[i];
					inputGeometry.AddSegment(edge.index1,edge.index2);
				}

				for(int i = 0; i < holes.Count; ++i)
				{
					Vector2 hole = holes[i].vertex;
					inputGeometry.AddHole(hole.x,hole.y);
				}

				TriangleNet.Mesh triangleMesh = new TriangleNet.Mesh();

				triangleMesh.Triangulate(inputGeometry);

				foreach (TriangleNet.Data.Triangle triangle in triangleMesh.Triangles)
				{
					if(triangle.P0 >= 0 && triangle.P0 < vertices.Count &&
						triangle.P0 >= 0 && triangle.P1 < vertices.Count &&
						triangle.P0 >= 0 && triangle.P2 < vertices.Count)
					{
						indices.Add(triangle.P0);
						indices.Add(triangle.P2);
						indices.Add(triangle.P1);
					}
				}
			}
		}

		public static void Tessellate(List<Vector2> vertices, List<IndexedEdge> indexedEdges, List<Hole> holes, List<int> indices, float tessellationAmount)
		{
			if(tessellationAmount <= 0f)
			{
				return;
			}

			indices.Clear();

			if(vertices.Count >= 3)
			{
				InputGeometry inputGeometry = new InputGeometry(vertices.Count);

				for(int i = 0; i < vertices.Count; ++i)
				{
					Vector2 vertex = vertices[i];
					inputGeometry.AddPoint(vertex.x,vertex.y);
				}

				for(int i = 0; i < indexedEdges.Count; ++i)
				{
					IndexedEdge edge = indexedEdges[i];
					inputGeometry.AddSegment(edge.index1,edge.index2);
				}

				for(int i = 0; i < holes.Count; ++i)
				{
					Vector2 hole = holes[i].vertex;
					inputGeometry.AddHole(hole.x,hole.y);
				}

				TriangleNet.Mesh triangleMesh = new TriangleNet.Mesh();
				TriangleNet.Tools.Statistic statistic = new TriangleNet.Tools.Statistic();

				triangleMesh.Triangulate(inputGeometry);

				triangleMesh.Behavior.MinAngle = 20.0;
				triangleMesh.Behavior.SteinerPoints = -1;
				triangleMesh.Refine(true);

				statistic.Update(triangleMesh, 1);

				triangleMesh.Refine(statistic.LargestArea / tessellationAmount);
				triangleMesh.Renumber();

				vertices.Clear();
				indexedEdges.Clear();

				foreach(TriangleNet.Data.Vertex vertex in triangleMesh.Vertices)
				{
					vertices.Add(new Vector2((float)vertex.X,(float)vertex.Y));
				}

				foreach(TriangleNet.Data.Segment segment in triangleMesh.Segments)
				{
					indexedEdges.Add(new IndexedEdge(segment.P0,segment.P1));
				}

				foreach (TriangleNet.Data.Triangle triangle in triangleMesh.Triangles)
				{
					if(triangle.P0 >= 0 && triangle.P0 < vertices.Count &&
						triangle.P0 >= 0 && triangle.P1 < vertices.Count &&
						triangle.P0 >= 0 && triangle.P2 < vertices.Count)
					{
						indices.Add(triangle.P0);
						indices.Add(triangle.P2);
						indices.Add(triangle.P1);
					}
				}
			}
		}

		public static Vector2 VertexToTexCoord(SpriteMesh spriteMesh, Vector2 pivotPoint, Vector3 vertex, float pixelsPerUnit)
		{
			Vector2 texCoord = Vector3.zero;

			if(spriteMesh != null)
			{
				texCoord = (Vector2)vertex * pixelsPerUnit + pivotPoint;
			}

			return texCoord;
		}
	}
}