using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1BlackHoleCircle : BlackHoleBase {

		public Animator _Anim;

		public bool _bAbsorbing = false;
		private bool _bAbsorbingPrev = false;

		public List<MiroV1AbsorbPoint> _AbsorbPoss = new List<MiroV1AbsorbPoint>();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			if (_bAbsorbing != _bAbsorbingPrev) {
				if (_bAbsorbing) {
					_Anim.SetBool ("Absorbing", true);
				} else {
					_Anim.SetBool ("Absorbing", false);
				}
				_bAbsorbingPrev = _bAbsorbing;
			}

		}
			
		[ContextMenu("GrowUp")]
		override public void GrowUp()
		{
			base.GrowUp ();
			_Anim.SetTrigger ("GrowUp");
		}

		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			base.Shrink ();
			_Anim.SetTrigger ("Shrink");
		}

		[ContextMenu("AbsorbingON")]
		override public void AbsorbingON()
		{
			_bAbsorbing = true;
		}

		[ContextMenu("AbsorbingOFF")]
		override public void AbsorbingOFF()
		{
			_bAbsorbing = false;
		}

		public override bool IsGrown()
		{
			return base.IsGrown();
		}

		[ContextMenu("GetChdAbsorberPos")]
		public void GetChdAbsorberPos()
		{
			_AbsorbPoss.Clear ();
			MiroV1AbsorbPoint[] abp = GetComponentsInChildren<MiroV1AbsorbPoint> ();
			foreach (MiroV1AbsorbPoint ap in abp) {
				_AbsorbPoss.Add (ap);
			}
		}

		override public List<MiroV1AbsorbPoint> GetAbsorbingPos()
		{
			return _AbsorbPoss;
		}

	}
}
