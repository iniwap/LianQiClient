using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LerpMoveTo : MonoBehaviour {
		public bool _3D = false;
		public float _LerpSpd = 7.0f;
		public Transform _TgtTF = null;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_TgtTF == null) {
				return;
			}
			float dt = Time.deltaTime;
			float lerpT = dt * _LerpSpd;
			Vector3 pos = transform.position;
			Vector3 tgtPos = _TgtTF.position;

			if (!_3D) {
				tgtPos.z = pos.z;
			}

			Vector3 lpos = Vector3.Lerp (pos, tgtPos, lerpT);

			transform.position = lpos;
		}
	}
}
