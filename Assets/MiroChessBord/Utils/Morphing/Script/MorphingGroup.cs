using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class MorphingGroup : MonoBehaviour {

		public float _LerpSpd = 1.0f;
		public List<CharPrimitive> _CharPrims = 
			new List<CharPrimitive> ();
		public CharStyleBase _GlobalStyle;
		public GameObject _CharPrefab;
		public Transform _CharParentTF;
		private List<GameObject> _Chars =
			new List<GameObject>();
		
		// Use this for initialization
		void Start () {
			//InitChars ();
		}
		
		// Update is called once per frame
		void Update () {
			
			int pCount = _CharPrims.Count;
			int cCount = _Chars.Count;
			int Inc = pCount - cCount;
			/*
			if (Inc > 0) {
				for (int i = cCount; i < pCount; i++) {
					NewAnimChar (_CharPrims[i]);
				}
			}
			*/

			foreach (CharPrimitive cp in _CharPrims) {
				if (cp._CharObj == null) {
					NewAnimChar (cp);
				}
			}

			if (Inc < 0) {

				foreach (GameObject charObj in _Chars) {
					bool bContain = false;
					foreach (CharPrimitive cp in _CharPrims) {
						if (cp._CharObj == charObj) {
							bContain = true;
							break;
						}
					}
					if (!bContain) {
						_Chars.Remove (charObj);
						Destroy (charObj);
						return;
					}
				}
					
			}

			float dt = Time.deltaTime;
			float lt = dt * _LerpSpd;
			foreach (CharPrimitive cp in _CharPrims) {
				cp.UpdateChar (lt);
			}

			if (_GlobalStyle != null) {
				foreach (GameObject charObj in _Chars) {
					_GlobalStyle.SetStyle (charObj);
				}
			}

		}

		[ContextMenu("InitChars")]
		public void InitChars()
		{
			ClearCharObjs ();

			foreach (CharPrimitive prim in _CharPrims) {
				NewAnimChar (prim);
			}
		}

		void ClearCharObjs ()
		{
			foreach (GameObject charObj in _Chars) {
				Destroy (charObj);
			}
			_Chars.Clear ();
		}

		[ContextMenu("ClearCharObjImmediately")]
		public void ClearCharObjImmediately()
		{
			foreach (GameObject item in _Chars) {
				DestroyImmediate (item);
			}
			_Chars.Clear ();
		}

		void NewAnimChar (CharPrimitive prim)
		{
			GameObject newCharObj = Instantiate (_CharPrefab) as GameObject;
			InitNewCharObjTF (newCharObj);
			prim.LinkCharObj(newCharObj);
			prim.UpdateChar ();
			_Chars.Add (newCharObj);
		}

		void InitNewCharObjTF (GameObject newCharObj)
		{
			newCharObj.transform.SetParent (_CharParentTF);
			newCharObj.transform.localPosition = Vector3.zero;
			newCharObj.transform.localRotation = Quaternion.identity;
			newCharObj.transform.localScale = Vector3.one;
		}

		public void LoadFromAnother(MorphingGroup other)
		{
			int cntOther = other._CharPrims.Count;
			int cntThis = _CharPrims.Count;

			if (cntThis > cntOther) {
				int rmCnt = cntThis - cntOther;
				for (int i = 0; i < rmCnt; i++) {
					RemoveLastChar ();
				}
			} else if(cntThis < cntOther){
				int addCnt = cntOther - cntThis;
				for (int i = 0; i < addCnt; i++) {
					_CharPrims.Add (new CharPrimitive ());
				}
			}
				
			for (int i = 0; i < _CharPrims.Count; i++) {
				_CharPrims [i].CopyFromAnother (other._CharPrims [i]);
			}
		}

		public void RemoveChar(int id)
		{
			if (id <= _CharPrims.Count - 1) {
				Destroy (_Chars [id]);
				_Chars.RemoveAt (id);
				_CharPrims.RemoveAt (id);
			}
		}
		public void RemoveLastChar()
		{
			int id = _CharPrims.Count - 1;
			RemoveChar (id);
		}

		public void SetCharIndexAt(int charPrimId, int charIndex)
		{
			if (charPrimId < 0) {
				return;
			}
			if (charPrimId >= _CharPrims.Count) {
				int newCount = charPrimId - _CharPrims.Count +1;
				for (int i = 0; i < newCount; i++) {
					CharPrimitive cp = new CharPrimitive ();
					_CharPrims.Add (cp);
					NewAnimChar (cp);
				}
			}
			_CharPrims [charPrimId]._Index = charIndex;



		}
	}
}
