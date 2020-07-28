using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENSource : MiroV1BulletEmitterBase {

		public MiroV1PumpBase _pumpCtrl;
		public int _enDotId = 0;

		private MiroModelV1 modelA;

		override public void Start()
		{
			
		}

		void Update()
		{
			Transform srcTF = GetSourceTF ();
			if (srcTF == null) {
				MiroModelV1 modelA = GetMiroModelA ();
				_modelSetting = modelA.GetComponent<MiroV1ModelSetting> ();
				SetSourceTFFromENGenerator (modelA);

				Transform tfB = _pumpCtrl.GetAnchorTFB ();
				Lyu.KeepOffset keepOffset = 
					tfB.GetComponent<Lyu.KeepOffset> ();
				Transform tfBAnchor = keepOffset._Anchor;
				SetTarget (tfBAnchor);
			}
		}


		override public void Emit()
		{
			base.Emit ();

		}

		void SetSourceTFFromENGenerator (MiroModelV1 model)
		{
			
			List<Transform> enDotTFs = model._ENGenerator._Generator.GetENDotTFs ();
			Transform edTF = enDotTFs [0];
			if (enDotTFs.Count > _enDotId) {
				edTF = enDotTFs [_enDotId];
			}
			SetSourceTF (edTF);
		}

		MiroModelV1 GetMiroModelA ()
		{
			Transform anchorTF = _pumpCtrl.GetAnchorTFA ();
			Lyu.KeepOffset keepOffset = anchorTF.GetComponent<Lyu.KeepOffset> ();
			Transform tf = keepOffset._Anchor;
			modelA = tf.GetComponentInParent<MiroModelV1> ();
			return modelA;
		}
	
	}
}
