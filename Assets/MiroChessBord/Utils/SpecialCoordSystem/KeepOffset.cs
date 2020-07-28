using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class KeepOffset : MonoBehaviour {
		public Transform _Anchor;
		public Vector3 _Offset = Vector3.zero;
		public float _LerpSpd = 3.0f;
		public UnityEvent _AnchorLost;
		private bool _AnchorExist = true;

		// Update is called once per frame
		void Update () {
			bool akrExist = (_Anchor != null);
			if (!akrExist) {
				if (_AnchorExist) {
					_AnchorLost.Invoke ();
				}
			} else {
				Vector3 tgtPos = _Anchor.position + _Offset;
				Vector3 pos = transform.position;
				Vector3 LPos = Vector3.Lerp (pos, tgtPos, Time.deltaTime * _LerpSpd);
				transform.position = LPos;
			}
			_AnchorExist = akrExist;
		}
	}
}
