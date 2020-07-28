using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1MainBody : MiroV1MainBodyBase {

		public AnimationCurve _LengthOnHPMax;
		public AnimationCurve _WidthOnHPMax;
		public AnimationCurve _AlphaOnHPN;

		public Lyu.LineRendererPathCtrl _pthCtrl;
		public LineRenderer _lineRenderer;

		public Animator _AnimScatter;
		public MiroV1GenObjFromInterpCurve _ScatterGenerator;
		public AnimationCurve _ScatterAmtONHPMax;

		public AnimationCurve _BodyAnchorDistOnHPMax;

		override public void AdjustBodyAnchors(MiroV1MainBodyAnchors bodyAnchors)
		{
			float hpf = 
				GetHPMaxFloat ();
			float d = _BodyAnchorDistOnHPMax.Evaluate (hpf);

			AdjustEachBodyAnchor (bodyAnchors, d);

		}

		override public void UpdateDisplay()
		{
			float hpf = GetHPFloat ();
			float hpMaxF = GetHPMaxFloat ();
			float len = _LengthOnHPMax.Evaluate (hpMaxF);
			float wd = _WidthOnHPMax.Evaluate (hpMaxF);
			_pthCtrl._curLength = len;
			_lineRenderer.widthMultiplier = wd;

			float hpn = hpf / (hpMaxF+0.01f);
			float alpha = _AlphaOnHPN.Evaluate (hpn);
			Color colorStart = _lineRenderer.startColor;
			Color colorEnd = _lineRenderer.endColor;
			colorStart.a = alpha;
			colorEnd.a = alpha;
			_lineRenderer.startColor = colorStart;
			_lineRenderer.endColor = colorEnd;
		}

		override public void UpdateOther()
		{
			UpdateScatterAmtOnHPMax ();
		}
			
		void UpdateScatterAmtOnHPMax ()
		{
			float hpmf = GetHPMaxFloat ();
			float scatterAmt = _ScatterAmtONHPMax.Evaluate (hpmf);
			_ScatterGenerator._MaxAmount = scatterAmt;
		}

		[ContextMenu("Scatter")]
		override public void Scatter()
		{
			_HPMax = 0;
			_AnimScatter.Play ("Scatter");
		}

		public Lyu.BaryCoordsMgr _baryMgr;
		public Lyu.LineRendererPathCtrl _lrPthCtrl;
		public Lyu.LRPathCtrlFromVts _lrPthFromVts;
		override public void TurnDynamics(bool bON)
		{
			_baryMgr.TurnEnable (bON);
			_lrPthCtrl.enabled = bON;
			_lrPthFromVts.enabled = bON;
		}
			
		static void AdjustEachBodyAnchor (MiroV1MainBodyAnchors bodyAnchors, float d)
		{
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < 3; j++) {
					Transform tf = bodyAnchors.GetAnchor (i, j);
					float angle = (float)i * 60.0f;
					float dist = d * ((float)j + 1.0f);
					Vector2 lpos = Vector2.right;
					lpos = Quaternion.AngleAxis (angle, Vector3.forward) * lpos;
					lpos *= dist;
					tf.localPosition = lpos;
				}
			}
		}
	}
}
