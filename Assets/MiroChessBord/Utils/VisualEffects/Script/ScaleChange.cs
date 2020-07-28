using UnityEngine;
using System.Collections;

public class ScaleChange : MonoBehaviour {
    public AnimationCurve _ScaleCurve;
    public float _TimeScale = 1.0f;
    public float _ValueScale = 1.0f;

    private float _BirthTime;
	// Use this for initialization
	void Start () {
        _BirthTime = Time.realtimeSinceStartup;	
	}
	
	// Update is called once per frame
	void Update () {
        float T = Time.realtimeSinceStartup;
        float dt = T - _BirthTime;
        float scl = 
            _ValueScale*_ScaleCurve.Evaluate(_TimeScale*dt);
        transform.localScale = new Vector3(scl, scl, 1.0f);	
	}
}
