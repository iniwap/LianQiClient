using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MiroV1
{
	public class MiroV1Selection : MonoBehaviour {
		
		[System.Serializable]
		public class EventWithGameObject: UnityEvent<GameObject>{};
		public string _Tag = "";

		public EventWithGameObject _Click,_Enter,_Exit;
		public Renderer _RdrHighlight, _RdrSelection;

		public EventWithGameObject _Select, _Deselect;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		void OnMouseDown()
		{
			Lyu.AttractToNearestCld2D att = 
				GetComponent<Lyu.AttractToNearestCld2D> ();
			Transform tgtTf = att.GetTargetTF ();

			MonoBehaviour[] bhs = 
				tgtTf.gameObject.GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour bh in bhs) {
				
			}
			
			/*
			if (gameObject.tag == _Tag) {
				_Click.Invoke (gameObject);
				if (_RdrSelection.enabled) {
					SelectionOFF ();
				} else {
					SelectionON ();
				}
			}
			*/
		}

		void OnMouseEnter()
		{
			/*
			if (gameObject.tag == _Tag) {
				_Enter.Invoke (gameObject);
				if (!_RdrSelection.enabled) {
					HighlightON ();
				}
			}*/
		}

		void OnMouseExit()
		{
			/*
			if (gameObject.tag == _Tag) {
				_Exit.Invoke (gameObject);
				HighlightOFF ();
			}*/
		}

		public void HighlightON()
		{
			_RdrHighlight.enabled = true;
		}

		public void HighlightOFF()
		{
			_RdrHighlight.enabled = false;
		}

		public void SelectionON()
		{
			_Select.Invoke (gameObject);
			_RdrSelection.enabled = true;
			_RdrHighlight.enabled = false;
		}

		public void SelectionOFF()
		{
			_Deselect.Invoke (gameObject);
			_RdrSelection.enabled = false;
			HighlightON ();
		}
	}
}
