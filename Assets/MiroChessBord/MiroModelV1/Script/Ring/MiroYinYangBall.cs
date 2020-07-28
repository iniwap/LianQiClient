using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroYinYangBall : MiroV1RingBallBase {

		public GameObject _YinYang;
		public AnimationCurve _YinYangSizeOnHP;
		public SpriteRenderer _SREdge;
		public float _edgeSizeMultiplier = 1.2f;
		public LineRenderer _LRLight;
		private MiroV1ModelSetting _modelSetting;

		public ParticleSystem _PSScatter;

		void Start()
		{
			//MiroV1BlackDotMgr mgr = GetComponentInParent<MiroV1BlackDotMgr> ();
			//mgr.NewBlackDot (this);
		}

		override public void UpdateRingBall()
		{
			UpdateSREdgeSize ();
			UpdateYinYangSize ();
			CheckPSScatter ();
			UpdateYinYangColor ();
		}

		void UpdateYinYangColor()
		{
			if (_modelSetting == null) {
				_modelSetting = GetComponentInParent<MiroV1ModelSetting> ();
			}
			if (_modelSetting != null) {
				Lyu.LineRendererSetting.SetColor (
					_LRLight, _modelSetting._colorSetting._ENMax);
			}
		}

		void UpdateYinYangSize ()
		{
			float hpf = GetHPFloat ();
			float wd = _YinYangSizeOnHP.Evaluate (hpf);
			LineRenderer[] lrs = 
				_YinYang.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				lr.widthMultiplier = wd;
			}
		}

		void UpdateSREdgeSize()
		{
			if (_SREdge == null)
				return;

			float scl = _edgeSizeMultiplier * 
				_YinYangSizeOnHP.Evaluate ((float)_HPMax);
			_SREdge.transform.localScale = new Vector3 (scl,scl,1.0f);

		}

		[ContextMenu("Grow")]
		override public void Grow()
		{
			base.Grow ();
		}

		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			base.Shrink ();
		}

		[ContextMenu("Break")]
		override public void Break()
		{
			base.Break ();
			if (_PSScatter.isPlaying) {
				_PSScatter.Stop ();
			}
			_PSScatter.Play ();
		}

		[ContextMenu("Recover")]
		override public void Recover()
		{
			base.Recover ();
		}
		void CheckPSScatter ()
		{
			float timeThres = 0.8f * _PSScatter.main.duration;
			if (_PSScatter.isPlaying && _PSScatter.time >= timeThres) {
				_PSScatter.Stop ();
			}
		}
	}
}
