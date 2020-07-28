using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class AttractToNearestCld2D : MonoBehaviour {

		public List<Collider2D> _clds = new List<Collider2D> ();
		private Collider2D _Tgt = null;

		public string _TargetTag = "";

		public float _Freq = 4.0f;
		public float _Damp = 1.0f;

		public float _LerpSpd = 3.0f;


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
			UpdateTargetCollider2D ();
			if (_Tgt == null) {
				return;
			}
			//Debug.Log ("Tgt:" + _Tgt.gameObject.name);

			/*
			SpringJoint2D sp = GetComponent<SpringJoint2D> ();
			if (sp == null) {
				sp = gameObject.AddComponent<SpringJoint2D> ();
			}
			sp.connectedBody = _Tgt.GetComponent<Rigidbody2D>();
			sp.frequency = _Freq;
			sp.dampingRatio = _Damp;
			sp.autoConfigureDistance = false;
			sp.distance = 0.0f;


			if (_Tgt == null) {
				sp.enabled = false;
			} else {
				sp.enabled = true;
			}*/

			float dt = Time.deltaTime;
			float lerpT = dt * _LerpSpd;
			Vector3 pos = transform.position;
			float z = pos.z;
			Vector3 posTgt = _Tgt.transform.position;
			Vector3 posL = Vector3.Lerp (pos, posTgt, lerpT);
			posL.z = z;
			transform.position = posL;


				
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (!_clds.Contains (other) &&
				(other.gameObject.tag==_TargetTag)) {
				_clds.Add (other);
			}
		}
		void OnTriggerStay2D(Collider2D other)
		{

		}

		void OnTriggerExit2D(Collider2D other)
		{
			//_clds.Remove (other);
		}

		public Transform GetTargetTF()
		{
			return _Tgt.transform;
		}

		void UpdateTargetCollider2D ()
		{
			_Tgt = null;
			float minDist = 100000.0f;
			Collider2D tgt = null;

			Collider2D cldMe = GetComponent<Collider2D> ();
			for (int i = _clds.Count - 1; i > 0; i--) {
				Collider2D cld = _clds [i];
				bool bTouching = cldMe.IsTouching (cld);
				if (!bTouching) {
					_clds.Remove (cld);
				}
			}

			foreach (Collider2D cld in _clds) {
				Vector3 offset = transform.position - cld.transform.position;
				float dist = offset.magnitude;
				if (dist < minDist) {
					minDist = dist;
					tgt = cld;
				}
			}
			_Tgt = tgt;
		}
	}
}
