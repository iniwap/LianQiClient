using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroModelDynamicsCtrl : MonoBehaviour {
		public MiroModelV1 _Model;
		public GameObject _PhysicalFrameObj;

		public bool _PhyFrameON = true;
		private bool _PhyFrameONPrev = true;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			if (_PhyFrameONPrev != _PhyFrameON) {
				TurnPhysicalFramwork (_PhyFrameON);
			}
			_PhyFrameONPrev = _PhyFrameON;
			
		}

		public void TurnAllDynamics(bool bON)
		{
			TurnPhysicalFramwork (bON);
			TurnDispAssistDynamics (bON);
			TurnENGeneratorDynamics (bON);
			TurnMainBodyDynamics (bON);
			TurnAntellersDynamics (bON);
			TurnWeaponsDynamics (bON);
			TurnBlackHoleDynamics (bON);
			TurnPumpDynamics (bON);
			TurnENFarmDynamics (bON);
			TurnRingDynamics (bON);
		}


		public void TurnPhysicalFramwork(bool bON)
		{
			Rigidbody2D[] rbs = 
				GetComponentsInChildren<Rigidbody2D> ();
			/*
			SpringJoint2DCtrl jntCtrl = 
				_PhysicalFrameObj.GetComponent<SpringJoint2DCtrl> ();

			MIroPoissionInvokerCtrl psnIkrCtrl = 
				_PhysicalFrameObj.GetComponent<MIroPoissionInvokerCtrl> ();
			*/

			/*
			if (bON) {
				jntCtrl.TurnON ();
				psnIkrCtrl.TurnON ();
			} else {
				jntCtrl.TurnOFF ();
				psnIkrCtrl.TurnOFF ();
			}*/

			foreach (Rigidbody2D rb in rbs) {
				
				if (bON) {
					//RigidbodyConstraints2D con = rb.constraints;
					rb.bodyType = RigidbodyType2D.Dynamic;
					rb.velocity = Vector2.zero;
				} else {
					//rb.freezeRotation ();
					rb.bodyType = RigidbodyType2D.Static;
				}
			}

		}

		public List<MorphAnimDynamicCtrl> _MorphDynCtrl = 
			new List<MorphAnimDynamicCtrl>();
		public List<NoiseOffsetFromBirthPos> _NPosFromBths = 
			new List<NoiseOffsetFromBirthPos>();

		public void TurnDispAssistDynamics(bool bON)
		{
			foreach (var dctrl in _MorphDynCtrl) {
				dctrl.TurnDynamics (bON);
			}
			foreach (var nf in _NPosFromBths) {
				nf.enabled = bON;
			}
		}

		public void TurnENGeneratorDynamics(bool bON)
		{
			_Model._ENGenerator._Generator.TurnDynamics (bON);
		}

		public void TurnMainBodyDynamics(bool bON)
		{
			_Model._MainBody._mainbody.TurnDynamics (bON);
		}

		public void TurnAntellersDynamics(bool bON)
		{
			foreach (var ats in _Model._Antellers) {
				ats._anteller.TurnDynamics (bON);
			}

		}

		public void TurnWeaponsDynamics(bool bON)
		{
			foreach (var wps in _Model._WeaponSlots) {
				wps._Weapon.TurnDynamics (bON);
			}
		}

		public void TurnBlackHoleDynamics(bool bON)
		{
			_Model._BlackHole._BlackHole.TurnDynamics (bON);
		}

		public void TurnPumpDynamics(bool bON)
		{
			if (_Model._Pump == null) {
				return;
			}
			_Model._Pump.TurnDynamics (bON);
		}

		public void TurnENFarmDynamics(bool bON)
		{
			// to-do: add this function
		}

		public void TurnRingDynamics(bool bON)
		{
			// to-do: implement
		}

	}
}
