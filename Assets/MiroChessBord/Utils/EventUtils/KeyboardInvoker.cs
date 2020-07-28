using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class KeyboardInvoker : MonoBehaviour {

		[System.Serializable]
		public class KeyEvent 
		{
			public KeyCode _Key;
			public UnityEvent _Event;
		}
		public List<KeyEvent> _KeyEvents = new List<KeyEvent> ();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			foreach (KeyEvent ke in _KeyEvents) {
				if (Input.GetKeyDown (ke._Key)) {
					ke._Event.Invoke ();
				}
			}
			
		}
	}
}
