using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepLossyScale : MonoBehaviour {

		public Vector3 _LsyScl = Vector3.one;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 LSScl = transform.lossyScale;
			Vector3 lScl = transform.localScale;
			Vector3 locSclFac = Vector3.zero;
			for (int i = 0; i < 3; i++) {
				locSclFac[i] = _LsyScl [i] / LSScl [i];
				lScl [i] *= locSclFac [i];
			}
			transform.localScale = lScl;
		}
	}
}
