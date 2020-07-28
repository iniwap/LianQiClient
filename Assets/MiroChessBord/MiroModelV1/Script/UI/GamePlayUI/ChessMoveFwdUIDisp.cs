using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ChessMoveFwdUIDisp : MonoBehaviour {
		public ChessMoveFwdUI _movFwdUI;
		public CellObjCtrl _cctrl;

		public bool _bUpdatingDisp = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bUpdatingDisp) {
				UpdateDisp ();
			}
			UpdateRotation ();
		}

		public void UpdateDisp()
		{
			bool bShow = _movFwdUI._bON;
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			sr.enabled = bShow;


			//UpdateRotByCellDir ();

		}

		public void UpdateRotation()
		{
			if (_movFwdUI._bON) {
				MiroModelV1 model = 
					CellObjCtrlUtils.GetMiroModelFromCell (_cctrl);
				if (model != null) {
					transform.rotation = model.transform.rotation;
				}
			}
		}

		void UpdateRotByCellDir ()
		{
			Vector3 dirVec = CellObjCtrlUtils.GetDirVector3 (_cctrl);
			Quaternion rot = Quaternion.FromToRotation (Vector3.right, dirVec);
			transform.rotation = rot;
		}
	}
}
