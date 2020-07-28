using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENCoreCavity : MiroV1ENContainerBase {
		
		public AnimationCurve _NoiseSpdOnAT, _NoiseAmpOnAT;
		public MiroLocalNoisePosCtrl _CavityNoiseCtrl;
		public AnimationCurve _LengthOnAT,_WidthOnAT;
		public AnimationCurve _AlpaOnAT;
		public AnimationCurve _SatuatoinOnAT;
		public LineRenderer _LineRenderer;
		public Lyu.LineRendererPathCtrl _LineRdrPathCtrl;

		//public 

		// Update is called once per frame
		override public void Update () {
			base.Update ();

			UpdateNoiseMovementOnATF ();
			UpdateCavityVolume ();
			UpdateCavityColorAlpha ();
		}

		void UpdateNoiseMovementOnATF ()
		{
			float nspd = _NoiseSpdOnAT.Evaluate (GetATFloat());
			float namp = _NoiseAmpOnAT.Evaluate (GetATFloat());
			_CavityNoiseCtrl._Spd = nspd;
			_CavityNoiseCtrl._MaxDist = namp;
		}

		void UpdateCavityVolume()
		{
			float wd = _WidthOnAT.Evaluate (GetATFloat());
			_LineRenderer.widthMultiplier = wd;
			float len = _LengthOnAT.Evaluate (GetATFloat());
			_LineRdrPathCtrl._curLength = len;
		}

		void UpdateCavityColorAlpha ()
		{
			float alpha = _AlpaOnAT.Evaluate (GetATFloat());
			float sat = _SatuatoinOnAT.Evaluate (GetATFloat());
			Color cr0 = _LineRenderer.startColor;
			Color cr1 = _LineRenderer.endColor;
			cr0.a = alpha;
			cr1.a = alpha;

			cr0 = SetSaturation (cr0, sat);
			cr1 = SetSaturation (cr1, sat);

			_LineRenderer.startColor = cr0;
			_LineRenderer.endColor = cr1;
		}

		Color SetSaturation(Color cr,float sat)
		{
			float h, s, v;
			Color.RGBToHSV (cr, out h, out s, out v);
			Color cr2 = Color.HSVToRGB (h, sat, v);
			return cr2;
		}

		[ContextMenu("TurnONNoise")]
		public void TurnONNoise()
		{
			_CavityNoiseCtrl.TurnON ();
		}

		[ContextMenu("TurnOFFNoise")]
		public void TurnOFFNoise()
		{
			_CavityNoiseCtrl.TurnOFF ();
		}


	}
}
