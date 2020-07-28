using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeShellBeatingCtrl : MonoBehaviour {

		[Range(0,2)]
		public int _BeatingLevel = 0;
		private int _BeatingLevelPrev;

		[System.Serializable]
		public class State
		{
			public List<Vector2> _Vertices = 
				new List<Vector2>();
			public string _Name;
		}

		public List<State> _States;
		public MiroV1PipeShellCtrl _PipeShellCtrl;

		// Use this for initialization
		void Start () {
			_BeatingLevelPrev = _BeatingLevel;
		}
		
		// Update is called once per frame
		void Update () {
			if (_BeatingLevel != _BeatingLevelPrev) {
				UpdateBeatingAnim ();
				_BeatingLevelPrev = _BeatingLevel;
			}
			
		}

		[ContextMenu("UpdateBeatingAnim")]
		public void UpdateBeatingAnim()
		{
			//LineRenderer LR = GetComponent<LineRenderer> ();
			Animator Anim = GetComponent<Animator> ();
			Anim.SetInteger ("Beating", _BeatingLevel);

			if (_BeatingLevel == 0) {
				_PipeShellCtrl.SetVerticeState (
					_States [0]._Vertices);
			} else if (_BeatingLevel == 1) {
				//Anim.Play (_States [1]._Name);
				_PipeShellCtrl.SetVerticeState (
					_States [1]._Vertices);
			} else if (_BeatingLevel == 2) {
				//Anim.Play (_States [2]._Name);
				_PipeShellCtrl.SetVerticeState (
					_States [2]._Vertices);
			}
		}
	}
}
