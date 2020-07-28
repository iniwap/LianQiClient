using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV0
{
	public class MiroPeriodicInvoker : MonoBehaviour {

		public float _Period = 1.0f;

		public float Period {
			get {
				return _Period;
			}
			set {
				_Period = Mathf.Clamp(value,0,float.PositiveInfinity);
			}
		}

		private float _LeftTime;
		public UnityEvent _Happen;

		// Use this for initialization
		void Start () {
			_LeftTime = _Period;
		}
		
		// Update is called once per frame
		void Update () {
			if (_LeftTime <= 0.0f) {
				_Happen.Invoke ();
				_LeftTime = _Period;
				//Debug.Log ("Happen!");
			}
			_LeftTime -= Time.deltaTime;

		}
	}
}
