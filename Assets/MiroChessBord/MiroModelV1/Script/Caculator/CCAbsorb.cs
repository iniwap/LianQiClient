using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCAbsorb : MiroV1Caculator {

		public float _Delay0 = 0.1f;
		public float _Delay1 = 0.2f;

		public CellObjCtrl _cellCtrl;

		private static List<CCAbsorb> _ccs = new List<CCAbsorb>();

		private List<CellObjCtrl> _AbsorbingCtrls = new List<CellObjCtrl> ();

		CellObjCtrl _ToAbsorbCtrl = null;
		int _AT = 0;
		bool _ToAbsorb = false;

		public CCAbsorb()
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
			if (_ToAbsorb) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (_ToAbsorbCtrl);
				int at = model.GetAT ();
				//print ("at:" + at);
				if (at != _AT) {
					ConfigBackToBackNb ();
					_ToAbsorb = false;
				}
			}
			
		}


		override protected void _Calculate()
		{
			//ConfigHole ();
			Invoke("DeConfigAbsorbCtrls",_Delay0);
			//DeConfigAbsorbCtrls ();
			Invoke ("ConfigAbsorbing", _Delay1);
			//ConfigAbsorbing ();
		}

		[ContextMenu("ConfigAbsorbing")]
		void ConfigAbsorbing()
		{
			//bool bFaceEnemy = CellObjCtrlUtils.IsNbEnemy (_cellCtrl,0);
			bool bShouldAttack = CellObjCtrlUtils.ShouldAttacking (_cellCtrl);
			if (!bShouldAttack) {
				return;
			}

			for (int dir = 1; dir < 6; dir++) {
				bool bNbAidingMe = CellObjCtrlUtils.IsNbAidingMe (_cellCtrl, dir);
				if (!bNbAidingMe) {
					continue;
				}
				CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (_cellCtrl, dir);
				ConfigAbsorbForCtrl2 (nbCtrl);
			}

			/*
			bool bBackToBack = CellObjCtrlUtils.IsBackToBack (_cellCtrl);
			if (bBackToBack) {
				CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (_cellCtrl, 3);
				_ToAbsorbCtrl = nbCtrl;
				//ConfigAbsorbForCtrl (nbCtrl);
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell(nbCtrl);
				_AT = model.GetAT ();
				_ToAbsorb = true;
				//print ("AT:" + _AT);
				//print ("ConfigAbsorbForCtrl (nbCtrl):" + nbCtrl);
				print("_ToAbsorb:" + nbCtrl);
			}*/

			ConfigAbsorbForCtrl2 (_cellCtrl);
		}

		void ConfigBackToBackNb()
		{
			if (_ToAbsorbCtrl != null) {
				ConfigAbsorbForCtrl (_ToAbsorbCtrl);
			}
		}

		void ConfigAbsorbForCtrl2(CellObjCtrl ctrl)
		{
			for (int dir = 0; dir < 6; dir++) {
				CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (ctrl,dir);

				bool bShouldBeingAbsorbed = 
					CellObjCtrlUtils.ShouldBeingAbsorbedToDir (ctrl, dir);
				if (!bShouldBeingAbsorbed) {
					continue;
				}

				MiroV1PlacementMgr.ConfigAbsorbingForTF (nbCtrl.transform);
				if (!_AbsorbingCtrls.Contains (nbCtrl)) {
					_AbsorbingCtrls.Add (nbCtrl);
				}
					
			}
		}

		void ConfigAbsorbForCtrl(CellObjCtrl ctrl)
		{
			for (int dir = 0; dir < 6; dir++) {
				//bool bAbsorb = CellObjCtrlUtils.ShouldBeingAbsorbedToDir (ctrl, dir);

				//CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (ctrl,dir);

				//bool bIsAbsorbing = _AbsorbingCtrls.Contains (nbCtrl);

				//ConfigHole (nbCtrl);


				//MiroV1PlacementMgr.StartAbsorbingForTF (nbCtrl.transform);
				/*
				if (bAbsorb) {
					_AbsorbingCtrls.Add (nbCtrl);
					//print ("StartAbsorbingForTF (nbCtrl.transform);" + nbCtrl);
				} else {
					_AbsorbingCtrls.Remove (nbCtrl);
					//DeConfigAbsorbCtrls
					//MiroV1PlacementMgr.StopAbsorbingForCtrl (nbCtrl,true);
					//print ("StopAbsorbingForCtrl (nbCtrl);" + nbCtrl);
					//_AbsorbingCtrls.Remove (nbCtrl);
				}
				*/



			}

		}

		[ContextMenu("DeConfigAbsorbCtrls")]
		void DeConfigAbsorbCtrls()
		{
			foreach (var ctrl in _AbsorbingCtrls) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (ctrl);	

				if (model == null)
					continue;

				model.ReleaseAbsorbing ();
			}

			_AbsorbingCtrls.Clear ();
		}

		void ConfigHole(CellObjCtrl cellCtrl)
		{
			bool bFaceToFace = CellObjCtrlUtils.IsFaceToFace (cellCtrl);

			MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (cellCtrl);
			CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (cellCtrl);
			MiroModelV1 modelFwd = CellObjCtrlUtils.GetMiroModelFromCell (fwdCtrl);

			if (modelMe != null) {
				ConfigureHole2 (modelMe, bFaceToFace);
			}
			if (modelFwd != null) {
				ConfigureHole2 (modelFwd, bFaceToFace);
			}

			if(!bFaceToFace)
			{
				bFaceToFace = CellObjCtrlUtils.IsFaceToFace (cellCtrl);
			}
		}

		private void ConfigureHole2(MiroModelV1 model, bool bFaceToFace)
		{
			if (bFaceToFace) {
				model._BlackHole.IsGrown ();
			} 
			bool IsGrown = model._BlackHole.IsGrown ();

			bool bToGrow = (!IsGrown && bFaceToFace);
			bool bToShrink = (IsGrown && !bFaceToFace);
			if (bToGrow) {
				model._BlackHole._GrowUpTrigger = true;
			} else if (bToShrink) {
				model._BlackHole._ShrinkTrigger = true;
			}
		}


	}
}
