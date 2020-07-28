using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCEN : MiroV1Caculator {

		public CellObjCtrl _cellCtrl;
		//public MiroV1PlacementMgr _mgr;

		private static List<CCEN> _ccs = new List<CCEN>();

		public CCEN()
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
			if (!bCtrlling)
				return;
			MiroModelV1 modelMe = 
				_cellCtrl._TgtObj.GetComponent<MiroModelV1> ();
			modelMe._bTurnMainWeaponByEN = false;

			bool bFwdEmpty = CellObjCtrlUtils.IsNbEmpty (_cellCtrl,0);
			bool bFwdEnemy = CellObjCtrlUtils.IsNbEnemy (_cellCtrl,0);
			bool bBackToBack = CellObjCtrlUtils.IsBackToBack (_cellCtrl);
			int en = bBackToBack ? 3 : 1;

			if (bFwdEmpty || bFwdEnemy) {
				SetEN (modelMe,en);
				return;
			} 

			bool bFwdFriend = CellObjCtrlUtils.IsNbSameCamp (_cellCtrl,0);
			if (bFwdFriend) {
				CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (_cellCtrl, 0);
				bool bFriendFaceEnemy = CellObjCtrlUtils.IsNbEnemy (fwdCtrl, 0);
				bool bFriendFaceEmpty = CellObjCtrlUtils.IsNbEmpty (fwdCtrl, 0);
				bool bFriendAttack = (bFriendFaceEnemy || bFriendFaceEmpty);
				if (bFriendAttack) {
					SetEN (modelMe, en);
				} else {
					SetEN (modelMe, 0);
				}
			} 
		}

		public static void SetEN (MiroModelV1 model,  int en)
		{
			model._ENGenerator._ENMax = en;
			model._ENGenerator._EN = en;
		}
			
	}
}
