using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1EF2Ctrl : MiroV1ENFarmBase {

		public Animator[] _AnimMainLine = new Animator[]{ };

		public MiroPoissonInvoker _wriggleSwitch0, _wriggleSwitch1;
		public FixedJoint2D _Fix0, _Fix1;
		public GameObject [] _Plants;
		private MiroV1ModelSetting _modelSetting;

		public bool _bDieOnShrink = true;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			UpdateENColor ();
		}

		void UpdateENColor ()
		{
			if (_modelSetting == null) {
				_modelSetting = _Fix0.connectedBody.GetComponentInParent<MiroV1ModelSetting> ();
			}
			if (_modelSetting != null) {
				foreach (GameObject p in _Plants) {
					SpriteRenderer sr = p.GetComponentInChildren<SpriteRenderer> ();
					sr.color = _modelSetting._colorSetting._ENEmpty;
					TrailRenderer tr = p.GetComponentInChildren<TrailRenderer> ();
					Lyu.TrailRendererCtrl.SetColor (tr, _modelSetting._colorSetting._ENEmpty);
				}
			}
		}

		public void SetModelSetting(MiroV1ModelSetting modelSetting)
		{
			_modelSetting = modelSetting;
			foreach (GameObject gb in _Plants) {
				MiroV1BulletEmitterBase em = 
					gb.GetComponentInChildren<MiroV1BulletEmitterBase> ();
				em._modelSetting = modelSetting;
			}
		}

		override public FixedJoint2D GetJointA()
		{
			return _Fix0;	
		}

		override public FixedJoint2D GetJointB()
		{
			return _Fix1;
		}
			
		[ContextMenu("GrowUp")]
		override public void GrowUp()
		{
			for (int i = 0; i < 2; i++) {
				_AnimMainLine [i].enabled = true;
			}

			_AnimMainLine [0].Play ("GrowUp");
			_AnimMainLine [1].Play ("GrowUp");
		}
			
		[ContextMenu("Shrink")]
		override public void Shrink()
		{
			Lyu.ChildAnimCtrl animsCtrl = GetComponent<Lyu.ChildAnimCtrl> ();
			animsCtrl._AnimClipName = "Shrink";
			animsCtrl.PlayAnimByName ();

			if (_bDieOnShrink) {
				MiroV1TimeToDie timeDie = GetComponent<MiroV1TimeToDie> ();
				timeDie.enabled = true;
			}
		}
			
		[ContextMenu("Scatter")]
		override public void Scatter()
		{
			Lyu.ChildAnimCtrl animsCtrl = GetComponent<Lyu.ChildAnimCtrl> ();
			animsCtrl._AnimClipName = "Scatter";
			animsCtrl.PlayAnimByName ();

			DetachFromMiroModels ();
			MiroV1TimeToDie timeDier = GetComponent<MiroV1TimeToDie> ();
			timeDier.enabled = true;
		}
			
		[ContextMenu("TurnOnRepeatAnimation")]
		override public void TurnOnRepeatAnimation()
		{
			TurnNoiseWriggling (true);
		}
			
		[ContextMenu("TurnOffRepeatAnimation")]
		override public void TurnOffRepeatAnimation()
		{
			TurnNoiseWriggling (false);
		}

		public Lyu.BaryCoordsMgr [] _baryMgrs;
		public Rigidbody2D [] _rbs;
		override public void TurnDynamics(bool bON)
		{
			Lyu.ChildAnimCtrl animCtrl = GetComponent<Lyu.ChildAnimCtrl> ();
			animCtrl.TurnAnimPlay (bON);

			foreach (var baryMgr in _baryMgrs) {
				baryMgr.TurnEnable (bON);
			}


			foreach (var rb in _rbs) {
				if (bON) {
					rb.bodyType = RigidbodyType2D.Dynamic;
				} else {
					rb.bodyType = RigidbodyType2D.Static;
				}
			}

		}

		[ContextMenu("GetChildRigidbody2Ds")]
		public void GetChildRigidbody2Ds()
		{
			_rbs = GetComponentsInChildren<Rigidbody2D> ();
		}

		private void TurnNoiseWriggling(bool bON)
		{
			_wriggleSwitch0.enabled = bON;
			_wriggleSwitch1.enabled = bON;
			_Fix0.enabled = bON;
			_Fix1.enabled = bON;
		}

		private void DetachFromMiroModels()
		{
			MiroModelV1 A = GetModel (GetJointA ());
			MiroModelV1 B = GetModel (GetJointB ());
			A.DetachFarm ();
			B.DetachFarm ();
		}

		private MiroModelV1 GetModel(FixedJoint2D jnt)
		{
			MiroModelV1 model = 
				jnt.connectedBody.GetComponentInParent<MiroModelV1> ();
			return model;
		}

	}
}
