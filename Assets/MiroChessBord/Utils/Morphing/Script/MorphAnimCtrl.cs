using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Lyu
{
	public class MorphAnimCtrl : MonoBehaviour {
		

		public Animator _Anim;
		public char _Char;


		public KeyCode _Up,_Down;

		// Use this for initialization
		void Start () {
			
			
		}
		
		// Update is called once per frame
		void Update () {
			//Debug.Log ("index: " + index);

			_Anim.SetInteger ("index", _Char);

			if (Input.GetKeyDown (_Up)) {
				_Char++;
			} else if (Input.GetKeyDown (_Down)) {
				_Char--;
			}


		}

		public void SetIndex(int id)
		{
			_Char = (char)id;
		}

		public void SetIndexFloat(float idf)
		{
			SetIndex ((int)idf);
		}
	}
}
