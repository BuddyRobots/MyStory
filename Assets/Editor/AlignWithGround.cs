using UnityEditor;
using UnityEngine;

namespace MyUtils
{
	public class EditorTools {

		[MenuItem ("Tools/Transform Tools/Align With Ground")]
		static void AlignWithGround()
		{
			Transform[] transforms = Selection.transforms;
			foreach (Transform transform in transforms)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, Vector3.down, out hit))
				{
					Vector3 targetPosition = hit.point;
					if (transform.gameObject.GetComponent<MeshFilter>() != null)
					{
						Bounds bounds = transform.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
						targetPosition.y += bounds.extents.y;
					}
					transform.position = targetPosition;
					Vector3 targetRotation = new Vector3(hit.normal.x, transform.eulerAngles.y, hit.normal.z);
					transform.eulerAngles = targetRotation;
				}
			}
		}
	}
}
