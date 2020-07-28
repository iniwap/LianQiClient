using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroEventStart : MonoBehaviour {

		public UnityEvent _Start;

		// Use this for initialization
		void Start () {
			_Start.Invoke ();
		}
	}
}
