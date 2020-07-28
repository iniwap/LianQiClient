using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class NoiseValueInvoker : MonoBehaviour {

		[System.Serializable]
		public class EventWithFloat: UnityEvent<float>{};
		public float Max,Min;
		public float _NoiseSpd = 1.0f;
		public EventWithFloat _NoiseValue;

		private Vector2 _NoiseStart;

		void Start()
		{
			_NoiseStart = new Vector2(Random.value,Random.value);

		}
		// Update is called once per frame
		void Update () {
			float nvalue = Mathf.PerlinNoise (
				_NoiseStart.x + Time.realtimeSinceStartup * _NoiseSpd, _NoiseStart.y);
			float interval = Max - Min;
			float nvalue2 = nvalue * interval + Min;
			_NoiseValue.Invoke (nvalue2);
		}
	}
}
