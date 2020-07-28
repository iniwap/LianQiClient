using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Caculator : MonoBehaviour {

		public bool _ON = false;

		private static List<MiroV1Caculator> _Caculators = 
			new List<MiroV1Caculator> ();

		public MiroV1Caculator()
		{
			_Caculators.Add (this);
		}

		public static void TurnAll(bool bON)
		{
			CheckCaculators ();

			foreach (MiroV1Caculator ca in _Caculators) {
				if (bON) {
					ca.TurnON ();
				} else {
					ca.TurnOFF ();
				}

			}
		}

		static void CheckCaculators()
		{
			for (int i = _Caculators.Count - 1; i >= 0; i--) {
				if (_Caculators [i] == null) {
					_Caculators.RemoveAt (i);
				}
			}
		}

		public void TurnON()
		{
			_ON = true;
		}

		public void TurnOFF()
		{
			_ON = false;
		}

		public void Calculate()
		{
			if (_ON) {
				_Calculate ();
			}
		}

		virtual protected void _Calculate()
		{
		}
	}
}
