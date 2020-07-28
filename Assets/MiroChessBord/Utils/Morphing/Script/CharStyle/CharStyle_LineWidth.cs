using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_LineWidth:CharStyleBase
	{
		public float _LineWidthOnLossyScale;
		override public void SetStyle(
			GameObject CharObj)
		{
			Vector3 lscl = CharObj.transform.lossyScale;
			float xys = 0.5f * (lscl.x + lscl.y);
			float wd = xys * +_LineWidthOnLossyScale;
			LineRenderer [] lrs = 
				CharObj.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				lr.widthMultiplier = wd;
			}
		}
	}
}