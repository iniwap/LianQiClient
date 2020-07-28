using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class PressUpPlacerMgr : MonoBehaviour {
		public ChessPressUpPlacer [] _pressUpPlrs;

		[ContextMenu("GetChessPressUpPlacers")]
		public void GetChessPressUpPlacers()
		{
			_pressUpPlrs = 
				GetComponentsInChildren<ChessPressUpPlacer> ();
			
		}

		public void Turn(bool bON)
		{
			foreach (ChessPressUpPlacer plr in _pressUpPlrs) {
				plr.enabled = bON;
			}
		}

		public void TurnOFF(bool bOFF)
		{
			Turn (!bOFF);
		}
	}
}
