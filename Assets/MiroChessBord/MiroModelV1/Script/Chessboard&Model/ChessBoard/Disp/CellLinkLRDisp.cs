using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellLinkLRDisp : MonoBehaviour {

		public TwoObjectLink _TwoObj;

		public Color _Cr=Color.black;
		public float _MaxAlpha=0.2f,_MinAlpha=0.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			CellObjCtrl ctrlA = 
				_TwoObj._A.GetComponent<CellObjCtrl> ();
			CellObjCtrl ctrlB = 
				_TwoObj._B.GetComponent<CellObjCtrl> ();
			bool bBlockA = ctrlA.IsBlocked ();
			bool bBlockB = ctrlB.IsBlocked ();
			bool bBlock = bBlockA || bBlockB;

			LineRenderer lr = GetComponent<LineRenderer> ();
			Color startCr = GetColorWithBlockState (bBlock);
			Color endCr = GetColorWithBlockState (bBlock);
			lr.startColor = startCr;
			lr.endColor = endCr;
		}

		private Color GetColorWithBlockState(bool bBlock)
		{
			Color color = _Cr;
			if (bBlock) {
				color.a = _MinAlpha;
			} else {
				color.a = _MaxAlpha;
			}
			return color;
		}
	}
}