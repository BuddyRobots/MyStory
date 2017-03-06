﻿using UnityEngine;
using System.Collections;

namespace Anima2DRuntimeEngine
{
	// May need this to specify the smoothness of created triangles
	public class SliceEditor
	{
		public SpriteMeshCache spriteMeshCache;

		public float alpha { get; set; }
		public float detail { get; set; }
		public float tessellation { get; set; }
		public bool holes { get; set; }

		public SliceEditor()
		{
			detail = 0.25f;
			alpha = 0.05f;
			tessellation = 0.0f;
			holes = false;
		}

		public void slice()
		{
			if(spriteMeshCache.spriteMeshInstance)
			{
				//spriteMeshCache.InitFromOutline(detail, alpha, holes, tessellation);
			}
		}
	}
}