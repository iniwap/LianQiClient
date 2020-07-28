using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CharStyle_Queue : CharStyleBase {

		public List<CharStyleBase> _CharStyles = new List<CharStyleBase>();

		override public void SetStyle(GameObject CharObj)
		{
			foreach (CharStyleBase cs in _CharStyles) {
				cs.SetStyle (CharObj);
			}
		}

	}
}
