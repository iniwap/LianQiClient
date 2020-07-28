using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellObjCtrlUtils  {

		public static bool IsControllingObj(CellObjCtrl cctrl)
		{
			if (cctrl == null) {
				return false;
			}

			if (cctrl._TgtObj == null) {
				return false;
			}

			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			if (hc == null) {
				return false;
			}

			return true;
		}

		public static bool IsInChessBoard(CellObjCtrl cctrl)
		{
			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			return (hc!=null);
		}

		public static MiroV1ModelSetting GetModelSettingFromCtrl(CellObjCtrl cctrl)
		{
			if (cctrl == null || cctrl._TgtObj == null) {
				return null;
			}
			MiroV1ModelSetting setting = cctrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
			return setting;
		}

		public static CellObjCtrl GetFwdCellObjCtrl(CellObjCtrl cctrl)
		{
			CellObjCtrl fwdCCtrl = GetNbCellObjCtrl (cctrl, 0);
			return fwdCCtrl;
		}

		public static CellObjCtrl GetNbCellObjCtrlInAbsDir(CellObjCtrl cctrl, int Dir)
		{
			int rdir = Dir - cctrl.GetDir();
			rdir = (int)Mathf.Repeat ((float)rdir, 6.0f);
			return GetNbCellObjCtrl (cctrl, rdir);
		}

		public static CellObjCtrl GetNbCellObjCtrl(CellObjCtrl cctrl, int RelativeDir)
		{
			if (!IsInChessBoard (cctrl))
				return null;

			int FwdDir = cctrl.GetFwdDir ();
			int dir = FwdDir + RelativeDir;
			dir = (int)Mathf.Repeat ((float)dir, 6.0f);
			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			Transform NbTF = hc._Neighbors [dir];
			if (NbTF == null)
				return null;

			CellObjCtrl nbCtrl = 
				NbTF.GetComponent<CellObjCtrl> ();
			return nbCtrl;
		}

		public static bool IsNbCellControllingObj(CellObjCtrl cctrl, int NbRelativeDir)
		{
			CellObjCtrl cctrlNb = 
				GetNbCellObjCtrl (cctrl, NbRelativeDir);

			if (cctrlNb == null ) {
				return false;
			}

			bool bCtrlling = (cctrlNb._TgtObj != null);
			return bCtrlling;

		}

		public static MiroModelV1 GetMiroModelFromCell(CellObjCtrl cctrl)
		{
			MiroModelV1 model = null;

			if (cctrl != null && cctrl._TgtObj != null) {
				model = cctrl._TgtObj.GetComponent<MiroModelV1> ();
			}

			return model;
		}



		public static bool IsControllingSameCampS(CellObjCtrl cctrl0, CellObjCtrl cctrl1)
		{
			
			if (cctrl0 == null || cctrl0._TgtObj == null) {
				return false;
			}
			MiroV1ModelSetting setting0 = cctrl0._TgtObj.GetComponent<MiroV1ModelSetting> ();


			if (cctrl1 == null || cctrl1._TgtObj == null) {
				return false;
			}
			MiroV1ModelSetting setting1 = cctrl1._TgtObj.GetComponent<MiroV1ModelSetting> ();

			bool bSame = setting0.IsSameCamp (setting1);
			return bSame;
		}

		public static bool DetectRingFrom(CellObjCtrl cctrl, ref List<CellObjCtrl> ringObjs)
		{
			bool bRing = false;
			bool bCtrlling = CellObjCtrlUtils.IsControllingObj (cctrl);
			if (!bCtrlling)
				return false;
			ringObjs.Add (cctrl);

			CellObjCtrl ctrlNxt = CellObjCtrlUtils.GetFwdCellObjCtrl (cctrl);
			ringObjs.Add (ctrlNxt);

			int count = 0;
			bool bReturn = false;



			while (CellObjCtrlUtils.IsControllingSameCampS (cctrl,ctrlNxt)) {
				
				ctrlNxt = CellObjCtrlUtils.GetFwdCellObjCtrl (ctrlNxt);
				if (ctrlNxt == cctrl) {
					bReturn = true;
					break;
				}

				if(ringObjs.Contains(ctrlNxt))
				{
					int id = ringObjs.IndexOf (ctrlNxt);
					ringObjs.RemoveRange (0, id + 1);
					ringObjs.Add (ctrlNxt);
					bReturn = true;
					break;
				}

				ringObjs.Add (ctrlNxt);
				count++;

				if (count > 100) {
					break;
				}

			}

			if (bReturn && ringObjs.Count >= 3) {
				bRing = true;
			}

			return bRing;
		}

		public static bool ShouldBeingAbsorbedToDir(CellObjCtrl cctrl, int relativeDir)
		{
			if (cctrl == null) {
				return false;
			}

			bool bAttacking = CellObjCtrlUtils.ShouldAttacking (cctrl);
			bool bAssisting = CellObjCtrlUtils.ShouldAssistingAttacking (cctrl);
			if (!(bAttacking || bAssisting)) {
				return false;
			}


			CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (cctrl,relativeDir);
			if (nbCtrl == null) {
				return false;
			}

			bool bIsEnemy = CellObjCtrlUtils.IsNbEnemy (cctrl, relativeDir);
			if (!bIsEnemy) {
				return false;
			}

			CellObjCtrl nbBwdCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (nbCtrl, 3);
			if (nbBwdCtrl == null) {
				return false;
			}

			bool bNbBackToMe = (nbBwdCtrl == cctrl);
			if (!bNbBackToMe) {
				return false;
			}

			MiroModelV1 nbModel = CellObjCtrlUtils.GetMiroModelFromCell (nbCtrl);
			bool bNbAlive = nbModel.IsAlive ();
			if (!bNbAlive) {
				return false;
			}

			bool bNbFaceToFace = CellObjCtrlUtils.IsFaceToFace (nbCtrl);
			if (!bNbFaceToFace) {
				return false;
			}

			CellObjCtrl nbFwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (nbCtrl);
			MiroModelV1 nbFwdModel = CellObjCtrlUtils.GetMiroModelFromCell (nbFwdCtrl);

			if (!nbFwdModel.IsAlive()) {
				return false;
			}


			return true;
		}

		public static bool IsNbAttacking(CellObjCtrl cctrl, int relativeDir)
		{
			CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (cctrl, relativeDir);
			if (nbCtrl == null) {
				return false;
			}

			bool bNbAtt = ShouldAttacking (nbCtrl);
			return bNbAtt;
		}

		public static bool ShouldAttacking(CellObjCtrl cctrl)
		{
			bool bFwdEnemy = IsNbEnemy (cctrl, 0);
			bool bFwdEmpty = IsNbEmpty (cctrl, 0);
			if (bFwdEnemy || bFwdEmpty) {
				return true;
			} else {
				return false;
			}
		}

		public static bool ShouldAssistingAttacking(CellObjCtrl cctrl)
		{
			bool bShouldAT = ShouldAttacking (cctrl);
			if (bShouldAT) {
				return false;
			}
			bool bFwdSameCamp = IsNbSameCamp (cctrl, 0);
			if (!bFwdSameCamp) {
				return false;
			}
			CellObjCtrl FwdCtrl = GetNbCellObjCtrl (cctrl, 0);
			bool bFwdAttacking = ShouldAttacking (FwdCtrl);
			return bFwdAttacking;
		}

		public static bool IsFwdSameCamp(CellObjCtrl cctrl)
		{
			return IsNbSameCamp (cctrl, 0);
		}

		public static bool IsNbSameCamp(CellObjCtrl cctrl,int relativeDir)
		{
			bool bSame = false;
			if (cctrl == null || cctrl._TgtObj == null) {
				return false;
			}
			MiroV1ModelSetting setting0 = cctrl._TgtObj.GetComponent<MiroV1ModelSetting> ();

			CellObjCtrl nbCtrl = GetNbCellObjCtrl(cctrl,relativeDir);
			if (nbCtrl._TgtObj != null) {
				MiroV1ModelSetting settingNb = 
					nbCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
				bSame = settingNb.IsSameCamp (setting0);
			}

			return bSame;
		}

		public static bool IsNbEnemy(CellObjCtrl ctrl,int relativeDir)
		{
			bool bEnemy = false;
			if (ctrl == null) {
				return false;
			}

			MiroModelV1 modelThis = GetMiroModelFromCell (ctrl);
			MiroModelV1 modelNb = GetNbModel (ctrl, relativeDir);
			if (modelNb == null || modelThis == null) {
				return false;
			}

			MiroV1ModelSetting settingThis = 
				modelThis.gameObject.GetComponent<MiroV1ModelSetting> ();
			MiroV1ModelSetting settingNb = 
				modelNb.gameObject.GetComponent<MiroV1ModelSetting> ();
			bEnemy = !settingNb.IsSameCamp (settingThis);
			return bEnemy;
		}

		public static bool IsFwdEmpty(CellObjCtrl cctrl)
		{
			return IsNbEmpty (cctrl, 0);
		}

		public static bool IsNbEmpty(CellObjCtrl cctrl, int relativeDir)
		{
			CellObjCtrl fwdCtrl = 
				GetNbCellObjCtrl (cctrl,relativeDir);
			bool bEmpty = false;
			if (fwdCtrl == null) {
				bEmpty = true;
			} else {
				bEmpty = (fwdCtrl._TgtObj == null);
			}
			return bEmpty;
		}

		/*
		public static bool IsNbEnemy(CellObjCtrl cctrl, int relativeDir)
		{
			if (cctrl._TgtObj == null) {
				return false;
			}

			CellObjCtrl nbCtrl = GetNbCellObjCtrl (cctrl, relativeDir);
			bool bEnemy = false;
			if (nbCtrl != null && nbCtrl._TgtObj!=null) {
				var settingMe = 
					cctrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
				var settingNb  = 
					nbCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
				bEnemy = (settingMe._CampName!=settingNb._CampName);
			}

			return bEnemy;
		}*/

		public static bool IsBackToBack(CellObjCtrl ctrl)
		{
			if (ctrl == null||ctrl._TgtObj==null) {
				return false;
			}

			bool bBwdFriend = 
				CellObjCtrlUtils.IsNbSameCamp (ctrl, 3);
			if (bBwdFriend) {
				CellObjCtrl bwdCtrl = GetNbCellObjCtrl (ctrl, 3);
				if (bwdCtrl.GetDir() == ctrl.GetBwdDir ()) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}


		}

		public static bool IsFaceToFace(CellObjCtrl ctrl)
		{
			if (ctrl == null||ctrl._TgtObj==null) {
				return false;
			}

			bool bFwdFriend = 
				CellObjCtrlUtils.IsNbSameCamp (ctrl, 0);
			if (bFwdFriend) {
				CellObjCtrl fwdCtrl = GetNbCellObjCtrl (ctrl, 0);
				if (fwdCtrl.GetDir() == ctrl.GetBwdDir ()) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}

		}

		public static bool IsNbAssistingAT(CellObjCtrl ctrl, int relativeDir)
		{
			if (ctrl == null || ctrl._TgtObj == null) {
				return false;
			}

			bool bFriend = IsNbSameCamp (ctrl, relativeDir);
			if (!bFriend) {
				return false;
			}

			CellObjCtrl ctrlNb = GetNbCellObjCtrl (ctrl, relativeDir);
			CellObjCtrl ctrlNbFwd = GetFwdCellObjCtrl (ctrlNb);
			if (ctrlNbFwd == ctrl) {
				return true;
			} else {
				return false;
			}

		}

		public static MiroModelV1 GetNbModel(CellObjCtrl ctrl, int relativeDir)
		{
			CellObjCtrl nbCtrl = GetNbCellObjCtrl (ctrl, relativeDir);
			if (nbCtrl == null || nbCtrl._TgtObj == null) {
				return null;
			}
			MiroModelV1 model = nbCtrl._TgtObj.GetComponent<MiroModelV1> ();
			return model;
		}

		public static bool DetectNPCCloseChainFrom(CellObjCtrl ctrl, ref List<CellObjCtrl> ringObjs, ref List<CellObjCtrl> _chainObjs)
		{
			bool bDetected = false;
			List<CellObjCtrl> lst = new List<CellObjCtrl> ();
			lst.Add (ctrl);
			while (CellObjCtrlUtils.IsFwdSameCamp (ctrl)) {
				ctrl = CellObjCtrlUtils.GetFwdCellObjCtrl (ctrl);
				if (lst.Contains (ctrl)) {
					int startId = lst.IndexOf (ctrl);
					int cnt = startId ;
					//Debug.Log ("remove cnt:" + cnt);
					_chainObjs = lst;
					lst.RemoveRange (0, cnt);
					bDetected = true;
					break;
				}
				lst.Add (ctrl);
			}
			ringObjs = lst;

			return bDetected;
		}
			
		public static Vector3 GetDirVector3(CellObjCtrl cctrl)
		{
			Vector3 dir = Vector3.right;
			CellObjCtrl fwdCtrl = GetFwdCellObjCtrl (cctrl);

			Vector3 p0 = cctrl.transform.position;
			if (fwdCtrl != null) {
				Vector3 p1 = fwdCtrl.transform.position;
				Vector3 vec = p1 - p0;
				dir = vec.normalized;
			} else {
				CellObjCtrl bwdCtrl = GetNbCellObjCtrl (cctrl, 3);
				if (bwdCtrl != null) {
					Vector3 p1 = bwdCtrl.transform.position;
					Vector3 vec = p0 - p1;
					dir = vec.normalized;
				}
			}
			return dir;
		}

		public static void ReorderRing(ref List<CellObjCtrl> ringCtrls)
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

		static public List<MiroModelV1> GetNbEnemiesPointToThis(CellObjCtrl ctrl)
		{
			List<MiroModelV1> enemies = new List<MiroModelV1> ();
			for (int dir= 0;  dir< 6; dir++) {
				CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (ctrl, dir);
				if (nbCtrl == null)
					continue;

				CellObjCtrl nbCtrlFwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (nbCtrl);
				if (nbCtrlFwdCtrl != ctrl)
					continue;

				MiroModelV1 nbModel = CellObjCtrlUtils.GetMiroModelFromCell (nbCtrl);
				bool bEnemy = IsNbEnemy (ctrl, dir);

				if (bEnemy) {
					enemies.Add (nbModel);
				}

			}
			return enemies;
		}

		static public T GetComponentInNbCtrl<T>(CellObjCtrl ctrl, int relativeDir)
		{
			
			CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (ctrl, relativeDir);
			if (nbCtrl != null) {
				T comp = 
					nbCtrl.gameObject.GetComponentInChildren<T> ();
				return comp;
			} else {
				return ctrl.gameObject.GetComponent<T>();
			}
		}

		public static bool IsNbAidingMe(CellObjCtrl cctrl, int relativeDir)
		{
			bool bSameCamp = IsNbSameCamp (cctrl, relativeDir);
			if (!bSameCamp)
				return false;

			//MiroModelV1 modelMe = CellObjCtrlUtils.GetMiroModelFromCell (cctrl);

			CellObjCtrl nbCtrl = 
				CellObjCtrlUtils.GetNbCellObjCtrl (cctrl, relativeDir);

			MiroModelV1 nbModel = CellObjCtrlUtils.GetMiroModelFromCell (nbCtrl);
			bool bNbAlive = nbModel.IsAlive ();

			CellObjCtrl nbFwdCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (nbCtrl);

			bool bAiding = (nbFwdCtrl==cctrl) && bNbAlive;

			return bAiding;
		}

		public static bool IsNbAlive(CellObjCtrl cctrl, int relativeDir)
		{
			CellObjCtrl nbCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (cctrl,relativeDir);
			if (nbCtrl == null) {
				return false;
			}

			if (!CellObjCtrlUtils.IsControllingObj (nbCtrl)) {
				return false;
			}

			MiroModelV1 nbModel = CellObjCtrlUtils.GetMiroModelFromCell (nbCtrl);
			bool bNbAlive = nbModel.IsAlive ();
			return bNbAlive;
		}
			

	}
}
