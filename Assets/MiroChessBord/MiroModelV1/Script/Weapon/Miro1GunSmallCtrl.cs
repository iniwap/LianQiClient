using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class Miro1GunSmallCtrl : MonoBehaviour {

		public Animator _animator;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("Anim_StretchOut")]
		public void Anim_StretchOut()
		{
			_animator.Play ("GunSmallStretchOut");
		}

		[ContextMenu("Anim_Shrink")]
		public void Anim_Shrink()
		{
			_animator.Play ("GunSmallShrink");
		}
	}
}
