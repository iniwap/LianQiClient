using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroV1LerpMove : MonoBehaviour {
	public Vector3 _Movement = Vector3.zero;
	public float _LerpSpd = 1.0f;
	public float _MoveThres = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;

		if (_Movement.magnitude == 0.0f) {
			return;
		}

		if (_Movement.magnitude < _MoveThres) {
			transform.Translate (_Movement);
			_Movement = Vector3.zero;
		} else {
			Vector3 MovementLeft =
				Vector3.Lerp (_Movement, Vector3.zero, dt * _LerpSpd);
			Vector3 Move = _Movement - MovementLeft;
			transform.Translate (Move);
			_Movement = MovementLeft;
		}

	}
}
