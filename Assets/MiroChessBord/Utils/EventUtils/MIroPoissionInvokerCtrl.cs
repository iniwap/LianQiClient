using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MIroPoissionInvokerCtrl : MonoBehaviour {
		public MiroPoissonInvoker [] _pInvokers;

		public bool _bUpdating = false;
		public float _Lamda = 1.0f;

		public void Update()
		{
			if (_bUpdating) {
				foreach (MiroPoissonInvoker mpikr in _pInvokers) {
					mpikr.enabled = true;
					mpikr._lamda = _Lamda;
				}
			} else {
				foreach (MiroPoissonInvoker mpikr in _pInvokers) {
					mpikr.enabled = false;
				}
			}
		}

		[ContextMenu("GetAllPoissionInvokers")]
		public void GetAllPoissionInvokers()
		{
			_pInvokers = GetComponentsInChildren<MiroPoissonInvoker> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (MiroPoissonInvoker mpikr in _pInvokers) {
				mpikr.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (MiroPoissonInvoker mpikr in _pInvokers) {
				mpikr.enabled = false;
			}
		}
	}
}
