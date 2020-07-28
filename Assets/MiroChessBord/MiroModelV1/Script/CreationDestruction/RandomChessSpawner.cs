using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class RandomChessSpawner : MonoBehaviour {
		public MiroV1PlacementMgr _mgr;
		public HexGridCtrl _gridCtrl;
		public UnityEvent _Spawned;

		public int _campId = 1;

		public int _batchCount = 2;

		[ContextMenu("SpawnOneBatch")]
		public void SpawnOneBatch()
		{
			for (int i = 0; i < _batchCount; i++) {
				SpawnOne ();
			}
		}

		[ContextMenu("SpawnOne")]
		public void SpawnOne()
		{
			bool bFind = false;
			CellObjCtrl emptyCCtrl = null;
			int count = 0;

			List<CellObjCtrl> epCtrls =
				_gridCtrl.GetEmptyPlacableCellObjCtrls ();
			if (epCtrls.Count == 0) {
				return;
			}
			int rid = (int)Random.Range (0, (float)(epCtrls.Count-0.0001f));

			emptyCCtrl = epCtrls [rid];

			int dir = Mathf.FloorToInt (Random.Range (0.0f, 5.999999f));

			_mgr.SetMiroPrefabID (_campId);
			emptyCCtrl.SetDir (dir);
			_mgr.SpawnAtCellCtrl (emptyCCtrl);
			_Spawned.Invoke ();
			print ("Spawned!");


		}
	
		private bool GetRandomEmptyCellObjCtrl(
			List<CellObjCtrl> epCtrls,out CellObjCtrl ctrlA)
		{
			ctrlA = null;

			int cellCnt = 
				epCtrls.Count;

			int rId = (int)Random.Range (0, (float)cellCnt - 1.0f);

			ctrlA = epCtrls [rId];
			bool bPlacableA = !ctrlA.IsBlocked ();
			bool bOccupiedA = CellObjCtrlUtils.IsControllingObj (ctrlA) && bPlacableA;
			if (bOccupiedA)
				return false;

			return true;
		}


	}
}
