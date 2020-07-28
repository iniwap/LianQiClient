using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lyu
{
	public class MorphAnimDynamicCtrl : MonoBehaviour {
		
		public Animator _MorphingAnim;
		public MiroV1.MiroLocalNoisePosCtrl _noisePosCtrl;

		public bool _bControlAnim = true;
		public bool _bControlLRDynamics = true;
		public bool _bControlNoise = false;

		LRFollower[] lrFollowers ;
		LRPathCtrlFromVts[] lrPthCtrlFromVts;
		LineRendererPathCtrl[] lrPthCtrls;

		// Use this for initialization
		void Start () {
			
		}

		public void TurnDynamics(bool bON)
		{
			
			if (_bControlAnim) {
				if (bON) {			
					_MorphingAnim.StartPlayback ();
				} else {
					_MorphingAnim.StopPlayback ();
				}
			}

			if (_bControlNoise) {
				if (bON) {
					_noisePosCtrl.TurnON ();
				} else {
					_noisePosCtrl.TurnOFF ();
				}
			}

			if (_bControlLRDynamics) {
				GetDynamicObjs ();
				foreach (var lrf in lrFollowers) {
					lrf.enabled = bON;
				}
				foreach (var lrpc in lrPthCtrls) {
					lrpc.enabled = bON;
				}
				foreach (var lrpcVts in lrPthCtrlFromVts) {
					lrpcVts.enabled = bON;
				}
			}

		}


		void GetDynamicObjs ()
		{
			lrFollowers = GetComponentsInChildren<LRFollower> ();
			lrPthCtrlFromVts = GetComponentsInChildren<LRPathCtrlFromVts> ();
			lrPthCtrls = GetComponentsInChildren<LineRendererPathCtrl> ();
		}
	}
}
