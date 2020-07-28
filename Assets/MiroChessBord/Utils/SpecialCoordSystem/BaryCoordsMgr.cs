using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class BaryCoordsMgr : MonoBehaviour {
		public BaryCoord[] _baryCoords;

		[ContextMenu("GetChildBaryCoords")]
		public void GetChildBaryCoords()
		{
			_baryCoords = GetComponentsInChildren<BaryCoord> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			TurnEnable (true);
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			TurnEnable (false);
		}

		public void TurnEnable(bool bON)
		{
			foreach(var BaryCoord in _baryCoords)
			{
				BaryCoord.enabled = bON;	
			}
		}


	}
}
