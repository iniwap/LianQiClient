using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ModelSetting : MonoBehaviour {
		public string _CampName;

		[System.Serializable]
		public struct ColorSetting
		{
			public Color _ENEmpty, _ENExhausted, _ENMax, _ENBG, _Empty;
			public Gradient _BulletTrailGrad;
		}
		public ColorSetting _colorSetting;

		public bool IsSameCamp(MiroV1ModelSetting other)
		{
			bool bSame = false;
			if (other == null) {
				bSame = false;
			} else {
				bSame = (_CampName == other._CampName);
			}
			/*
			if (bSame) {
				//Debug.Log ("What Same!");
				bSame = bSame;
			}*/
			//Debug.Log ("bSame:" + bSame);
			return bSame;
		}
	}
}
