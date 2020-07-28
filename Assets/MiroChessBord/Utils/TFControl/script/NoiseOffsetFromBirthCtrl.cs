using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class NoiseOffsetFromBirthCtrl : MonoBehaviour {

		public NoiseOffsetFromBirthPos[] _noiseOffsets;

		public float _NoiseSpd = 0.3f;
		public float _MaxDist = 0.3f;


		[ContextMenu("GetChdNoiseOffsets")]
		public void GetChdNoiseOffsets()
		{
			_noiseOffsets = GetComponentsInChildren<NoiseOffsetFromBirthPos> ();
		}

		// Use this for initialization
		void Start () {
			//GetNoiseOffsets ();
		}
		
		// Update is called once per frame
		void Update () {
			foreach (NoiseOffsetFromBirthPos nf in _noiseOffsets) {
				nf._noiseSpd = _NoiseSpd;
				nf._maxOffset = _MaxDist;
			}
		}
	}
}
