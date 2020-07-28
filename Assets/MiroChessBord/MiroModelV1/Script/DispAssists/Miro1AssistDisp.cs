using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class Miro1AssistDisp : MonoBehaviour {

		public List<Renderer> _Renderers = new List<Renderer>();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (Renderer rdr in _Renderers) {
				rdr.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (Renderer rdr in _Renderers) {
				rdr.enabled = false;
			}
		}
	}
}
