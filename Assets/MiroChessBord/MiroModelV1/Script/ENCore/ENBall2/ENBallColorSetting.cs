using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ENBallColorSetting : MonoBehaviour {

		public MiroV1ModelSetting _modelSetting;
		public List<SpriteRenderer> _enDotSRs = new List<SpriteRenderer>();
		public bool _SetAtStart = true;

		// Use this for initialization
		void Start () {
			if (_SetAtStart) {
				GetSpriteRenderers ();
				SetColor ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("SetColor")]
		public void SetColor()
		{
			foreach (SpriteRenderer sr in _enDotSRs) {
				Lyu.SpriteColorCtrl crCtrl = sr.GetComponent<Lyu.SpriteColorCtrl> ();
				if (crCtrl._Colors.Count != 5) {
					crCtrl._Colors.Clear ();
					for (int i = 0; i < 5; i++) {
						crCtrl._Colors.Add (Color.white);
					}
				}
				crCtrl._Colors [0] = _modelSetting._colorSetting._Empty;
				crCtrl._Colors [1] = _modelSetting._colorSetting._ENBG;
				crCtrl._Colors [2] = _modelSetting._colorSetting._ENEmpty;
				crCtrl._Colors [3] = _modelSetting._colorSetting._ENExhausted;
				crCtrl._Colors [4] = _modelSetting._colorSetting._ENMax;
			}
		}

		[ContextMenu("GetSpriteRenderers")]
		public void GetSpriteRenderers()
		{
			_enDotSRs.Clear ();
			SpriteRenderer [] srs = GetComponentsInChildren<SpriteRenderer> ();
			foreach (SpriteRenderer sr in srs) {
				if (sr.tag == "Energy") {
					_enDotSRs.Add (sr);
				}
			}
		}
	}
}
