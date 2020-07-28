using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class TurnAllPS : MonoBehaviour {
		public ParticleSystem [] _PSs;


		[ContextMenu("GetAllChildPSs")]
		public void GetAllChildPSs()
		{
			_PSs = GetComponentsInChildren<ParticleSystem> ();
		}

		[ContextMenu("Play")]
		public void Play()
		{
			foreach (ParticleSystem ps in _PSs) {
				ps.Play ();
			}
		}

		[ContextMenu("Stop")]
		public void Stop()
		{
			foreach (ParticleSystem ps in _PSs) {
				ps.Stop ();
			}
		}
	}
}
