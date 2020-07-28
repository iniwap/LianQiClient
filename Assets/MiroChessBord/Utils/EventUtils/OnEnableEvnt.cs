using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class OnEnableEvnt : MonoBehaviour {
		public UnityEvent _OnEnable;

		void OnEnable()
		{
			_OnEnable.Invoke ();
		}
			
	}
}
