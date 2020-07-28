using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class AddNoiseOffsetFromBirthToEach : MonoBehaviour {

		public List<GameObject> _Objs = new List<GameObject>();
		public float _NoiseSpd;
		public float _MaxOffset;

		[ContextMenu("GetChildren")]
		public void GetChildren()
		{
			int cnt = transform.childCount;
			_Objs.Clear ();
			for (int i = 0; i < cnt; i++) {
				_Objs.Add (transform.GetChild (i).gameObject);
			}
		}

		[ContextMenu("AddNoiseOffset")]
		public void AddNoiseOffset()
		{
			if (_Objs.Count == 0) {
				GetChildren ();
			}
			foreach (GameObject gb in _Objs) {
				NoiseOffsetFromBirthPos n = 
					gb.GetComponent<NoiseOffsetFromBirthPos> ();
				if (n == null) {
					n = gb.AddComponent<NoiseOffsetFromBirthPos> ();
				}
				n._noiseSpd = _NoiseSpd;
				n._maxOffset = _MaxOffset;
			}
		}

		[ContextMenu("GetChildThenAddNoiseOffset")]
		public void GetChildThenAddNoiseOffset()
		{
			GetChildren ();
			AddNoiseOffset ();
		}

	}
}
