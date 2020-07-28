using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SmartAgent
{
	public class TargetTransform : MonoBehaviour {

		public Transform _Target;
		public UnityEvent _MissTarget;

		private bool _bTargetNull = false;

		public void Update()
		{
			bool bNull = (_Target == null);
			if (!_bTargetNull&&bNull) {
				_MissTarget.Invoke ();
			}

			_bTargetNull = bNull;
		}
	
	}
}
