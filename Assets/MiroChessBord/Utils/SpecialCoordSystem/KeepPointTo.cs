using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class KeepPointTo : MonoBehaviour {
		public Transform _Tgt;
		public Vector3 _EulerBias;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 posMe  = transform.position;
			Vector3 posTgt = _Tgt.position;

			Vector3 dir = posTgt - posMe;
			dir.Normalize ();
			Quaternion rot0 = Quaternion.FromToRotation (Vector3.right, dir);
			Quaternion rotBias = Quaternion.Euler (_EulerBias);
			Quaternion rotSum = rot0 * rotBias;

			transform.rotation = rotSum;
		}
	}
}
