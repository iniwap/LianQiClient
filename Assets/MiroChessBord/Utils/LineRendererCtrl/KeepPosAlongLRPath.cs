using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepPosAlongLRPath : MonoBehaviour {

		public Transform _TF;
		public LineRendererPathCtrl _PathCtrl;

		public float _Dist = 0.0f;

		// Use this for initialization
		void Start () {
			if (_TF == null) {
				_TF = transform;
			}
		}

		// Update is called once per frame
		void Update () {
			Vector3 pos = 
				_PathCtrl.GetPosAtT01 (_Dist, true);
			_TF.position = pos;
		}
			
	}
}
