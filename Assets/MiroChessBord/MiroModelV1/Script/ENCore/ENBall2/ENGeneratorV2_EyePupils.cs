using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class ENGeneratorV2_EyePupils : MiroV1ENGeneratorBase {

		public Animator[] _Anims;
		public SpriteColorCtrl[] _crCtrls;

		// Use this for initialization
		void Start () {
			
		}
		override public void UpdateAnyway()
		{
			for (int i = 0; i < 9; i++) {
				Animator anim = _Anims[i];
				SpriteColorCtrl crCtrl = _crCtrls [i];
				if (i < _EN) {
					anim.SetInteger ("EN", 2);
					crCtrl._id = 4;
				} else if (i >= _EN && i < _ENMax) {
					anim.SetInteger ("EN", 1);
					crCtrl._id = 3;
				} else {
					anim.SetInteger ("EN", 0);
					crCtrl._id = 0;
				}
			}
		}

		override public void UpdateOnENChange(int EN, int ENPrev)
		{

		}

		override public void UpdateOnENMaxChange(int ENMax, int ENMaxPrev)
		{

		}

		override public List<Transform> GetENDotTFs()
		{
			List<Transform> tfList = new List<Transform> ();

			for (int i = 0; i < 9; i++) {
				tfList.Add (_Anims[i].transform);
			}
			return tfList;
		}

		public MiroLocalNoisePosCtrl _nposCtrl;
		override public void TurnDynamics(bool bON)
		{
			if (bON) {
				_nposCtrl.TurnON ();
			} else {
				_nposCtrl.TurnOFF ();
			}

			foreach(var an in _Anims)
			{
				if (bON) {
					an.enabled = true;
					an.StartPlayback ();
				} else {
					an.enabled = false;
					an.StopPlayback ();
				}
			}

			foreach (var crctrl in _crCtrls) {
				crctrl.enabled = bON;
			}

			BaryCoord bary = GetComponent<BaryCoord> ();
			bary.enabled = bON;
		}






	}
}
