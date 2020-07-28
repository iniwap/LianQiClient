using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class StdChessSpawner : MonoBehaviour {
		public MiroV1PlacementMgr _mgr;
		public bool _bControlDragRotator = true;

		[System.Serializable]
		public class EventWithCellObjCtrl: UnityEvent<CellObjCtrl>{};
		public EventWithCellObjCtrl _Sow, _Move, _Confirm,_Cancel;


		private CellObjCtrl _NewConfirmCtrl = null;
		private CellObjCtrl _NewCancelCtrl = null;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private CellObjCtrl _cctrl = null;

		public void TrySpawnAt(Transform cellTf)
		{
			CellObjCtrl ctrl = cellTf.GetComponent<CellObjCtrl> ();
			bool bControlling = CellObjCtrlUtils.IsControllingObj (ctrl);
			if (bControlling) {
				return;
			}

			if (_cctrl == null) {
				_cctrl = ctrl;
				Sow ();
			} else {
				if (_cctrl != ctrl) {
					Move (ctrl);
				}
			}
		}

		private void Sow()
		{
			_NewConfirmCtrl = null;

			if (_bControlDragRotator) {
				TurnDragRotatorForCellObjCtrl (_cctrl, true);
			}

			_mgr.SpawnAtCellCtrl (_cctrl);
			_Sow.Invoke (_cctrl);
		}

		private void Move(CellObjCtrl _curCtrl)
		{
			_cctrl.TeleportTo (_curCtrl);
			_cctrl = _curCtrl;
			_Move.Invoke (_cctrl);
		}

		public CellObjCtrl Confirm()
		{
			if (_bControlDragRotator) {
				TurnDragRotatorForCellObjCtrl (_cctrl, false);
			}

			_NewConfirmCtrl = _cctrl;
			_Confirm.Invoke (_NewConfirmCtrl);

			_cctrl = null;

			return _NewConfirmCtrl;
		}

		public CellObjCtrl GetSpawningCellObjCtrl()
		{
			return _cctrl;
		}

		public CellObjCtrl Cancel()
		{
			if (_bControlDragRotator) {
				TurnDragRotatorForCellObjCtrl (_cctrl, false);
			}

			_NewCancelCtrl = _cctrl;
			_Cancel.Invoke (_NewCancelCtrl);

			_mgr.DisappearMiroAtCellCtrl (_NewCancelCtrl);

			//_NewCancelCtrl.DisappearTgtObj ();

			_cctrl = null;

			return _NewCancelCtrl;
		}

		private void TurnDragRotatorForCellObjCtrl(CellObjCtrl ctrl, bool bON)
		{
			if (ctrl == null) {
				return;
			}
			ChessDragRotator dragRotator = 
				ctrl.GetComponentInChildren<ChessDragRotator> ();
			dragRotator.enabled = bON;
		}

		public CellObjCtrl GetNewestConfirmedCellObjCtrl()
		{
			return _NewConfirmCtrl;
		}

	}
}
