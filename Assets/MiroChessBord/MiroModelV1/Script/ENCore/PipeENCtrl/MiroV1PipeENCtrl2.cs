using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeENCtrl2 : MiroV1PipeENCtrlBase {

		public MiroV1ENSource[] _enSrcs;

		override public void TurnENPumpBySP ()
		{
			for (int i = 0; i < 9; i++) {
				MiroV1ENSource enSrc = _enSrcs [i];
				if (i < _sp) {
					enSrc.TurnON ();
				} else {
					enSrc.TurnOFF ();
				}
			}
		}

		[ContextMenu("GetENSources")]
		void FindENSources()
		{
			_enSrcs = GetComponentsInChildren<MiroV1ENSource> ();
		}

		public void Emit()
		{
			foreach (MiroV1ENSource ensrc in _enSrcs) {
				ensrc.Emit ();
			}
		}
	}
}
