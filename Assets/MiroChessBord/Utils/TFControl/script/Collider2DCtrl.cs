using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class Collider2DCtrl : MonoBehaviour {

		public List<Collider2D> _AllColliders = 
			new List<Collider2D>();

		public List<Collider2D> _colliders = 
			new List<Collider2D>();
		public List<Collider2D> _triggers = 
			new List<Collider2D>();


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("GetChildCollider2Ds")]
		public void GetChildCollider2Ds ()
		{
			ClearContainers ();

			Collider2D [] clds = 
				GetComponentsInChildren<Collider2D> ();
			_AllColliders = new List<Collider2D> (clds);
			foreach (Collider2D cld in _AllColliders) {
				if (cld.isTrigger) {
					_triggers.Add (cld);
				} else {
					_colliders.Add (cld);
				}
			}

		}

		void ClearContainers ()
		{
			_AllColliders.Clear ();
			_colliders.Clear ();
			_triggers.Clear ();
		}
			
		public void TurnColliders(bool bEnable)
		{
			foreach (Collider2D cld in _colliders) {
				cld.enabled = bEnable;
			}
		}

		public void TurnTriggers(bool bEnable)
		{
			foreach (Collider2D cld in _triggers) {
				cld.enabled = bEnable;
			}
		}

		public void TurnAllColliders(bool bEnable)
		{
			foreach (Collider2D cld in _AllColliders) {
				cld.enabled = bEnable;
			}
		}



	}
}
