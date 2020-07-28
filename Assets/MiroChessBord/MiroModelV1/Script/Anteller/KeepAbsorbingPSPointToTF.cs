using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class KeepAbsorbingPSPointToTF : MonoBehaviour {

		public Transform _Tgt;
		public Vector3 _BaseDir = Vector3.right;
		public float _Spd = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			Vector3 PosTgt = _Tgt.position;
			Vector3 PosThis = transform.position;
			Vector3 Offset = PosTgt - PosThis;
			Vector3 Dir = Offset.normalized;

			Quaternion rot =
				Quaternion.FromToRotation (_BaseDir, Dir);

			transform.rotation = rot;

			float Dist = Offset.magnitude;
			float LifeTime = Dist / _Spd;

			ParticleSystem ps = GetComponent<ParticleSystem> ();
			//ps.main.startL
			ParticleSystem.MainModule mainModule = ps.main;
			mainModule.startLifetimeMultiplier = LifeTime;
			ParticleSystem.VelocityOverLifetimeModule velModule = 
				ps.velocityOverLifetime;
			velModule.xMultiplier = _Spd;

		}
	}
}