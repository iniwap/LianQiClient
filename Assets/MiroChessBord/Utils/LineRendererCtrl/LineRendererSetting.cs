using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LineRendererSetting {

		static public void SetColor(LineRenderer lr, Color cr)
		{
			lr.startColor = cr;
			lr.endColor = cr;
		}
	}
}
