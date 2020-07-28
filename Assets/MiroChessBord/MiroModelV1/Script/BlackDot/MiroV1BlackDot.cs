using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroV1BlackDot : MiroV1BlackDotBase {

		public ParticleSystem _PS;
		public ScaleStates _ScaleCtrl;
		public SpriteRenderer _SREdge;
		public LineRenderer _LRLink;

		// Use this for initialization
		void Start () {

			MiroV1BlackDotMgr mgr = GetComponentInParent<MiroV1BlackDotMgr> ();
			mgr.NewBlackDot (this);
			
		}
		
		// Update is called once per frame
		override public void UpdateHPDisp () {
			_HPMax = Mathf.Clamp (_HPMax, 0, 1);

			_ScaleCtrl.SetScaleId (_HP);

			if (_PS.time >= 4.0f && _PS.isPlaying) {
				_PS.Stop ();
			}
			if (_LRLink != null) {
				if (_HP > 0) {
					_LRLink.gameObject.SetActive (true);
				} else {
					_LRLink.gameObject.SetActive (false);
				}
			}
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
			_SREdge.enabled = false;
			if (_HP == 0 && _LRLink!=null) {
				_LRLink.gameObject.SetActive( false);
			}
		}

		[ContextMenu("Break")]
		override public void Break()
		{
			base.Break ();

			if (_PS.isPlaying) {
				_PS.Stop ();
			}
			_PS.Play ();
			_SREdge.enabled = true;
		}

		[ContextMenu("Recover")]
		override public void Recover()
		{
			base.Recover ();
		}



	}
}
