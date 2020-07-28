using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetTextAsSliderValue : MonoBehaviour {

	public string _Prefix = "";

	public Text _txt;
	public Slider _sld;

	public void SetText(float value)
	{
		if (_sld.wholeNumbers) {
			int ivalue = (int)value;
			_txt.text = _Prefix + ivalue.ToString ();
		} else {
			_txt.text = _Prefix + value.ToString ();
		}
	}
}
