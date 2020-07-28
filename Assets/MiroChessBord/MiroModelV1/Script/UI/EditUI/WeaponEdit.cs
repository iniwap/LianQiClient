using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MiroV1
{
	public class WeaponEdit : MonoBehaviour {

		public Text _TxtDispAT;
		//public Slider _ATSlider;
		public int _WeaponID = 0;
		public MiroV1PlacementMgr _MiroPlacer;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetAT(float at)
		{
			//_MiroPlacer.
			int AT = (int)at;
			_TxtDispAT.text = "AT " + AT.ToString ();
			_MiroPlacer.SetWeaponAT (_WeaponID, at, at);
		}

		public void TurnActive(bool bActive)
		{
			_MiroPlacer.TurnWeaponActive (_WeaponID, bActive);
		}

	}
}
