using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftAnchor
{
	public class SA_WASDForcer : MonoBehaviour {

		public float _Power = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();

			Vector2 force = Vector2.zero;
			if (Input.GetKey (KeyCode.UpArrow)) {
				force += Vector2.up;
			}

			if (Input.GetKey (KeyCode.DownArrow)) {
				force += Vector2.down;
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				force += Vector2.right;
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				force += Vector2.left;
			}

			if (force.magnitude > 0.0f) {
				force = force.normalized * _Power;
				rb.AddForce (force);
			}
			
		}
	}
}
