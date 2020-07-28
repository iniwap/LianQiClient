using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAlphaChange : MonoBehaviour {
    public AnimationCurve _AlphaCurve;
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
        float alpha =
            _ValueScale * _AlphaCurve.Evaluate(_TimeScale * dt);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color cr = sr.color;
        cr.a = alpha;
        sr.color = cr;
    }
}
