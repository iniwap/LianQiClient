using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Hole2Ctrl : MiroV1ENAbsorberBase {


		public Transform [] _TFSource = new Transform[4]{null,null,null,null};
		public Transform _HoleCtrTF;
		public Animator _AnimAtrL, _AnimAtrR;
		public MiroV1PipeShellCtrl _PipeCtrlL, _PipeCtrlR;
		public MiroV1PipeShellBeatingCtrl _PipeBeatingL, _PipeBeatingR;

		public Lyu.NoiseValueInvoker[] _NoiseValueIkrs;
		public Lyu.KeepOffset _KepPos0, _KepPos1;

		public Animator _AnimHL0, _AnimHL1, _AnimHR0, _AnimHR1;
		private bool [] _bAbsorbing = new bool[4]{false,false,false,false};

		public enum HoleStateChange
		{
			STEADY,
			GROWUP,
			SCATTER,
			SHRINK,
			START_ABSORB,
			STOP_ABSORB
		}
		public HoleStateChange [] _holeSChange = 
			new HoleStateChange[4] {
			HoleStateChange.STEADY,
			HoleStateChange.STEADY,
			HoleStateChange.STEADY,
			HoleStateChange.STEADY};

		private HoleStateChange [] _holeSChangePrev = 
			new HoleStateChange[4] {
			HoleStateChange.STEADY,
			HoleStateChange.STEADY,
			HoleStateChange.STEADY,
			HoleStateChange.STEADY};

		public void Start()
		{
			LBeating0 ();
			RBeating0 ();
		}

		public void Update()
		{
			UpdateHolesAnimOnChange ();
		}

		override public Transform GetAnchorTFA()
		{
			return _KepPos0.transform;
		}
		override public Transform GetAnchorTFB()
		{
			return _KepPos1.transform;
		}
			
		[ContextMenu("GrowUp")]
		override public void GrowUp()
		{
			_AnimAtrL.Play ("GrowUp");
			_AnimAtrR.Play ("GrowUp");

			_PipeCtrlL._WCStateIdInt = 2;
			_PipeCtrlR._WCStateIdInt = 2;

			SetHoleStateChange(HoleStateChange.GROWUP);
		}

		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			_AnimAtrL.Play ("Shrink");
			_AnimAtrR.Play ("Shrink");
			SetHoleStateChange(HoleStateChange.SHRINK);

		}

		[ContextMenu("ScatterL")]
		override public void ScatterL()
		{
			_AnimAtrL.Play ("Scatter");
			_PipeCtrlL._WCStateIdInt = 0;
			_holeSChange [0] = HoleStateChange.SCATTER;
			_holeSChange [1] = HoleStateChange.SCATTER;
		}

		[ContextMenu("ScatterR")]
		override public void ScatterR()
		{
			_AnimAtrR.Play ("Scatter");	
			_PipeCtrlR._WCStateIdInt = 0;
			_holeSChange [2] = HoleStateChange.SCATTER;
			_holeSChange [3] = HoleStateChange.SCATTER;
		}

		[ContextMenu("RecoverL")]
		override public void RecoverL()
		{
			_PipeCtrlL._WCStateIdInt = 2;
			_AnimAtrL.Play ("Recover");
			_holeSChange [0] = HoleStateChange.GROWUP;
			_holeSChange [1] = HoleStateChange.GROWUP;
		}

		[ContextMenu("RecoverR")]
		override public void RecoverR()
		{
			_PipeCtrlR._WCStateIdInt = 2;
			_AnimAtrR.Play ("Recover");
			_holeSChange [2] = HoleStateChange.GROWUP;
			_holeSChange [3] = HoleStateChange.GROWUP;
		}

		[ContextMenu("LBeating0")]
		override public void LBeating0()
		{
			_PipeBeatingL._BeatingLevel = 0;
			_PipeBeatingL.UpdateBeatingAnim ();
		}

		[ContextMenu("LBeating1")]
		override public void LBeating1()
		{
			_PipeBeatingL._BeatingLevel= 1;
			_PipeBeatingL.UpdateBeatingAnim ();
		}

		[ContextMenu("LBeating2")]
		override public void LBeating2()
		{
			_PipeBeatingL._BeatingLevel = 2;
			_PipeBeatingL.UpdateBeatingAnim ();
		}

		[ContextMenu("RBeating0")]
		override public void RBeating0()
		{
			_PipeBeatingR._BeatingLevel = 0;
			_PipeBeatingR.UpdateBeatingAnim ();
		}

		[ContextMenu("RBeating1")]
		override public void RBeating1()
		{
			_PipeBeatingR._BeatingLevel = 1;
			_PipeBeatingR.UpdateBeatingAnim ();
		}

		[ContextMenu("RBeating2")]
		override public void RBeating2()
		{
			_PipeBeatingR._BeatingLevel = 2;
			_PipeBeatingR.UpdateBeatingAnim ();
		}

		[ContextMenu("TurnOnNoiseWriggling")]
		override public void TurnOnNoiseWriggling()
		{
			TurnNoiseWriggling (true);
		}

		[ContextMenu("TurnOffNoiseWriggling")]
		override public void TurnOffNoiseWriggling()
		{
			TurnNoiseWriggling (false);
		}


		private void TurnNoiseWriggling(bool bON)
		{
			foreach (Lyu.NoiseValueInvoker ikr in _NoiseValueIkrs) {
				ikr.enabled = bON;
			}	
		}

		void UpdateHolesAnimOnChange ()
		{
			for (int i = 0; i < 4; i++) {
				HoleStateChange hState = _holeSChange [i];
				bool bChanged = (hState != _holeSChangePrev [i]);
				if (bChanged) {
					UpdateHoleState (hState,i);
					_holeSChangePrev [i] = _holeSChange [i];
				}
			}
		}

		void UpdateHolesAnim()
		{
			for (int i = 0; i < 4; i++) {
				HoleStateChange hState = _holeSChange [i];
				UpdateHoleState (hState,i);
				_holeSChangePrev [i] = _holeSChange [i];
			}
		}

		private void UpdateHoleState (HoleStateChange hState, int i)
		{
			Animator[] hAnims = new Animator[4] {
				_AnimHL0,
				_AnimHL1,
				_AnimHR0,
				_AnimHR1
			};

			if (hState == HoleStateChange.STEADY) {
				hAnims [i].Play ("Steady");
			}
			else
				if (hState == HoleStateChange.GROWUP) {
					hAnims [i].Play ("GrowUp");
				}
				else
					if (hState == HoleStateChange.SHRINK) {
						hAnims [i].Play ("Shrink");
					}
					else
						if (hState == HoleStateChange.SCATTER) {
							hAnims [i].Play ("Scatter");
						}
						else
							if (hState == HoleStateChange.START_ABSORB) {
								hAnims [i].Play ("AbsorbingON");
							}
							else
								if (hState == HoleStateChange.STOP_ABSORB) {
									hAnims [i].Play ("AbsorbingOFF");
								}
		}

		private void SetHoleStateChange(
			HoleStateChange C)
		{
			_holeSChange [0] = C;
			_holeSChange [1] = C;
			_holeSChange [2] = C;
			_holeSChange [3] = C;
		}


	}
}
