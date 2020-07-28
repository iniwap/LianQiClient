using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class ChessBoardEdit : MonoBehaviour {
		public Toggle [] _Tgs;
		public List<GameObject> _Panels = new List<GameObject> ();

		public CellSelectionMgr _CellSelMgr;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("GetToggles")]
		public void GetToggles()
		{
			_Tgs = GetComponentsInChildren<Toggle> ();
		}

		public void TurnONEditting()
		{
			TurnEditing (true);
		}

		public void TurnOFFEditting()
		{
			TurnEditing (false);
		}

		public void TurnEditing(bool bON)
		{
			for (int i = 0; i < _Tgs.Length; i++) {
				Toggle tg = _Tgs [i];
				//tg.interactable = bON;

				GameObject gb = _Panels [i];
				if (bON) {
					gb.SetActive (tg.isOn);
				} else {
					gb.SetActive (false);
				}
			}

			_CellSelMgr.Turn (bON);
			if (!bON) {
				_CellSelMgr.Unselect ();
			} 

		}
	}
}