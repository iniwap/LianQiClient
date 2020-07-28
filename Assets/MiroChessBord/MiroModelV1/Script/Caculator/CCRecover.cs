using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCRecover : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		private static List<CCRecover> _ccs = new List<CCRecover>();

		public CCRecover()
		{
			_ccs.Add (this);
		}

		static public void TurnAllON()
		{
			foreach (var cca in _ccs) {
				cca.TurnON ();
			}
		}

		static public void TurnAllOFF()
		{
			foreach (var cca in _ccs) {
				cca.TurnOFF();
			}
		}

		override protected void _Calculate()
		{
			Invoke ("RecoverAllDots", 0.5f);
			//RecoverAllDots ();
		}

		void RecoverAllDots ()
		{
			if (_cellCtrl._TgtObj == null) {
				return;
			}
			MiroModelV1 modelMe = _cellCtrl._TgtObj.GetComponent<MiroModelV1> ();
			if (modelMe != null) {
				//Debug.Log (modelMe + " Recover!");
				modelMe.RecoverAllBlackDots ();
			}
		}
	}
}
