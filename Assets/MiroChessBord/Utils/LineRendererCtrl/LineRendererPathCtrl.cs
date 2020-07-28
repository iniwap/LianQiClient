using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MiroV1;

namespace Lyu
{
	public class LineRendererPathCtrl : MonoBehaviour {

		[System.Serializable]
		public class EventWithListVts: UnityEvent<List<Vector3> >{}

		public List<Vector3> _Trace = new List<Vector3>();
		public EventWithListVts _SendTraceToOther;

		public AnimationCurve _X,_Y,_Z;
		public int _TangentMode = 0;
		public float _SmoothWeight = 1.0f;
		public int _resolution = 12;
		public float _curLength = 0.0f;

		private float _lastLength = 0.0f;

		public bool _ToGlobal = false;

		// Use this for initialization
		void Start () {
			_SendTraceToOther.Invoke (_Trace);
		}

		// Update is called once per frame
		void Update () {
			if (_curLength != _lastLength) {
				UpdateLineRendererVts ();
				_lastLength = _curLength;
			}

		}

		[ContextMenu("GenUpdateLR")]
		public void GenUpdateLR()
		{
			GenerateAnimCurves ();
			UpdateLineRendererVts ();
		}

		[ContextMenu("UpdateLineRendererVts")]
		public void UpdateLineRendererVts()
		{
			LineRenderer lr = GetComponent<LineRenderer> ();
			lr.positionCount = _resolution;

			float step = _curLength / (float)(_resolution-1);
			List<Vector3> _poss = EvaluatePoss (step);

			if (_poss [0] == _poss [1]) {
				//print ("_poss [0] == _poss [1]");
				//EvaluatePoss (step);
			}

			lr.SetPositions (_poss.ToArray());

			if (_ToGlobal) {
				lr.useWorldSpace = true;
			} else {
				lr.useWorldSpace = false;
			}


		}

		List<Vector3> EvaluatePoss(float step)
		{
			List<Vector3> _poss = new List<Vector3> ();
			for (int i = 0; i < _resolution; i++) {
				float t = i * step;

				Vector3 vt = new Vector3 (
					_X.Evaluate (t), 
					_Y.Evaluate (t), 
					_Z.Evaluate (t));

				if (_ToGlobal) {
					vt = transform.TransformPoint (vt);
				}

				_poss.Add (vt);
			}
			return _poss;
		}

		[ContextMenu("GenerateAnimCurves")]
		void GenerateAnimCurves()
		{
			ClearXYZCurves ();

			float i = 0.0f;
			int tanMode = _TangentMode;
			int count = _Trace.Count;
			float step = 1.0f / (float)(count-1);

			for (int k = 0; k < _Trace.Count; k++) {
				Keyframe xk, yk, zk;

				Vector3 vt = _Trace [k];
				float t = (float)k * step;
				xk = new Keyframe (t, vt.x);
				yk = new Keyframe (t, vt.y);
				zk = new Keyframe (t, vt.z);

				xk.tangentMode = tanMode;
				yk.tangentMode = tanMode;
				zk.tangentMode = tanMode;

				_X.AddKey (xk);
				_Y.AddKey (yk);
				_Z.AddKey (zk);

			}

			foreach (Vector3 vt in _Trace) {
				i+= step;

				//i+= step;
			}

			SmoothAnimCurves ();


		}

		public Vector3 GetPosAtT01(float t, bool bToGlobal = true)
		{
			float t2 = t * _curLength;

			Vector3 BasePos = GetLocPosAtT01 (0);
			Vector3 EndPos = GetLocPosAtT01 (1.0f);
			Vector3 Start2End = EndPos - BasePos;
			Vector3 Offset = Vector3.zero;
			while (t2 > 1.0f) {
				Offset += Start2End;
				t2--;
			}

			//Debug.Log ("Offset:" + Offset);

			float t01 = t2 - Mathf.Floor (t2);
			Vector3 vt0 = GetLocPosAtT01(t01);
			Vector3 vt0FromBase = vt0 - BasePos;
			Vector3 vt =  vt0FromBase + Offset + BasePos;
			//Debug.Log ("vt0:" + vt0 + " vt:" + vt);

			Vector3 vt2 = Vector3.zero;
			if (bToGlobal) {
				vt2 = transform.TransformPoint (vt);
			} else {
				vt2 = vt;
			}

			return vt2;
		}

		Vector3 GetLocPosAtT01(float t)
		{
			Vector3 v = new Vector3 (
				_X.Evaluate (t), 
				_Y.Evaluate (t), 
				_Z.Evaluate (t));
			return v;
		}

		public void SetTraceVts(
			List<Vector3> vts, 
			bool FromGlobal2Local = true)
		{
			List<Vector3> lvts = vts;
			if (FromGlobal2Local) {
				for (int i = 0; i < lvts.Count; i++) {
					lvts [i] = transform.InverseTransformPoint (vts [i]);
				}
			}
			_Trace = lvts;
			GenUpdateLR ();
			_SendTraceToOther.Invoke (_Trace);
		}

		void SmoothAnimCurves ()
		{
			MiroV1Utils.SmoothAnimCurve(_X,_SmoothWeight);
			MiroV1Utils.SmoothAnimCurve(_Y,_SmoothWeight);
			MiroV1Utils.SmoothAnimCurve(_Z,_SmoothWeight);
		}

		void ClearXYZCurves ()
		{
			MiroV1Utils.ClearAnimCurve (_X);
			MiroV1Utils.ClearAnimCurve (_Y);
			MiroV1Utils.ClearAnimCurve (_Z);
		}
	}
}
