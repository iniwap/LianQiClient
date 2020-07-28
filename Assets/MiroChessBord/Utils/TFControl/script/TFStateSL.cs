using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TFStateSL : MonoBehaviour {

		[System.Serializable]
		public class TFState
		{
			public Vector3 _Pos;
			public Quaternion _Rot;
			public Vector3 _Scale;
			public TFState(Transform tf)
			{
				SaveState(tf);
			}
			public void SaveState(
				Transform tf)
			{
					_Pos = tf.localPosition;
					_Rot = tf.localRotation;
					_Scale = tf.localScale;

			}
			public void LoadState(
				Transform tf)
			{
				tf.localPosition = _Pos;
				tf.localRotation = _Rot;
				tf.localScale = _Scale;
			}

			public void LerpState (
				Transform tf, float t)
			{
				tf.localPosition = Vector3.Lerp (
					tf.localPosition, _Pos, t);
				tf.localRotation = Quaternion.Lerp (
					tf.localRotation, _Rot, t);
				tf.localScale = Vector3.Lerp (
					tf.localScale, _Scale, t);
			}

		}
		public  Transform _TF;

		public List<TFState> _States = new List<TFState>();

		public int _TgtStateId = 0;
		public bool _bLerpingState = false;
		public float _LerpSpd = 1.0f;

		public GameObject _Prefab;

		// Use this for initialization
		void Start () {
			if (_TF == null) {
				_TF = transform;
			}
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_TgtStateId >= 0 &&
				_TgtStateId < _States.Count &&
				_bLerpingState) {
				float t = Mathf.Clamp01(Time.deltaTime * _LerpSpd);
				_States [_TgtStateId].LerpState (_TF, t);
			}
		}


		[ContextMenu("SaveState")]
		public void SaveState()
		{
			_States.Add (new TFState (_TF));
		}

		[ContextMenu("SaveToPrefab")]
		public void SaveToPrefab()
		{
			TFStateSL tfs = _Prefab.GetComponent<TFStateSL> ();
			tfs.LoadStates (this);
		}

		[ContextMenu("LoadFromPrefab")]
		public void LoadFromPrefab()
		{
			TFStateSL tfs = _Prefab.GetComponent<TFStateSL> ();
			LoadStates (tfs);
		}

		private void LoadStates(TFStateSL prefabStates)
		{
			_States = prefabStates._States;
		}
	}
}