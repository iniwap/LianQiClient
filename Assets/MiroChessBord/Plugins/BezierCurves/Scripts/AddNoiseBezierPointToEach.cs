using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddNoiseBezierPointToEach : MonoBehaviour {
	public float _noiseSpd = 1.0f;
	public float _lenMin=0.1f, _lenMax=1.0f;
	public float _angleMax = 90.0f;
	public UnityEvent _AddNoiseBPs;

	[ContextMenu("AddNoiseBezierPoints")]
	public void AddNoiseBezierPoints()
	{
		BezierPoint[] bps = GetComponentsInChildren<BezierPoint> ();
		foreach (BezierPoint bp in bps) {
			NoiseBezierPoint nbp = bp.gameObject.GetComponent<NoiseBezierPoint> ();
			if (nbp == null) {
				nbp = bp.gameObject.AddComponent<NoiseBezierPoint> ();
				nbp._NoiseSpd = _noiseSpd;
				nbp._LengthMin = _lenMin;
				nbp._LengthMax = _lenMax;
				nbp._AngleMax = _angleMax;
			}
		}
		_AddNoiseBPs.Invoke ();
	}
}
