using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MiroV1
{
	public class ConnectImplseToPoissionIkr : MonoBehaviour {
		public UnityEvent _Connected;

		[ContextMenu("Connect")]
		public void Connect()
		{
			
			MiroPoissonInvoker[] ikrs = 
				GetComponentsInChildren<MiroPoissonInvoker> ();
			foreach (MiroPoissonInvoker ikr in ikrs) {
				MiroRandImpluse imp = ikr.GetComponent<MiroRandImpluse> ();
				UnityAction act = imp.RandImpluse;
				ikr._PoissonProcessEvent = new UnityEvent ();
				ikr._PoissonProcessEvent.AddListener (act);
			}
			_Connected.Invoke ();
		}
	}
}
