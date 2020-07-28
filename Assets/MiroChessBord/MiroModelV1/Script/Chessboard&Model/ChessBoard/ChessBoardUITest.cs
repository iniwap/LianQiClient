using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ChessBoardUITest : MonoBehaviour {

		public KeyCode _UIEditorKey, _UIPortTestKey;

		public GameObject _UIEidtor, _UIPortTest;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			bool bTurnEditorUI = 
				Input.GetKeyUp (_UIEditorKey);
			if (bTurnEditorUI) {
				_UIEidtor.SetActive (!_UIEidtor.activeSelf);
			}

			bool bTurnTestUI =
				Input.GetKeyUp (_UIPortTestKey);
			if (bTurnTestUI) {
				_UIPortTest.SetActive (!_UIPortTest.activeSelf);
			}
			
		}
	}

}