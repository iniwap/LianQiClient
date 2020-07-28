using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetChildObjName : MonoBehaviour {

		public List<GameObject> _Children = new List<GameObject>();

		[System.Serializable]
		public class NameField
		{
			public enum Type
			{
				TEXT,
				TIME,
				ID,
				RAND,
				POS,
				ORIGINAL,
				ARITH_SEQUENCE
			}
			public Type _Type;
			public string _Text;
			public int _SeqStart =0, _SeqStep = 1;
		}
		public List<NameField> _NamingRules = new List<NameField>();

		[ContextMenu("GetChildrenLV1")]
		public void GetChildrenLV1()
		{
			_Children.Clear ();
			int chdCnt = transform.childCount;
			for (int i = 0; i < chdCnt; i++) {
				Transform tf = transform.GetChild (i);
				_Children.Add (tf.gameObject);
			}
		}

		[ContextMenu("SetName")]
		public void SetName()
		{
			for (int i = 0; i < _Children.Count; i++) {
				GameObject gb = _Children [i];
				string Name = "";
				for (int ri = 0; ri < _NamingRules.Count; ri++) {
					NameField nf = _NamingRules [ri];
					if (nf._Type == NameField.Type.ID) {
						Name += i.ToString ();
					} else if (nf._Type == NameField.Type.POS) {
						Name += "(" + gb.transform.position.ToString () + ")";
					} else if (nf._Type == NameField.Type.RAND) {
						Name += Random.Range (0, 9999999);
					} else if (nf._Type == NameField.Type.TEXT) {
						Name += nf._Text;
					} else if (nf._Type == NameField.Type.TIME) {
						Name += Time.realtimeSinceStartup.ToString ();
					} else if (nf._Type == NameField.Type.ORIGINAL) {
						Name += gb.name;
					} else if (nf._Type == NameField.Type.ARITH_SEQUENCE) {
						string text = (nf._SeqStart + i * nf._SeqStep).ToString ();
						Name += text;
					}
				}
				gb.name = Name;
			}
		}




	}
}
