using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV2SubWeapon : MiroV1WeaponBase {
		
		public MiroV1SubWeaponTooth [] _Teeth;

		public Lyu.LineRendererPathCtrl _MainPathCtrl;
		public AnimationCurve _PathLenOnAT;

		public MiroV1AidBlackDots _aidDotAccess;
		public int _dir = 0;

		// Use this for initialization
		void Start () {
			AttachToAidBlackDot ();


		}

		[ContextMenu("AttachToAidBlackDot")]
		public void AttachToAidBlackDot()
		{
			MiroV1BlackDotBase bkDot = 
				_aidDotAccess.GetBlackDot (_dir);
			Transform tfBKDot = bkDot.transform;
			Lyu.KeepOffset keepOffset = GetComponent<Lyu.KeepOffset> ();
			keepOffset._Anchor = tfBKDot;


		}

		override public int GetValidAT()
		{
			int cnt = 0;
			foreach (MiroV1SubWeaponTooth thn in _Teeth) {
				foreach (MiroV1AimRb2Emitter em in thn._Emitters) {
					if (em.IsAttacking () && !em.IsAbsorberON()) {
						cnt++;
					}
				}
			}
			return cnt;
		}

		override public void UpdateByATMax(int ATMax)
		{
		}
		override public void UpdateByATMaxFloat(float ATMaxFloat)
		{

		}
		override public void UpdateBYAT(int AT)
		{
			//int atLeft = AT;
			List<int> ats = new List<int> (){ 0, 0, 0 };
			for (int i = 0; i < AT; i++) {
				int id = (int)Mathf.Repeat ((float)i, 3.0f);
				ats [id]++;
			}
			for (int i = 0; i < 3; i++) {
				MiroV1SubWeaponTooth tooth = _Teeth [i];
				tooth._AT = ats [i];
			}
		}

		override public void UpdateByATFloat(float ATF)
		{
			//float len = _PathLenOnAT.Evaluate (GetATF());
			//_MainPathCtrl._curLength = len;
		}

		override public void UpdateAfterOther()
		{

		}
			
		override public void AttachLineRendererPathCtrl(
			Lyu.LineRendererPathCtrl lrCtrl)
		{
			foreach(MiroV1SubWeaponTooth wt in _Teeth)
			{
				var poser = 
					wt.GetComponent<Lyu.GetPosOnLRPathCtrol> ();
				poser._lrPthCtrl = lrCtrl;
			}
		}

		override public void DettachLineRendererPathCtrl()
		{
			foreach(MiroV1SubWeaponTooth wt in _Teeth)
			{
				var poser = 
					wt.GetComponent<Lyu.GetPosOnLRPathCtrol> ();
				poser._lrPthCtrl = _MainPathCtrl;
			}
		}

		override public void TurnDynamics(bool bON)
		{
			foreach (var subWp in _Teeth) {
				subWp.TurnDynamics (bON);
			}
		}


	}
}
