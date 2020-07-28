using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_MaterialPropertyBlock:CharStyleBase
	{
		public MaterialPropertyBlock _MatPropBlock;
		override public void SetStyle(GameObject CharObj)
		{
			LineRenderer [] lrs = 
				CharObj.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				lr.SetPropertyBlock(_MatPropBlock);
			}
		}
	}
}
