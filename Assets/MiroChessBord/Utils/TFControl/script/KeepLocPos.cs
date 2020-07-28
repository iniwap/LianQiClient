using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepLocPos : MonoBehaviour {

		public Vector3 _LocPos;

		// Update is called once per frame
		void Update () {
			transform.localPosition = _LocPos;
		}
	}
}
