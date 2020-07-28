using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1Invoker : MonoBehaviour {

		public UnityEvent _Happen,_Happen2,_Happen3,_Happen4;

		[ContextMenu("Happen")]
		public void Happen()
		{
			_Happen.Invoke ();
		}

		[ContextMenu("Happen2")]
		public void Happen2()
		{
			_Happen2.Invoke ();
		}

		[ContextMenu("Happen3")]
		public void Happen3()
		{
			_Happen3.Invoke ();
		}

		[ContextMenu("Happen4")]
		public void Happen4()
		{
			_Happen4.Invoke ();
		}
	}
}
