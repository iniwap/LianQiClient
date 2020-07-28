using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellSelectionMgr : MonoBehaviour {
		public CellSelection [] _cellSels;



		public void TurnON()
		{
			foreach (CellSelection csel in _cellSels) {
				csel.enabled = true;
			}
		}

		public void TurnOFF()
		{
			foreach (CellSelection csel in _cellSels) {
				csel.enabled = false;
			}
		}

		public void Turn(bool bON)
		{
			foreach (CellSelection csel in _cellSels) {
				csel.enabled = bON;
			}
		}

		public void Unselect()
		{
			foreach (CellSelection csel in _cellSels) {
				csel.SelectionOFF ();
			}
		}

		public void Select()
		{
			foreach (CellSelection csel in _cellSels) {
				csel.SelectionON ();
			}
		}

		[ContextMenu("GetCellSelections")]
		public void GetCellSelections()
		{
			_cellSels = GetComponentsInChildren<CellSelection> ();
		}
	}
}
