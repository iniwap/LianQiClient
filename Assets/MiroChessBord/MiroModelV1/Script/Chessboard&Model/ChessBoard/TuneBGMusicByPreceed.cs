using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class TuneBGMusicByPreceed : MonoBehaviour {

		public ChessSituationStats _chessSituStats;
		public MiroV1PlacementMgr _mgr;

		public float _LerpSpd = 1.0f;

		private float _tgtPitch = 1.0f;

		[Range(0.0f,5.0f)]
		public float _DistortionPwr = 1.0f;

		[Range(0.0f,1.0f)]
		public float _Volume = 1.0f;

		public float _PitchMin=0.4f, _PitchMax=3.0f;

		// Use this for initialization
		void Start () {
			
		}

		public void SetDistortionPwr(float dp)
		{
			_DistortionPwr = dp;
		}

		public void SetVolume(float v)
		{
			_Volume = v;
		}
		
		// Update is called once per frame
		void Update () {
			float dt = Time.deltaTime;

			AudioSource audioSrc = GetComponent<AudioSource> ();
			float cPitch = audioSrc.pitch;
			cPitch = Mathf.Lerp(cPitch,_tgtPitch, _LerpSpd*dt);
			audioSrc.pitch = cPitch;
			audioSrc.volume = _Volume;
		}

		public void UpdateTgtPitch()
		{
			string campName = 
				_mgr.GetSelectedMiroPrefabCampName ();

			float preceed = 
				_chessSituStats.GetPreceedScore (campName);
			//print ("Preceed:" + preceed);

			float pitch = Mathf.Pow (preceed, _DistortionPwr);
			pitch = Mathf.Clamp (pitch, _PitchMin, _PitchMax);
			_tgtPitch = pitch;
		}
	}
}
