using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class RandScale : MonoBehaviour {

		public Bounds _ScaleBound = new Bounds();
		public bool _RandAtStart = true;
		public bool _KeepXYZSame = true;

		public void Start()
		{
			if (_RandAtStart) {
				RandomizeScale ();
			}
		}
		[ContextMenu("RandomizeScale")]
		public void RandomizeScale()
		{
			Vector3 locScl = Vector3.one;

			if (_KeepXYZSame) {
				float rscal = Random.Range (_ScaleBound.min [0], _ScaleBound.max [0]);
				for (int i = 0; i < 3; i++) {
					locScl[i] = rscal;
				}
			} else {
				for (int i = 0; i < 3; i++) {
					locScl[i] = Random.Range (_ScaleBound.min [i], _ScaleBound.max [i]);
				}
			}

			transform.localScale = locScl;
		}


	}
}
