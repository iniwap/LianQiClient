using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroPoissonInvoker : MonoBehaviour {

		public UnityEvent _PoissonProcessEvent;
		public float _lamda = 1.0f;

		private float _rvalue;
		private float _passedTime = 0.0f;

		private int _count = 0;

		void Start()
		{
			_rvalue = Random.value;
		}

		void Update () {
			_passedTime += Time.deltaTime;

			float pvalue = 
				1.0f - Mathf.Exp (-_lamda * _passedTime);
			//Debug.Log("pvalue:"+pvalue);

			if (pvalue >= _rvalue) {
				_PoissonProcessEvent.Invoke ();
				//Debug.Log ("Poisson Invoke: " + _count);
				_rvalue = Random.value;
				_passedTime = 0.0f;
				_count++;
			}	
		}

	}
}
