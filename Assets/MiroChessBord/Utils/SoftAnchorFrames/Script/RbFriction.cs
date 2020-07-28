using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbFriction : MonoBehaviour {

	public float _K = 1.0f;
	
	// Update is called once per frame
	void Update () {

		Rigidbody2D rb = GetComponent<Rigidbody2D> ();

		Vector2 vel = rb.velocity;

		Vector2 force = -vel * _K;
		rb.AddForce (force);
		
	}
}
