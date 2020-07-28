using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class VtFollower : MonoBehaviour {
		public MorphCharComps _comps;
		[Range(0,14)]
		public int _vtId = 0;
		public float _zBias = -1.0f;
		public float _LerpSpd = 3.0f;
		public float _SclOnCharScl = 0.7f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			float lt = Time.deltaTime * _LerpSpd;

			LerpPos (lt);
			LerpScale (lt);
		}

		void LerpScale (float lt)
		{
			Vector3 scl = _comps.transform.lossyScale;
			float xys = 0.5f * (scl.x + scl.y);
			Vector3 scl2 = _SclOnCharScl * xys * Vector3.one;
			Vector3 lscl0 = transform.localScale;
			Vector3 lscl1 = Vector3.Lerp (lscl0, scl2, lt);
			transform.localScale = lscl1;
		}

		void LerpPos (float lt)
		{
			Vector3 pos = _comps._Ds [_vtId].position;
			pos.z += _zBias;

			Vector3 posMe = transform.position;
			Vector3 posL = Vector3.Lerp (posMe, pos, lt);
			transform.position = posL;
		}

		public void RandomChangeVtId()
		{
			_vtId = Mathf.RoundToInt(Random.Range (0, 14));
		}
	}
}