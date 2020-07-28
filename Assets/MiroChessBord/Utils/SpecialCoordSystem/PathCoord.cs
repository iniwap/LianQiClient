using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class PathCoord : MonoBehaviour {
		public TraceCurveFromVts _Tracer;

		public Transform _TF;

		public float _Along, _Shift, _Depth;
		public bool _UpdatingByPathCoord = true;

		// Use this for initialization
		void Start () {
			if (_TF == null) {
				_TF = transform;
			}
		}
		
		// Update is called once per frame
		void Update () {
			if (_UpdatingByPathCoord) {
				float length = _Tracer._Curve._TotalLength;
				float l = Mathf.Repeat (_Along, length);
				Vector3 BasePos = _Tracer.GetPosAtLength (l);
				Vector3 Tangent = _Tracer.GetTangentAtLength (l).normalized;
				Vector3 Up = _Tracer.GetUpVecAtLength (l).normalized;
				Vector3 Right = _Tracer.GetRightVecAtLength (l).normalized;

				Vector3 Pos = BasePos +  _Shift * Right + _Depth * Up;
				_TF.position = Pos;
			}
			
		}
	}
}
