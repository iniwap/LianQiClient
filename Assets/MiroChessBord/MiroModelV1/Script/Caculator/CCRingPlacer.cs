using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiroV1
{
	public class CCRingPlacer : MonoBehaviour {
		public CCRing[] _ccrings;
		public MiroV1PlacementMgr _mgr;

		// data container
		private HashSet<MiroModelV1> _miros = 
			new HashSet<MiroModelV1> ();
		private Dictionary<CellObjCtrl, bool> _ctrls = 
			new Dictionary<CellObjCtrl, bool>();
		private List<CellObjCtrl> _ctrlLst = 
			new List<CellObjCtrl>();

		// ring container
		private List<List<CellObjCtrl>> _Rings = 
			new List<List<CellObjCtrl>>();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void ClearRings()
		{
			_Rings.Clear ();
		}

		public void ReadyToUpdate(CCRing ccr)
		{
			//print ("ReadyToUpdate!" + ccr);
			MiroModelV1 miro = CellObjCtrlUtils.GetMiroModelFromCell (ccr._cellCtrl);
			_miros.Add (miro);
			_ctrls[ccr._cellCtrl] = false;

			if (_miros.Count == _mgr._MiroObjs.Count) {
				PlaceRing ();
			}
		}

		public void PlaceRing()
		{
			//print ("PlaceRing!");

			foreach (var item in _ctrls) {
				_ctrlLst.Add (item.Key);
			}

			List<List<CellObjCtrl>> currentRings = new List<List<CellObjCtrl>> ();
			DetectRingCtrlsLst (ref currentRings);
				
			// detect obsolete rings
			List<List<CellObjCtrl>> obsoleteRings = new List<List<CellObjCtrl>> ();
			for (int i = _Rings.Count - 1; i >= 0; i--) {
				var ringObjs = _Rings [i];
				bool bExist = false;
				for (int j = currentRings.Count - 1; j >= 0; j--) {
					var newRingObjs = currentRings [j];
					bExist =
						Lyu.DataStructureUtils.ContainsSame<CellObjCtrl> (ringObjs, newRingObjs);	
					if (bExist)
						break;
				}

				if (!bExist) {
					obsoleteRings.Add (ringObjs);
					_Rings.RemoveAt (i);
				} 
			}

			// detect new rings
			List<List<CellObjCtrl>> newCreateRings = new List<List<CellObjCtrl>> ();
			for (int i = currentRings.Count - 1; i >= 0; i--) {
				var ringObjs = currentRings [i];
				bool bExist = false;
				for (int j = _Rings.Count - 1; j >= 0; j--) {
					var existRObjs = _Rings [j];
					bExist =
						Lyu.DataStructureUtils.ContainsSame<CellObjCtrl> (ringObjs, existRObjs);	
					if (bExist)
						break;
				}

				if (!bExist) {
					newCreateRings.Add (ringObjs);
					_Rings.Add (ringObjs);
				} 
			}

			//delete obsolete rings
			foreach (var lstCtrl in obsoleteRings) {
				//print ("Delete Ring:" + lstCtrl);
				MiroModelV1 miro = CellObjCtrlUtils.GetMiroModelFromCell (lstCtrl [0]);
				miro.ShrinkRing ();
			}

			// create new ring
			foreach (var lstCtrl in newCreateRings) {
				//print ("new Ring:" + lstCtrl);
				List<GameObject> rObjs = new List<GameObject> ();
				foreach (CellObjCtrl ctrl in lstCtrl) {
					rObjs.Add (ctrl._TgtObj);
				}
				_mgr.CreateRing (rObjs);
			}

			//_Rings = currentRings;

			ClearContainers ();
		}

		void DetectRingCtrlsLst (ref List<List<CellObjCtrl>> rings)
		{
			foreach (CellObjCtrl ctrl in _ctrlLst) {
				bool bChecked = _ctrls [ctrl];
				if (bChecked) {
					continue;
				}
				List<CellObjCtrl> ringObjs = new List<CellObjCtrl> ();
				List<CellObjCtrl> chainObjs = new List<CellObjCtrl> ();
				bool bDetected = CellObjCtrlUtils.DetectNPCCloseChainFrom (ctrl, ref ringObjs, ref chainObjs);
				bDetected = (bDetected && (ringObjs.Count > 2));
				// tag each ctrl Checked in chainObjs
				foreach (CellObjCtrl rctrl in chainObjs) {
					_ctrls [rctrl] = true;
				}
				if (bDetected) {
					CellObjCtrlUtils.ReorderRing (ref ringObjs);
					rings.Add (ringObjs);
				}
				_ctrls [ctrl] = true;
			}
		}

		void ClearContainers ()
		{
			_miros.Clear ();
			_ctrls.Clear ();
			_ctrlLst.Clear ();
		}

		public void GetAllCCRings()
		{
			_ccrings = GetComponentsInChildren<CCRing> ();
			foreach (var cc in _ccrings) {
				cc.SetRingPlacer (this);
			}
		}
	}
}
