using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_Material:CharStyleBase
	{
		public Material _Material;
		override public void SetStyle(GameObject CharObj)
		{
			LineRenderer [] lrs = 
				CharObj.GetComponentsInChildren<LineRenderer> ();
			foreach (LineRenderer lr in lrs) {
				lr.material = _Material;
			}
		}
	}
}
