using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lyu;

namespace MiroV1
{
	public class CellObjCtrl : MonoBehaviour {

		public GameObject _TgtObj;

		private int _Dir = 0;

		private bool _bBlocked = false;

		public bool _bExhaustMovePwr = true;
		public bool _bExhaustRotPwr = true;

		public UnityEvent _CheckEdge;
		//public UnityEvent _TgtObjChanged;



		public List<GameObject> _6DirLinks = new List<GameObject> ();


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		public bool IsBlocked()
		{
			return _bBlocked;
		}

		public void SetBlock(bool block)
		{
			_bBlocked = block;
		}

		public bool HasTargetObj()
		{
			return (_TgtObj != null);
		}

		public void AttachObj(GameObject obj)
		{
			_TgtObj = obj;
			Invoke ("InvokeTgtObjChanged", 0.3f);
//			InvokeTgtObjChanged ();
		}

		public void DieTgtObj()
		{
			MiroModelV1 model = _TgtObj.GetComponent<MiroModelV1> ();
			if (model != null) {
				model.Die ();
				_TgtObj = null;
				//Destroy (_TgtObj);
			}
			Invoke ("InvokeTgtObjChanged", 1.0f);
		}

		public bool DisappearTgtObj()
		{
			if (_TgtObj == null) {
				return false;
			}
			Destroy (_TgtObj);
			_TgtObj = null;
			Invoke ("InvokeTgtObjChanged", 1.0f);
			return true;
		}

		public HexGridCtrl _gridCtrl;
		private void InvokeTgtObjChanged()
		{
			_gridCtrl.PlacementChanged(this);
		}

