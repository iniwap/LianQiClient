using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCFarm2 : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		public MiroV1PlacementMgr _mgr;

		public bool _bControlAttacking = true;

		private static List<CCFarm2> _ccs = new List<CCFarm2>();

		private static List<List<CellObjCtrl> > _Farms = 
			new List<List<CellObjCtrl> >();

		public CCFarm2()
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

		public static List<List<Hex> > GetBackToBacks()
		{
			List<List<Hex> > bbs = new List<List<Hex> > ();
			foreach (var lstCtrl in _Farms) {
				List<Hex> lstHex = new List<Hex> ();
				foreach (CellObjCtrl ctrl in lstCtrl) {
					HexCoord hc = ctrl.GetComponent<HexCoord> ();
					lstHex.Add (hc._hex);
				}
				bbs.Add (lstHex);
			}
			return bbs;
		}



		override protected void _Calculate()
		{
			CellObjCtrl ctrlBwd = 
				CellObjCtrlUtils.GetNbCellObjCtrl (_cellCtrl, 3);
			if (_cellCtrl._TgtObj == null) {
				return;
			}

			bool bBackToBack = CellObjCtrlUtils.IsBackToBack (_cellCtrl);

			MiroModelV1 modelMe = 
				_cellCtrl._TgtObj.GetComponent<MiroModelV1> ();
			bool bHasFarm = modelMe.HasFarm ();
			bool bToGrow = (!bHasFarm && bBackToBack);
			bool bToShrink = (bHasFarm && !bBackToBack);
			if (bToGrow) {
				MiroModelV1 modelBwd = 
					ctrlBwd._TgtObj.GetComponent<MiroModelV1> ();
				_mgr.CreateEN2FarmFor2 (
					modelMe,_cellCtrl.transform, 
					modelBwd,ctrlBwd.transform);
				RecordNewFarm (_cellCtrl, ctrlBwd);
				//Debug.Log ("GrowFarm2 at " + _cellCtrl);
			} else if (bToShrink) {
				ShrinkFarm2 (_cellCtrl);
				//Debug.Log ("ShrinkFarm2 at " + _cellCtrl);
			}

		}

		void ShrinkFarm2 (CellObjCtrl ctrl)
		{
			MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (ctrl);
			model.ShrinkFarm2 ();
			RemoveFarmRecord (_cellCtrl);
		}

		void RemoveFarmRecord (CellObjCtrl ctrlA)
		{
			foreach (List<CellObjCtrl> lstCtrl in _Farms) {
				foreach (CellObjCtrl ctrl in lstCtrl) {
					if (ctrl == ctrlA) {
						_Farms.Remove (lstCtrl);
						return;
					}
				}
			}
		}

		void RecordNewFarm (CellObjCtrl ctrlA, CellObjCtrl ctrlBwd)
		{
			List<CellObjCtrl> BackToBack = new List<CellObjCtrl> ();
			BackToBack.Add (ctrlA);
			BackToBack.Add (ctrlBwd);
			_Farms.Add (BackToBack);
		}
	}
}
