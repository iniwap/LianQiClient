using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class NoiseScaleFromBase : MonoBehaviour {

		public Vector3 _BaseScale;

		public Vector2 [] _NStarts = new Vector2[3]{
			Vector2.zero,Vector2.zero,Vector2.zero};
		public Vector3 _NoiseSpds = Vector3.one;
		public Vector3 _NoisePwr = Vector3.zero;

		// Use this for initialization
		void Start () {
			for (int i = 0; i < 3; i++) {
				_NStarts [i] = Random.insideUnitSphere;
			}
		}
		
		// Update is called once per frame
		void Update () {
			float t = Time.realtimeSinceStartup;

			Vector3 scl = Vector3.zero;
			for (int i = 0; i < 3; i++) {
				float s = _BaseScale [i];
				s += _NoisePwr[i] *( Mathf.PerlinNoise (
					_NStarts [i].x + t * _NoiseSpds[i], _NStarts [i].y)-0.5f);
				scl [i] = s;
			}
			transform.localScale = scl;
		}

		[ContextMenu("RecordBaseScale")]
		public void RecordBaseScale()
		{
			_BaseScale = transform.localScale;
		}
	}
}