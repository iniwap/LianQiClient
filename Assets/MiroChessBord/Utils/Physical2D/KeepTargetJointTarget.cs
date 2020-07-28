using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepTargetJointTarget : MonoBehaviour {

		public Transform _Tgt;

		public TargetJoint2D _TgtJnt;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_TgtJnt == null) {
				_TgtJnt = GetComponent<TargetJoint2D> ();
			}

			_TgtJnt.target = _Tgt.position;
		}
	}
}
