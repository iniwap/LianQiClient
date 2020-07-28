using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class ChessPlacerUI : MonoBehaviour {

		public MiroV1PlacementMgr _MiroMgr;
		public ChessDragRotaterMgr _DragRotMgr;

		public bool[] _BanDirs = new bool[6];
		public UnityEvent _BanDirsChanged;
		private bool[] _BanDirsPrev = new bool[6];

		public ChessCtrlENLine _enLine;

		// Use this for initialization
		void Start () {
			_BanDirsChanged.Invoke ();
		}
		
		// Update is called once per frame
		void Update () {
			if (CheckChange ()) {
				ActOnChange ();
			}
			RecordBanDirs ();
		}

		bool CheckChange()
		{
			bool bChanged = false;
			for (int i = 0; i < 6; i++) {
				if (_BanDirsPrev [i] != _BanDirs [i]) {
					bChanged = true;
					break;
				}
			}
			return bChanged;
		}

		void ActOnChange ()
		{
			List<int> banDirs = new List<int> ();
			for (int i = 0; i < 6; i++) {
				if (_BanDirs [i]) {
					banDirs.Add (i);
				}
			}
			_DragRotMgr.SetBanDirs (banDirs);
			_BanDirsChanged.Invoke ();
		}

		public void InvokeBanDirsChanged()
		{
			_BanDirsChanged.Invoke ();
		}

		void RecordBanDirs ()
		{
			for (int i = 0; i < 6; i++) {
				_BanDirsPrev [i] = _BanDirs [i];
			}
		}

		public List<int> GetBanDirs()
		{
			List<int> banDirs = new List<int> ();
			for (int i = 0; i < 6; i++) {
				if (_BanDirs [i]) {
					banDirs.Add (i);
				}
			}
			return banDirs;
		}

		public void SetBanDirs(List<int> banDirs)
		{
			for (int i = 0; i < 6; i++) {
				_BanDirs [i] = false;
			}

			for (int i = 0; i < banDirs.Count; i++) {
				int bdir = banDirs [i];
				bdir = (int)Mathf.Repeat ((float)bdir, 6.0f);
				_BanDirs [bdir] = true;
			}
		}

		public void SetBanDir(bool ban ,int dir)
		{
			_BanDirs [dir] = ban;
		}


		public void SetSpawningMiro(MiroModelV1 miro)
		{
			_enLine.SetTgtMiro (miro);
		}

		public void SetSpawningMiroTF(Transform miroTF)
		{
			_enLine.SetTgtMiroTF (miroTF);
		}

		public void ClearSpawningMiro()
		{
			_enLine.ClearTgtMiro ();
		}



	}
}
