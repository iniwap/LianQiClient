using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCRing : MiroV1Caculator {
		public CellObjCtrl _cellCtrl;
		public MiroV1PlacementMgr _mgr;

		private CCRingPlacer _ringPlacer;

		private int _IdInRing = -1;

		private static List< List<CellObjCtrl> > _Rings = 
			new List< List<CellObjCtrl> >();

		private static List<CCRing> _ccRings = new List<CCRing> ();

		public static List<List<Hex>> GetRings()
		{
			List<List<Hex>> rings = new List<List<Hex>> ();
			foreach (var lstRing in _Rings) {
				List<Hex> lstHex = new List<Hex> ();
				foreach (var cctrl in lstRing) {
					HexCoord hc = cctrl.GetComponent<HexCoord> ();
					Hex hcoord = hc._hex;
					lstHex.Add (hcoord);
				}
				rings.Add (lstHex);
			}
			return rings;
		}

		public CCRing()
		{
			_ccRings.Add (this);
		}

		// Use this for initialization
		void Start () {
			
		}

		public void SetRingPlacer(CCRingPlacer ringPlacer)
		{
			_ringPlacer = ringPlacer;
		}

		public void ResetAllCCRingID()
		{
			foreach(var ccr in _ccRings)
			{
				ccr.ResetIdInRing ();
			}
		}

		static public void TurnAllON()
		{
			foreach (var cca in _ccRings) {
				cca.TurnON ();
			}
		}

		static public void TurnAllOFF()
		{
			foreach (var cca in _ccRings) {
				cca.TurnOFF();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private bool IsIdAdded()
		{
			return _IdInRing >= 0;
		}

		static private void SClearRing(List<Transform> RingObjsTFs)
		{
			List<MiroModelV1> ringObjMiros = new List<MiroModelV1> ();
			foreach (Transform tf in RingObjsTFs) {
				MiroModelV1 miro = tf.GetComponent<MiroModelV1> ();
				if (miro != null) {
					ringObjMiros.Add (miro);
				}
			}

			for (int i = _Rings.Count - 1; i >= 0; i--) {
				var rc = _Rings [i];
				List<MiroModelV1> miros = new List<MiroModelV1> ();
				foreach (CellObjCtrl ctrl in rc) {
					MiroModelV1 miro = CellObjCtrlUtils.GetMiroModelFromCell (ctrl);
					if (miro != null) {
						miros.Add (miro);
					}
				}

				bool bSame = 
					Lyu.DataStructureUtils.ContainsSame<MiroModelV1> (miros, ringObjMiros);
				if (bSame) {
					_Rings.RemoveAt (i);
					break;
				}
			}

		}

		public void ClearRing(List<Transform> RingObjsTFs)
		{
			SClearRing (RingObjsTFs);
		}
		override protected void _Calculate()
		{
			_ringPlacer.ReadyToUpdate (this);
		}
		/*
		override protected void _Calculate()
		{
			List<CellObjCtrl> ringObjs = new List<CellObjCtrl>();
			bool bDetected = 
				CellObjCtrlUtils.DetectNPCCloseChainFrom (_cellCtrl, ref ringObjs);
		
			bool bRing = (ringObjs.Count > 2)&&bDetected;

			if (bRing) {
				//Debug.Log ("SetUpRing");
				SetupRing (ringObjs);
			} else {
				//Debug.Log ("UnSetUpRing");
				UnSetupRing (ringObjs);
			}

			bool bInRing = ringObjs.Contains (_cellCtrl);
			if (!bInRing) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (_cellCtrl);
				model.ShrinkRing ();
			}

		}
*/


		void SetupRing (List<CellObjCtrl> ringObjs)
		{
			
			bool bExist = false;
			foreach (List<CellObjCtrl> ringObjLst in _Rings) {
				bool bSame = IsContainSame (ringObjs, ringObjLst);
				if (bSame) {
					bExist = true;
					IsContainSame (ringObjs, ringObjLst);
					//Debug.Log ("bSame!");
					return;
				}
			}
			bool bIdAdded = true;
			foreach (CellObjCtrl ctrl in ringObjs) {
				CCRing cc = ctrl.GetComponentInChildren<CCRing> ();
				bIdAdded = bIdAdded && cc.IsIdAdded ();
			}
			if (!bExist && !bIdAdded) {
				ReorderRing (ref ringObjs);
				AddIdInRing (ringObjs);
				// shrink smaller ring
				for (int i = _Rings.Count - 1; i >= 0; i--) {
					List<CellObjCtrl> lstCtrl = _Rings [i];
					foreach (var ctrl in lstCtrl) {
						if (ringObjs.Contains (ctrl)) {
							MiroModelV1 model = ctrl._TgtObj.GetComponent<MiroModelV1> ();
							model.ShrinkRing ();
							Debug.Log ("model.ShrinkRing (): " + model.name);
							break;
						}
					}

				}
				foreach (List<CellObjCtrl> lstCtrl in _Rings) {
					for (int i = lstCtrl.Count - 1; i >= 0; i--) {
					}
					foreach (var ctrl in lstCtrl) {
						if (ringObjs.Contains (ctrl)) {
							MiroModelV1 model = ctrl._TgtObj.GetComponent<MiroModelV1> ();
							model.ShrinkRing ();
							Debug.Log ("model.ShrinkRing (): " + model.name);
						}
					}
				}
				// create new ring
				List<GameObject> rObjs = new List<GameObject> ();
				foreach (CellObjCtrl ctrl in ringObjs) {
					rObjs.Add (ctrl._TgtObj);
				}
				_mgr.CreateRing (rObjs);
				Debug.Log ("_mgr.CreateRing (rObjs);");
				_Rings.Add (ringObjs);
			}
			if (_IdInRing == ringObjs.Count - 1) {
				ResetIdInRing (ringObjs);
			}
		}

		static void UnSetupRing (List<CellObjCtrl> ringObjs)
		{
			foreach (CellObjCtrl ctrl in ringObjs) {
				for (int i = _Rings.Count - 1; i >= 0; i--) {
					List<CellObjCtrl> lst = _Rings [i];
					if (lst.Contains (ctrl)) {
						// shink this ring
						MiroModelV1 model = ctrl._TgtObj.GetComponent<MiroModelV1> ();
						model.ShrinkRing ();
						_Rings.RemoveAt (i);
					}
				}
			}
		}

		static void AddIdInRing (List<CellObjCtrl> ringObjs)
		{
			for (int i = 0; i < ringObjs.Count; i++) {
				CCRing cring = ringObjs [i].GetComponentInChildren<CCRing> ();
				cring._IdInRing = i;
			}
		}

		static void ResetIdInRing (List<CellObjCtrl> ringObjs)
		{
			foreach (CellObjCtrl cctrl in ringObjs) {
				CCRing cring = cctrl.GetComponentInChildren<CCRing> ();
				cring.ResetIdInRing ();
			}
		}

		private void ResetIdInRing()
		{
			_IdInRing =-1;
		}



		private static bool IsContainSame(List<CellObjCtrl> A, List<CellObjCtrl> B)
		{
			bool bSame =
				Lyu.DataStructureUtils.ContainsSame<CellObjCtrl> (A, B);	
			return bSame;
		}

		static private void ReorderRing(ref List<CellObjCtrl> ringCtrls)
		{
			if (ringCtrls.Count < 3)
				return;
			ringCtrls.Add (ringCtrls [0]);

			List<Vector3> vecs = new List<Vector3> ();
			for (int i = 1; i < ringCtrls.Count; i++) {
				CellObjCtrl a = ringCtrls [i - 1];
				CellObjCtrl b = ringCtrls [i];
				Vector3 shift = b.transform.position - a.transform.position;
				vecs.Add (shift);
			}

			float z = 0.0f;
			for (int i = 1; i < vecs.Count; i++) {
				Vector3 vecPrev = vecs [i - 1];
				Vector3 vecThis = vecs [i];
				Vector3 crs = 
					Vector3.Cross (vecPrev, vecThis);
				z += crs.z;
			}
				
			if (z > 0.0f) {
				ringCtrls.Reverse ();
			}
			ringCtrls.RemoveAt (ringCtrls.Count - 1);



		}





		[ContextMenu("TryCaculate")]
		public void TryCaculate()
		{
			_Calculate ();
		}

	}
}
