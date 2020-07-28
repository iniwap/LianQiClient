using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LineCoord : MonoBehaviour {

		public Transform _Start,_End;
		public float _Along,_Shift,_Depth;
		public bool _GetLineCoordAtStart = false;
		public bool _KeepPosAtLineCoord = true;
		public bool _NormalizedAlong = true;
		public bool _NormalizedShift = false;
		public bool _NormalizedDepth = false;

		public bool _Lerping = true;
		public float _LerpSpd = 10.0f;

		// Use this for initialization
		void Start () {
			if (_GetLineCoordAtStart) {
				GetLineCoord ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			if (_KeepPosAtLineCoord) {
				if (_Lerping) {
					LerpTowardLineCoord ();
				} else {
					PlaceAtLineCoord ();
				}
			}
		}

		public void SetAlong(float value)
		{
			_Along = value;
		}
		public void SetShift(float value)
		{
			_Shift = value;
		}
		public void SetDepth(float value)
		{
			_Depth = value;
		}

		[ContextMenu("GetLineCoord")]
		public void GetLineCoord()
		{
			Vector3 AlongDir, ShiftDir, DepthDir;
			Get3Dir (out AlongDir, out ShiftDir, out DepthDir);

			GameObject newObj = new GameObject ();
			newObj.transform.SetParent (_Start);
			newObj.transform.position = transform.position;

			Vector3 AlongDir2, ShiftDir2, DepthDir2;
			AlongDir2 = _Start.InverseTransformVector (AlongDir);
			ShiftDir2 = _Start.InverseTransformVector (ShiftDir);
			DepthDir2 = _Start.InverseTransformVector (DepthDir);

			Vector3 Pos = 
				_Start.InverseTransformPoint (newObj.transform.position);

			_Along = Vector3.Dot (Pos, AlongDir2) / AlongDir2.magnitude;
			_Shift = Vector3.Dot (Pos, ShiftDir2) / ShiftDir2.magnitude;
			_Depth = Vector3.Dot (Pos, DepthDir2) / DepthDir2.magnitude;

			DestroyImmediate (newObj);
		}

		[ContextMenu("PlaceAtLineCoord")]
		public void PlaceAtLineCoord()
		{
			Vector3 Pos = GetPosAtLineCoord ();
			transform.position = Pos;
		}

		private void LerpTowardLineCoord()
		{
			Vector3 Pos = GetPosAtLineCoord ();
			Vector3 CPos = transform.position;
			float lt = Time.deltaTime * _LerpSpd;
			lt = Mathf.Clamp01 (lt);
			Vector3 LPos = Vector3.Lerp (CPos, Pos, lt);
			transform.position = LPos;
		}

		private Vector3 GetPosAtLineCoord()
		{
			Vector3 AlongDir, ShiftDir, DepthDir;
			Get3Dir (out AlongDir, out ShiftDir, out DepthDir);

			Vector3 Bias = Vector3.zero;
			Bias += AlongDir * _Along;
			Bias += ShiftDir * _Shift;
			Bias += DepthDir * _Depth;
			Vector3 Pos = _Start.position + Bias;
			return Pos;
		}

		void Get3Dir (out Vector3 AlongDir, out Vector3 ShiftDir, out Vector3 DepthDir)
		{
			Vector3 Start2End = _End.position - _Start.position;
			//float angle = Vector3.Angle (Vector3.right, Start2End);
			float len = Start2End.magnitude;
			AlongDir = Start2End;
			ShiftDir = Quaternion.AngleAxis (-90.0f, Vector3.forward) * AlongDir;
			DepthDir = Vector3.Cross (ShiftDir, AlongDir);

			if (!_NormalizedAlong) {
				AlongDir.Normalize ();
			}
			if (!_NormalizedShift) {
				ShiftDir.Normalize();
			}
			if (!_NormalizedDepth) {
				DepthDir.Normalize ();
			}
		}
	}
}
