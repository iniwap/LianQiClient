using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartAgent;

namespace MiroV1
{
	public class MiroV1BulletArrive : MiroV1Caculator {

		override protected void _Calculate()
		{
			TargetTransform tgtTF = GetComponent<TargetTransform> ();

			Transform tgt = tgtTF._Target;

			MiroV1BlackDotBase bdot = tgt.GetComponent<MiroV1BlackDotBase> ();
			if (bdot != null && bdot._HP>0) {
				bdot.Break ();
			}
		}
	}
}
