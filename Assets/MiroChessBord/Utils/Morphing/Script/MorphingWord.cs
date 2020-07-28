using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class MorphingWord : MonoBehaviour {
		public MorphingGroup _mphGroup;

		public string _Word;
		private string _WordPrev;
		public Vector3 _LocScale = Vector3.one;
		public float _CharInterval = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_Word != _WordPrev) {
				int charCnt = _Word.Length;

				int cpCnt = 
					_mphGroup._CharPrims.Count;

				if (charCnt < cpCnt) {
					int rmCount = cpCnt - charCnt;
					for (int i = 0; i < rmCount; i++) {
						_mphGroup.RemoveLastChar ();
					}
				} 

				for (int i = 0; i < charCnt; i++) {
					_mphGroup.SetCharIndexAt (i, _Word [i]);
				}

				float startX = -(float)(_mphGroup._CharPrims.Count - 1) *
					_CharInterval / 2.0f;
				for (int i = 0; i < _Word.Length; i++) {
					float x = startX + i * _CharInterval;
					_mphGroup._CharPrims [i]._LPos = new Vector3 (x, 0.0f, 0.0f);
					_mphGroup._CharPrims [i]._LScl = _LocScale;
				}

				_WordPrev = _Word;
			}
		}

		public void SetWord(string word)
		{
			_Word = word;
		}
	}
}
