using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace MiroV1
{
	public class ENEdit : MonoBehaviour {
		public Text _Text;
		public Slider _SDEN,_SDENMax;
		public MiroV1PlacementMgr _mgr;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {


		}

		public void ENChanged(float e)
		{
			CheckSlidersValue ();
			_mgr.SetEN ((int)_SDEN.value);
			_mgr.SetENMax ((int)_SDENMax.value);
		}

		public void ENMaxChanged(float emax)
		{
			CheckSlidersValue ();
			_mgr.SetEN ((int)_SDEN.value);
			_mgr.SetENMax ((int)_SDENMax.value);
			_mgr.ConfigAttackers ();
		}

		void CheckSlidersValue ()
		{
			int en = (int)_SDEN.value;
			int enmax = (int)_SDENMax.value;
			string enText = en.ToString () + "/" + enmax.ToString ();
			_Text.text = enText;
			_SDEN.value = Mathf.Clamp (_SDEN.value, _SDENMax.minValue, _SDENMax.value);
		}
	}
}
