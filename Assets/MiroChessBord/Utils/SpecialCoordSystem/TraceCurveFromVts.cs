using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TraceCurveFromVts : MonoBehaviour {

		public List<Transform> _Vts = new List<Transform>();

		public Lyu.TraceCurve _Curve = new Lyu.TraceCurve();
		public bool _bClose = true;

		[ContextMenu("GetChildTFAsVts")]
		public void GetChildTFAsVts()
		{
			int cnt = transform.childCount;
			for (int i = 0; i < cnt; i++) {
				Transform tf = transform.GetChild (i);
				_Vts.Add (tf);
			}

		}

		[ContextMenu("InitTrace")]
		public void InitTrace()
		{
			List<Vector3> _trace = new List<Vector3> ();
			foreach (Transform tf in _Vts) {
				_trace.Add (tf.position);
			}
			if (_bClose) {
				_trace.Add (_Vts [0].position);
			}
			_Curve._TotalLength = _trace.Count;
			_Curve.GenerateAnimCurves (_trace);
		}

		public Vector3 GetPosAtLength(float l)
		{
			Vector4 pos = 
				_Curve.Evaluate (l);
			return (Vector3)pos;
		}

		public Vector3 GetPosAtT01(float t)
		{
			float l = t / _Curve._TotalLength;
			return GetPosAtLength (l);
		}

		public Vector3 GetTangentAtLength(float l)
		{
			Vector3 tan = 
				(Vector3)_Curve.EvaluateTangent (l);
			return tan;
		}

		public Vector3 GetUpVecAtLength(float l)
		{
			Vector3 up = _Curve.EvaluateUpVector (l);
			return up;
		}

		public Vector3 GetRightVecAtLength(float l)
		{
			Vector3 right = _Curve.EvaluateRightVector (l);
			return right;
		}
	}
}
