using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCAnteller : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		//public MiroV1PlacementMgr _mgr;

		static private List<CCAnteller> _ccs = new List<CCAnteller>();

		public CCAnteller()
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

		override protected void _Calculate()
		{
			bool bCtrlling = CellObjCtrlUtils.IsControllingObj (_cellCtrl);
			if (!bCtrlling) {
				return;
			}
			HexCoord hc = _cellCtrl.GetComponent<HexCoord> ();

			MiroV1ModelSetting mSetThis= 
				_cellCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
			MiroModelV1 model = mSetThis.GetComponent<MiroModelV1> ();

			OperateFwdAnteller (hc, mSetThis, model);
			OperateBwdAnteller (hc, mSetThis, model);

			MiroV1PlacementMgr.ConfigAbsorbingSrcForTF (_cellCtrl.transform);
		}

		void OperateFwdAnteller (
			HexCoord hc, 
			MiroV1ModelSetting mSetThis, 
			MiroModelV1 model)
		{
			int DirFwd = _cellCtrl.GetFwdDir ();
			bool bFwd = CheckValidInDir (mSetThis, hc, DirFwd);
			ChangeAnteller (bFwd, model._Antellers [0]);
		}

		void OperateBwdAnteller (
			HexCoord hc, 
			MiroV1ModelSetting mSetThis, 
			MiroModelV1 model)
		{
			int DirBwd = _cellCtrl.GetBwdDir ();
			bool bBwd = CheckValidInDir (mSetThis, hc, DirBwd);
			ChangeAnteller (bBwd, model._Antellers [1]);
		}

		private void ChangeAnteller(bool bStateTgt, BlackAntellerSlot ant)
		{
			bool bStateNow = ant._anteller.IsGrown ();
			if (bStateNow && !bStateTgt) {
				ant._ShrinkTrigger = true;
			} else if (!bStateNow && bStateTgt) {
				ant._GrowUpTrigger = true;
			}
		}

		private bool CheckValidInDir(MiroV1ModelSetting mSetThis, HexCoord hc, int dir)
		{
			bool bValid = true;
			if (hc._Neighbors [dir] != null) {
				CellObjCtrl fwdCtrl = 
					hc._Neighbors [dir].GetComponent<CellObjCtrl> ();
				if (fwdCtrl._TgtObj != null) {
					MiroV1ModelSetting mSetFwd = 
						fwdCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
					if (mSetFwd != null && !mSetFwd.IsSameCamp (mSetThis)) {
						bValid = false;
					}
				}
			}
			return bValid;
		}


	}
}
