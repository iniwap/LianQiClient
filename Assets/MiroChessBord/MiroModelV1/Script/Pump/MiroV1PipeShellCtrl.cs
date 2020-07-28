using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeShellCtrl : MonoBehaviour {

		[System.Serializable]
		public class WdCurveState
		{
			public AnimationCurve _WdCurve;
		}
			
		public List<WdCurveState> _WCStates = 
			new List<WdCurveState> ();
		public int _WCStateIdInt = 0;
		public float _LerpSpd = 1.0f;
		public float _WCStateId = 0;
		public float _WdStart = 1.0f, _WdEnd = 1.0f;

		private float _WdStartPrev,_WdEndPrev;

		public int _TangentMode = 1;
		public float _Smoothness = 1.0f;

		private List<Vector2> _Vertices = new List<Vector2> ();
		private TraceCurve _Trace = new TraceCurve();

		[Range(0.0f,1.0f)]
		public float _t = 0.0f;
		private float _tPrev = 0.0f;

		public AnimationCurve _WdCurve;

		// Use this for initialization
		void Start () {
			GenerateTrace ();
			UpdagteWCurveByInterpState ();

			_WdStartPrev = _WdStart;
			_WdEndPrev = _WdEnd;
		}
		
		// Update is called once per frame
		void Update () {
			UpdateWDCurveOnChange ();
			UpdateLROnChange ();
		}

		void UpdateLROnChange ()
		{
			bool bUpdateLR = (_t != _tPrev) || (_WdStart != _WdStartPrev) || (_WdEnd != _WdEndPrev);
			if (bUpdateLR) {
				UpdateLineRendererWidthCurve ();
			}
			_WdStartPrev = _WdStart;
			_WdEndPrev = _WdEnd;
			_tPrev = _t;
		}

		void UpdateWDCurveOnChange ()
		{
			bool bUpdateWDCurve = false;
			float Delta = Mathf.Abs ((float)_WCStateIdInt - _WCStateId);
			if (Delta == 0) {
			}
			if (Delta < 0.01f) {
				_WCStateId = (float)_WCStateIdInt;
				bUpdateWDCurve = true;
			}
			else {
				_WCStateId = Mathf.Lerp (_WCStateId, (float)_WCStateIdInt, Time.deltaTime * _LerpSpd);
				bUpdateWDCurve = true;
			}
			if (bUpdateWDCurve) {
				UpdagteWCurveByInterpState ();
			}
		}

		public void SetVerticeState (List<Vector2> _VtsState)
		{
			_Vertices = _VtsState;
			GenerateTrace ();
		}

		[ContextMenu("UpdagteWCurveByInterpState")]
		public void UpdagteWCurveByInterpState()
		{
			float id = Mathf.Clamp (_WCStateId, 0, _WCStates.Count-1);
			int id0 = Mathf.FloorToInt (id);
			int id1 = Mathf.CeilToInt (id);
			AnimationCurve ac0 = _WCStates [id0]._WdCurve;
			AnimationCurve ac1 = _WCStates [id1]._WdCurve;

			if (_WdCurve.length != 6) {
				MiroV1Utils.ClearAnimCurve (_WdCurve);
				for (float t = 0.0f; t < 1.0f; t += 0.2f) {
					_WdCurve.AddKey (new Keyframe (t, 0.0f));
				}
			}

			int i = 0;
			for (float t = 0.0f; t < 1.0f; t += 0.2f) {
				float v0 = ac0.Evaluate (t);
				float v1 = ac1.Evaluate (t);
				float v = Mathf.Lerp (v0, v1, id - id0);

				Keyframe kf = new Keyframe (t, v, 0, 0);
				kf.tangentMode = 1;
				_WdCurve.MoveKey (i, kf);
				i++;
			}
			MiroV1Utils.SmoothAnimCurve (_WdCurve, 0.0f);
		}

		[ContextMenu("UpdateLineRendererWidthCurve")]
		public void UpdateLineRendererWidthCurve ()
		{
			Vector4 pt = _Trace.Evaluate (_t);
			LineRenderer lr = GetComponent<LineRenderer> ();
			AnimationCurve ac = new AnimationCurve ();
			float ystart = _WdCurve.Evaluate (0.0f) * _WdStart;
			Keyframe kf0 = new Keyframe (0, ystart);
			kf0.tangentMode = _TangentMode;
			ac.AddKey (kf0);
			float y = _WdCurve.Evaluate (pt.x) * pt.y;
			Keyframe kf1 = new Keyframe (pt.x, y);
			kf1.tangentMode = _TangentMode;
			ac.AddKey (kf1);
			float yend = _WdCurve.Evaluate (1.0f) * _WdEnd;
			Keyframe kf2 = new Keyframe (1.0f, yend);
			kf2.tangentMode = _TangentMode;
			ac.AddKey (kf2);
			for (int i = 0; i < ac.length; i++) {
				ac.SmoothTangents (i, _Smoothness);
			}
			lr.widthCurve = ac;

		}

		[ContextMenu("GenerateTrace")]
		public void GenerateTrace()
		{
			_Trace.GenerateAnimCurves (_Vertices);
		}

	}
}
