using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LineCoordMgr : MonoBehaviour {
		public LineCoord [] _lineCoords;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("GetChildLineCoords")]
		public void GetChildLineCoords()
		{
			_lineCoords = GetComponentsInChildren<LineCoord> ();
		}


		public void TurnEnable(bool bON)
		{
			foreach (var lc in _lineCoords) {
				lc.enabled = bON;
			}
		}








	}
}
