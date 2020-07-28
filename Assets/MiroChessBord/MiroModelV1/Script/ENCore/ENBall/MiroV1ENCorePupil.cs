using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENCorePupil : MiroV1ENGeneratorBase {

		public SpriteRenderer[] _spRdrs;

		override public void UpdateAnyway()
		{
			UpdateAnimActivity ();
		}

		void UpdateAnimActivity()
		{
			for (int i = 0; i < 9; i++) {
				SpriteRenderer sdr = _spRdrs [i];
				Animator anim = sdr.GetComponent<Animator> ();
				if (i < _EN) {
					anim.SetInteger ("State", 2);
				} else if (i >= _EN && i < _ENMax) {
					anim.SetInteger ("State", 1);
				} else {
					anim.SetInteger ("State", 0);
				}
			}
		}
		void ENExhaust ()
		{
			int count = _ENPrev - _EN;
			int idStart = _ENPrev;
			for (int i = 0; i < count; i++) {
				int id = idStart + i;
				//Animator anim = _spRdrs [id].GetComponent<Animator> ();
				//anim.Play ("Recover");
			}
		}

		void ENRecover ()
		{
			int count = _ENPrev - _EN;
			int idEnd = _ENPrev - 1;
			for (int i = 0; i < count; i++) {
				int id = idEnd - i;
				//Animator anim = _spRdrs [id].GetComponent<Animator> ();
				//anim.Play ("Exhaust");
			}
		}

		void ENMaxIncrease ()
		{
			int count = _ENMax - _ENMaxPrev;
			int idStart = _ENMaxPrev;
			for (int i = 0; i < count; i++) {
				int id = idStart + i;
				Animator anim = _spRdrs [id].GetComponent<Animator> ();
				anim.Play ("GrowUp");
			}
		}

		void ENMaxDecrease ()
		{
			int count = _ENMaxPrev - _ENMax;
			int idEnd = _ENMaxPrev - 1;
			for (int i = 0; i < count; i++) {
				int id = idEnd - i;
				Animator anim = _spRdrs [id].GetComponent<Animator> ();
				anim.Play ("Shrink");
			}
		}
	}
}
