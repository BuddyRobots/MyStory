using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Anima2DRuntimeEngine
{
	public static class SpriteMeshInstanceEditor
	{
		public static void SetupBoneList(SpriteMeshInstance spriteMeshInstance, List<Bone2D> boneList)
		{
			spriteMeshInstance.bones = boneList;
		}
	}
}