using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class Miro1GunType0 : MonoBehaviour {

		public AnimationCurve _XScaleOnLevel;
		public int _Level = 1;
		public float _LerpSpd = 1.0f;
		private int _PrevLevel = 1;
		private bool _Resizing = false;
		public float _LerpThres = 0.01f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			DetectLevelChange ();
			ResizingOnLevelChange ();
		}

		void DetectLevelChange ()
		{
			if (_Level != _PrevLevel) {
				_Resizing = true;
				_PrevLevel = _Level;
			}
		}

		void ResizingOnLevelChange ()
		{
			if (_Resizing) {
				float xscale = _XScaleOnLevel.Evaluate (_Level);
				Vector3 lscl = transform.localScale;
				float xscl0 = lscl.x;
				float dt = Time.deltaTime;
				float xscl = Mathf.Lerp (xscl0, xscale, dt * _LerpSpd);
				lscl.x = xscl;
				transform.localScale = lscl;
				if (Mathf.Abs (xscale - xscl) < _LerpThres) {
					lscl.x = xscale;
					transform.localScale = lscl;
					_Resizing = false;
				}
			}
		}
	}
}
