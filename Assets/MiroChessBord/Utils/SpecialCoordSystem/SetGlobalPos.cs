using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetGlobalPos : MonoBehaviour {

		public Transform _TF;

		// Use this for initialization
		void Start () {
			if (_TF == null) {
				_TF = transform;
			}
		}
		
		public void SetGlobalPosAt(Vector3 gpos)
		{
			_TF.position = gpos;
		}
	}
}
