using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroV1GunEmitter : MonoBehaviour {

	public List<GameObject> _BulletPrefabs = new List<GameObject>();

	public int _BulletType = -1;
	public Transform _Parent = null;

	// Use this for initialization
	void Start () {

		//AddAnimEvents ();
		//clip.AddEvent (aent);

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[ContextMenu("Fire")]
	public void Fire()
	{
		if (_BulletType >= 0 && 
			_BulletType <= _BulletPrefabs.Count - 1) {

			Vector3 lscl = _BulletPrefabs [_BulletType].transform.localScale;

			GameObject newBullet = Instantiate (
				_BulletPrefabs [_BulletType]) as GameObject;
			newBullet.transform.SetParent (transform, false);

			//newBullet.transform.localPosition = Vector3.zero;
			newBullet.transform.SetParent (_Parent, true);
			newBullet.transform.localScale = lscl;

			Debug.Log ("Fire Bullet:" + _BulletType + " GameObject:" + gameObject.name);

		}
	}

	void AddAnimEvents ()
	{
		Animator Anim = GetComponent<Animator> ();
		AnimationEvent aent = new AnimationEvent ();
		aent.time = 35.0f / 60.0f;
		aent.functionName = "Fire";
		//AnimationClip clip = Anim.runtimeAnimatorController.animationClips [0];
		foreach (AnimationClip aclip in Anim.runtimeAnimatorController.animationClips) {
			aclip.AddEvent (aent);
		}
	}
}
