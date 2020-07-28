using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class NoisePosFromBegin : MonoBehaviour {

		public Vector3 _BeginPos = Vector3.zero;
		public float _MaxBias = 1.0f;
		public float _NoiseSpd = 1.0f;
		public List<Vector2> _NStarts = new List<Vector2>();

		public bool _bRunning = false;
		public bool _FixZ = true;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (!_bRunning)
				return;
			
			float t = Time.realtimeSinceStartup;
			float nt = _NoiseSpd * t;
			Vector3 bias = Vector3.zero;

			int chl = 3;
			if (_FixZ) {
				chl = 2;
			}
			for (int i = 0; i < chl; i++) {
				bias [i] = 2.0f*(Mathf.PerlinNoise (_NStarts [i].x + nt, _NStarts [i].y)-0.5f);
				bias [i] *= _MaxBias;
			}

			Vector3 pos = _BeginPos + bias;
			transform.position = pos;
		}

		[ContextMenu("Begin")]
		public void Begin()
		{
			_BeginPos = transform.position;
				
			_NStarts.Clear ();
			for (int i = 0; i < 3; i++) {
				_NStarts.Add (Random.insideUnitCircle);
			}

			//_NStart = Random.insideUnitCircle;
			_bRunning = true;
		}

		[ContextMenu("End")]
		public void End()
		{
			_bRunning = false;
		}
	}
}
