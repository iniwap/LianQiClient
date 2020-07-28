using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1PlacementMgr : MonoBehaviour {

		public Transform _MiroPrefabsParentTF;
		public List<GameObject> _MiroPrefabs = new List<GameObject>();
		public int _MiroPrefID = 0;
		public UnityEvent _ChangeMiroPrefab;

		public List<Transform> _BirthTF = new List<Transform>();

		public Transform _MiroParent;

		public List<GameObject> _MiroObjs = new List<GameObject> ();

		[System.Serializable]
		public class EventWithListObj: UnityEvent<List<GameObject> > {};

		//private List<GameObject> _SelectedMObjs = new List<GameObject> ();
		public EventWithListObj _SelectMiroObjs;

		public float _BirthZ = -1.0f;

		public string _NamePrefix = "Miro";
		private int _SpawnCount = 0;

		public GameObject _EN2FarmPrefab;
		public Transform _EN2FarmTFParent;

		public GameObject _PumpPrefab;
		public Transform _PumpTFParent;

		public GameObject _EN2AbsorbPrefab;
		public Transform _EN2AbsorbTFParent;

		public GameObject _RingPrefab;
		public Transform _RingParent;



		public UnityEvent _MoveFwd, _MoveBwd;
		public UnityEvent _EN2FarmCreated, _EN2FarmShrinked;

		public CCManager _ccMgr;

		private List<GameObject> _Connections = new List<GameObject> ();
		public void ClearConnections()
		{
			foreach (GameObject gb in _Connections) {
				Destroy (gb);
			}
			_Connections.Clear ();
		}

		public void SetCampsCount(int count)
		{
			int prefCnt = 
				_MiroPrefabsParentTF.childCount;

			int cnt = (int)Mathf.Clamp ((float)count, 0, 4.0f);

			_MiroPrefabs.Clear ();
			for (int i = 0; i < cnt; i++) {
				_MiroPrefabs.Add (_MiroPrefabsParentTF.GetChild (i).gameObject);
			}

			_MiroPrefID = (int)Mathf.Clamp (_MiroPrefID, 0, (float)(cnt - 1));
		}

		public int GetIdOfMiroModelSetting(MiroV1ModelSetting miroSetting)
		{
			int id = -1;
			for (int i = 0; i < _MiroPrefabs.Count; i++) {
				MiroV1ModelSetting prefSetting = 
					_MiroPrefabs [i].GetComponent<MiroV1ModelSetting> ();
				if (miroSetting._CampName == prefSetting._CampName) {
					id = i;
					break;
				}
			}
			return id;
		}

		public void SetMiroPrefabID(int id)
		{
			_MiroPrefID = id;
			_ChangeMiroPrefab.Invoke ();
		}

		public void IncMiroPrefabID()
		{
			_MiroPrefID++;
			_MiroPrefID = (int)Mathf.Repeat ((float)_MiroPrefID, (float)_MiroPrefabs.Count);
			_ChangeMiroPrefab.Invoke ();
		}

		public string GetSelectedMiroPrefabCampName()
		{
			MiroV1ModelSetting setting =
				_MiroPrefabs [_MiroPrefID].GetComponent<MiroV1ModelSetting> ();
			return setting._CampName;
		}
		public List<string> GetMiroPrefabsCampNames()
		{
			List<string> cNames = new List<string> ();
			foreach (GameObject gb in _MiroPrefabs) {
				var setting = gb.GetComponent<MiroV1ModelSetting> ();
				cNames.Add (setting._CampName);
			}
			return cNames;
		}
		public GameObject GetSelectedMiroPrefab()
		{
			return _MiroPrefabs [_MiroPrefID];
		}

		public void SpawnAtTF(Transform tf)
		{
			AddBirthTF (tf, true);
			Spawn ();
			ClearBirthTFs ();
		}

		public void SpawnAtCellCtrl(CellObjCtrl cctrl)
		{
			AddBirthTF (cctrl.transform, true);
			Spawn ();
			ClearBirthTFs ();
		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			if (_BirthTF.Count==0) {
				Debug.Log ("No Birth Pos!");
				return;
			}

			foreach (Transform bth in _BirthTF) {
				SpawnAtCellTF (bth);
			}

			//PointToCurrentDir ();
			ConfigAttackers ();
			ConfigAbsorbingSrcTF ();
		}

		public void ClearBirthTFs()
		{
			_BirthTF.Clear ();
		}

		public void AddBirthTF(Transform bth, bool bClear = false)
		{
			//Debug.Log ("AddBirthTF clear?" + bClear);
			if (bClear) {
				ClearBirthTFs ();
			}
			if (!_BirthTF.Contains (bth)) {
				_BirthTF.Add (bth);
			}
		}

		public void SpawnAtCellTF (Transform bth)
		{
			
			bool occupied = IsOccupied (bth);
			if (occupied) {
				return;
			}
			bool bBlock = IsBlocked (bth);
			if (bBlock) {
				return;
			}

			GameObject newMiro = Instantiate (_MiroPrefabs [_MiroPrefID]) as GameObject;
			MiroV1ModelSetting _modelSetting = newMiro.GetComponent<MiroV1ModelSetting> ();
			Vector3 pos = bth.position;
			pos.z = _BirthZ;
			newMiro.transform.SetParent (_MiroParent);
			newMiro.transform.position = pos;
			newMiro.name = _modelSetting._CampName + _SpawnCount.ToString ();
			RecordMiroObjOnBirthObj (newMiro, bth);
			CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
			//Debug.Log ("Spawn at: " + cellObjCtrl.name);
			//cellObjCtrl._Dir = 0;
			cellObjCtrl.PointToCurrentDir ();
			AddMiroModel (newMiro, cellObjCtrl);
			_SpawnCount++;
		}

		void AddMiroModel (GameObject newMiro,CellObjCtrl cctrl)
		{
			_MiroObjs.Add (newMiro);
			_ccMgr.ChangeItem (newMiro.GetComponent<MiroModelV1>(),cctrl);
		}

		void RemoveMiroModel (int id)
		{
			if(_MiroObjs[id]!=null)
			{
				MiroModelV1 model = _MiroObjs [id].GetComponent<MiroModelV1> ();
				if (model != null) {
					_ccMgr.RemoveItem (model);
				}
			}
			_MiroObjs.RemoveAt (id);
		}

		public List<MiroModelV1> GetMiroModels()
		{
			List<MiroModelV1> models = new List<MiroModelV1> ();
			foreach (var obj in _MiroObjs) {
				MiroModelV1 m = obj.GetComponent<MiroModelV1> ();
				models.Add (m);
			}
			return models;
		}

		void RemoveMiroModel(MiroModelV1 model)
		{
			if (model != null) {
				_ccMgr.RemoveItem (model);
				_MiroObjs.Remove (model.gameObject);
			} 
		}

		void RemoveNullModel()
		{
		}

		void ClearMiroModels ()
		{
			_MiroObjs.Clear ();
			_ccMgr.ClearAllItems ();
		}

		private bool IsBlocked(Transform tf)
		{
			bool bBlock = true;

			CellObjCtrl cctrl = 
				tf.GetComponent<CellObjCtrl> ();
			if (cctrl != null) {
				bBlock = cctrl.IsBlocked ();
			}

			return bBlock;
		}

		public void CheckMiroObjs()
		{
			for (int i = _MiroObjs.Count - 1; i >= 0; i--) {
				if (_MiroObjs [i] == null) {
					RemoveMiroModel (i);
				}
			}
		}

		[ContextMenu("Disappear")]
		public void Disappear()
		{
			foreach (Transform bth in _BirthTF) {
				if (!IsOccupied (bth)) {
					continue;
				}

				CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl == null)
					continue;

				KillMiroAtCellCtrl (cellObjCtrl);
			}

		}

		public bool KillMiroAtCellCtrl (CellObjCtrl cellObjCtrl)
		{
			MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (cellObjCtrl);
			if (model == null) {
				return false;
			}
			ShrinkOrgans (model);
			RemoveMiroModel (model);
			cellObjCtrl.DieTgtObj ();

			return true;
		}

		public bool DisappearMiroAtCellCtrl(CellObjCtrl cellObjCtrl)
		{
			MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (cellObjCtrl);
			if (model == null) {
				return false;
			}

			for (int i = 0; i < 6; i++) {
				MiroV1BlackDot bdot = 
					model._6DirAidDots._AidDots [i];;
				bool bGrown = (bdot._HPMax > 0);
				if (bGrown) {
					CellObjCtrl nbCtrl = 
						CellObjCtrlUtils.GetNbCellObjCtrl (cellObjCtrl, i);
					MiroModelV1 nbModel = CellObjCtrlUtils.GetMiroModelFromCell (nbCtrl);
					nbModel.ShrinkPump ();
					print (nbModel.name + " shrink pump");
				}
			}

			RemoveMiroModel (model);
			cellObjCtrl.DisappearTgtObj ();
			return true;
		}

		static void ShrinkOrgans (MiroModelV1 model)
		{
			model.ShrinkRing ();
			model.ShrinkFarm2 ();
			model.ShrinkPump ();
		}

		public CCRingPlacer _ringPlacer;
		[ContextMenu("ClearAll")]
		public void ClearAll(bool bResetMiroBirthCount = true)
		{
			foreach (GameObject gb in _MiroObjs) {
				Destroy (gb);
			}
			ClearMiroModels ();
			if (bResetMiroBirthCount) {
				MiroModelV1.ResetBirthCount ();
			}

			_ringPlacer.ClearRings ();
			ClearConnections ();
		}

		//private List<Transform>
		private void ConfigAttackerForFwdTF(CellObjCtrl cctrl)
		{
			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			Transform fwdTF = hc._Neighbors [cctrl.GetDir()];
			if (fwdTF != null) {
				ConfigAttackersFor (fwdTF, true);
				print ("ConfigAttackersFor:" + cctrl);
			}
		}

		private CellObjCtrl GetFwdCellObjCtrl(CellObjCtrl cctrl)
		{
			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			int dir = cctrl.GetFwdDir ();
			Transform fwdTF = hc._Neighbors [dir];
			if (fwdTF != null) {
				CellObjCtrl nxtCCtrl = fwdTF.GetComponent<CellObjCtrl> ();
				return nxtCCtrl;
			} else {
				return null;
			}

		}

		private CellObjCtrl GetBwdCellObjCtrl(CellObjCtrl cctrl)
		{
			HexCoord hc = cctrl.GetComponent<HexCoord> ();
			int dir = cctrl.GetBwdDir ();
			Transform bwdTF = hc._Neighbors [dir];
			if (bwdTF != null) {
				CellObjCtrl nxtCCtrl = bwdTF.GetComponent<CellObjCtrl> ();
				return nxtCCtrl;
			} else {
				return null;
			}
		}

		void CeaseAttackingIfNotPointToEnemy (Transform bth, CellObjCtrl cellObjCtrl)
		{
			CellObjCtrl cctrlFwd = GetFwdCellObjCtrl (cellObjCtrl);
			if (cctrlFwd._TgtObj == null) {
				CeaseAttacking (bth);
				return;
			}
			MiroV1ModelSetting SThis = 
				cellObjCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
			MiroV1ModelSetting SFwd = null;
			if (cctrlFwd._TgtObj != null) {
				cctrlFwd._TgtObj.GetComponent<MiroV1ModelSetting> ();
			}
			bool bNPC = SThis.IsSameCamp (SFwd);
				
			if (bNPC) {
				CeaseAttacking (bth);
				return;
			}
		}

		[ContextMenu("RotateRight")]
		public void RotateRight()
		{
			foreach (Transform bth in _BirthTF) {
				RotRightInTF (bth);
			}
			ConfigAttackers ();
			ConfigAbsorbingSrcTF ();
		}

		public void RotRightInTF (Transform bth)
		{
			CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null || !cellObjCtrl.HasTargetObj ())
				return;
			cellObjCtrl.RotateClockwise ();
			ConfigAttackerForFwdTF (cellObjCtrl);
			CeaseAttackingIfNotPointToEnemy (bth, cellObjCtrl);
		}

		[ContextMenu("RotateLeft")]
		public void RotateLeft()
		{
			foreach (Transform bth in _BirthTF) {
				RotLeftInTF (bth);
			}
			ConfigAttackers ();
			ConfigAbsorbingSrcTF ();
		}

		public void RotLeftInTF (Transform bth)
		{
			CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null || !cellObjCtrl.HasTargetObj ())
				return;
			cellObjCtrl.RotateCounterClockwise ();
			ConfigAttackerForFwdTF (cellObjCtrl);
			CeaseAttackingIfNotPointToEnemy (bth, cellObjCtrl);
		}

		[ContextMenu("MoveFwd")]
		public void MoveFwd()
		{
			foreach (Transform bth in _BirthTF) {
				MoveFwdInTF (bth);
			}
			ConfigAttackers ();
			_MoveFwd.Invoke ();
			//ConfigAbsorbingSrcTF ();

		}

		public void MoveFwdInTF (Transform bth)
		{
			CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null || !cellObjCtrl.HasTargetObj ())
				return;
			cellObjCtrl.MoveFwd ();
			CellObjCtrl fwdCtrl = GetFwdCellObjCtrl (cellObjCtrl);
			CeaseAttackingIfNotPointToEnemy (bth, fwdCtrl);
		}

		[ContextMenu("MoveBwd")]
		public void MoveBwd()
		{
			foreach (Transform bth in _BirthTF) {
				MoveBwdInTF (bth);
			}
			ConfigAttackers ();
			_MoveBwd.Invoke ();
			//ConfigAbsorbingSrcTF ();
		}

		public void MoveBwdInTF (Transform bth)
		{
			CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null || !cellObjCtrl.HasTargetObj ())
				return;
			cellObjCtrl.MoveBwd ();
			CellObjCtrl nxtCCtrl = GetFwdCellObjCtrl (cellObjCtrl);
			ConfigAttackerForFwdTF (nxtCCtrl);
			//GetFwdCellObjCtrl
			CellObjCtrl bwdCtrl = GetBwdCellObjCtrl (cellObjCtrl);
			CeaseAttackingIfNotPointToEnemy (bth, bwdCtrl);
		}



		[ContextMenu("CreateEN2Farm")]
		public void CreateEN2Farm()
		{
			int tfCnt = _BirthTF.Count;

			if (_BirthTF.Count != 2) {
				Debug.Log ("must choose 2 cells with Miros!");
				return;
			}

			MiroModelV1 modelA = GetModelFromCellTF (_BirthTF [0]);
			MiroModelV1 modelB = GetModelFromCellTF (_BirthTF [1]);
			if (modelA == null || modelB == null) {
				return;
			}
			modelA.ShrinkFarm2 ();
			modelB.ShrinkFarm2 ();
				
			CreateEN2FarmFor2 (modelA, _BirthTF [0], modelB, _BirthTF [1]);

		}


		public void CreateEN2FarmFor2 (
			MiroModelV1 modelA, 
			Transform CellTFA,
			MiroModelV1 modelB,
			Transform CellTFB)
		{
			Rigidbody2D rbA, rbB;
			List<Transform> tfs = new List<Transform> ();
			tfs.Add (CellTFA);
			tfs.Add (CellTFB);
			bool bGet = GetRb2DFromTwoObjs (
				out rbA, out rbB, tfs);
			if (!bGet) {
				Debug.Log (
					"not get two Rigidbody2D from two MiroObjs!");
				return;
			}
			GameObject newENFarm = Instantiate (_EN2FarmPrefab, _EN2FarmTFParent) as GameObject;
			Vector3 pos = 0.5f * (rbA.transform.position + rbB.transform.position);
			newENFarm.transform.position = pos;
			MiroV1ENFarmBase enFarm = newENFarm.GetComponent<MiroV1ENFarmBase> ();
			FixedJoint2D jnt0 = enFarm.GetJointA ();
			FixedJoint2D jnt1 = enFarm.GetJointB ();
			jnt0.connectedBody = rbA;
			jnt1.connectedBody = rbB;
			SetRotOfNewObjONTwoAnchorTFs (enFarm.transform, rbA.transform, rbB.transform);
			MiroV1EF2Ctrl en2FarmCtrl = newENFarm.GetComponent<MiroV1EF2Ctrl> ();
			modelA.SetFarm (en2FarmCtrl);
			modelB.SetFarm (en2FarmCtrl);
			en2FarmCtrl.SetModelSetting (modelA.GetComponent<MiroV1ModelSetting> ());

			_EN2FarmCreated.Invoke ();

			_Connections.Add (newENFarm);
		}

		private MiroModelV1 GetModelFromCellTF(Transform tf)
		{
			MiroModelV1 model = null;
			CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
			if (cctrl._TgtObj != null) {
				model = cctrl._TgtObj.GetComponent<MiroModelV1> ();
			}
			return model;
		}

		[ContextMenu("ShrinkEN2Farm")]
		public void ShrinkEN2Farm()
		{
			bool bShrinked = false;
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl._TgtObj == null) {
					return;
				}

				MiroModelV1 model = 
					cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model.ShrinkFarm2 ();
					bShrinked = true;
				}
			}

			if (bShrinked) {
				_EN2FarmShrinked.Invoke ();
			}
		}

		[ContextMenu("CreateEN2Absorber")]
		public void CreateEN2Absorber()
		{
			int tfCnt = _BirthTF.Count;

			if (_BirthTF.Count != 2) {
				Debug.Log ("must choose 2 cells with Miros!");
			}

			Rigidbody2D rbA, rbB;
			bool bGet = GetRb2DFromTwoObjs (out rbA, out rbB, _BirthTF);
			if (!bGet) {
				Debug.Log ("not get two Rigidbody2D from two MiroObjs!");
				return;
			}
			Transform TA = _BirthTF [0];
			Transform TB = _BirthTF [1];

			GameObject newENAbsorb = 
				Instantiate (_EN2AbsorbPrefab, _EN2AbsorbTFParent) as GameObject;
			Vector3 pos = 0.5f * (TA.position + TB.transform.position);
			newENAbsorb.transform.position = pos;

			ConfigAbsorberAnchors (TA, TB, newENAbsorb);

			SetRotOfNewObjONTwoAnchorTFs (
				newENAbsorb.transform, rbA.transform, rbB.transform);

			_Connections.Add (newENAbsorb);
		}

		static void ConfigAbsorberAnchors (
			Transform tfA, 
			Transform tfB, 
			GameObject newENAbsorb)
		{
			MiroV1ENAbsorberBase absorber = newENAbsorb.GetComponent<MiroV1ENAbsorberBase> ();
			Transform akra = absorber.GetAnchorTFA ();
			Transform akrb = absorber.GetAnchorTFB ();
			Lyu.KeepOffset kepA = akra.GetComponent<Lyu.KeepOffset> ();
			Lyu.KeepOffset kepB = akrb.GetComponent<Lyu.KeepOffset> ();
			kepA._Anchor = tfA;
			kepB._Anchor = tfB;
		}

		static void SetRotOfNewObjONTwoAnchorTFs (Transform tfNewObj, Transform tfa, Transform tfb)
		{
			Vector3 DirVec = tfa.position - tfb.position;
			Quaternion Rot = Quaternion.FromToRotation (Vector3.right, DirVec);
			tfNewObj.rotation = Rot;
		}

		static bool GetRb2DFromTwoObjs(
			out Rigidbody2D rbA,
			out Rigidbody2D rbB,
			List<Transform> _ObjTFs)
		{
			rbA = null;
			rbB = null;
			Transform anchorA, anchorB;
			bool bGetAnchors = 
				GetAnchorFromTwoObjs (out anchorA, out anchorB, _ObjTFs );
			if (!bGetAnchors) {
				return false;

			}
			rbA = anchorA.GetComponent<Rigidbody2D> ();
			rbB = anchorB.GetComponent<Rigidbody2D> ();

			return true;
		}

		static bool GetAnchorFromAToBForPump (
			Transform A, Transform B,
			out Transform anchorA,
		  out Transform anchorB,
			out int anchorBDir)
		{
			anchorA = null;
			anchorB = null;
			anchorBDir = -1;

			CellObjCtrl ctrlA = A.GetComponent<CellObjCtrl> ();
			CellObjCtrl ctrlB = B.GetComponent<CellObjCtrl> ();

			if (ctrlA._TgtObj == null || ctrlB._TgtObj == null) {
				return false;
			}

			MiroV1MainBodyAnchors akr = 
				ctrlA._TgtObj.GetComponentInChildren<MiroV1MainBodyAnchors> ();
			MiroV1AidBlackDots adots = 
				ctrlB._TgtObj.GetComponentInChildren<MiroV1AidBlackDots> ();

			int rdir0;
			int rdir1;
			GetFacingDirs (ctrlA, ctrlB, out rdir0, out rdir1);
		
			anchorA = akr.GetAnchor (rdir0, 0);
			anchorB = adots.GetBlackDot (rdir1).transform;

			anchorBDir = rdir1;

			return true;
		}

		static bool GetAnchorFromTwoObjs (
			out Transform anchorA,
			out Transform anchorB,
			List<Transform> _ObjTFs)
		{
			anchorA = null;
			anchorB = null;

			List<CellObjCtrl> cctrls = new List<CellObjCtrl> ();
			List<MiroV1MainBodyAnchors> akrs = new List<MiroV1MainBodyAnchors> ();
			//List<MiroV1AidBlackDots> aidDots = new List<MiroV1AidBlackDots> ();
			for (int i = 0; i < 2; i++) {
				CellObjCtrl cellObjCtrl = 
					_ObjTFs [i].GetComponent<CellObjCtrl> ();
				cctrls.Add (cellObjCtrl);
				if (cellObjCtrl._TgtObj == null) {
					
					return false;
				}
				MiroV1MainBodyAnchors akr = 
					cellObjCtrl._TgtObj.GetComponentInChildren<MiroV1MainBodyAnchors> ();
				if (akr == null) {
					return false;
				}
				akrs.Add (akr);
			}
		
			int rdir0;
			int rdir1;
			GetFacingDirs (cctrls [0], cctrls [1], out rdir0, out rdir1);
			anchorA = akrs [0].GetAnchor (rdir0, 0);
			anchorB = akrs [1].GetAnchor (rdir1, 0);
			//anchorB = aidDots [1].GetBlackDot (rdir1).transform;
			return true;
		}

		static private void GetFacingDirs(
			CellObjCtrl ctrlA, CellObjCtrl ctrlB,
			out int rdir0, out int rdir1)
		{
			//int relativeDir01 = ctrlA.ComputeDirRelativeToAnother (ctrlB);
			HexCoord h0 = ctrlA.GetComponent<HexCoord> ();
			HexCoord h1 = ctrlB.GetComponent<HexCoord> ();
			int hexDist = Hex.Distance (h0._hex, h1._hex);
			Hex h0toh1 = Hex.Subtract (h0._hex, h1._hex);
			Hex h1toh0 = Hex.Subtract (h1._hex, h0._hex);
			int dir01 = Hex.CheckDirectionHex (h0toh1);
			int dir10 = Hex.CheckDirectionHex (h1toh0);
			rdir0 = ctrlA.GetDir() - dir01;
			rdir1 = ctrlB.GetDir() - dir10;
			rdir1 = 3-rdir1;
			rdir0 = (int)Mathf.Repeat (rdir0, 6);
			rdir1 = (int)Mathf.Repeat (rdir1, 6);
		}

		[ContextMenu("CreatePumps")]
		public void CreatePumps()
		{
			foreach (Transform tf in _BirthTF) {
				CreatePumpForTF (tf);
			}
		}

		private void CreatePumpForTF (Transform tf)
		{
			CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
			HexCoord hCoord = tf.GetComponent<HexCoord> ();
			Transform fwdTf = hCoord._Neighbors [cellObjCtrl.GetDir()];
			if (fwdTf == null) {
				return;
			}
			CreatePumpForAB (tf, fwdTf);
		}

		public bool IsPumpGrown(CellObjCtrl ctrl)
		{
			if (ctrl._TgtObj == null) {
				return false;
			}
			MiroModelV1 model = ctrl._TgtObj.GetComponent<MiroModelV1> ();
			bool bHasPump = model.HasPump ();
			if (!bHasPump) {
				return false;
			}

			return model._Pump.IsGrown ();
		}

		public bool IsPumpingRight(Transform TF)
		{
			CellObjCtrl ctrl = TF.GetComponent<CellObjCtrl> ();
			return IsPumpingRight (ctrl);
		}


		public bool IsPumpingRight(CellObjCtrl ctrl)
		{
			CellObjCtrl fwdCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (ctrl, 0);
			Transform TA = ctrl.transform;
			Transform TB = fwdCtrl.transform;
			Transform akrA, akrB;
			int akrBDir = -1;
			bool bGetAnchors = GetAnchorFromAToBForPump (
				TA,TB,out akrA, out akrB, out akrBDir);

			if (!bGetAnchors) {
				return false;
			}

			if (ctrl._TgtObj == null || fwdCtrl._TgtObj == null) {
				return false;
			}

			MiroModelV1 mA = ctrl._TgtObj.GetComponent<MiroModelV1> ();
			//MiroModelV1 mB = fwdCtrl._TgtObj.GetComponent<MiroModelV1> ();

			bool bHasPump = mA.HasPump ();
			if (!bHasPump) {
				return false;
			}

			MiroV1PumpBase pump = mA.GetPump ();
			bool bPumpGrown = pump.IsGrown ();
			if (!bPumpGrown) {
				return false;
			}
			Transform akrB2 = pump.GetAnchorTFB ();
			Lyu.KeepOffset kep = akrB2.GetComponent<Lyu.KeepOffset> ();
			bool bRight = false;
			if (kep != null) {
				bRight = (kep._Anchor == akrB);
			}
			return bRight;

		}

		public void CreatePumpForAB(Transform TA, Transform TB)
		{
			MiroModelV1 modelA = 
				TA.GetComponent<CellObjCtrl> ()._TgtObj.GetComponent<MiroModelV1> ();
			
			Transform akrA, akrB;
			int akrBDir = -1;
			bool bGetAnchors = MiroV1PlacementMgr.GetAnchorFromAToBForPump (
				TA,TB,out akrA, out akrB, out akrBDir);

			if (!bGetAnchors) {
				return;
			}

			GameObject PumpObj = null;
			MiroV1PumpBase pump = modelA.GetPump ();
			if (pump != null) {
				pump.GrowUP ();
				PumpObj = pump.gameObject;
			} else {
				PumpObj = Instantiate (_PumpPrefab) as GameObject;
				pump = PumpObj.GetComponent<MiroV1PumpBase> ();
				modelA.AddPump (pump);
				modelA.UpdateEmittersTrigger ();
			}
			pump.SetAidingRDir (akrBDir);

			Vector3 pos = 0.5f * (akrA.position + akrB.position);
			PumpObj.transform.position = pos;
			MiroV1PumpBase newPump = PumpObj.GetComponent<MiroV1PumpBase>();
			Transform akA = newPump.GetAnchorTFA ();
			Transform akB = newPump.GetAnchorTFB ();
			Lyu.KeepOffset kepA = akA.GetComponent<Lyu.KeepOffset>();
			Lyu.KeepOffset kepB = akB.GetComponent<Lyu.KeepOffset> ();
			kepA._Anchor = akrA.transform;
			kepB._Anchor = akrB.transform;

			_Connections.Add (PumpObj);
		}

		[ContextMenu("ShrinkPumps")]
		public void ShrinkPumps()
		{
			foreach (Transform tf in _BirthTF) {
				ShrinkPumpAt (tf);
			}
		}

		private void ShrinkPumpAt(Transform tf)
		{
			CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl._TgtObj == null) {
				return;
			}

			MiroModelV1 model = 
				cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
			if (model != null) {
				model.ShrinkPump ();
			}
		}

		[ContextMenu("BreakPumps")]
		public void BreakPumps()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl._TgtObj == null) {
					return;
				}

				MiroModelV1 model = 
					cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model.BreakPump ();
				}
			}
		}

		[ContextMenu("RecoverPumps")]
		public void RecoverPumps()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl._TgtObj == null) {
					return;
				}

				MiroModelV1 model = 
					cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model.RecoverPump ();
				}
			}
		}


		[ContextMenu("RetargetPumps")]
		public void RetargetPumps()
		{
			ShrinkPumps ();
			CreatePumps ();
		}

		public void RetargetPumpsFor(Transform TF)
		{
			bool bRight = IsPumpingRight (TF);
			if (bRight) {
				return;
			}

			ShrinkPumpAt (TF);
			CreatePumpForTF (TF);
		}

		[ContextMenu("ChooseAttackTarget")]
		public void ChooseAttackTarget()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl._TgtObj == null) {
					return;
				}

				HexCoord hCoord = tf.GetComponent<HexCoord> ();
				Transform fwdTf = 
					hCoord._Neighbors [cellObjCtrl.GetDir()];
				if (fwdTf == null) {
					continue;
				}

				MiroModelV1 model0 = 
					cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
				CellObjCtrl cellObjctrl2 = 
					fwdTf.GetComponent<CellObjCtrl> ();
				MiroModelV1 model1 = null;
				if(cellObjctrl2._TgtObj!=null)
				{
					model1 = cellObjctrl2._TgtObj.GetComponent<MiroModelV1> ();
				}

				if (model1 == null) {
					Debug.Log ("model1 == null");
					continue;
				}
				model0.SetAttackTarget (model1);
			}
		}

		[ContextMenu("StartAbsorbing")]
		public void StartAbsorbing()
		{
			foreach (Transform tf in _BirthTF) {
				ConfigAbsorbingForTF (tf);
			}
		}

		public static void ConfigAbsorbingForTF (Transform tf)
		{
			CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl._TgtObj == null) {
				return;
			}

			//bool bControlling = 
				CellObjCtrlUtils.IsControllingObj (cellObjCtrl);

			bool bFaceToFace = CellObjCtrlUtils.IsFaceToFace (cellObjCtrl);

			//bool bNbAssisting = CellObjCtrlUtils.IsNbAssistingAT (cellObjCtrl,3);
			//bool bNbAttacking = CellObjCtrlUtils.IsNbAttacking (cellObjCtrl,3);
			bool bNbEnemy = CellObjCtrlUtils.IsNbEnemy (cellObjCtrl, 3);

			CellObjCtrl bwdCellCtrl = CellObjCtrlUtils.GetNbCellObjCtrl (cellObjCtrl, 3);
			MiroModelV1 AbsorberM = CellObjCtrlUtils.GetMiroModelFromCell (cellObjCtrl);
			MiroModelV1 AbsorbeeM = CellObjCtrlUtils.GetMiroModelFromCell (bwdCellCtrl);

			if (bFaceToFace) {
				bool bGrown = AbsorberM._BlackHole.IsGrown ();
				if (!bGrown) {
					AbsorberM._BlackHole.GrowUp();
				}
				AbsorberM.ReleaseAbsorbing ();
				if (bNbEnemy) {
					AbsorberM._BlackHole._bAbsorbing = true;
					AbsorbeeM.AddAbsorber (AbsorberM);
				} else {
					AbsorberM._BlackHole._bAbsorbing = false;
				}
			} else {
				StopAbsorbingForCtrl (cellObjCtrl);
				//bFaceToFace = CellObjCtrlUtils.IsFaceToFace (cellObjCtrl);
				//print ("StopAbsorbingForCtrl:" + cellObjCtrl + "," + cellObjCtrl._TgtObj);
			}

		}

		public void ConfigAbsorbingForCellCtrl(CellObjCtrl ctrl)
		{
			
		}

		[ContextMenu("StopAbsorbing")]
		public void StopAbsorbing()
		{
			foreach (Transform tf in _BirthTF) {
				StopAbsorbingForTF (tf);
			}

		}

		public static void StopAbsorbingForTF (Transform tf)
		{
			CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
			StopAbsorbingForCtrl (cellObjCtrl);
		}

		public static void StopAbsorbingForCtrl (
			CellObjCtrl cellObjCtrl)
		{
			if (cellObjCtrl._TgtObj == null) {
				return;
			}
			MiroModelV1 AbsorberM = cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
			AbsorberM.ReleaseAbsorbing ();
			AbsorberM._BlackHole._bAbsorbing = false;
		}

		public static void ShrinkAbsorberForCtrl(
			CellObjCtrl cellObjCtrl)
		{
			MiroModelV1 AbsorberM = cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
			if (AbsorberM._BlackHole._BlackHole != null && AbsorberM._BlackHole.IsGrown ()) {
				AbsorberM._BlackHole._ShrinkTrigger = true;
			}
		}

		[ContextMenu("CreateRing")]
		public void CreateRing()
		{
			
			List<GameObject> miroObjs = new List<GameObject> ();
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					miroObjs.Add (objCell._TgtObj);
				}
			}

			CreateRing (miroObjs);

		}

		public void CreateRing (List<GameObject> miroObjs)
		{
			GameObject newRingObj = Instantiate (_RingPrefab, _RingParent) as GameObject;
			MiroRing ring = newRingObj.GetComponent<MiroRing> ();
			ring.ClearRingObjTfs ();
			foreach (GameObject gb in miroObjs) {
				ring.AddObjAsRingObj (gb);
			}
			ring.InitRing ();

			_Connections.Add (newRingObj);
		}

		[ContextMenu("ShrinkRing")]
		public void ShrinkRing()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model.ShrinkRing ();
				}
			}
		}

		[ContextMenu("ConfigAttackers")]
		public void ConfigAttackers()
		{
			ConfigAttackers (true);
		}

		[ContextMenu("ConfigAttackersAnyway")]
		public void ConfigAttackersAnyway()
		{
			ConfigAttackers (false);
		}

		private void ConfigAttackers(bool bCheckCamp = true)
		{
			for(int i=0;i<2;i++)
			{
				foreach (Transform tf in _BirthTF) {
					ConfigAttackersFor (tf, bCheckCamp);
				}
			}
		}

		[ContextMenu("ConfigAttackersForEveryMiro")]
		public void ConfigAttackersForEveryMiro()
		{
			
		}

		static void CeaseAttacking(Transform TF)
		{
			CellObjCtrl cctrl = TF.GetComponent<CellObjCtrl> ();
			if (cctrl == null) {
				return;
			}

			if (cctrl._TgtObj == null) {
				return;
			}

			MiroModelV1 model = cctrl._TgtObj.GetComponent<MiroModelV1> ();
			model.CeaseAttacking ();
		}

		//static void 

		public static void ConfigAttackersFor ( Transform tf, bool bCheckCamp)
		{
			CellObjCtrl cellObjCtrl = tf.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl._TgtObj == null) {
				return;
			}
			MiroModelV1 modelThis = cellObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
			if (modelThis == null) {
				return;
			}


			//modelThis.ReleaseAllAttackers ();
			HexCoord hCoord = tf.GetComponent<HexCoord> ();
			for (int dir = 0; dir < 6; dir++) {
				Transform nbTF = hCoord._Neighbors [dir];
				if (nbTF == null) {
					
					continue;
				}
				CellObjCtrl nbObjCtrl = nbTF.GetComponent<CellObjCtrl> ();
				if (nbObjCtrl._TgtObj == null) {
					continue;
				}
				MiroModelV1 nbModel = nbObjCtrl._TgtObj.GetComponent<MiroModelV1> ();
				if (nbModel == null) {
					continue;
				}

				HexCoord nbHCoord = nbTF.GetComponent<HexCoord> ();
				Transform fwdTf = nbHCoord._Neighbors [nbObjCtrl.GetDir()];
				if (fwdTf != tf) {
					//CeaseAttacking (fwdTf);
					continue;
				}

				if (bCheckCamp && IsSameCamp (nbModel, modelThis)) {
					continue;
				}
				//nbModel.CeaseAttacking ();
				nbModel.SetAttackTarget (modelThis);
				nbModel.UpdateAttackTarget ();
			}
		}

		public void SetENMax(float enMax)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model._ENGenerator._ENMax = (int)enMax;
				}
			}
		}

		public void SetEN(float en)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model._ENGenerator._EN = (int)en;
				}
			}
		}

		public void SetENLinkMainWeapon(bool bLink)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model._bTurnMainWeaponByEN = bLink;
				}
			}
		}

		public void SetWeaponAT (int weaponId, float atmax, float at)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && 
					objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model._WeaponSlots [weaponId]._ATMax = (int)atmax;
					model._WeaponSlots [weaponId]._AT = (int)at;
				}
			}
		}

		public void TurnWeaponActive(int weaponId, bool bActive)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl objCell = 
					tf.GetComponent<CellObjCtrl> ();
				if (objCell._TgtObj != null && 
					objCell._TgtObj.GetComponent<MiroModelV1> () != null) {
					MiroModelV1 model = objCell._TgtObj.GetComponent<MiroModelV1> ();
					model._WeaponSlots [weaponId]._Active = bActive;
				}
			}
		}

		static bool IsSameCamp(MiroModelV1 m0, MiroModelV1 m1)
		{
			MiroV1ModelSetting nbModelSet = 
				m0.GetComponent<MiroV1ModelSetting> ();
			MiroV1ModelSetting modelSet = 
				m1.GetComponent<MiroV1ModelSetting> ();

			bool bSame = 
				nbModelSet.IsSameCamp (modelSet);
			return bSame;
		}

		public void AddTargetHexCoord(HexCoord hc)
		{
			_BirthTF.Add(hc.transform);
			InformSelectedMiroObjs ();
		}

		public void RemoveTargetHexCoord(HexCoord hc)
		{
			_BirthTF.Remove (hc.transform);
			InformSelectedMiroObjs ();
		}

		public void InformSelectedMiroObjs()
		{
			List<GameObject> Objs = new List<GameObject> ();
			foreach (Transform bth in _BirthTF) {
				CellObjCtrl cellObjCtrl = bth.GetComponent<CellObjCtrl> ();
				if (cellObjCtrl == null)
					continue;
				Objs.Add (
					cellObjCtrl._TgtObj);
				_SelectMiroObjs.Invoke (Objs);
				
			}
		}
		/*
		public void SelectMiroObj(GameObject mobj)
		{
			_SelectedMObjs.Add (mobj);
			_SelectionChanged.Invoke (_SelectedMObjs);
		}

		public void DeselectMiroObj(GameObject mobj)
		{
			_SelectedMObjs.Remove (mobj);
			_SelectionChanged.Invoke (_SelectedMObjs);
		}*/

		void RecordMiroObjOnBirthObj (GameObject newMiro , Transform birthTF)
		{
			CellObjCtrl cellObjCtrl = birthTF.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null) {
				cellObjCtrl = birthTF.gameObject.AddComponent<CellObjCtrl> ();
			}
			cellObjCtrl.AttachObj (newMiro);
		}

		private bool IsOccupied(Transform birthTF)
		{
			CellObjCtrl cellObjCtrl = birthTF.GetComponent<CellObjCtrl> ();
			if (cellObjCtrl == null)
				return false;

			return cellObjCtrl.HasTargetObj ();
		}

		[ContextMenu("ConfigAbsorbingSrcTF")]
		public void ConfigAbsorbingSrcTF()
		{
			foreach (Transform tf in _BirthTF) {
				ConfigAbsorbingSrcForTF (tf);
			}
		}

		public static void ConfigAbsorbingSrcForTF (Transform tf)
		{
			CellObjCtrl cCtrl = tf.GetComponent<CellObjCtrl> ();
			ConfigAbsorbingSrcForCCtrl (cCtrl);
			int dirFwd = cCtrl.GetDir();
			int dirBwd = (int)Mathf.Repeat ((float)(cCtrl.GetDir() + 3), (float)6);
			HexCoord hc = cCtrl.GetComponent<HexCoord> ();
			if (hc._Neighbors [dirFwd] != null) {
				CellObjCtrl cctrlFwd = hc._Neighbors [dirFwd].GetComponent<CellObjCtrl> ();
				ConfigAbsorbingSrcForCCtrl (cctrlFwd);
			}
			if (hc._Neighbors [dirBwd] != null) {
				CellObjCtrl cctrlBwd = hc._Neighbors [dirBwd].GetComponent<CellObjCtrl> ();
				ConfigAbsorbingSrcForCCtrl (cctrlBwd);
			}
		}

		public void SetMiroPrefabId(float id)
		{
			_MiroPrefID = (int)id;
		}

		[ContextMenu("Recover All BlackDots")]
		public void RecoverAllBlackDots()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model.RecoverAllBlackDots ();
				}

			}
				
		}

		[ContextMenu("GrowFwdAnteller")]
		public void GrowFwdAnteller()
		{
			GrowAnteller (0);
		}

		[ContextMenu("ShrinkFwdAnteller")]
		public void ShrinkFwdAnteller()
		{
			ShrinkAnteller (0);
		}

		[ContextMenu("BreakFwdAnteller")]
		public void BreakFwdAnteller()
		{
			BreakAnteller (0);
		}

		[ContextMenu("GrowBwdAnteller")]
		public void GrowBwdAnteller()
		{
			GrowAnteller (1);
		}

		[ContextMenu("ShrinkBwdAnteller")]
		public void ShrinkBwdAnteller()
		{
			ShrinkAnteller (1);
		}

		[ContextMenu("BreakBwdAnteller")]
		public void BreakBwdAnteller()
		{
			BreakAnteller (1);
		}

		[ContextMenu("GrowUPAbsorbers")]
		public void GrowUpAbsorbers()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._BlackHole._GrowUpTrigger = true;
				}
			}	
		}

		[ContextMenu("ShrinkAbsorbers")]
		public void ShrinkAbsorbers()
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._BlackHole._ShrinkTrigger = true;
				}
			}
		}

		[ContextMenu("TurnONAbsorbing")]
		public void TurnONAbsorbing()
		{
			TurnAbsorbing (true);
		}
			
		[ContextMenu("TurnOFFAbsorbing")]
		public void TurnOFFAbsorbing()
		{
			TurnAbsorbing (false);
		}

		public void TurnAbsorbing(bool bON)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._BlackHole._bAbsorbing = bON;
				}
			}
		}


		public void GrowUpBDotsAll()
		{
			for (int i = 0; i < 14; i++) {
				GrowUpBDot (i);
			}
		}

	

		public void GrowUpBDotCore()
		{
			GrowUpBDot (0);
		}

		public void GrowUpBDotAntellerFwd()
		{
			GrowUpBDot (1);
		}

		public void GrowUpBDotAntellerBwd()
		{
			GrowUpBDot (2);
		}

		public void GrowUpBDotFwd()
		{
			GrowUpBDot (3);
		}

		public void GrowUpBDotFR()
		{
			GrowUpBDot (4);
		}

		public void GrowUpBDotBR()
		{
			GrowUpBDot (5);
		}

		public void GrowUpBDotB()
		{
			GrowUpBDot (6);
		}

		public void GrowUpBDotBL()
		{
			GrowUpBDot (7);
		}
		public void GrowUpBDotFL()
		{
			GrowUpBDot (8);
		}

		public void GrowUpBDotRingBall0()
		{
			GrowUpBDot (9);
		}

		public void GrowUpBDotRingBall1()
		{
			GrowUpBDot (10);
		}

		public void GrowUpBDotRingBall2()
		{
			GrowUpBDot (11);
		}

		public void GrowUpBDotRingBall3()
		{
			GrowUpBDot (12);
		}

		public void GrowUpBDotRingBall4()
		{
			GrowUpBDot (13);
		}

		public void ShrinkBDotCore()
		{
			ShrinkBDot (0);
		}

		public void ShrinkBDotAntellerFwd()
		{
			ShrinkBDot (1);
		}

		public void ShrinkBDotAntellerBwd()
		{
			ShrinkBDot (2);
		}

		public void ShrinkBDotFwd()
		{
			ShrinkBDot (3);
		}

		public void ShrinkBDotFR()
		{
			ShrinkBDot (4);
		}

		public void ShrinkBDotBR()
		{
			ShrinkBDot (5);
		}

		public void ShrinkBDotB()
		{
			ShrinkBDot (6);
		}

		public void ShrinkBDotBL()
		{
			ShrinkBDot (7);
		}
		public void ShrinkBDotFL()
		{
			ShrinkBDot (8);
		}

		public void ShrinkBDotRingBall0()
		{
			ShrinkBDot (9);
		}

		public void ShrinkBDotRingBall1()
		{
			ShrinkBDot (10);
		}

		public void ShrinkBDotRingBall2()
		{
			ShrinkBDot (11);
		}

		public void ShrinkBDotRingBall3()
		{
			ShrinkBDot (12);
		}

		public void ShrinkBDotRingBall4()
		{
			ShrinkBDot (13);
		}

		public void BreakBDotCore()
		{
			BreakBDot (0);
		}

		public void BreakBDotAntellerFwd()
		{
			BreakBDot (1);
		}

		public void BreakBDotAntellerBwd()
		{
			BreakBDot (2);
		}

		public void BreakBDotFwd()
		{
			BreakBDot (3);
		}

		public void BreakBDotFR()
		{
			BreakBDot (4);
		}

		public void BreakBDotBR()
		{
			BreakBDot (5);
		}

		public void BreakBDotB()
		{
			BreakBDot (6);
		}

		public void BreakBDotBL()
		{
			BreakBDot (7);
		}
		public void BreakBDotFL()
		{
			BreakBDot (8);
		}

		public void BreakBDotRingBall0()
		{
			BreakBDot (9);
		}

		public void BreakBDotRingBall1()
		{
			BreakBDot (10);
		}

		public void BreakBDotRingBall2()
		{
			BreakBDot (11);
		}

		public void BreakBDotRingBall3()
		{
			BreakBDot (12);
		}

		public void BreakBDotRingBall4()
		{
			BreakBDot (13);
		}

		public void RecoverBDotsAll()
		{
			for (int i = 0; i < 14; i++) {
				RecoverBDot (i);
			}
		}

		public void RecoverBDotCore()
		{
			RecoverBDot (0);
		}

		public void RecoverBDotAntellerFwd()
		{
			RecoverBDot (1);
		}

		public void RecoverBDotAntellerBwd()
		{
			RecoverBDot (2);
		}

		public void RecoverBDotFwd()
		{
			RecoverBDot (3);
		}

		public void RecoverBDotFR()
		{
			RecoverBDot (4);
		}

		public void RecoverBDotBR()
		{
			RecoverBDot (5);
		}

		public void RecoverBDotB()
		{
			RecoverBDot (6);
		}

		public void RecoverBDotBL()
		{
			RecoverBDot (7);
		}
		public void RecoverBDotFL()
		{
			RecoverBDot (8);
		}

		public void RecoverBDotRingBall0()
		{
			RecoverBDot (9);
		}

		public void RecoverBDotRingBall1()
		{
			RecoverBDot (10);
		}

		public void RecoverBDotRingBall2()
		{
			RecoverBDot (11);
		}

		public void RecoverBDotRingBall3()
		{
			RecoverBDot (12);
		}

		public void RecoverBDotRingBall4()
		{
			RecoverBDot (13);
		}

		private void GrowUpBDot(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null && model._BlackDots.Count>id) {
					model._BlackDots [id]._GrowTrigger = true;
					model._BlackDots [id]._RecoverTrigger = true;
				}
			}
		}

		private void ShrinkBDot(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null && model._BlackDots.Count>id) {
					model._BlackDots [id]._ShrinkTrigger = true;
				}
			}
		}

		private void BreakBDot(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null && model._BlackDots.Count>id) {
					model._BlackDots [id]._BreakTrigger = true;
				}
			}
		}

		private void RecoverBDot(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null && model._BlackDots.Count>id) {
					model._BlackDots [id]._RecoverTrigger = true;
				}
			}
		}

		private void GrowAnteller(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._Antellers [id]._GrowUpTrigger = true;
				}
			}
		}

		private void ShrinkAnteller(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._Antellers [id]._ShrinkTrigger = true;
				}
			}
		}

		private void BreakAnteller(int id)
		{
			foreach (Transform tf in _BirthTF) {
				CellObjCtrl cctrl = tf.GetComponent<CellObjCtrl> ();
				if(cctrl._TgtObj==null)
				{
					return;
				}

				MiroModelV1 model = 
					cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					model._Antellers [id]._ScatterTrigger = true;
				}
			}
		}


		public static void ConfigAbsorbingSrcForCCtrl (CellObjCtrl cCtrl)
		{
			int dirFwd = cCtrl.GetDir();
			int dirBwd = (int)Mathf.Repeat ((float)(cCtrl.GetDir() + 3), (float)6);
			if (cCtrl._TgtObj == null) {
				return;
			}
			MiroModelV1 model = cCtrl._TgtObj.GetComponent<MiroModelV1> ();
			HexCoord hc = cCtrl.GetComponent<HexCoord> ();
			Transform tfFwd = hc._Neighbors [dirFwd];
			Transform tfBwd = hc._Neighbors [dirBwd];
			model.SetFwdAbsorbingSrcTF (tfFwd);
			model.SetBwdAbsorbingSrcTF (tfBwd);
		}

		[ContextMenu("IncMovePower")]
		public void IncMovePower()
		{
			foreach (Transform bth in _BirthTF) {
				CellObjCtrl ctrl = bth.GetComponent<CellObjCtrl> ();
				ctrl.IncMovePwr ();
			}
		}
			
		[ContextMenu("ClearMovePower")]
		public void ClearMovePower()
		{
			foreach (Transform bth in _BirthTF) {
				CellObjCtrl ctrl = bth.GetComponent<CellObjCtrl> ();
				ctrl.ClearMovePwr ();
			}
		}

		[ContextMenu("DeleteHP0Miros")]
		public void DeleteHP0Miros()
		{
			foreach (GameObject gb in _MiroObjs) {
				MiroModelV1 model = gb.GetComponent<MiroModelV1> ();
				int hp = model.GetHP ();
				if (hp <= 0) {
					model.Die ();
				}
			}
		}

		[ContextMenu("DeleteMaxHurtMiros")]
		public void DeleteMaxHurtMiros()
		{
			foreach (GameObject gb in _MiroObjs) {
				MiroModelV1 model = gb.GetComponent<MiroModelV1> ();
				int mhp = model.GetMaxHP ();
				int atkrCnt = model.GetAttackersCount ();
				if (atkrCnt >= mhp) {
					model.Die ();
				}
			}
		}



	}
}
