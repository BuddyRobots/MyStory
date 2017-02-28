using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Anima2DRuntimeEngine
{
	public class BbwPlugin
	{
		/*[DllImport ("Anima2D")]
		private static extern int Bbw([In,Out] IntPtr vertices, int vertexCount, int originalVertexCount,
		                              [In,Out] IntPtr indices, int indexCount,
		                              [In,Out] IntPtr edges, int edgesCount,
		                              [In,Out] IntPtr controlPoints, int controlPointsCount,
		                              [In,Out] IntPtr boneEdges, int boneEdgesCount,
		                              [In,Out] IntPtr weights
		);*/
		

//		public static void CalculateBbw(Vector2[] vertices, IndexedEdge[] edges, Vector2[] controlPoints, IndexedEdge[] controlPointEdges, out float[,] weights)
//		{
//			///
//			/*for (var i = 0; i < vertices.Length; i++)
//				Debug.Log("vertices["+i+"] = " + vertices[i].ToString("F4"));
//			for (var i = 0; i < controlPoints.Length; i++)
//				Debug.Log("controlPointes["+i+"] = " + controlPoints[i]);*/
//			///
//
//
//
//
//			Vector2[] sampledEdges = SampleEdges(controlPoints,controlPointEdges,10);
//
//			List<Vector2> verticesAndSamplesList = new List<Vector2>(vertices.Length + sampledEdges.Length);
//
//			verticesAndSamplesList.AddRange(vertices);
//			verticesAndSamplesList.AddRange(sampledEdges);
//
//			List<IndexedEdge> edgesList = new List<IndexedEdge>(edges);
//			List<Hole> holes = new List<Hole>();
//			List<int> indicesList = new List<int>();
//
//			SpriteMeshUtils.Tessellate(verticesAndSamplesList, edgesList, holes, indicesList, 4f);
//
//			Vector2[] verticesAndSamples = verticesAndSamplesList.ToArray();
//			int[] indices = indicesList.ToArray();
//
//			weights = new float[controlPointEdges.Length, vertices.Length];
//
//			GCHandle verticesHandle = GCHandle.Alloc(verticesAndSamples, GCHandleType.Pinned);
//			GCHandle indicesHandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
//			GCHandle edgesHandle = GCHandle.Alloc(edges, GCHandleType.Pinned);
//			GCHandle controlPointsHandle = GCHandle.Alloc(controlPoints, GCHandleType.Pinned);
//			GCHandle boneEdgesHandle = GCHandle.Alloc(controlPointEdges, GCHandleType.Pinned);
//			GCHandle weightsHandle = GCHandle.Alloc(weights, GCHandleType.Pinned);
//
//			Bbw(verticesHandle.AddrOfPinnedObject(), verticesAndSamples.Length, vertices.Length,
//			    indicesHandle.AddrOfPinnedObject(), indices.Length,
//			    edgesHandle.AddrOfPinnedObject(), edges.Length,
//			    controlPointsHandle.AddrOfPinnedObject(), controlPoints.Length,
//			    boneEdgesHandle.AddrOfPinnedObject(), controlPointEdges.Length,
//			    weightsHandle.AddrOfPinnedObject());
//
//
//
//
//			///
//			/*for (var i = 0; i < verticesAndSamples.Length; i++)
//				Debug.Log("vertices["+i+"] = " + verticesAndSamples[i]);			
//			for (var i = 0; i < indices.Length; i++)
//				Debug.Log("indices["+i+"] = " + indices[i]);
//			for (var i = 0; i < edges.Length; i++)
//				Debug.Log("edges["+i+"] = " + edges[i].index1 + " " + edges[i].index2);
//			for (var i = 0; i < controlPoints.Length; i++)
//				Debug.Log("controlPoints["+i+"] = " + controlPoints[i]);
//			for (var i = 0; i < controlPointEdges.Length; i++)
//				Debug.Log("controlPointEdges["+i+"] = " + controlPointEdges[i].index1 + " " + controlPointEdges[i].index2);*/
//			for (var i = 0; i < weights.GetLength(0); i++)
//				for (var j = 0; j < weights.GetLength(1); j++)
//					Debug.Log("weights["+i+", "+j+"] = " + weights[i, j]);
//			///
//
//
//
//
//			verticesHandle.Free();
//			indicesHandle.Free();
//			edgesHandle.Free();
//			controlPointsHandle.Free();
//			boneEdgesHandle.Free();
//			weightsHandle.Free();
//		}

//		static Vector2[] SampleEdges(Vector2[] controlPoints, IndexedEdge[] controlPointEdges, int samplesPerEdge)
//		{
//			int totalCount = controlPoints.Length + samplesPerEdge * controlPointEdges.Length;
//				
//			List<Vector2> sampledVertices = new List<Vector2>(totalCount);
//
//			sampledVertices.AddRange(controlPoints);
//
//			for(int i = 0; i < controlPointEdges.Length; i++)
//			{
//				IndexedEdge edge = controlPointEdges[i];
//
//				Vector2 tip = controlPoints[edge.index1];
//				Vector2 tail = controlPoints[edge.index2];
//
//				for(int s = 0; s < samplesPerEdge; s++)
//				{
//					float f = (s+1f)/(float)(samplesPerEdge+1f);
//					sampledVertices.Add(f * tail + (1f-f)*tip);
//				}
//			}
//
//			return sampledVertices.ToArray();
//		}

		// TODO need to tune this
		// The weights does not need to have sum 1.
		public static void SimplifiedCalculateBbw(Vector2[] vertices, Vector2[] controlPoints, IndexedEdge[] controlPointEdges, out float[,] weights)
		{
			weights = new float[controlPointEdges.Length, vertices.Length];

			Vector2[] oneThirdPoints = new Vector2[controlPointEdges.Length];

			for (var i = 0; i < controlPointEdges.Length; i++)
			{
				Vector2 tip = controlPoints[controlPointEdges[i].index1];
				Vector2 tail = controlPoints[controlPointEdges[i].index2];

				oneThirdPoints[i] = tip + (tail - tip)/3;
			}
				
			List<float> distanceList = new List<float>();
			for (var i = 0; i < vertices.Length; i++)
			{				
				for (var j = 0; j < oneThirdPoints.Length; j++)
				{
					distanceList.Add(Vector2.Distance(vertices[i], oneThirdPoints[j]));
				}

				for (var k = 0; k < distanceList.Count; k++)
					distanceList[k] = 1/Mathf.Pow(distanceList[k], 2);				
				float sum = distanceList.Sum();
				for (var k = 0; k < distanceList.Count; k++)
					distanceList[k] = distanceList[k]/sum;

				for (var j = 0; j < oneThirdPoints.Length; j++)
					weights[j, i] = distanceList[j];

				distanceList.Clear();
			}
		}
	}
}