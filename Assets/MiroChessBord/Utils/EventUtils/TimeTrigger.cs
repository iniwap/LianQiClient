using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class TimeTrigger : MonoBehaviour {

		public float _Time = 1.0f;
		public UnityEvent _TimeOver;

		private float _TriggerTime = 0.0f;
		private bool _bReady = false;


		
		// Update is called once per frame
		void Update () {
			if (!_bReady) {
				return;
			}

			_TriggerTime -= Time.deltaTime;
			if (_TriggerTime <= 0.0f) {
				_TimeOver.Invoke ();
				_bReady = false;
			}
		}

		public void Trigger()
		{
			_TriggerTime = _Time;
			_bReady = true;
		}
	}
}
