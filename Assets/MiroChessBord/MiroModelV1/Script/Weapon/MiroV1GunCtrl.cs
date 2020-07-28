using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1GunCtrl : MonoBehaviour {

		public Animator _AnimMain, _AnimS1, _AnimS2, _AnimMain2;
		public MiroV1LerpMoveTgt _lerpMove1, _lerpMove2;
		public MiroV1GunEmitter _EmM, _Em1, _Em2, _EmM2;
	
		public int _AT = 0;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("AttackUp")]
		public void AttackUp()
		{
			if (_AT >= 10)
				return;
			_AT++;

			if (_AT == 1) {
				_AnimMain.Play ("GunSmallStretchOut");


			} else if (_AT == 2) {
				_AnimS1.Play ("GunSmallStretchOut");

			} else if (_AT == 3) {
				_AnimS2.Play ("GunSmallStretchOut");

			} else if (_AT == 4) {
				_AnimMain.Play ("SizeUp");
				_AnimS1.Play ("GunSmallShrink");
				_AnimS2.Play ("GunSmallShrink");
				_EmM._BulletType = 1;

			} else if (_AT == 5) {
				_lerpMove1._TgtPos.y = 0.25f;
				_AnimS1.Play ("GunSmallStretchOut");

			} else if (_AT == 6) {
				_lerpMove2._TgtPos.y = -0.25f;
				_AnimS2.Play ("GunSmallStretchOut");

			} else if (_AT == 7) {
				_AnimMain.Play ("SizeUp2");
				_AnimS1.Play ("GunSmallShrink");
				_AnimS2.Play ("GunSmallShrink");
				_EmM._BulletType = 2;

			} else if (_AT == 8) {
				_AnimS1.Play ("GunSmallStretchOut2");

				_lerpMove1._TgtPos.y = 0.3f;
			} else if (_AT == 9) {
				_AnimS2.Play ("GunSmallStretchOut2");

				_lerpMove2._TgtPos.y = -0.3f;
			} else if (_AT == 10) {
				_AnimMain2.Play ("GunMainStretchOut2");

				_AnimS1.Play ("GunSmallShrink2");
				_AnimS2.Play ("GunSmallShrink2");
				_AnimMain.Play ("GunSmallShrinked");

				_EmM2._BulletType = 3;
			}

		}

		[ContextMenu("AttackDown")]
		public void AttackDown()
		{
			if (_AT <= 0)
				return;

			_AT--;

			if (_AT == 9) {
				_AnimMain2.Play ("GunMainShrink");
				_AnimMain.Play ("SizeUp2");
				_AnimS2.Play ("GunSmallStretchOut2");
				_AnimS1.Play ("GunSmallStretchOut2");
				TurnBulletEmitters (true, true, true, false);
				_EmM._BulletType = 2;
			} else if (_AT == 8) {
				_AnimS2.Play ("GunSmallShrink2");
				_lerpMove2._TgtPos.y = -0.25f;
				TurnBulletEmitters (true, false, true, false);
			} else if (_AT == 7) {
				_AnimS1.Play ("GunSmallShrink2");
				_lerpMove1._TgtPos.y = 0.25f;
				TurnBulletEmitters (true, false, false, false);
			} else if (_AT == 6) {
				_AnimMain.Play ("SizeDown2");
				_AnimS2.Play ("GunSmallStretchOut");
				_AnimS1.Play ("GunSmallStretchOut");
				TurnBulletEmitters (true, true, true, false);
				_EmM._BulletType = 1;
			} else if (_AT == 5) {
				_lerpMove2._TgtPos.y = -0.2f;
				_AnimS2.Play ("GunSmallShrink");
				TurnBulletEmitters (true, false, true, false);
			} else if (_AT == 4) {
				_lerpMove1._TgtPos.y = 0.2f;
				_AnimS1.Play ("GunSmallShrink");
				TurnBulletEmitters (true, false, false, false);
			} else if (_AT == 3) {
				_AnimMain.Play ("SizeDown1");
				_AnimS1.Play ("GunSmallStretchOut");
				_AnimS2.Play ("GunSmallStretchOut");
				_EmM._BulletType = 0;
				TurnBulletEmitters (true, true, true, false);
			} else if (_AT == 2) {
				_AnimS2.Play ("GunSmallShrink");
				TurnBulletEmitters (true, false, true, false);
			} else if (_AT == 1) {
				_AnimS1.Play ("GunSmallShrink");
				TurnBulletEmitters (true, false, false, false);
			} else if (_AT == 0) {
				_AnimMain.Play ("GunSmallShrink");
				TurnBulletEmitters (false, false, false, false);
			}

		}

		private void TurnBulletEmitters(bool bm,bool b1, bool b2, bool bm2)
		{
			// to-do
		}


	}
}
