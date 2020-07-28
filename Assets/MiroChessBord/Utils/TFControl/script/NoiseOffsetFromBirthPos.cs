using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class NoiseOffsetFromBirthPos : MonoBehaviour {

		private Vector3 _BirthPos;
		private Vector2 _NXS, _NYS, _NZS;
		public float _noiseSpd = 1.0f;
		public float _maxOffset = 0.1f;

		// Use this for initialization
		void Start () {
			_NXS = Random.insideUnitCircle;
			_NYS = Random.insideUnitCircle;
			_NZS = Random.insideUnitCircle;
			_BirthPos = transform.localPosition;
		}
		
		// Update is called once per frame
		void Update () {
			float t = Time.realtimeSinceStartup;
			Vector3 offset = Vector3.zero;
			Vector2[] nss = new Vector2[3]{ _NXS, _NYS, _NZS };
			for (int i = 0; i < 3; i++) {
				offset [i] = Mathf.PerlinNoise (nss [i].x + t * _noiseSpd, nss [i].y) 
					* _maxOffset * 2.0f - _maxOffset ;
			}
			Vector3 pos = _BirthPos + offset;
			transform.localPosition = pos;
		}
	}
}
