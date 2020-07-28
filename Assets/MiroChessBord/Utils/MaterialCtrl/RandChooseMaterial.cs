using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class RandChooseMaterial : MonoBehaviour {

		public Renderer[] _renderers;
		public List<Material> _materials = new List<Material>();

		[ContextMenu("RandChooseDifferentMaterial")]
		public void RandChooseMaterialDifferently()
		{
			foreach (Renderer rdr in _renderers) {
				Material mat = RandChooseMat();
				rdr.material = mat;
			}
		}

		[ContextMenu("RandChooseSameMaterial")]
		public void RandChooseSameMaterial()
		{
			Material mat = RandChooseMat();
			foreach (Renderer rdr in _renderers) {
				rdr.material = mat;
			}
		}

		private Material RandChooseMat()
		{
			int rvalue = Random.Range (0, _materials.Count - 1);
			Material mat = _materials [rvalue];
			return mat;
		}
	}
}
