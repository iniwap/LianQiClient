using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1TimeToInvoke : MonoBehaviour {
		public float _LeftTime = 0.5f;
		public UnityEvent _Event;

		public float _DefaultLeftTime = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			_LeftTime -= Time.deltaTime;
			if (_LeftTime <= 0.0f) {
				_Event.Invoke ();
				enabled = false;
			}
			
		}

		public void ResetLeftTime()
		{
			_LeftTime = _DefaultLeftTime;
		}
	}
}
