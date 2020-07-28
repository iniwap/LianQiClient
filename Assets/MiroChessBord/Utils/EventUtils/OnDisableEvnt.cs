using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class OnDisableEvnt : MonoBehaviour {

		public UnityEvent _OnDisable;

		void OnDisable()
		{
			_OnDisable.Invoke ();
		}

	}
}
