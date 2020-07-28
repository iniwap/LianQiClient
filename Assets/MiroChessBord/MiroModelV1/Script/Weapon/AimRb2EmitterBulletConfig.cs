using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class AimRb2EmitterBulletConfig : MonoBehaviour {

		public MiroV1AimRb2Emitter [] _emitters;

		public GameObject _BulletPrefab;
		public GameObject _ENBallAbsorbPrefab;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("GetMiroV1AimRb2Emitters")]
		public void GetMiroV1AimRb2Emitters()
		{
			_emitters = GetComponentsInChildren<MiroV1AimRb2Emitter> ();
		}

		[ContextMenu("SetBulletPrefab")]
		public void SetBulletPrefab()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter._BulletPrefab = _BulletPrefab;
			}
		}

		[ContextMenu("GetModelSetting")]
		public void GetModelSetting()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter.GetModelSetting ();
			}
		}

		[ContextMenu("SetENBallAbsorbPrefab")]
		public void SetENBallAbsorbPrefab()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter._ENAbsorbPrefab = _ENBallAbsorbPrefab;
			}
		}

		public float _StartImplusePower = 2.0f;
		[ContextMenu("SetStartImpluse")]
		public void SetStartImpluse()
		{
			foreach (MiroV1AimRb2Emitter emitter in _emitters) {
				emitter._StartImplusePwr = _StartImplusePower;
			}
		}
	}
}
