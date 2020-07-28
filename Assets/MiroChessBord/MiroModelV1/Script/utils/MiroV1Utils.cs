using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Utils {

		static public void SetLineRendererSaturation(LineRenderer lr, float sat)
		{
			Color crs = lr.startColor;
			Color cre = lr.endColor;
			float hs, ss, vs;
			float he, se, ve;
			Color.RGBToHSV (crs, out hs, out ss, out vs);
			Color.RGBToHSV (cre, out he, out se, out ve);
			Color crs2 = Color.HSVToRGB (hs, sat, vs);
			Color cre2 = Color.HSVToRGB (he, sat, ve);
			lr.startColor = crs2;
			lr.endColor = cre2;
		}

		static public void ClearAnimCurve(AnimationCurve C)
		{
			int numKeys = C.length;
			for (int i = numKeys - 1; i >= 0; i--) {
				C.RemoveKey (i);
			}
		}

		static public void SmoothAnimCurve(AnimationCurve acurve, float SmoothWeight)
		{
			int num = acurve.length;
			for (int i = 0; i < num; i++) {
				acurve.SmoothTangents (i, SmoothWeight);
			}
		}


		static public void GenerateAnimCurveFromVertices<T>(List<T> trace, List<AnimationCurve> acurves)
		{
		}

	
	}


	[System.Serializable]
	public class TraceCurve{
		public AnimationCurve [] ACurves = new AnimationCurve[4]{
			new AnimationCurve(),
			new AnimationCurve(),
			new AnimationCurve(),
			new AnimationCurve()};
		public int _TangentMode = 0;
		public float _SmoothWeight = 1.0f;
		public float _TotalLength = 1.0f;

		public void GenerateAnimCurves(List<Vector4> trace)
		{
			ClearAnimCurves ();
			int count = trace.Count;
			float step = _TotalLength / (float)(count-1);

			for (int k = 0; k < trace.Count; k++) {
				Vector4 vt = (Vector4)trace [k];
				float t = (float)(k) * step;
				for(int p=0;p<4;p++)
				{
					Keyframe kf;
					kf = new Keyframe (t, vt[p]);
					kf.tangentMode = _TangentMode;
					ACurves [p].AddKey (kf);
				}
			}
		}

		public void GenerateAnimCurves(List<Vector3> trace)
		{
			ClearAnimCurves ();
			int count = trace.Count;
			float step = _TotalLength / (float)(count-1);

			for (int k = 0; k <= trace.Count; k++) {
				Vector4 vt = (Vector4)trace [k];
				float t = (float)(k) * step;
				for(int p=0;p<4;p++)
				{
					Keyframe kf;
					kf = new Keyframe (t, vt[p]);
					kf.tangentMode = _TangentMode;
					ACurves [p].AddKey (kf);
				}
			}
		}

		public void GenerateAnimCurves(List<Vector2> trace)
		{
			ClearAnimCurves ();
			int count = trace.Count;
			float step = _TotalLength / (float)(count-1);

			for (int k = 0; k < trace.Count; k++) {
				Vector4 vt = (Vector4)trace [k];
				float t = (float)(k) * step;
				for(int p=0;p<4;p++)
				{
					Keyframe kf;
					kf = new Keyframe (t, vt[p]);
					kf.tangentMode = _TangentMode;
					ACurves [p].AddKey (kf);
				}
			}
		}

		void ClearAnimCurves ()
		{
			for (int i = 0; i < 4; i++) {
				MiroV1Utils.ClearAnimCurve (ACurves[i]);
			}
		}

		void SmoothAnimCurves ()
		{
			for (int i = 0; i < 4; i++) {
				MiroV1Utils.SmoothAnimCurve (ACurves[i],_SmoothWeight);
			}
		}

		public Vector4 Evaluate(float t)
		{
			Vector4 v = Vector4.zero;
			for (int i = 0; i < 4; i++) {
				v [i] = ACurves [i].Evaluate (t);
			}

			return v;
		}
	}




}
