using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Thorn : MonoBehaviour {

		[Range(0,5)]
		public int _AT = 0,_ATMax =1;
		private int _ATPrev, _ATMaxPrev;

		public AnimationCurve _AlphaOnATN;
		public LineRenderer _LineRenderer;

		public Animator _AnimMain;
		public Lyu.KeepPosAlongLRPath _MaxLenthCtrl;
		public AnimationCurve _MaxLenOnATMax;

		public List<MiroV1AimRb2Emitter> _Emitters = 
			new List<MiroV1AimRb2Emitter>();

		// Use this for initialization
		void Start () {
			_ATPrev = _AT;
			_ATMaxPrev = _ATMax;
		}
		
		// Update is called once per frame
		void Update () {
			_AT = Mathf.Clamp (_AT, 0, _ATMax);

			PlayAnimONAttackChange ();
			UpdateLineRendererAlphaByATN ();
			CheckAndRecover ();
			UpdateMaxLength ();
			if (_AT != _ATPrev) {
				TurnEmittersByAT ();
			}

			_ATPrev = _AT;
			_ATMaxPrev = _ATMax;
		}

		public void TurnEmittersByAT ()
		{
			for (int i = 0; i < _Emitters.Count; i++) {
				if (i < _AT) {
					_Emitters [i].TurnON ();
				} else {
					_Emitters [i].TurnOFF ();
				}
			}
		}

		void PlayAnimONAttackChange ()
		{
			_AnimMain.SetInteger ("Attack", _ATMax);
			//_AnimMain.SetTrigger ("StartAttack");
		}

		void CheckAndRecover ()
		{
			if (_ATMax != _ATMaxPrev) {
				_AnimMain.SetTrigger ("Recover");
			}
		}

		void UpdateMaxLength ()
		{
			float maxLen = _MaxLenOnATMax.Evaluate ((float)_ATMax);
			_MaxLenthCtrl._Dist = maxLen;
		}

		void UpdateLineRendererAlphaByATN ()
		{
			float atn = (float)_AT / ((float)_ATMax+0.01f);
			float alpha = _AlphaOnATN.Evaluate (atn);
			SetLineRendererAlpha (alpha);
		}

		void SetLineRendererAlpha (float alpha)
		{
			Color crs = _LineRenderer.startColor;
			Color cre = _LineRenderer.endColor;
			crs.a = alpha;
			cre.a = alpha;
			_LineRenderer.startColor = crs;
			_LineRenderer.endColor = cre;
		}

		[ContextMenu("Shrink")]
		public void Shrink()
		{
			_AnimMain.SetTrigger ("Shrink");
			_AT = 0;
			_ATMax = 0;
		}

		[ContextMenu("Scatter")]
		public void Scatter()
		{
			_AnimMain.SetTrigger ("Scatter");
			_AT = 0;
			_ATMax = 0;
		}

		public Lyu.LineRendererPathCtrl _lrPthCtrl;
		public Lyu.LRPathCtrlFromVts _lrPthFromVts;
		public Lyu.KeepPosAlongLRPath [] _keepPosOnLRPths;

		public void TurnDynamics(bool bON)
		{
			_lrPthCtrl.enabled = bON;
			_lrPthFromVts.enabled = bON;
			foreach (var keeper in _keepPosOnLRPths) {
				keeper.enabled = bON;
			}
		}

		[ContextMenu("GetChildKeepPosAlongLRPaths")]
		public void GetChildKeepPosAlongLRPaths()
		{
			_keepPosOnLRPths = 
				GetComponentsInChildren<Lyu.KeepPosAlongLRPath> ();
		}

	}
}
