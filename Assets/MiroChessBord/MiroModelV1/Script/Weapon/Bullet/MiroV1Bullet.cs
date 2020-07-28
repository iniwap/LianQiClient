using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Bullet : MonoBehaviour {

		public float _Force = 1.0f;
		public float _TorqueFand = 1.0f;
		public float _DirRand = 10.0f;
		public float _RotAngle = 0.0f;

		// Use this for initialization
		void Start () {

			Rigidbody2D rb = GetComponent<Rigidbody2D> ();

			Quaternion Rot = Quaternion.AngleAxis (Random.Range (-_DirRand, _DirRand), Vector3.forward);

			Vector3 dir = transform.TransformDirection (Vector2.right);
			dir = Quaternion.AngleAxis (_RotAngle, Vector3.forward) * dir;

			Vector2 force =  Rot * dir * _Force;
			//force = transform.TransformDirection (force);

			rb.AddForce (force, ForceMode2D.Impulse);

			rb.AddTorque (Random.Range (-1.0f, 1.0f) * _TorqueFand);

		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
