using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellSpriteCtrl : MonoBehaviour {

		public CellObjCtrl _cellObjCtrl;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void TurnSpriteOnBlockState()
		{
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			sr.enabled = !_cellObjCtrl.IsBlocked();

		}
	}
}
