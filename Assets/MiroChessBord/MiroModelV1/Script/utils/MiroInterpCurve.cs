using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	[System.Serializable]
	public class MiroInterpCurve {

		public AnimationCurve _X,_Y,_Z;

		public void SetPositions(List<Vector3> vts)
		{
			ClearXYZ ();

			float dist = 0.0f;
			for (int i = 0; i < vts.Count; i++) {
				if (i > 0) {
					Vector3 vtprev = vts [i - 1];
					Vector3 vt = vts [i];
					float d = (vt - vtprev).magnitude;
					dist += d;
				} 
				AddVertex (vts [i], dist);
			}

			MiroV1Utils.SmoothAnimCurve (_X, 0);
			MiroV1Utils.SmoothAnimCurve (_Y, 0);
			MiroV1Utils.SmoothAnimCurve (_Z, 0);
		}

		public float GetTotalLength()
		{
			if (_X.length > 0) {
				return _X.keys [_X.length - 1].time;
			} else {
				return 0.0f;
			}
		}

		public Vector3 GetPosAtT01(float t01)
		{
			float totalLen = GetTotalLength ();
			float dist = t01 * totalLen;
			Vector3 pos = GetPosAtDist (dist);
			return pos;
		}

		public Vector3 GetPosAtDist(float dist)
		{
			float x = _X.Evaluate (dist);
			float y = _Y.Evaluate (dist);
			float z = _Z.Evaluate (dist);

			Vector3 v = new Vector3 (x, y, z);
			return v;
		}

		private void AddVertex(Vector3 vt, float dist)
		{
			List<AnimationCurve> acs = new List<AnimationCurve> ();
			acs.Add (_X);
			acs.Add (_Y);
			acs.Add (_Z);

			for (int i = 0; i < 3; i++) {
				AnimationCurve ac = acs [i];
				Keyframe kf = new Keyframe (dist, vt [i]);
				ac.AddKey (kf);
			}

		}

		private void ClearXYZ ()
		{
			MiroV1Utils.ClearAnimCurve (_X);
			MiroV1Utils.ClearAnimCurve (_Y);
			MiroV1Utils.ClearAnimCurve (_Z);
		}
	}
}
