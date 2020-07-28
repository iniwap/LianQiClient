using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetRect : MonoBehaviour {

		public float _Bottom = 0;
		public float _Top = 0;

		// Use this for initialization
		void Start () {
			
			SetRectProps ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetRectProps()
		{
			RectTransform rt = GetComponent<RectTransform> ();
			rt.offsetMin = new Vector2 (rt.offsetMin.x, _Bottom);
			rt.offsetMax = new Vector2 (rt.offsetMax.x, _Top);



		}
	}
}
