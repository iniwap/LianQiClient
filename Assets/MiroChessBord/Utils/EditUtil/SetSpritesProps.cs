using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetSpritesProps : MonoBehaviour {

		public SpriteRenderer[] _sprites;

		[Range(0,1.0f)]
		public float Alpha = 1.0f;

		[Range(0,360.0f)]
		public float Hue = 0.0f;

		[Range(0,1.0f)]
		public float Sat = 0.5f;

		[Range(0,1.0f)]
		public float Val = 1.0f;

		[ContextMenu("GetChildSpriteRenderers")]
		public void GetChildSpriteRenderers()
		{
			_sprites = GetComponentsInChildren<SpriteRenderer> ();
		}

		[ContextMenu("SetAlpha")]
		public void SetAlpha()
		{
			foreach (SpriteRenderer sr in _sprites) {
				Color cr = sr.color;
				cr.a = Alpha;
				sr.color = cr;
			}
		}

		[ContextMenu("SetHue")]
		public void SetHue()
		{
			foreach (SpriteRenderer sr in _sprites) {
				float h, s, v;
				Color.RGBToHSV (sr.color, out h, out s, out v);
				h = Hue;
				sr.color = Color.HSVToRGB (h, s, v);
			}
		}

		[ContextMenu("SetSat")]
		public void SetSat()
		{
			foreach (SpriteRenderer sr in _sprites) {
				float h, s, v;
				Color.RGBToHSV (sr.color, out h, out s, out v);
				s = Sat;
				sr.color = Color.HSVToRGB (h, s, v);
			}
		}

		[ContextMenu("SetVal")]
		public void SetVal()
		{
			foreach (SpriteRenderer sr in _sprites) {
				float h, s, v;
				Color.RGBToHSV (sr.color, out h, out s, out v);
				v = Val;
				sr.color = Color.HSVToRGB (h, s, v);
			}
		}

		[ContextMenu("SetHSVA")]
		public void SetHSVA()
		{
			foreach (SpriteRenderer sr in _sprites) {
				float h, s, v;
				Color.RGBToHSV (sr.color, out h, out s, out v);
				h = Hue;
				s = Sat;
				v = Val;
				Color color = Color.HSVToRGB (h, s, v);
				color.a = Alpha;
				sr.color = color;
			}
		}

	}
}
