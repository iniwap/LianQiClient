using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1TimeToDie : MonoBehaviour {

		public float _LeftTime = 1.0f;
		
		// Update is called once per frame
		void Update () {
			if (_LeftTime <= 0.0f) {
				Destroy (gameObject);
			}
			_LeftTime -= Time.deltaTime;
			
		}
	}
}