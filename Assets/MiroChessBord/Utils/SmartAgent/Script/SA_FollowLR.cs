using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartAgent
{
	public class SA_FollowLR : MonoBehaviour {
		public bool _bReverse = true;
		public LineRenderer _LR;

		public float _Speed = 0.33f;
		public float _LerpSpd = 0.3f;
		public float _ZBias = -0.5f;

		private bool _bFollowing = false;

		private float _Dist01 = 0.0f;
		private Vector3 _FollowPos;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bFollowing) {
				Following ();
			}
		}

		private void Following()
		{
			_Dist01 += _Speed * Time.deltaTime;
			_Dist01 = Mathf.Clamp (_Dist01, 0, 1.0f);
			_FollowPos = EvaluateFollowPosAtDist (_Dist01, _LR);
			float dt = _LerpSpd * Time.deltaTime;
			Vector3 CurPos = transform.position;
			CurPos.z += _ZBias;
			Vector3 posLerped = Vector3.Lerp (
				CurPos, _FollowPos, dt);
			transform.position = posLerped;
		}

		private Vector3 EvaluateFollowPosAtDist(float dist01, LineRenderer lr)
		{
			if (_bReverse) {
				dist01 = 1.0f-dist01;
			}
			int pcnt = lr.positionCount;
			float dist = dist01 * (float)(pcnt-1);
			float t = dist - Mathf.Floor (dist);
			int idPrev = Mathf.FloorToInt (dist);
			int idNext = Mathf.CeilToInt (dist);

			Vector3 pos = Vector3.Lerp (
				lr.GetPosition (idPrev), 
				lr.GetPosition (idNext),
				t);
			return pos;
		}

		[ContextMenu("StartFollowing")]
		public void StartFollowing()
		{
			_bFollowing = true;
		}

		[ContextMenu("StopFollowing")]
		public void StopFollowing()
		{
			_bFollowing = false;
		}

		[ContextMenu("ResetDist")]
		public void ResetDist()
		{
			_Dist01 = 0.0f;
		}

		public bool IsFollowing()
		{
			return _bFollowing;
		}

		public void SetLineRenderer(LineRenderer lr)
		{
			_LR = lr;
		}

	}
}
