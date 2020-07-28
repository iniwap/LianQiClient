using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class MiroTimeToInvoke : MonoBehaviour {

		public float _LeftTime = 0.0f;
		public UnityEvent _Happen;

		public float _DefLeftTime = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
			if (_LeftTime >= 0.0f) {
				_LeftTime -= Time.deltaTime;
				if (_LeftTime <= 0.0f) {
					_Happen.Invoke ();
				}
			}
			
		}

		public void ResetLeftTime()
		{
			_LeftTime = _DefLeftTime;
		}
	}
}
