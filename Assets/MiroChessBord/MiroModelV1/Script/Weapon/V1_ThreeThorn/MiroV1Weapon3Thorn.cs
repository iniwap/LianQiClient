using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Weapon3Thorn : MiroV1WeaponBase {

		public MiroV1Thorn _TA,_TB,_TC;

		private List<MiroV1Thorn> _Thorns = 
			new List<MiroV1Thorn>();

		private List<MiroV1Thorn> _3Thorns = 
			new List<MiroV1Thorn> ();

		// Use this for initialization
		void Start () {

			CheckInit3Thorns ();
			CheckInitThorns ();
		}

		override public int GetValidAT()
		{
			int cnt = 0;
			foreach (MiroV1Thorn thn in _3Thorns) {
				foreach (MiroV1AimRb2Emitter em in thn._Emitters) {
					if (em.IsAttacking () && em.isActiveAndEnabled) {
						cnt++;
					}
				}
			}
			return cnt;
		}

		override public void UpdateByATMax(int ATMax)
		{
			CheckInitThorns ();
			SetSubThornATMaxasZero ();
			for (int i = 0; i < ATMax; i++) {
				_Thorns [i]._ATMax++;
			}
		}

		override public void UpdateByATMaxFloat(float ATMaxFloat)
		{
			
		}

		override public void UpdateBYAT(int AT)
		{
			CheckInitThorns ();
			SetSubThornATasZero ();

			for (int i = 0; i < AT; i++) {
				_Thorns [i]._AT++;
			}

		}

		override public void UpdateByATFloat(float ATF)
		{
			
		}

		override public void UpdateAfterOther()
		{
			foreach (MiroV1Thorn thorn in _Thorns) {
				thorn.TurnEmittersByAT ();
			}
		}


		override public void Shrink()
		{
			foreach (MiroV1Thorn thorn in _3Thorns) {
				thorn.Shrink ();
			}
		}

		override public void Scatter()
		{
			foreach (MiroV1Thorn thorn in _3Thorns) {
				thorn.Scatter ();
			}
		}

		override public void TurnDynamics(bool bON)
		{
			foreach (var thorn in _3Thorns) {
				thorn.TurnDynamics (bON);
			}
		}

		void CheckInitThorns ()
		{
			if (_Thorns.Count != 9) {
				for (int i = 0; i < 3; i++) {
					_Thorns.Add (_TA);
					_Thorns.Add (_TB);
					_Thorns.Add (_TC);
				}
			}
		}

		void CheckInit3Thorns()
		{
			if (_3Thorns.Count != 3) {
					_3Thorns.Add (_TA);
					_3Thorns.Add (_TB);
					_3Thorns.Add (_TC);
			}
		}
		void SetSubThornATMaxasZero ()
		{
			_TA._ATMax = 0;
			_TB._ATMax = 0;
			_TC._ATMax = 0;
		}

		void SetSubThornATasZero ()
		{
			_TA._AT = 0;
			_TB._AT = 0;
			_TC._AT = 0;
		}




	}
}
