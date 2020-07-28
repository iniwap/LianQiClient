using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Anteller : MiroV1AntellerBase {

		public Animator _AnimMain;
		public KeepAbsorbingPSPointToTF _absorbingPSDirCtrl;
		public Transform _TFAbsorbingSrc;
		private Vector3 LocPos;
		private bool _bGrown = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_TFAbsorbingSrc != null) {
				LocPos = transform.InverseTransformPoint(
					_TFAbsorbingSrc.position);
			} 
			_absorbingPSDirCtrl.transform.localPosition = LocPos;
		}

		override public void SetSrcTF(Transform TFSrc)
		{
			_TFAbsorbingSrc = TFSrc;
		}

		[ContextMenu("GrowUp")]
		override public void GrowUp()
		{
			_AnimMain.SetInteger ("HP", 1);
			_AnimMain.SetTrigger ("Grow");
			_bGrown = true;
		}

		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			_AnimMain.SetInteger ("HP", 0);
			_AnimMain.SetTrigger ("Shrink");
			_bGrown = false;
		}

		[ContextMenu("Scatter")]
		override public void Scatter()
		{
			_AnimMain.SetInteger ("HP", 0);
			_AnimMain.SetTrigger ("Break");
		}

		public Lyu.LineRendererPathCtrl _lrPthCtrl;
		public Lyu.LRPathCtrlFromVts _lrPthFromVts;
		public Lyu.LineCoordMgr _lrCoordMgr;

		override public void TurnDynamics(bool bON)
		{
			if (bON) {
				_AnimMain.StartPlayback ();
			} else {
				_AnimMain.StopPlayback ();
			}

			_lrPthCtrl.enabled = bON;
			_lrPthFromVts.enabled = bON;

			_lrCoordMgr.TurnEnable (bON);

		}

		override public bool IsGrown()
		{
			return _bGrown;
		}




	}
}
