using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class RingBalls : MonoBehaviour {
		public List<MiroV1RingBallBase> _Balls = 
			new List<MiroV1RingBallBase>();

		[System.Serializable]
		public class EventWithGameObject: UnityEvent<GameObject>{}; 
		public  List<EventWithGameObject> _RingBallAdded = 
			new List<EventWithGameObject>();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}



		public void AddBall(MiroV1RingBallBase ball)
		{

			ball.transform.SetParent (transform);

			MiroV1BlackDotMgr mgr = GetComponentInParent<MiroV1BlackDotMgr> ();
			mgr.NewBlackDot (ball);

			_Balls.Add (ball);

			/*
			int count = _Balls.Count;
			if (count >= _RingBallAdded.Count)
				return;





			if (!_Balls.Contains (ball)) {
				EventWithGameObject evnt = _RingBallAdded [count];
				evnt.Invoke (ball.gameObject);
				_Balls.Add (ball);
			}
			*/
		}

		public void ClearBalls()
		{
			_Balls.Clear ();
		}

		public void DestroyBalls()
		{
			foreach (MiroV1RingBallBase ball in _Balls) {
				Destroy (ball.gameObject);
			}
			_Balls.Clear ();
		}

		[ContextMenu("ShrinkRing")]
		public void ShrinkRing()
		{
			if (_Balls.Count == 0)
				return;
			_Balls [0].ShringWholeRing ();
			_Balls.Clear ();
		}

		[ContextMenu("Grow")]
		public void Grow()
		{
			foreach (MiroV1RingBallBase ball in _Balls) {
				//Destroy (ball.gameObject);
				ball.Grow();

			}
		}
	}
}
