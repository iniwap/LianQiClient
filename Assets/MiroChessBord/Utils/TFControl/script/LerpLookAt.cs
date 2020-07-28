using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LerpLookAt : MonoBehaviour {

		public Transform _Tgt;
		public bool _3D = false;
		public float _LerpSpd = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_Tgt == null)
				return;
			
			float dt = Time.deltaTime;
			Vector3 RightW = transform.TransformDirection (Vector3.right);

			Vector3 Dir = (_Tgt.position - transform.position).normalized;
			if (!_3D) {
				Dir.z = 0.0f;
			}

			Quaternion Rot = Quaternion.FromToRotation (RightW, Dir);
			Quaternion TgtRot = transform.rotation * Rot;
			float lerpT = _LerpSpd * dt;
			Quaternion rotL = Quaternion.Lerp (transform.rotation, TgtRot, lerpT);
			transform.rotation = rotL;


		}

		public void ClearTgt()
		{
			_Tgt = null;
		}
	}
}
