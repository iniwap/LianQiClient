using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lyu
{
	public class FPSText : MonoBehaviour {

		float _fps = 0.0f;
		public float _LerpSpd = 1.0f;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Text fpsText = GetComponent<Text> ();
			float fps = 1.0f / Time.deltaTime;

			_fps = Mathf.Lerp (fps, fps, Time.deltaTime * _LerpSpd);
			float dfps = Mathf.Round( _fps * 10.0f)/10.0f;
			fpsText.text = "FPS:" + dfps.ToString ();
			
		}
	}
}
