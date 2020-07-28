using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class UITextLanguageMgr : MonoBehaviour {

		public Text[] _uiTxts;
		public int _idLan = 0;
		private int _idLanPrev = 0;
		/*
		public List<float> _RotZ = new List<float> ();
		public List<int> _FontSizes = new List<int>();
		*/

		public void Update()
		{
			if (_idLan != _idLanPrev) {
				SetIDLanForEach ();
			}
			_idLanPrev = _idLan;
		}

		[ContextMenu("GetUITexts")]
		public void GetUITexts()
		{
			_uiTxts = GetComponentsInChildren<Text> ();
		}

		[ContextMenu("AddUITxtLanguageToEach")]
		public void AddUITxtLanguageToEach()
		{
			foreach (Text uitxt in _uiTxts) {
				UITextLanguage tlan = uitxt.GetComponent<UITextLanguage> ();
				if (tlan==null) {
					tlan = uitxt.gameObject.AddComponent<UITextLanguage> ();
				} 
			}
		}

		[ContextMenu("SetIDLanForEach")]
		public void SetIDLanForEach()
		{
			foreach (Text uitxt in _uiTxts) {
				UITextLanguage tlan = uitxt.GetComponent<UITextLanguage> ();
				if (tlan != null) {
					tlan._LanID = _idLan;
				}
				tlan.SetUIText ();
			}
		}
		/*
		[ContextMenu("SetRotZs")]
		public void SetRotZs()
		{
			foreach (Text uitxt in _uiTxts) {
				UITextLanguage tlan = uitxt.GetComponent<UITextLanguage> ();
				if (tlan != null) {
					tlan.SetRotZ(_RotZ);
				}
			}
		}

		[ContextMenu("SetFontSizes")]
		public void SetRotZs()
		{
			foreach (Text uitxt in _uiTxts) {
				UITextLanguage tlan = uitxt.GetComponent<UITextLanguage> ();
				if (tlan != null) {
					tlan.SetFontSizes(_FontSizes);
				}
			}
		}
		*/

	}
}
