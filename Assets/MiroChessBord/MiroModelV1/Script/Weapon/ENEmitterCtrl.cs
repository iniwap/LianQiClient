using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ENEmitterCtrl : MonoBehaviour {

		public MiroV1BulletEmitterBase [] _emitters;

		public GameObject _BulletPrefab;
		public GameObject _ENBallAbsorbPrefab;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		[ContextMenu("GetEmitters")]
		public void GetMiroV1AimRb2Emitters()
		{
			_emitters = GetComponentsInChildren<MiroV1BulletEmitterBase> ();
		}

		[ContextMenu("SetBulletPrefab")]
		public void SetBulletPrefab()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter._BulletPrefab = _BulletPrefab;
			}
		}

		[ContextMenu("SetENBallAbsorbPrefab")]
		public void SetENBallAbsorbPrefab()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter._ENAbsorbPrefab = _ENBallAbsorbPrefab;
			}
		}

		[ContextMenu("GetModelSetting")]
		public void GetModelSetting()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter.GetModelSetting ();
			}
		}


	}

}
