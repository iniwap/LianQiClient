using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class UIMusic : MonoBehaviour {

		public TuneBGMusicByPreceed _tuneBGMusic;

		public Text _TxtVolume, _TxtPitchDistort;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetVolume(float volume)
		{
			_tuneBGMusic.SetVolume (volume);
			_TxtVolume.text = volume.ToString ();
		}

		public void SetDistortionPwr(float dp)
		{
			_tuneBGMusic.SetDistortionPwr (dp);
			_TxtPitchDistort.text = dp.ToString ();
		}

	}
}
