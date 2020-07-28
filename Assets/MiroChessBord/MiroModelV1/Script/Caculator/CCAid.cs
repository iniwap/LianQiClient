using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCAid : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		public MiroV1PlacementMgr _mgr;

		private static List<CCAid> _ccs = new List<CCAid>();

		public CCAid()
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

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		override protected void _Calculate()
		{
			//TryToAid ();
			//Invoke ("TryToAid", 0.01f);

			TryToAid();
		
		}

		void TryToAid ()
		{
			TryAid (_cellCtrl);


			bool bFaceToFace = CellObjCtrlUtils.IsFaceToFace (_cellCtrl);
			if (bFaceToFace) {
				CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (_cellCtrl);
				TryAid (fwdCtrl);
			}

		}

		void TryAid (CellObjCtrl ctrl)
		{
			bool bCtrlling = CellObjCtrlUtils.IsControllingObj (ctrl);
			if (!bCtrlling)
				return;
			//bool bFwdCtrlling = CellObjCtrlUtils.IsNbCellControllingObj (_cellCtrl, 0);
			bool bShouldAid = ShouldAid ();
			MiroModelV1 model = ctrl._TgtObj.GetComponent<MiroModelV1> ();
			bool bHasPump = model.HasPump ();
			bool bAiding = model.IsAiding ();
			CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (ctrl);
			bool bAidingRight = false;
			if (fwdCtrl._TgtObj != null) {
				//bAidingRight = model.IsAiding (modelTgt);
				//MiroModelV1 modelTgt = fwdCtrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model.HasPump () && model._Pump.IsGrown ()) {
					int AidingDir = model._Pump.GetAidingRDir ();
					CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (fwdCtrl, AidingDir);
					if (nbCtrl == ctrl) {
						bAidingRight = true;
					}
				}
			}
			if (bShouldAid) {
				bool bRight = _mgr.IsPumpingRight (ctrl);
				if (bRight) {
					//Debug.Log ("Aid Right");
				}
				else
					if (!bHasPump) {
						_mgr.CreatePumpForAB (ctrl.transform, fwdCtrl.transform);
					}
					else {
						if (!bAidingRight) {
							_mgr.RetargetPumpsFor (ctrl.transform);
							//Debug.Log ("_mgr.RetargetPumpsFor (_cellCtrl.transform);");
						}
						else {
							if (!bAiding) {
								//model._GrowPumpTrigger = true;
								model.GrowUpPump ();
								//Debug.Log ("GrowUpPump: " + model);
							}
							else {
								model.ShrinkPump ();
								//model._ShrinkPumpTrigger = true;
							}
						}
					}
			}
			else {
				model.ShrinkPump ();
				//model._ShrinkPumpTrigger = true;
				//print ("model._ShrinkPumpTrigger = true;");
				if (bAiding) {
				}
			}
		}


		private bool ShouldAid()
		{
			
			CellObjCtrl FwdCtrl = 
				CellObjCtrlUtils.GetFwdCellObjCtrl (_cellCtrl);
			bool bFwdCtrlling = CellObjCtrlUtils.IsControllingObj (FwdCtrl);
			if (!bFwdCtrlling) {
				return false;
			}

			MiroV1ModelSetting MyMSetting = 
				_cellCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
			MiroV1ModelSetting FwdMSetting = 
				FwdCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
			bool bFwdFriend = MyMSetting.IsSameCamp (FwdMSetting);

			MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);
			bool bAlive = modelMe.IsAlive ();

			bool bAid = bFwdFriend && bAlive;
			if (bAid) {
				//print (_cellCtrl + " bAid?" + bAid + "    FwdObj:" + FwdCtrl._TgtObj);
			}

			return bAid;
		}


	}
}
