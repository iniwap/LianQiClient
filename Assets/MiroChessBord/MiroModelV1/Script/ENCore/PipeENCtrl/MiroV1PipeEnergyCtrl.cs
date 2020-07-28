using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeEnergyCtrl : MiroV1PipeENCtrlBase {
		public List<ParticleSystem> _PSs =
			new List<ParticleSystem>();

		override public void TurnENPumpBySP ()
		{
			if (_sp == 0) {
				TurnENPump (false, false, false, false, false);
			}
			else if (_sp == 1) {
				TurnENPump (true, false, false, false, false);
			} 
			else if (_sp == 2) {
				TurnENPump (true, true, false, false, false);
			} 
			else if (_sp == 3) {
				TurnENPump (false, false, true, false, false);
			} 
			else if (_sp == 4) {
				TurnENPump (true, false, true, false, false);
			} 
			else if (_sp == 5) {
				TurnENPump (true, true, true, false, false);
			} 
			else if (_sp == 6) {
				TurnENPump (false, false, false, true, false);
			} 
			else if (_sp == 7) {
				TurnENPump (true, false, false, true, false);
			} 
			else if (_sp == 8) {
				TurnENPump (true, true, false, true, false);
			} 
			else if (_sp == 9) {
				TurnENPump (false, false, false, false, true);
			}
		}

		private void TurnENPump(
			bool b1a, bool b1b, bool b3, bool b6, bool b9)
		{
			bool[] Bs = new bool[] { b1a, b1b, b3, b6, b9 };
			for (int i = 0; i < 5; i++) {
				if (Bs [i]) {
					_PSs [i].Play ();
				} else {
					_PSs [i].Stop ();
				}
			}
		}



	}

}