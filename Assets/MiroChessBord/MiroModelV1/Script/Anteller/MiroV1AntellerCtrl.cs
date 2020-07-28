using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	[RequireComponent(typeof(LineRenderer))]
	public class MiroV1AntellerCtrl : MonoBehaviour {

		[System.Serializable]
		public class EventWithListVts: UnityEvent<List<Vector3> >{}

		public List<Vector3> _Trace = new List<Vector3>();
		public EventWithListVts _SendTrace;

		public AnimationCurve _X,_Y,_Z;
		public int _TangentMode = 0;
		public float _SmoothWeight = 1.0f;

		public int _resolution = 10;
		public float _curLength = 0.0f;

		private float _lastLength = 0.0f;

		// Use this for initialization
		void Start () {
			_SendTrace.Invoke (_Trace);
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

			float step = _curLength / (float)_resolution;
			for (int i = 0; i < _resolution; i++) {
				float t = i * step;

				Vector3 vt = new Vector3 (
					_X.Evaluate (t), 
					_Y.Evaluate (t), 
					_Z.Evaluate (t));

				lr.SetPosition (i, vt);
			}
		}

		[ContextMenu("GenerateAnimCurves")]
		void GenerateAnimCurves()
		{
			ClearXYZCurves ();

			float i = 0.0f;
			int tanMode = _TangentMode;
			int count = _Trace.Count;
			float step = 1.0f / (float)count;

			for (int k = 0; k < _Trace.Count; k++) {
				Keyframe xk, yk, zk;

				Vector3 vt = _Trace [k];
				float t = (float)(k + 1) * step;
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
