using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveCoord : MonoBehaviour {
	public Transform _TF;
	public BezierCurve _bCurve;

	public float _Along;
	public float _Shift;
	public float _Depth;
	public bool _UpdatingPos= true;
	public float _Delta = 0.01f;

	// Use this for initialization
	void Start () {
		CheckTF ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_UpdatingPos) {
			UpdatePos ();
		}
	}

	public void UpdatePos ()
	{
		int bpCount = _bCurve.pointCount;
		float t = Mathf.Repeat (_Along, 1.0f);
		t *= 1.0f;
		Vector3 pos, tan, right, up;
		GetAt (t, out pos, out tan, out right, out up);
		Vector3 PosCur = pos + _Shift * right + _Depth * up;
		CheckTF ();
		_TF.position = PosCur;
	}

	public void GetAt(float t, 
		out Vector3 Pos, 
		out Vector3 Tangent, 
		out Vector3 Right,
		out Vector3 Up)
	{
		float dt = _Delta / (float)_bCurve.pointCount;
		Pos = _bCurve.GetPointAt (t);
		float t1 = Mathf.Clamp01(t - dt);
		float t2 = Mathf.Clamp01 (t + dt);
		Vector3 PosNext = _bCurve.GetPointAt (t2);
		Vector3 PosPrev = _bCurve.GetPointAt (t1);
		Tangent = PosNext - PosPrev;
		Tangent.Normalize ();
		if (Tangent.magnitude == 0.0f) {
			//Debug.Log ("0");
			Vector3 PosThis = _bCurve.GetPointAt (0.1f);
			PosNext = _bCurve.GetPointAt(0.0f);
			Tangent = PosNext-PosThis;
			Tangent.Normalize ();
		}

		Quaternion RotToRight = Quaternion.AngleAxis (90.0f, Vector3.forward);
		Right = RotToRight * Tangent;

		Up = Vector3.Cross (Right, Tangent);
	}

	void CheckTF ()
	{
		if (_TF == null) {
			_TF = transform;
		}
	}
}
