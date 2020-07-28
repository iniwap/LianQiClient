using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroV1BiasFromCtrOnHPMax : MonoBehaviour {

		public LineCoord _LCoord;
		public AnimationCurve _AlongOnHPMax;
		public AnimationCurve _ShiftOnHPMax;

		[Range(0,100.0f)]
		public float _HPMax =0.0f;

		// Use this for initialization
		void Start () {
			if (_LCoord == null) {
				_LCoord = GetComponent<LineCoord> ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			float along = _AlongOnHPMax.Evaluate (_HPMax);
			_LCoord._Along = along;
			float shift = _ShiftOnHPMax.Evaluate (_HPMax);
			_LCoord._Shift = shift;
		}

		public void SetHPMax(float hpmax)
		{
			_HPMax = hpmax;
		}
	}
}
