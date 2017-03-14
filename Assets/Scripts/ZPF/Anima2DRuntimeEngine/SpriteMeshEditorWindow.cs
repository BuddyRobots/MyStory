using UnityEngine;
using System.Collections;

namespace Anima2DRuntimeEngine
{
	public class SpriteMeshEditorWindow
	{
		SpriteMeshCache m_SpriteMeshCache;


		public void UpdateFromSelection(GameObject selection, SpriteMeshData l_spriteMeshData)
		{
			if(!m_SpriteMeshCache)
			{				
				InvalidateCache();
			}

			SpriteMesh l_spriteMesh = null;
			SpriteMeshInstance l_spriteMeshInstance = null;

			if(selection)
			{
				l_spriteMeshInstance = selection.GetComponent<SpriteMeshInstance>();
				m_SpriteMeshCache.name = l_spriteMeshInstance.name;
			}

			if(l_spriteMeshInstance)
			{
				l_spriteMesh = l_spriteMeshInstance.spriteMesh;
			}

			if(l_spriteMeshInstance || l_spriteMesh)
			{
				m_SpriteMeshCache.spriteMeshInstance = l_spriteMeshInstance;
			}

			if(l_spriteMesh && l_spriteMesh != m_SpriteMeshCache.spriteMesh)
			{
				InvalidateCache();

				if(l_spriteMesh && l_spriteMesh.sprite)
				{
					m_SpriteMeshCache.SetSpriteMesh(l_spriteMesh, l_spriteMeshData, l_spriteMeshInstance);
				}
			}

			SliceEditor sliceEditor = new SliceEditor();
			sliceEditor.spriteMeshCache = m_SpriteMeshCache;
			sliceEditor.slice();
		}

		void InvalidateCache()
		{
			if(m_SpriteMeshCache)
			{
				m_SpriteMeshCache = null;
			}
			m_SpriteMeshCache = ScriptableObject.CreateInstance<SpriteMeshCache>();
		}

		public void HandleAddBindPose()
		{
			if(!m_SpriteMeshCache.isBound && m_SpriteMeshCache.spriteMeshInstance)
			{
				m_SpriteMeshCache.BindBones();
				m_SpriteMeshCache.CalculateAutomaticWeights();
				m_SpriteMeshCache.ApplyChanges();
			}
		}
	}
}	