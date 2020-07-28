using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class ChildAnimCtrl : MonoBehaviour {

		public string _AnimClipName;

		public Animator[] _Anims;

		[ContextMenu("GetChildAnims")]
		public void GetChildAnims()
		{
			_Anims = GetComponentsInChildren<Animator> ();
		}

		[ContextMenu("PlayAnimByName")]
		public void PlayAnimByName()
		{
			foreach (Animator anim in _Anims) {
				anim.enabled = true;
				anim.Play (_AnimClipName);
			}
		}

		public void TurnAnimPlay(bool bON)
		{
			foreach (var anim in _Anims) {
				if (bON) {
					anim.StartPlayback ();
				} else {
					anim.StopPlayback ();
				}
			}

		}

	}
}
