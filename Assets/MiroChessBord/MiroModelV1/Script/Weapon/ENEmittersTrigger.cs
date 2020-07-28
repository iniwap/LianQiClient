using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ENEmittersTrigger : MonoBehaviour {

		public List<MiroV1BulletEmitterBase> _ems = 
			new List<MiroV1BulletEmitterBase>();
		private List<MiroV1BulletEmitterBase> _OnEms = new List<MiroV1BulletEmitterBase> ();
		private bool _bAllEmitterReady = false;
		public MiroModelV1 _Model;
		// Use this for initialization
		void Start () {
			_Model = GetComponentInParent<MiroModelV1> ();
		}

		bool bAllOnEmittersReadyPrev = false;
		// Update is called once per frame
		void Update () {
			bool bAllOnEmittersReady = IsAllONEmitterReady ();
			bool bReady = bAllOnEmittersReady && !bAllOnEmittersReadyPrev;


			if (bReady) {
				MiroV1WeaponBase[] wps =
					GetComponentsInChildren<MiroV1WeaponBase> ();

				bool bTriggered = false;
				foreach (MiroV1WeaponBase wp in wps) {
					bool bEmit = wp.Emit ();
					if (bEmit) {
						bTriggered = true;
					}
				}

				if (bTriggered) {
					bAllOnEmittersReadyPrev = bAllOnEmittersReady;
				}

			}


			/*
			if (bReady) {
				//IsAllONEmitterReady ();
				Animator[] anims = GetComponentsInChildren<Animator> ();


				bool bTriggered = false;
				// to-do: make it by weapon
				foreach (Animator an in anims) {
					AnimatorStateInfo asinf = 
						an.GetCurrentAnimatorStateInfo (0);


					//print ("an.SetTrigger (Emit);");
					an.SetTrigger ("Emit");
					bTriggered = true;

				}

				if (bTriggered) {
					bAllOnEmittersReadyPrev = bAllOnEmittersReady;
				}
				//Debug.Log ("Trigger Emit");
			} else {
				//Debug.Log ("Not Ready");
			}
			*/


		}

		public void ResetbAllOnEmitters()
		{
			bAllOnEmittersReadyPrev = false;
			UpdateONEmitters ();
			foreach (var em in _OnEms) {
				em.Arrived ();
			}
		}

		bool IsAllONEmitterReady()
		{
			UpdateONEmitters ();

			bool bAllENArriveTgt= _OnEms.Count>0;
			foreach (MiroV1BulletEmitterBase em in _OnEms) {
				bool bArrive = em.IsEmitArriveTgt ();
				//bool bHasEmitEN = em.HasEmitEN ();;
				//bAllENArriveTgt = bAllENArriveTgt && bArrive && !bHasEmitEN;
				bAllENArriveTgt = bAllENArriveTgt && bArrive;
				if (!bAllENArriveTgt) {
					break;
				} else {
					//Debug.Log ("AllENArriveTgt!");
				}
			}

			return bAllENArriveTgt;
		}

		void UpdateONEmitters ()
		{
			_OnEms.Clear ();
			foreach (MiroV1BulletEmitterBase em in _ems) {
				if (em.IsON ()) {
					_OnEms.Add (em);
				}
			}


		}

		public void UpdateEmitters()
		{
			GetEmitters ();
			RemoveSleepingEmitters ();
		}

		void RemoveSleepingEmitters ()
		{
			for (int i = _ems.Count - 1; i >= 0; i--) {
				MiroV1BulletEmitterBase em = _ems [i];
				if (!em.isActiveAndEnabled) {
					_ems.RemoveAt (i);
				}
			}
		}

		[ContextMenu("GetEmitters")]
		public void GetEmitters()
		{
			MiroV1BulletEmitterBase[] ems = 
				GetComponentsInChildren<MiroV1BulletEmitterBase> ();

			foreach (MiroV1BulletEmitterBase em in ems) {
				if (!_ems.Contains (em)) {
					_ems.Add (em);
				}
			}

		}


	}
}
