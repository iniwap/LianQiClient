using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventUtils
{
	public class EventUpdate : MonoBehaviour {
		public UnityEvent _Update;
		
		// Update is called once per frame
		void Update () {
			_Update.Invoke ();
		}
	}
}
