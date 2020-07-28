using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCHurt : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		//public MiroV1PlacementMgr _mgr;

		private static List<CCHurt> _ccs = new List<CCHurt>();

		public CCHurt()
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
			//DoHurt ();
			Invoke ("DoHurt", 0.5f);
			/*
			Lyu.TimeTrigger tgr = GetComponent<Lyu.TimeTrigger> ();
			tgr.Trigger ();*/
		}

		public void DoHurt()
		{
			bool bFwdEnemy = CellObjCtrlUtils.IsNbEnemy (_cellCtrl, 0);
			if (bFwdEnemy) {
				CCHurt fwdCCHurt = 
					CellObjCtrlUtils.GetComponentInNbCtrl<CCHurt> (_cellCtrl, 0);
				
				fwdCCHurt.BeHurt ();
				//print (_cellCtrl._TgtObj + " do hurt on:" + fwdCCHurt._cellCtrl._TgtObj);
			} else {
				MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);
				if (modelMe != null) {
					modelMe.CeaseAttacking ();

				}
			}
		
		}

		public void BeHurt()
		{
			MiroModelV1 miro = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);

			List<MiroModelV1> enemies = 
				CellObjCtrlUtils.GetNbEnemiesPointToThis (_cellCtrl);

			foreach (var en in enemies) {
				//en.CeaseAttacking ();
				en.SetAttackTarget (miro);
				en.UpdateAttackTarget ();
				//print (en + " SetAttackTarget as:" + miro);
			}
		}
	}
}
