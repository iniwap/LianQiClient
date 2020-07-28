using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class SoftArmCtrl : MonoBehaviour {
		
		public Transform _Tgt;
		/*
		public float _Power = 1.0f;
		public bool _2D = true;
		public float _Resistance = 1.0f;
		public float _DistThres = 1.0f;
		*/

		public float _TipSpd0 = 0.5f;
		public float _TipSpd1 = 3.0f;
		public float _LerpSpd = 0.2f;

		public List<Rigidbody2D> _Anchors = new List<Rigidbody2D> ();

		private bool _bStretching = false;

		public UnityEvent _StrechOut;
		public UnityEvent _ShrinkBack;

		Vector3 TipPos;
		// Use this for initialization
		void Start () {
			TipPos = _Anchors [0].transform.position;

			InitTargetJointsTargets ();
			TurnAutoConfigSpringDistance (true);
		}

		void OnEnable()
		{
			InitTargetJointsTargets ();
			TurnAutoConfigSpringDistance (true);
		}

		// Update is called once per frame
		void Update () {

			/*
			if (_bStretching) {
				Stretching ();
			}*/

			if (_bStretching) {
				LinkTipToTgt ();
				UpdateTipSpd (_TipSpd1);
			} else {
				ResetTipFromTgt ();
				UpdateTipSpd (_TipSpd0);
			}

			TurnAutoConfigSpringDistance (false);

		}

		public void UpdateTipSpd(float spd)
		{
			float dt = Time.deltaTime;
			var rb = _Anchors [0];
			TargetJoint2D tj = rb.GetComponent<TargetJoint2D> ();
			tj.frequency = Mathf.Lerp (tj.frequency, spd, dt * _LerpSpd);
		}

		void InitTargetJointsTargets()
		{
			for (int i = 0; i < _Anchors.Count-1; i++) {
				var rb = _Anchors [i];
				TargetJoint2D tj = rb.GetComponent<TargetJoint2D> ();
				tj.target = tj.transform.position;
			}
		}

		void TurnAutoConfigSpringDistance(bool bON)
		{
			foreach (var rb in _Anchors) {
				SpringJoint2D sp = rb.GetComponent<SpringJoint2D> ();
				sp.autoConfigureDistance = bON;
			}
		}

		/*
		void Stretching ()
		{
			var rbTip = _Anchors [0];
			Vector3 pos = rbTip.transform.position;
			Vector3 posTgt = _Tgt.position;
			Vector3 Offset = posTgt - pos;
			if (_2D) {
				Offset.z = 0.0f;
			}
			Vector3 Dir = Offset;
			Dir.Normalize ();
			Vector3 force = Dir * _Power;
			rbTip.AddForce (force);

			//print ("distance:" + Offset.magnitude);

			if (Offset.magnitude < _DistThres) {
				Vector3 vel = rbTip.velocity;
				Vector3 resist = -_Resistance * vel;
				rbTip.AddForce (resist);
				//print ("Resist:" + resist);
			}
		}*/

		[ContextMenu("GetAnchorsFromChildRBs")]
		public void GetAnchorsFromChildRBs()
		{
			Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D> ();
			_Anchors = new List<Rigidbody2D> (rbs);
		}


		[ContextMenu("InitSprings")]
		public void InitSprings()
		{
			for (int i = 0; i < _Anchors.Count - 1; i++) {
				Rigidbody2D rbA = _Anchors [i];
				Rigidbody2D rbB = _Anchors [i + 1];

				var sp = rbA.GetComponent<SpringJoint2D> ();
				sp.connectedBody = rbB;
			}
		}

		private void TurnSprings(bool bEnable)
		{
			foreach (var rb in _Anchors) {
				SpringJoint2D sp = rb.GetComponent<SpringJoint2D> ();
				sp.enabled = bEnable;
			}
		}

		private void TurnTargetJoints(bool bEnable)
		{
			for (int i = 0; i < _Anchors.Count-1; i++) {
				var rb = _Anchors [i];
				TargetJoint2D tj = rb.GetComponent<TargetJoint2D> ();
				tj.enabled = bEnable;
			}
		}

		[ContextMenu("StretchToTarget")]
		public void StretchToTarget()
		{

			_StrechOut.Invoke ();

			TurnSprings (true);
			TurnTargetJoints (false);
			_bStretching = true;

			//LinkTipToTgt ();
		}

		[ContextMenu("ShrinkBack")]
		public void ShrinkBack()
		{
			_ShrinkBack.Invoke ();

			TurnSprings (false);
			TurnTargetJoints (true);
			_bStretching = false;

			//ResetTipFromTgt ();
		}

		void LinkTipToTgt ()
		{
			TargetJoint2D tj0 = _Anchors [0].GetComponent<TargetJoint2D> ();
			tj0.enabled = true;
			tj0.target = _Tgt.position;
		}

		void ResetTipFromTgt ()
		{
			TargetJoint2D tj0 = _Anchors [0].GetComponent<TargetJoint2D> ();
			tj0.target = TipPos;
		}



		[ContextMenu("TurnONShaking")]
		public void TurnONShaking()
		{
			TurnShaking (true);
		}

		[ContextMenu("TurnOFFShaking")]
		public void TurnOFFShaking()
		{
			TurnShaking (false);
		}

		public void TurnShaking(bool bON)
		{
			MiroV1.MIroPoissionInvokerCtrl ikrCtrl = 
				GetComponent<MiroV1.MIroPoissionInvokerCtrl> ();
			if (bON) {
				ikrCtrl.TurnON ();
			} else {
				ikrCtrl.TurnOFF ();
			}
		}

		public void SetTargetTF(Transform tgt)
		{
			_Tgt = tgt;
		}

	}
}
