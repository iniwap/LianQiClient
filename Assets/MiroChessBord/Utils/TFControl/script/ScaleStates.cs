using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class ScaleStates : MonoBehaviour {

		public List<Vector3> _Scales = new List<Vector3>();
		public int _StateId = 0;
		public float _LerpSpd = 2.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			_StateId = Mathf.Clamp (_StateId, 0, _Scales.Count);

			Vector3 tgtScl = _Scales [_StateId];

			Vector3 lscl = transform.localScale;

			float dt = Time.deltaTime;
			float lt = dt * _LerpSpd;
			Vector3 scl2 = Vector3.Lerp (lscl, tgtScl, lt);
			transform.localScale = scl2;

		}

		public void SetScaleId(int id)
		{
			_StateId = id;
		}
	}
}
