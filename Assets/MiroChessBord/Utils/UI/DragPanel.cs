using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class DragPanel : MonoBehaviour {

		private Vector2 _StartPos;
		private Vector2 _RectStartPos;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		public void StartDrag()
		{
			_StartPos = Input.mousePosition;
			RectTransform rtf = GetComponent<RectTransform> ();
			_RectStartPos = rtf.position;
			Debug.Log ("OnMouseDown");
		}

		public void Dragging()
		{
			Vector2 pos = Input.mousePosition;
			Vector2 movement = pos - _StartPos;

			Vector2 rectPos = _RectStartPos + movement;
			RectTransform rtf = GetComponent<RectTransform> ();
			rtf.position = rectPos;
			Debug.Log ("OnMouseDrag");
		}



	}
}
