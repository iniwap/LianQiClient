using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class CCHole2 : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		//public MiroV1PlacementMgr _mgr;

		private static List<CCHole2> _ccs = new List<CCHole2>();

		public UnityEvent _HoleON,_HoleOFF;
		public float _AbsorbingComputeDelay = 0.2f;

		public CCHole2()
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

		private static List<List<CellObjCtrl>> _Holes = 
			new List<List<CellObjCtrl>>();
		public static List<List<Hex> > GetFaceToFaces()
		{
			List<List<Hex> > FFs = new List<List<Hex> > ();
			foreach (var lstCtrl in _Holes) {
				List<Hex> lstHex = new List<Hex> ();
				foreach (CellObjCtrl ctrl in lstCtrl) {
					HexCoord hc = ctrl.GetComponent<HexCoord> ();
					lstHex.Add (hc._hex);
				}
				FFs.Add (lstHex);
			}
			return FFs;
		}

		override protected void _Calculate()
		{
			ConfigHole ();

			/*
			if (!bFaceToFace) {
				DeConfigAbsorbing ();
			} else {
				ConfigAbsorbing ();
			}*/

		}

		void ConfigHole()
		{
			bool bFaceToFace = CellObjCtrlUtils.IsFaceToFace (_cellCtrl);

			//int bwdDir = _cellCtrl.GetBwdDir ();
			MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);
			CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (_cellCtrl);
			bool bFaceToFaceFwd = CellObjCtrlUtils.IsFaceToFace (fwdCtrl);
			MiroModelV1 modelFwd = CellObjCtrlUtils.GetMiroModelFromCell (fwdCtrl);

			if (modelMe != null) {
				ConfigureHole2 (modelMe, bFaceToFace);
				//RecordNewFaceToFace (_cellCtrl,fwdCtrl);
				//_HoleON.Invoke ();
			}
			if (modelFwd != null) {
				ConfigureHole2 (modelFwd, bFaceToFaceFwd);
				//RemoveFaceToFaceRecord (_cellCtrl);
				//_HoleOFF.
				/*
				if (modelFwd.name == "Red0" && !bFaceToFace) {
					print ("!FaceToFace");
					CellObjCtrlUtils.IsFaceToFace (_cellCtrl);
				}*/
			}

			if (bFaceToFace) {
				RecordNewFaceToFace (_cellCtrl, fwdCtrl);
				CaculateAbsorbing (_cellCtrl);
				CaculateAbsorbing (fwdCtrl);
			} else {
				RemoveFaceToFaceRecord (_cellCtrl);
			}
		}

		void CaculateAbsorbing (CellObjCtrl cctrl)
		{
			CellObjCtrl backCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (cctrl, 3);
			CCAbsorb ccab = backCtrl.GetComponentInChildren<CCAbsorb> ();
			ccab.Invoke ("ConfigAbsorbing", _AbsorbingComputeDelay);
		}

		static void RecordNewFaceToFace (
			CellObjCtrl ctrlA, CellObjCtrl ctrlB)
		{
			foreach (List<CellObjCtrl> lstCtrl in _Holes) {
				foreach (CellObjCtrl ctrl in lstCtrl) {
					if (ctrl == ctrlA || ctrl == ctrlB) {
						return;
					}
				}
			}
			List<CellObjCtrl> ff = new List<CellObjCtrl> ();
			ff.Add (ctrlA);
			ff.Add (ctrlB);
			_Holes.Add (ff);
		}

		static void RemoveFaceToFaceRecord(CellObjCtrl ctrl)
		{
			foreach (List<CellObjCtrl> lstFF in _Holes) {
				foreach(CellObjCtrl thisCtrl in lstFF)
				{
					if (thisCtrl == ctrl) {
						_Holes.Remove (lstFF);
						return;
					}
				}
			}
		}

		private void ConfigureHole2(MiroModelV1 model, bool bFaceToFace)
		{
			
			if (bFaceToFace) {
				model._BlackHole.IsGrown ();
			}
			bool IsGrown = model._BlackHole.IsGrown ();
			//bool ToShrink = model._BlackHole._ShrinkTrigger;
			//bool bGrown = IsGrown && (!ToShrink);

			bool bToGrow = (!IsGrown && bFaceToFace);
			bool bToShrink = (IsGrown && !bFaceToFace);

			/*
			print ("model:" + model + 
				" IsGrown:" + IsGrown + 
				" FaceToFace" + bFaceToFace + 
				" ShrinkTrigger:" + model._BlackHole._ShrinkTrigger);
				*/
			if (bToGrow) {
				model._BlackHole.GrowUp();
				_HoleON.Invoke ();
			} else if (bToShrink) {
				model._BlackHole.Shrink ();
				_HoleOFF.Invoke ();
				//print ("model to shrink:" + model);
			}
			//model._BlackHole._bAbsorbing = bFaceToFace;
		}

		// absorbing configuration
		void ConfigAbsorbing ()
		{
			//int bwdDir = _cellCtrl.GetBwdDir ();
			MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);
			CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (_cellCtrl);
			MiroModelV1 modelFwd = CellObjCtrlUtils.GetMiroModelFromCell (fwdCtrl);

			ConfigureHole2 (modelMe, true);
			ConfigureHole2 (modelFwd, true);

			MiroV1PlacementMgr.ConfigAbsorbingForTF (_cellCtrl.transform);
			MiroV1PlacementMgr.ConfigAbsorbingForTF (fwdCtrl.transform);
		}

		void DeConfigAbsorbing()
		{
			MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);	
			if (modelMe == null)
				return;

			modelMe.ReleaseAbsorbing ();
		}

	}
}
