using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	[System.Serializable]
	public class CharPrimitive
	{
		public Vector3 _LPos = Vector3.zero;
		public Vector3 _LRot = Vector3.zero;
		public Vector3 _LScl = Vector3.one;

		public int _Index = 48;

		public CharStyleBase _Style = null;

		public GameObject _CharObj = null;

		public CharPrimitive()
		{
			_LPos = Vector3.zero;
			_LRot = Vector3.zero;
			_LScl = Vector3.one;
			_Index = 48;
		}

		~CharPrimitive()
		{
			//Destroy (_CharObj);
		}

		public void CopyFromAnother(CharPrimitive other)
		{
			_LPos = other._LPos;
			_LRot = other._LRot;
			_LScl = other._LScl;
			_Index = other._Index;
		}

		public void LinkCharObj(GameObject charObj)
		{
			_CharObj = charObj;
		}

		public void UpdateChar(float t = 1.0f)
		{
			if (_CharObj == null) {
				return;
			}
			LerpLocTF (t);
			SetCharIndex ();
			if (_Style != null) {
				_Style.SetStyle (_CharObj);
			}
			_CharObj.name = _Index.ToString () + _LPos.ToString ();
		}

		private void LerpLocTF (float t)
		{
			t = Mathf.Clamp01 (t);
			
			Vector3 lpos = _CharObj.transform.localPosition;
			Quaternion lrot = _CharObj.transform.localRotation;
			Vector3 lscl = _CharObj.transform.localScale;

			lpos = Vector3.Lerp (lpos, _LPos, t);
			lrot = Quaternion.Lerp (lrot, Quaternion.Euler (_LRot), t);
			lscl = Vector3.Lerp (lscl, _LScl, t);

			_CharObj.transform.localPosition = lpos;
			_CharObj.transform.localRotation = lrot;
			_CharObj.transform.localScale = lscl;
		}

		private void SetCharIndex ()
		{
			MorphAnimCtrl manim = _CharObj.GetComponent<MorphAnimCtrl> ();
			manim.SetIndex (_Index);
		}
	}
}
