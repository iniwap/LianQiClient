using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoiseBezierPoint : MonoBehaviour {

	public BezierPoint _bezierPoint;
	private Vector3 _Handle0;

	public float _NoiseSpd;
	public float _LengthMin, _LengthMax;
	public float _AngleMax;

	private Vector2 _LenNoiseStart, _AngleNoiseStart;

	// Use this for initialization
	void Start () {
		if (_bezierPoint == null) {
			_bezierPoint = GetComponent<BezierPoint> ();
			_Handle0 = _bezierPoint.handle1;
		}
		_LenNoiseStart = Random.insideUnitCircle;
		_AngleNoiseStart = Random.insideUnitCircle;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Time.realtimeSinceStartup;

		float lenScale = _LengthMax - _LengthMin;

		float len = Mathf.PerlinNoise (
			_LenNoiseStart.x + _NoiseSpd * t, _LenNoiseStart.y);
		len = len * lenScale + _LengthMin;

		float angle = 2.0f * Mathf.PerlinNoise (
			_AngleNoiseStart.x + _NoiseSpd * t, _AngleNoiseStart.y) ;
		angle -= 1.0f;
		angle *= _AngleMax;

		Vector3 handle = _Handle0.normalized;
		handle *= len;
		handle = Quaternion.AngleAxis (angle, Vector3.forward) * handle;

		_bezierPoint.handle1 = handle;
		_bezierPoint.handle2 = -handle;
	}
}
