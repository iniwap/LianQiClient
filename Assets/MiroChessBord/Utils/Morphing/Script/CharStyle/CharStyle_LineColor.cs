using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_LineColor:CharStyleBase
	{
		public Color _StartColor,_EndColor;
		override public void SetStyle(GameObject CharObj)
		{
			LineRenderer [] lrs = 
				CharObj.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				lr.startColor = _StartColor;
				lr.endColor = _EndColor;
			}
		}
	}

}
