using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour {
	public UnityEvent _event;

	public void Invoke()
	{
		_event.Invoke ();
	}

}
