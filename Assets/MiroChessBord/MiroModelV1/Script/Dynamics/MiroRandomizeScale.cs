using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroRandomizeScale : MonoBehaviour {
		public bool _bRandAtStart = true;
		public Bounds _ScaleBound = new Bounds();

		// Use this for initialization
		void Start () {
			if (_bRandAtStart) {
				RandomizeScale ();
			}
		}
		
		public void RandomizeScale()
		{
			Vector3 lscl = Vector3.one;
			for(int i=0;i<3;i++)
			{
				float scli = Random.Range (_ScaleBound.min [i], _ScaleBound.max [i]);
				lscl [i] = scli;
			}
			transform.localScale = lscl;
		}
	}
}
