using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class PlaceAlongLineSegment : MonoBehaviour {
		public Transform _A, _B;

		public Vector3 _RotBias = Vector3.zero;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_A == null || _B == null) {
				return;
			}

			Vector3 PosA, PosB;
			PosA = _A.position;
			PosB = _B.position;

			Vector3 Pos = Vector3.Lerp (PosA, PosB, 0.5f);
			transform.position = Pos;

			Vector3 dir = PosB - PosA;
			Quaternion Rot = Quaternion.FromToRotation (Vector3.right, dir.normalized);
			Rot *= Quaternion.Euler (_RotBias);
			transform.rotation = Rot;
		}
	}
}
