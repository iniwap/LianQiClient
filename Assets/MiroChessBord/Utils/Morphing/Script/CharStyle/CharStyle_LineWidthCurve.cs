using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_LineWidthCurve:CharStyleBase
	{
		public AnimationCurve _WidthCurve;

		override public void SetStyle(
			GameObject CharObj)
		{
			LineRenderer [] lrs = 
				CharObj.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				if (lr.tag == gameObject.tag) {
					lr.widthCurve = _WidthCurve;
				}
			}
		}
	}
}
