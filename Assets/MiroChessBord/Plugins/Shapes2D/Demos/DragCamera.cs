namespace Shapes2D {

	using UnityEngine;
	using System.Collections;

	public class DragCamera : MonoBehaviour {
		Vector3 anchor;
		Vector3 origin;
		bool dragging;

		void LateUpdate () {
			if (Input.GetMouseButtonDown(0)) {
				dragging = true;
				anchor = Input.mousePosition;
				origin = Camera.main.transform.position;
			}
			if (Input.GetMouseButtonUp(0)) {
				dragging = false;
			}
			if (dragging) {
				Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition - anchor 
						- new Vector3(-Screen.width / 2, -Screen.height / 2, Camera.main.transform.position.z)) 
						- new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
				Camera.main.transform.position = origin - delta;
			}
		}
	}

}