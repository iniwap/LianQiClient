using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TurnAllRenderer : MonoBehaviour {

		public Renderer[] _renderers;

		[ContextMenu("GetAllChildRenderers")]
		public void GetAllChildRenderers()
		{
			_renderers = 
				GetComponentsInChildren<Renderer> ();
		}

		[ContextMenu("TurnOnRenderers")]
		public void TurnOnRenderers()
		{
			foreach (Renderer rdr in _renderers) {
				rdr.enabled = true;
			}
		}

		[ContextMenu("TurnOffRenderers")]
		public void TurnOffRenderers()
		{
			foreach (Renderer rdr in _renderers) {
				rdr.enabled = false;
			}
		}

	}
}
