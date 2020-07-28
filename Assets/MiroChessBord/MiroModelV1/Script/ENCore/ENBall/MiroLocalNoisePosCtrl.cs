using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroLocalNoisePosCtrl : MonoBehaviour {

		public MiroNoiseLocalPos[] _NoisePoss;
		public bool _bUpdating = true;

		public float _Spd = 1.0f;
		public float _MaxDist = 0.05f;

		// Update is called once per frame
		void Update () {
			if (_bUpdating) {
				foreach (MiroNoiseLocalPos npos in _NoisePoss) {
					npos._Spd = _Spd;
					npos._MaxDist = _MaxDist;
				}
			}
			
		}

		[ContextMenu("GetAllNoiseLocPos")]
		public void GetAllNoiseLocPos()
		{
			_NoisePoss = 
				GetComponentsInChildren<MiroNoiseLocalPos> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (MiroNoiseLocalPos npos in _NoisePoss) {
				npos.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (MiroNoiseLocalPos npos in _NoisePoss) {
				npos.enabled = false;
			}
		}

	}
}
