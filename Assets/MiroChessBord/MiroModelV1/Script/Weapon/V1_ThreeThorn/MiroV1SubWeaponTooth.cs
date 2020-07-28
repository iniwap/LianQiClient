using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1SubWeaponTooth : MonoBehaviour {

		[Range(0,3)]
		public int _AT = 0;
		//private int _ATPrev = 0;

		public Animator _AnimBody,_AnimENCore;
		public List<MiroV1AimRb2Emitter> _Emitters = new List<MiroV1AimRb2Emitter>();
		public LineRenderer _LineRenderer;
		public AnimationCurve _SatOnAT;

		// Use this for initialization
		void Start () {
			//_ATPrev = _AT;

			SetLRColor ();
		}
		
		// Update is called once per frame
		void Update () {

			_AnimBody.SetInteger ("Attack", _AT);
			_AnimENCore.SetInteger ("Attack", _AT);

			TurnEmittersByAT ();

			float sat = 
				_SatOnAT.Evaluate ((float)_AT);
			MiroV1Utils.SetLineRendererSaturation (_LineRenderer, sat);

			TurnOffAttackingOnAllEmitted ();


			//_ATPrev = _AT;
		}

		void TurnOffAttackingOnAllEmitted ()
		{
			List<MiroV1AimRb2Emitter> onEms = new List<MiroV1AimRb2Emitter> ();
			foreach (MiroV1AimRb2Emitter em in _Emitters) {
				if (em.IsON ()) {
					onEms.Add (em);
				}
			}
			bool bHas = true;
			foreach (MiroV1AimRb2Emitter em in onEms) {
				bHas = bHas && em.HasEmitEN ();
			}
			if (bHas) {
				_AnimBody.SetBool ("Attacking", false);
				_AnimENCore.SetBool ("Attacking", false);
			}
		}

		public void TurnEmittersByAT ()
		{
			for (int i = 0; i < _Emitters.Count; i++) {
				if (i < _AT) {
					_Emitters [i].TurnON ();
				}
				else {
					_Emitters [i].TurnOFF ();
				}
			}
		}

		[ContextMenu("Shrink")]
		public void Shrink()
		{
			_AnimBody.SetTrigger ("Shrink");
			_AnimENCore.SetTrigger ("Shrink");
		}

		[ContextMenu("Scatter")]
		public void Scatter()
		{
			_AnimBody.SetTrigger ("Scatter");
			_AnimENCore.SetTrigger ("Scatter");
		}

		[ContextMenu("Recover")]
		public void Recover()
		{
			_AnimBody.SetTrigger ("Recover");
			_AnimENCore.SetTrigger ("Recover");
		}

		void SetLRColor ()
		{
			MiroV1ModelSetting modelSetting = GetComponentInParent<MiroV1ModelSetting> ();
			_LineRenderer.startColor = modelSetting._colorSetting._ENMax;
			_LineRenderer.endColor = modelSetting._colorSetting._ENMax;
		}



		public void TurnDynamics(bool bON)
		{
			if (bON) {
				_AnimBody.StartPlayback ();
				_AnimENCore.StartPlayback ();
			} else {
				_AnimBody.StopPlayback ();
				_AnimENCore.StopPlayback ();
			}
				
			Lyu.KeepOffset _kepOffset = GetComponent<Lyu.KeepOffset> ();
			if (_kepOffset != null) {
				_kepOffset.enabled = bON;
			}

			Lyu.GetPosOnLRPathCtrol getPoser = GetComponent<Lyu.GetPosOnLRPathCtrol> ();
			getPoser.enabled = bON;

			KeepPointTo dirKeeper = GetComponent<KeepPointTo> ();
			dirKeeper.enabled = bON;

			Lyu.LineRendererPathCtrl[] lrPthCtrls = 
				GetComponentsInChildren<Lyu.LineRendererPathCtrl> ();
			foreach (var pthCtrl in lrPthCtrls) {
				pthCtrl.enabled = bON;
			}
		}




	}
}
