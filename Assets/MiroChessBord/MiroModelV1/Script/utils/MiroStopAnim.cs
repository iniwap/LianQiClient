using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroStopAnim : MonoBehaviour {

		[ContextMenu("StopAnim")]
		public void StopAnim()
		{
			Animator anim = GetComponent<Animator> ();
			anim.StopPlayback ();
		}

		[ContextMenu("DisableAnim")]
		public void DisableAnim()
		{
			Animator anim = GetComponent<Animator> ();
			anim.enabled = false;
		}

		[ContextMenu("EnableAnim")]
		public void EnableAnim()
		{
			Animator anim = GetComponent<Animator> ();
			anim.enabled = true;
		}
	}
}
