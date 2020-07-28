using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
public class MiroV1LerpMoveTgt : MonoBehaviour {

	public bool _bLocalPosition = true;
	public Vector3 _TgtPos = Vector3.zero;
	public float _MoveDistThres = 0.01f;
	public float _LerpSpd = 5.0f;
	private bool _Moving = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 CurPos = transform.position;
		if (_bLocalPosition) {
			CurPos = transform.localPosition;
		}

		Vector3 LeftMove = _TgtPos - CurPos;
		float leftDist = LeftMove.magnitude;
		if (leftDist < _MoveDistThres) {
			PlaceAtPos (_bLocalPosition, _TgtPos);
			_Moving = false;
		} else {
			_Moving = true;
		}

		if (!_Moving)
			return;

		Vector3 LerpPos = Vector3.Lerp (CurPos, _TgtPos, _LerpSpd * Time.deltaTime);
		PlaceAtPos (_bLocalPosition, LerpPos);
	}

	private void PlaceAtPos(bool bLocal, Vector3 Pos)

	{
		if (bLocal) {
			transform.localPosition = Pos;
		} else {
			transform.position = Pos;
		}
	}
}
}
