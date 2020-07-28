using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartAgent;
using Lyu;

namespace MiroV1
{
	public class MiroV1BlackHoleSuckerArm : BlackHoleBase {

		public Animator [] _Anims;
		private List<LineRenderer> _LRs = new List<LineRenderer>();

		public bool _bAbsorbing = false;
		private bool _bAbsorbingPrev = false;

		public float _ImplusePwrL0 = 0.2f, _ImplusePwrL1=0.5f;
		public float _RandomnessL0 = 0.3f, _RandomnessL1 = 2.0f;

		public List<MiroV1AbsorbPoint> _AbsorbPoss = new List<MiroV1AbsorbPoint>();

		public List<Transform> _DefaultTgts = new List<Transform>();

		public GameObject [] _ENAbsorbPrefabs;

		// Use this for initialization
		void Start () {

			for (int i = 0; i < 2; i++) {
				LineRenderer lr = 
					_Anims [i].GetComponentInChildren<LineRenderer> ();
				_LRs.Add (lr);
			}
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bAbsorbing != _bAbsorbingPrev) {
				TurnAbsorbing (_bAbsorbing);
				_bAbsorbingPrev = _bAbsorbing;
			}

			UpdateAbsorbingTgt (_bAbsorbing);
		}

		void TurnAbsorbing (bool bAbsorbing)
		{
			for (int i = 0; i < 2; i++) {
				Animator an = _Anims [i];
				MIroRandImpluseCtrl impluseCtrl = an.GetComponent<MIroRandImpluseCtrl> ();
				MIroPoissionInvokerCtrl randIkr = an.GetComponent<MIroPoissionInvokerCtrl> ();
				//Lyu.SoftArmCtrl armCtrl = an.GetComponent<Lyu.SoftArmCtrl> ();
				MiroV1AbsorbPoint absorbPt = _AbsorbPoss [i];
				if (bAbsorbing) {
					impluseCtrl._Power = _ImplusePwrL1;
					randIkr._Lamda = _RandomnessL1;
					//armCtrl.StretchToTarget ();
				}
				else {
					impluseCtrl._Power = _ImplusePwrL0;
					randIkr._Lamda = _RandomnessL0;
					//armCtrl.ShrinkBack ();
				}
			}

		}



		void UpdateAbsorbingTgt(bool bAbsorbing)
		{
			for (int i = 0; i < 2; i++) {
				Animator an = _Anims [i];
				Lyu.SoftArmCtrl armCtrl = an.GetComponent<Lyu.SoftArmCtrl> ();
				MiroV1AbsorbPoint absorbPt = _AbsorbPoss [i];

				if (bAbsorbing && absorbPt._emitter != null) {
					Transform emTF = absorbPt._emitter.transform;
					armCtrl._Tgt = absorbPt._emitter.transform;
					//print ("absorbPt._emitter:" + absorbPt._emitter);
					GameObject ENBallPref = _ENAbsorbPrefabs [i];
					TargetTransform tgtTf = 
						ENBallPref.GetComponent<TargetTransform> ();
					var LRFollow = 
						ENBallPref.GetComponent<SA_FollowLR> ();
					tgtTf._Target = absorbPt._emitter.transform;
					LRFollow.SetLineRenderer (_LRs [i]);
					absorbPt._emitter._ENAbsorbPrefab = ENBallPref;
				} else {
					armCtrl.SetTargetTF (_DefaultTgts [i]);
				}

			}
		}

		[ContextMenu("GrowUp")]
		override public void GrowUp()
		{
			base.GrowUp ();
			foreach (Animator an in _Anims) {
				an.gameObject.SetActive (true);
				an.SetTrigger ("GrowUp");
			}


		}

		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			base.Shrink ();
			foreach (Animator an in _Anims) {
				an.SetTrigger ("Shrink");
			}
		}

		[ContextMenu("AbsorbingON")]
		override public void AbsorbingON()
		{
			_bAbsorbing = true;


		}

		[ContextMenu("AbsorbingOFF")]
		override public void AbsorbingOFF()
		{
			_bAbsorbing = false;
		}

		public MIroPoissionInvokerCtrl [] psnIkrCtrls;
		public KeepOffset[] _keepOffsets;
		public LineRendererPathCtrl [] _lrPthCtrls;
		public LRPathCtrlFromVts[] _lrPthFromVts;
		public Rigidbody2D[] _rbs;

		public override void TurnDynamics(bool bON)
		{
			GetDynamicComponents ();

			foreach (var anim in _Anims) {
				if (bON) {
					anim.StartPlayback ();
				} else {
					anim.StopPlayback ();
				}
			}

			TurnEnable (psnIkrCtrls, bON);
			TurnEnable (_keepOffsets, bON);
			TurnEnable (_lrPthCtrls, bON);
			TurnEnable (_lrPthFromVts, bON);
			foreach (var rb in _rbs) {
				if (bON) {
					rb.bodyType = RigidbodyType2D.Dynamic;
				} else {
					rb.bodyType = RigidbodyType2D.Static;
				}
			}
		}

		private void TurnEnable(MonoBehaviour [] items, bool bON)
		{
			foreach (var item in items) {
				item.enabled = bON;
			}
		}

		public void GetDynamicComponents()
		{
			GetChildPoissionInvokerCtrls ();
			GetChildKeepOffsets ();
			GetChildLRCtrls ();
			GetChildRigidbody2Ds ();
		}

		[ContextMenu("GetChildPoissionInvokerCtrls")]
		public void GetChildPoissionInvokerCtrls()
		{
			psnIkrCtrls = GetComponentsInChildren<MIroPoissionInvokerCtrl> ();
		}

		[ContextMenu("GetChildKeepOffsets")]
		public void GetChildKeepOffsets()
		{
			_keepOffsets = GetComponentsInChildren<KeepOffset> ();
		}

		[ContextMenu("GetChildLRCtrls")]
		public void GetChildLRCtrls()
		{
			_lrPthCtrls = GetComponentsInChildren<LineRendererPathCtrl> ();
			_lrPthFromVts = GetComponentsInChildren<LRPathCtrlFromVts> ();
		}

		[ContextMenu("GetChildRigidbody2Ds")]
		public void GetChildRigidbody2Ds()
		{
			_rbs = GetComponentsInChildren<Rigidbody2D> ();
		}

		public override bool IsGrown()
		{
			return base.IsGrown();
		}

		[ContextMenu("GetChdAbsorberPos")]
		public void GetChdAbsorberPos()
		{
			_AbsorbPoss.Clear ();
			MiroV1AbsorbPoint[] abp = GetComponentsInChildren<MiroV1AbsorbPoint> ();
			foreach (MiroV1AbsorbPoint ap in abp) {
				_AbsorbPoss.Add (ap);
			}
		}

		override public List<MiroV1AbsorbPoint> GetAbsorbingPos()
		{
			return _AbsorbPoss;
		}
	}
}
