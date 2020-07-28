using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroRing : MonoBehaviour {

		public List<Transform> _RingObjTfs= new List<Transform>();
		[Range(1,5)]
		public int _MaxBallCount = 3;
		public GameObject _RingBallPref;
		public bool _Clockwise = true;
		public BezierCurveFromTFs _BCurveFromTFs;
		public GenBezierCoordObjs _BCoordObjsGenerator;
		public int _CurveResOnObj = 9;

		public Animator _AnimRingLine;
		//public UnityEvent _Shrink;

		[System.Serializable]
		public class RingObjsEvents: UnityEvent< List<Transform> >{}
		public RingObjsEvents _Shrink;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void ClearRingObjTfs()
		{
			_RingObjTfs.Clear ();
		}

		public void DieOnTFMissing()
		{
			foreach (Transform tf in _RingObjTfs) {
				if (tf == null) {
					Destroy (gameObject);
				}
			}
		}

		public void AddObjAsRingObj(GameObject obj)
		{
			if(_RingObjTfs.Contains(obj.transform))
			{
				return;
			}
			_RingObjTfs.Add (obj.transform);
		}

		[ContextMenu("InitRing")]
		public void InitRing()
		{
			int count = (_RingObjTfs.Count) * _CurveResOnObj;
			_BCoordObjsGenerator.SetCount( count );
			_BCoordObjsGenerator.SetAlongStep (1.0f / (float)count);

			_BCurveFromTFs.SetListTFs (_RingObjTfs);
			_BCurveFromTFs.GenerateBezierCurve ();
		}

		[ContextMenu("InitRingBalls")]
		public void InitYinYangBalls()
		{
			int edgeObjCount = _BCoordObjsGenerator._BCoords.Count;

			int objCount = _RingObjTfs.Count;

			float objStep = (float)edgeObjCount / (float)objCount;

			InitRingBallsForEachObj (edgeObjCount, objCount, objStep);

			_AnimRingLine.SetBool ("Grown", true);
		}

		//public float BallStepMultiplier = 1.0f;
		void InitRingBallsForEachObj (int edgeObjCount, int objCount, float objStep)
		{
			for (int i = 0; i < objCount; i++) {

				GameObject MiroObj = _RingObjTfs [i].gameObject;
				RingBalls ringBalls = MiroObj.GetComponentInChildren<RingBalls> ();
				if (ringBalls == null) {
					continue;
				}

				float idCtr = objStep * (float)i;
				int ballCount = Mathf.Min (_MaxBallCount, objCount);
				float ballStep = objStep / (float)(ballCount + 1);
				int ballIdMin = (int)(idCtr - ballStep * (float)ballCount / 2.0f);
				List<int> ObjHPs = new List<int> ();
				for (int k = 0; k < ballCount; k++) {
					ObjHPs.Add (0);
				}
				int hp = objCount;
				for (int p = 0; p < hp; p++) {
					int idObj = (int)Mathf.Repeat (p, ObjHPs.Count);
					ObjHPs [idObj]++;
				}
				for (int k = 0; k < ballCount; k++) {
					float ballIdf = (float)ballIdMin + (float)k * ballStep;
					int id0 = Mathf.FloorToInt (ballIdf) + edgeObjCount;
					int id1 = Mathf.CeilToInt (ballIdf) + edgeObjCount;
					id0 = (int)Mathf.Repeat (id0, edgeObjCount-1);
					id1 = (int)Mathf.Repeat (id1, edgeObjCount-1);
					if (id0 == id1) {
						id1++;
					}
					Transform tf0 = _BCoordObjsGenerator._BCoords [id0].transform;
					Transform tf1 = _BCoordObjsGenerator._BCoords [id1].transform;
					GameObject newRingBall = Instantiate (_RingBallPref) as GameObject;

					InitRingBallPlacer (tf0, tf1, newRingBall);
					MiroV1RingBallBase ringBall = newRingBall.GetComponent<MiroV1RingBallBase> ();
					ringBall._HPMax = ObjHPs [k];
					ringBall._HP = ObjHPs [k];
					ringBall.SetRing (this);
					ringBalls.AddBall (ringBall);
				}



				//ringBalls.Grow ();
			}
		}

		void InitRingBallPlacer (
			Transform tf0, Transform tf1, GameObject newRingBall)
		{
			PlaceAlongLineSegment Placer = newRingBall.GetComponent<PlaceAlongLineSegment> ();
			if (Placer == null) {
				Placer = newRingBall.AddComponent<PlaceAlongLineSegment> ();
			}
			Placer._A = tf0;
			Placer._B = tf1;
			if (_Clockwise) {
				Placer._RotBias.z = 90.0f;
			}
			else {
				Placer._RotBias.z = -90.0f;
			}
		}

		[ContextMenu("Shrink")]
		public void Shrink()
		{
			ShrinkEveryRingBall ();
			Invoke ("ShrinkRingLine", 2.0f);
			//ShrinkRingLine ();

			MiroV1TimeToDie timeDie = 
				gameObject.AddComponent<MiroV1TimeToDie> ();
			timeDie._LeftTime = 2.0f;

			_Shrink.Invoke (_RingObjTfs);
		}

		void ShrinkRingLine ()
		{
			_AnimRingLine.SetBool ("Grown", false);
		}

		void ShrinkEveryRingBall ()
		{
			for (int i = _RingObjTfs.Count - 1; i >= 0; i--) {
				if (_RingObjTfs [i] == null) {
					_RingObjTfs.RemoveAt (i);
				}
			}

			foreach (Transform tf in _RingObjTfs) {
				/*
				MiroModelV1 miro = tf.gameObject.GetComponent<MiroModelV1> ();
				miro.DestroyEveryRingBall ();
				*/

				RingBalls ringballs = tf.gameObject.GetComponentInChildren<RingBalls> ();
				ringballs.DestroyBalls ();
				/*
				foreach (MiroV1RingBallBase ball in ringballs._Balls) {
					Destroy (ball.gameObject);
					print ("Destroy:" + ball.gameObject);
				}
				ringballs.ClearBalls ();
				*/

			}
		}
			
		public void TurnDynamics(bool bON)
		{
			NoisePosFromBegin[] nposes = 
				_BCurveFromTFs.GetComponentsInChildren<NoisePosFromBegin> ();

			foreach (var np in nposes) {
				np.enabled = bON;
			}

			PlaceAlongLineSegment[] placers =
				GetComponentsInChildren<PlaceAlongLineSegment> ();
			foreach (var p in placers) {
				p.enabled = bON;
			}

			if (bON) {
				_AnimRingLine.StartPlayback ();
			} else {
				_AnimRingLine.StopPlayback ();
			}
		}









	}
}
