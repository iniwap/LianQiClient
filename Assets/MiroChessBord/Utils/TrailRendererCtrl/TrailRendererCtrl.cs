using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TrailRendererCtrl {
		static public void SetColor(TrailRenderer tr, Color cr)
		{
			tr.startColor = cr;
			tr.endColor = cr;
		}

		static public void SetGradient(TrailRenderer tr, Gradient grad)
		{
			tr.colorGradient = grad;
		}

	}
}