		[ContextMenu("RotateClockwise")]
		public void RotateClockwise(bool bUseRotPwr = false)
		{
			_Dir++;
			_Dir = (int)Mathf.Repeat (_Dir,  6);

			bool bChangeDir = true;
			if (bUseRotPwr) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					int rotRPwr = model.GetRotRightPwr ();
					bChangeDir = (rotRPwr > 0);
				}
			}
			if (bChangeDir) {
				ChangeDir ();
				ExhaustRotRightPower ();
			}

		}

		void ExhaustRotRightPower ()
		{
			if (_bExhaustRotPwr) {
				MiroModelV1 model = 
					CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					model.AddRotRightPwr (-1);
				}
			}
		}

		[ContextMenu("RotateCounterClockwise")]
		public void RotateCounterClockwise(bool bUseRotPwr = false)
		{
			_Dir--;
			_Dir = (int)Mathf.Repeat (_Dir,  6);

			bool bChangeDir = true;
			if (bUseRotPwr) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					int rotLPwr = model.GetRotLeftPwr ();
					bChangeDir = (rotLPwr > 0);
				}
			}
			if (bChangeDir) {
				ChangeDir ();
				ExhaustRotLeftPower ();
			}


		}

		void ExhaustRotLeftPower ()
		{
			if (_bExhaustRotPwr) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					model.AddRotLeftPwr (-1);
				}
			}
		}

		public void ChangeDir()
		{
			//print ("ChangeDir at: " + gameObject);
			PointToCurrentDir ();
			InvokeTgtObjChanged ();
		}

		[ContextMenu("PointToCurrentDir")]
		public void PointToCurrentDir()
		{
			if (_TgtObj == null) {
				return;
			}
			HexCoord hc = GetComponent<HexCoord> ();
			if (hc._Neighbors [_Dir] != null) {
				LerpLookAt lerplookat = GetLerpLookAtFromObj (_TgtObj);
				lerplookat.enabled = true;
				lerplookat._Tgt = hc._Neighbors [_Dir].transform;
			}
		}

		private LerpLookAt GetLerpLookAtFromObj(GameObject obj)
		{
			LerpLookAt lerplookat = obj.GetComponent<LerpLookAt> ();
			if (lerplookat == null) {
				lerplookat = obj.AddComponent<LerpLookAt> ();
			}
			return lerplookat;
		}

		[ContextMenu("MoveFwd")]
		public void MoveFwd(bool bUseMovePwr = false)
		{
			bool bMove = true;
			if (bUseMovePwr) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					int movePwr = model.GetMovePwr ();
					bMove = (movePwr > 0);
				}
			}
			if (bMove && CanMove(_Dir)) {
				ExhaustMovePower ();
				MoveInDir (_Dir);
			}

		}

		public bool TeleportTo(CellObjCtrl otherCtrl, int dir = -1)
		{
			if (otherCtrl == null || 
				otherCtrl ==this || 
				otherCtrl._TgtObj != null ) {
				return false;
			}

			LerpMoveTo lerpMoveTo = _TgtObj.GetComponent<LerpMoveTo> ();
			if (lerpMoveTo == null) {
				lerpMoveTo = _TgtObj.AddComponent<LerpMoveTo> ();
			}
			lerpMoveTo.enabled = true;

			Transform tf = otherCtrl.transform;
			lerpMoveTo._TgtTF = tf;

			otherCtrl.SetTargetObj (_TgtObj);
			_TgtObj = null;

			if (dir < 0) {
				otherCtrl.SetDir (_Dir);
			} else {
				otherCtrl.SetDir (dir);
			}
			otherCtrl.PointToCurrentDir ();

			InvokeTgtObjChanged ();
			return true;
		}

		void ExhaustMovePower ()
		{
			if (_bExhaustMovePwr) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (this);
				if (model != null) {
					model.AddMovePwr (-1);
					print ("model.AddMovePwr (-1);");
				}
			}
		}

		[ContextMenu("MoveBwd")]
		public void MoveBwd()
		{
			int dir = (int)Mathf.Repeat (_Dir+3, 6);
			MoveInDir (dir);
		}

		public bool CanMove(int dir)
		{
			if (_TgtObj == null) {
				return false;
			}

			HexCoord hc = GetComponent<HexCoord> ();
			Transform tf = hc._Neighbors[dir];
			if (tf == null) {
				return false;
			}

			CellObjCtrl nxtCellObjCtrl = 
				tf.gameObject.GetComponent<CellObjCtrl> ();
			if (nxtCellObjCtrl._TgtObj != null || 
				nxtCellObjCtrl._bBlocked) {
				return false;
			}

			return true;
		}

		public void MoveInDir(int dir)
		{
			/*
			if (_TgtObj == null) {
				return;
			}

			HexCoord hc = GetComponent<HexCoord> ();
			Transform tf = hc._Neighbors[dir];
			if (tf == null) {
				return;
			}

			CellObjCtrl nxtCellObjCtrl = 
				tf.gameObject.GetComponent<CellObjCtrl> ();
			if (nxtCellObjCtrl._TgtObj != null || 
				nxtCellObjCtrl._bBlocked) {
				return;
			}*/

			bool bCanMove = CanMove (_Dir);
			if (!bCanMove)
				return;
			
			CellObjCtrl nxtCellObjCtrl = 
				CellObjCtrlUtils.GetNbCellObjCtrl (this,0);
			
			LerpMoveTo lerpMoveTo = _TgtObj.GetComponent<LerpMoveTo> ();
			if (lerpMoveTo == null) {
				lerpMoveTo = _TgtObj.AddComponent<LerpMoveTo> ();
			}
			lerpMoveTo.enabled = true;

			Transform tf = nxtCellObjCtrl.transform;
			lerpMoveTo._TgtTF = tf;

			nxtCellObjCtrl.SetTargetObj (_TgtObj);
			_TgtObj = null;


			nxtCellObjCtrl.SetDir (_Dir);
			nxtCellObjCtrl.PointToCurrentDir ();

			InvokeTgtObjChanged ();
		}

		public void SetDir(int dir)
		{
			//print ("Set Dir as " + dir);
			if (dir != _Dir) {
				InvokeTgtObjChanged ();
			}
			_Dir = dir;
		}

		public void SetTargetObj(GameObject tgtObj)
		{
			InvokeTgtObjChanged ();
			_TgtObj = tgtObj;
		}

		public int ComputeDirRelativeToAnother(CellObjCtrl other)
		{
			int rdir = _Dir - other._Dir;
			rdir = (int)Mathf.Repeat (rdir,  6);
			return rdir;
		}

		public void TurnONHighlightHPDist()
		{
			if (_TgtObj != null) {
				_TgtObj.SendMessage ("TurnONHighlightHPDist");
				/*
				MiroModelV1 model = 
					_TgtObj.GetComponent<MiroModelV1> ();
				model.TurnONHighlightHPDist ();*/
			}
		}


		public void CheckEdgeToBlock()
		{
			HexCoord hc = GetComponent<HexCoord> ();
			if (hc == null) {
				return;
			}
			bool bEdge = false;
			for (int i = 0; i < 6; i++) {
				bEdge = (hc._Neighbors [i] == null);
				if (bEdge) {
					break;
				}
			}

			_bBlocked = bEdge;
			_CheckEdge.Invoke ();
		}

		public int GetFwdDir()
		{
			return _Dir;
		}

		public int GetBwdDir()
		{
			return (int)Mathf.Repeat ((float)_Dir+3.0f, 6.0f);
		}


		public int GetMovePwr()
		{
			if (_TgtObj != null) {
				MiroModelV1 model = 
					CellObjCtrlUtils.GetMiroModelFromCell (this);
				int movePwr = model.GetMovePwr ();
				return movePwr;
			} else {
				return -1;
			}
		}
		public void IncMovePwr()
		{
			if (_TgtObj != null) {
				MiroModelV1 model = 
					CellObjCtrlUtils.GetMiroModelFromCell (this);
				model.MovePwrInc1 ();
				Debug.Log (model + " inc move power");
			}
		}

		public void ClearMovePwr()
		{
			if (_TgtObj != null) {
				MiroModelV1 model = 
					CellObjCtrlUtils.GetMiroModelFromCell (this);
				model.ClearMovePwr ();
			}
		}

		public void TurnToDir(int dir)
		{
			_Dir = (int)Mathf.Repeat((float)dir,6.0f);
			PointToCurrentDir ();
		}

		public int GetDir()
		{
			return _Dir;
		}

	}
}
