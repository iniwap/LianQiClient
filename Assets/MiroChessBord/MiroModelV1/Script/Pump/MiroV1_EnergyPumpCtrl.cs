using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public enum BeatingState
	{
		EXHAUSTED,
		BEING_DRAINED,
		FULL
	}

	public class MiroV1_EnergyPumpCtrl : MiroV1PumpBase {
		
		public BeatingState _beatingState = BeatingState.FULL;
		[Range(0,9)]
		public int _sp = 0;
		public bool _keepSPAsEN = true;

		private BeatingState _beatingStatePrev = BeatingState.FULL;
		private int _spPrev = 0;

		public MiroV1PipeShellCtrl _PipeCtrl;
		public float _ScatterSpd = 8.0f;
		public float _RecoverSpd = 1.0f;

		public MiroV1PipeShellBeatingCtrl _BeatingCtrl;

		public MiroV1PipeShellWidthStartCtrl _PipeWdStartCtrl;
		public MiroV1PipeENCtrlBase _EnCtrl;

		public Lyu.LineRendererPathCtrl _lrPthCtrl;
		public Lyu.LRPathCtrlFromVts _lrPthFromVts;

		public Animator _PipeAnim,_FragmentAnim;
		public ParticleSystem _PSRecover;

		public Transform _AnchorA,_AnchorB;

		private MiroModelV1 _modelAider;

		// Use this for initialization
		void Start () {
			_beatingStatePrev = _beatingState;
			_spPrev = _sp+1;
		}
		
		// Update is called once per frame
		void Update () {

			if (_beatingState != _beatingStatePrev) {
				UpdateBeatingState ();
				_beatingStatePrev = _beatingState;
			}

			if (_sp != _spPrev) {
				_PipeWdStartCtrl._SP = _sp;
				_EnCtrl._sp = _sp;
				_spPrev = _sp;
			}

			if (_modelAider == null) {
				Lyu.KeepOffset kepOffset 
					= _AnchorA.GetComponent<Lyu.KeepOffset> ();
				_modelAider = kepOffset._Anchor.GetComponentInParent<MiroModelV1> ();
			}
			if (_keepSPAsEN && _modelAider!=null ) {
				//_model.Get
				if (_modelAider.IsAttacking ()) {
					_sp = 0;
				} else {
					_sp = _modelAider._ENGenerator._EN;
				}
			}
			
		}

		void UpdateBeatingState ()
		{
			//_PipeAnim.SetInteger ("Beating", (int)_beatingState);
			if (_beatingState == BeatingState.FULL) {
				_BeatingCtrl._BeatingLevel = 2;
			}
			else
				if (_beatingState == BeatingState.BEING_DRAINED) {
					_BeatingCtrl._BeatingLevel = 1;
				}
				else
					if (_beatingState == BeatingState.EXHAUSTED) {
						_BeatingCtrl._BeatingLevel = 0;
					}
		}

		public void GrowUpNeighborAidBlackDot()
		{
			Lyu.KeepOffset kepOff = 
				_AnchorB.GetComponent<Lyu.KeepOffset> ();
			if (kepOff._Anchor != null) {
				MiroV1BlackDotBase bkDot = 
					kepOff._Anchor.GetComponent<MiroV1BlackDotBase> ();
				bkDot.Grow ();
				bkDot.Recover ();
			}
		}
			
		private bool _bGrown = false;
		[ContextMenu("GrowUP")]
		override public void GrowUP()
		{
			//_PipeAnim.SetTrigger ("GrowUp");
			_PipeAnim.SetBool ("Grow", true);
			_bGrown = true;

			Invoke ("ConfigAideeSubWeapon", 0.5f);
			//ConfigAideeSubWeapon ();
		}



		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			_PipeAnim.SetTrigger ("Shrink");
			_PipeAnim.SetBool ("Grow", false);
			_PipeAnim.Play ("Shrink");
			_bGrown = false;

			DeConfigAideeSubWeapon ();
		}

		[ContextMenu("Scatter")]
		override public void Scatter()
		{
			_PipeCtrl._WCStateIdInt = 1;
			_PipeCtrl._LerpSpd = _ScatterSpd;
			_FragmentAnim.Play ("Scatter");
		}

		[ContextMenu("Recover")]
		override public void Recover()
		{
			//_PipeAnim.SetTrigger ("GrowUp");
			_PipeAnim.SetBool ("Grow", true);
			_PipeCtrl._WCStateIdInt = 0;
			_PipeCtrl._LerpSpd = _RecoverSpd;
			_PSRecover.Play ();
		}

		void ConfigAideeSubWeapon ()
		{
			int aidDir = GetAidingRDir ();
			MiroModelV1 modelAidee = GetModelAidee ();
			if (aidDir >= 0 && aidDir <= 5) {
				Lyu.LineRendererPathCtrl pthCtrl = 
					_PipeWdStartCtrl.GetComponent<Lyu.LineRendererPathCtrl> ();
				if (modelAidee != null) {
					modelAidee._WeaponSlots [aidDir]._Weapon
					.AttachLineRendererPathCtrl (pthCtrl);
				}
				//print ("AttachLineRendererPathCtrl");
			}
		}

		void DeConfigAideeSubWeapon()
		{
			int aidDir = GetAidingRDir ();
			MiroModelV1 modelAidee = GetModelAidee ();
			if (aidDir >= 0 && aidDir <= 5) {
				/*Lyu.LineRendererPathCtrl pthCtrl = 
					_PipeWdStartCtrl.GetComponent<Lyu.LineRendererPathCtrl> ();
					*/
				if (modelAidee != null) {
					modelAidee._WeaponSlots [aidDir]._Weapon.DettachLineRendererPathCtrl ();
				}
			}
		}

		override public bool IsGrown()
		{
			return _bGrown;
		}

		override public Transform GetAnchorTFA()
		{
			return _AnchorA;
		}

		override public Transform GetAnchorTFB()
		{
			return _AnchorB;
		}

		override public MiroModelV1 GetModelAider()
		{
			return GetModelFromAnkorTF (_AnchorA);
		}

		override public MiroModelV1 GetModelAidee()
		{
			return GetModelFromAnkorTF (_AnchorB);
		}

		public Lyu.LineCoordMgr _lineCoordMgr;
		public Lyu.BaryCoordsMgr _barysMgr;

		override public void TurnDynamics(bool bON)
		{
			if (bON) {
				_PipeAnim.StartPlayback ();
			}
			else
			{
				_PipeAnim.StopPlayback ();
			}

			_BeatingCtrl.enabled = bON;
			_PipeCtrl.enabled = bON;
			_PipeWdStartCtrl.enabled = bON;
			_lrPthCtrl.enabled = bON;
			_lrPthFromVts.enabled = bON;
			_lineCoordMgr.TurnEnable (bON);
			_barysMgr.TurnEnable (bON);
		}

		private MiroModelV1 GetModelFromAnkorTF(Transform akTf)
		{
			Lyu.KeepOffset kp = akTf.GetComponent<Lyu.KeepOffset> ();
			MiroModelV1 model = null;
			if (kp._Anchor != null) {
				model = 
					kp._Anchor.GetComponentInParent<MiroModelV1> ();
			}
			return model;
		}

		public void SetBeatingState_Full(BeatingState beatingState)
		{
			_beatingState = beatingState;
		}

		public void SetSupport(int sp)
		{
			_sp = (int)Mathf.Clamp ((float)sp, 0.0f, 9.0f);;
		}



	}
}
