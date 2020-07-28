using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroNoiseLocalPos : MonoBehaviour {
		public float _MaxDist=0.05f;
		public float _Spd = 1.0f;

		private Vector2 _XStart,_YStart;
		private float _PassedT = 0.0f;
		private Vector3 _LPosBase = Vector3.zero;
		public bool _bGetLPosBaseAtStart = false;

		// Use this for initialization
		void Start () {
			_XStart = Random.insideUnitCircle;
			_YStart = Random.insideUnitCircle;

			if (_bGetLPosBaseAtStart) {
				_LPosBase = transform.localPosition;
			}
		}
		
		// Update is called once per frame
		void Update () {
			_PassedT += Time.deltaTime * _Spd;
			
			float x = Mathf.PerlinNoise (_XStart.x + 
				_PassedT, _XStart.y)*2.0f-1.0f;
			float y = Mathf.PerlinNoise (_YStart.x + 
				_PassedT, _YStart.y)*2.0f-1.0f;

			Vector2 lpos = new Vector2 (x, y) * _MaxDist;
			float z = transform.localPosition.z;
			Vector3 lpos3 = (Vector3)lpos + _LPosBase;
			lpos3.z = z;

			transform.localPosition = lpos3;
		}
	}
}
