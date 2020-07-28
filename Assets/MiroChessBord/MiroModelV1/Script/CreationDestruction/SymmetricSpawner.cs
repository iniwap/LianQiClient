using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class SymmetricSpawner : MonoBehaviour {
		public MiroV1PlacementMgr _mgr;
		public HexGridCtrl _gridCtrl;

		public int _campId0, _campId1;

		public int _batchCount = 3;



		public void SpawnOneBatch()
		{
			for (int i = 0; i < _batchCount; i++) {
				SpawnOnePair ();
			}
		}

		public void SpawnOnePair()
		{
			bool bFindEmpty = false;
			int tryCount = 0;
			while (!bFindEmpty) {
				tryCount++;
				CellObjCtrl ctrlA, ctrlB;

				bFindEmpty = GetCellObjCtrlPair (out ctrlA, out ctrlB);

				if (!bFindEmpty)
					continue;

				float dirAf = Mathf.Floor(Random.Range (0, 5.999999f));
				int dirA = (int)dirAf;
				float dirBf = dirAf + 3.0f;
				dirBf = Mathf.Repeat (dirBf, 6.0f);
				int dirB = (int)dirBf;

				_mgr.SetMiroPrefabID (_campId0);
				ctrlA.SetDir (dirA);
				_mgr.SpawnAtCellCtrl (ctrlA);

				_mgr.SetMiroPrefabID (_campId1);
				ctrlB.SetDir (dirB);
				_mgr.SpawnAtCellCtrl (ctrlB);

				if (tryCount > 200) {
					break;
				}
			}

		}

		private bool GetCellObjCtrlPair(out CellObjCtrl ctrlA, out CellObjCtrl ctrlB)
		{
			ctrlA = null;
			ctrlB = null;
			int cellCnt = 
					_gridCtrl._Cells.Count;

			int rId = (int)Random.Range (0, (float)cellCnt - 1.0f);

			GameObject gb = _gridCtrl._Cells [rId];
			ctrlA = gb.GetComponent<CellObjCtrl> ();
			bool bPlacableA = !ctrlA.IsBlocked ();
			bool bOccupiedA = CellObjCtrlUtils.IsControllingObj (ctrlA) && bPlacableA;
			if (bOccupiedA)
				return false;

			HexCoord hc = gb.GetComponent<HexCoord> ();
			if (hc._hex.q == 0 && hc._hex.r == 0 && hc._hex.s == 0) {
				return false;
			}

			Hex hA = hc._hex;
			Hex h0 = new Hex (0, 0, 0);
			Hex hB = Hex.Subtract (h0, hA);

			ctrlB = _gridCtrl.GetCellCtrlAt (hB);
			bool bPlacableB = !ctrlB.IsBlocked ();
			bool bOccupiedB = CellObjCtrlUtils.IsControllingObj (ctrlB) && bPlacableB;
			if (bOccupiedB) {
				return false;
			}

			return true;
		}

	}
}
