using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class ChessMoveFwdUI : MonoBehaviour {
		public MiroV1PlacementMgr _Mgr;
		public CellObjCtrl _CCtrl;
		public bool _bON = false;
		private bool _bONPrev = false;
		public UnityEvent _bONChanged;
		public UnityEvent _MoveFwd;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			MiroModelV1 model = 
				CellObjCtrlUtils.GetMiroModelFromCell (_CCtrl);
			if (model == null) {
				_bON = false;
			} else {
				int movePwr = model.GetMovePwr ();
				_bON = (movePwr > 0);
			}

			CircleCollider2D cld = GetComponent<CircleCollider2D> ();
			cld.enabled = _bON;

			if (_bONPrev != _bON) {
				_bONChanged.Invoke ();
				_bONPrev = _bON;
			}


		}

		public void OnMouseDown()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (_bON) {
				_CCtrl.MoveFwd ();
				_MoveFwd.Invoke ();
			}
		}

		public bool IsON()
		{
			return _bON;
		}


	}
}
