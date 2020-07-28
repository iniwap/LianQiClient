using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

namespace MiroV1
{
	public class MiroV1ENContainerColorSetting : MonoBehaviour {
		public MiroV1ModelSetting _modelSetting; 
		public Shape _shape;

		// Use this for initialization
		void Start () {
			if (_shape == null) {
				_shape = GetComponent<Shape> ();
			}
			SetColor ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("SetColor")]
		public void SetColor()
		{
			_shape.settings.fillColor = _modelSetting._colorSetting._ENBG;
		}
	}
}
