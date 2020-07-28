using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCAccess : MonoBehaviour {


		[System.Serializable]
		public class CalculatorGroup
		{
			public List<MiroV1Caculator> _Cals = new List<MiroV1Caculator> ();
			public void Calculate()
			{
				if (_Cals.Count > 0) {
					/*
					if (_Cals [0].GetType () == typeof(CCRing)) {
						CCRing ccr = (CCRing)_Cals [0];
						if (ccr != null) {
							ccr.ResetAllCCRingID ();
						}
					}*/

				} 
				for (int i = _Cals.Count - 1; i >= 0; i--) {
					MiroV1Caculator cc = _Cals [i];
					cc.Calculate ();
				}
			}
		}
			
		public List< CalculatorGroup > _CalGroups = 
			new List< CalculatorGroup > ();


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void CalculateGroup(int id)
		{
			_CalGroups [id].Calculate ();
			/*
			if (id == 0) {
				_CalGroups [id]._Cals
			}*/
		}

		public int  GetGroupCount()
		{
			return _CalGroups.Count;
		}
			
	}
}
