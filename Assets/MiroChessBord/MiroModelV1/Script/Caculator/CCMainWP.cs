using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCMainWP : MiroV1Caculator {

		public float _TimeDelay = 0.01f;
		public CellObjCtrl _cellCtrl;

		private static List<CCMainWP> _ccs = new List<CCMainWP>();

		public CCMainWP()
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
			//ConfigAttack ();

			Invoke ("ConfigAttack", _TimeDelay);

		}

		void ConfigAttack ()
		{
			bool bCtrlling = CellObjCtrlUtils.IsControllingObj (_cellCtrl);
			if (!bCtrlling)
				return;
			MiroModelV1 modelMe = _cellCtrl._TgtObj.GetComponent<MiroModelV1> ();
			modelMe.ResetEmitterTrigger ();
			// to avoid petrification 防止固化
			//modelMe._bTurnMainWeaponByEN = false;
			/*
			int mhp = modelMe.GetMaxHP ();
			bool bAlive = true;
			if (mhp > 0) {
				int atkrCnt = modelMe.GetAttackersCount ();
				bAlive = (atkrCnt < mhp);
			}*/

			bool bAlive = modelMe.IsAlive();

			bool bFwdEmpty = CellObjCtrlUtils.IsNbEmpty (_cellCtrl, 0);
			bool bFwdEnemy = CellObjCtrlUtils.IsNbEnemy (_cellCtrl, 0);
			bool bShouldAttacking = (bFwdEmpty || bFwdEnemy) && bAlive;
			if (bShouldAttacking) {
				//Debug.Log (_cellCtrl + " should attacking!");
			}
			bool bAttacking = modelMe.IsAttacking ();
			if (bAttacking) {
				//Debug.Log (_cellCtrl + " is attacking!");
			}
			// main weapon
			bool StartAttacking = (bShouldAttacking && !bAttacking);
			bool StopAttacking = (bAttacking && !bShouldAttacking);
			bool IsAttacking = bAttacking && bShouldAttacking;
			int atMain = bShouldAttacking ? modelMe._ENGenerator._EN : 0;
			if (StartAttacking) {
				//Debug.Log ("StartAttacking at=" + atMain + " of " +  _cellCtrl._TgtObj);
				modelMe._WeaponSlots [0].ActivateImmediate();
				SetWeaponAT (modelMe, 0, atMain);
			}
			else
				if (StopAttacking) {
					//Debug.Log ("StopAttacking: at=" + atMain+ " of " +  _cellCtrl._TgtObj);
					SetWeaponAT (modelMe, 0, atMain);
					modelMe._WeaponSlots [0].Shrink ();;
				}
				else
					if (IsAttacking) {
						//Debug.Log ("IsAttacking: at=" + atMain+ " of " +  _cellCtrl._TgtObj);
						SetWeaponAT (modelMe, 0, atMain);
					}
					else {
						//print ("Not Attack! " + " ShouldAtt" +  bShouldAttacking + " Atting" + bAttacking + " " + _cellCtrl._TgtObj);
						//print ("Dir:" + _cellCtrl._Dir);
						/*
				if (_cellCtrl._Dir == 5) {
					bFwdEmpty = CellObjCtrlUtils.IsNbEmpty (_cellCtrl,0);
					bFwdEnemy = CellObjCtrlUtils.IsNbEnemy (_cellCtrl,0);
				}*/}
			// sub weapons
			for (int dir = 1; dir < 6; dir++) {
				bool bAssistingAT = CellObjCtrlUtils.IsNbAssistingAT (_cellCtrl, dir);
				//bool bNbAlive = CellObjCtrlUtils.IsNb
				bAssistingAT = bAssistingAT && bAlive;
				MiroModelV1 nbModel = CellObjCtrlUtils.GetNbModel (_cellCtrl, dir);
				int en = 0;
				if (nbModel != null) {
					en = nbModel._ENGenerator._EN; // Change to Support;
				}
				if (bAssistingAT) {
					modelMe._WeaponSlots [dir].ActivateImmediate();
					SetWeaponAT (modelMe, dir, en);
				}
				else {
					modelMe._WeaponSlots [dir].Shrink();
					SetWeaponAT (modelMe, dir, 0);
				}
			}
		}

		static void SetWeaponAT (MiroModelV1 modelMe, int dir, int at)
		{
			//modelMe._WeaponSlots [dir]._ATMax = at;
			//modelMe._WeaponSlots [dir]._AT = at;
			modelMe._WeaponSlots [dir].SetATImmediate (at, at);
		}
	}
}
