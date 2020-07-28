using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartAgent;

namespace MiroV1
{
	public class MiroV1Hole2ENEmitAbsorb : MonoBehaviour {

		public GameObject _ENBallPrefab;

		public Transform _SourceTF;

		public List<Transform> _Path = new List<Transform> ();
		public Color _ENColor;

		public Transform _ENBallParent;
		public Vector3 _PosBias;

		[ContextMenu("Emit")]
		public void Emit()
		{
			GameObject newBall = Instantiate (_ENBallPrefab) as GameObject;
			SetENBallTF (newBall);
			SetENBallTarget (newBall);
			SetENBallPath (newBall);
			SetENBallColor (newBall);
		}

		void SetENBallTF (GameObject newBall)
		{
			newBall.transform.SetParent (_ENBallParent);
			newBall.transform.position = _SourceTF.position + _PosBias;
		}

		void SetENBallTarget (GameObject newBall)
		{
			TargetTransform TgtTF = newBall.GetComponent<TargetTransform> ();
			TgtTF._Target = transform;
		}

		void SetENBallPath (GameObject newBall)
		{
			SA_SeekAlongPath SeekPth = newBall.GetComponent<SA_SeekAlongPath> ();
			SeekPth.SetPathPointsList (_Path);
		}

		void SetENBallColor (GameObject newBall)
		{
			SpriteRenderer SR = newBall.GetComponent<SpriteRenderer> ();
			SR.color = _ENColor;
		}
	}
}
