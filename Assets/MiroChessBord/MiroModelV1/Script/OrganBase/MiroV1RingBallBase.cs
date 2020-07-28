using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1RingBallBase : MiroV1BlackDotBase {

		private float _HPF = 0.0f;
		public float _LerpSpd = 5.0f;
		private MiroRing _ParentRing = null;
		
		// Update is called once per frame
		override public void UpdateHPDisp () {
			LerpHPF ();
			UpdateRingBall ();
		}

		void LerpHPF ()
		{
			float dt = Time.deltaTime;
			float t = dt * _LerpSpd;
			_HPF = Mathf.Lerp (_HPF, (float)_HP, t);
		}

		public float GetHPFloat()
		{
			return _HPF;
		}

		virtual public void UpdateRingBall()
		{
			
		}

		[ContextMenu("Grow")]
		override public void Grow()
		{
			base.Grow ();
		}

		[ContextMenu("Recover")]
		override public void Recover()
		{
			base.Recover ();
		}

		public void SetRing(MiroRing ring)
		{
			_ParentRing = ring;
		}

		public MiroRing GetParentRing()
		{
			return _ParentRing;
		}

		[ContextMenu("ShrinkWholeRing")]
		public void ShringWholeRing()
		{
			_ParentRing.Shrink ();
		}


	}
}
