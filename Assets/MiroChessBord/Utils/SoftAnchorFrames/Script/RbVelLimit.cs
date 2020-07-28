using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbVelLimit : MonoBehaviour {

	public float _SPDMax = 10.0f;

	// Update is called once per frame
	void Update () {

		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		if (rb.velocity.magnitude > _SPDMax) {
			rb.velocity = rb.velocity.normalized * _SPDMax;
		}
	}
}
