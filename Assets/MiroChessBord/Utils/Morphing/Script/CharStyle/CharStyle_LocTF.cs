using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_LocTF: CharStyleBase
	{
		public Vector3 _LocPos,_LocRot,_LocScl;
		override public void SetStyle(
			GameObject CharObj)
		{
			CharObj.transform.localPosition = _LocPos;
			CharObj.transform.localRotation = Quaternion.Euler (_LocRot);
			CharObj.transform.localScale = _LocScl;
		}
	}
}