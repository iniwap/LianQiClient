using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class UITextLanguage : MonoBehaviour {

		public bool _InitAtStart = true;

		public int _LanID = 0;

		public List<string> _Text = new List<string> ();
		public List<float> _RotZ = new List<float> ();
		public List<int> _FontSize = new List<int>();

		public void Start()
		{
			if (_InitAtStart) {
				SetUIText ();
			}
		}

		[ContextMenu("SetUIText")]
		public void SetUIText()
		{
			Text uiTxt = GetComponent<Text> ();
			if (_Text.Count > _LanID) {
				uiTxt.text = _Text [_LanID];
			}

			if (_FontSize.Count > _LanID ) {
				uiTxt.fontSize = _FontSize [_LanID];
			}
			if (_RotZ.Count > _LanID) {
				Quaternion rot = 
					uiTxt.transform.localRotation;
				rot = Quaternion.Euler (new Vector3 (0, 0, _RotZ [_LanID]));
			}

		}

		public void SetRotZ(List<float> rz)
		{
			_RotZ = rz;
		}

		public void SetFontSizes(List<int> fontSizes)
		{
			_FontSize = fontSizes;
		}

	}
}
