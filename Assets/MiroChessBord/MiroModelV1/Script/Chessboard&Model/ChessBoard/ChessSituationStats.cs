using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class ChessSituationStats : MonoBehaviour {

		public MiroV1PlacementMgr _mgr;
		public HexGridCtrl _gridCtrl;

		public Dictionary<CellObjCtrl,string> _ChessBoardSitu = 
			new Dictionary<CellObjCtrl, string>();

		private Dictionary<string, int> _CampChessCount = 
			new Dictionary<string, int>();

		public AnimationCurve _PreceedScoreONCount;
		private Dictionary<string, float> _CampPreceedScores = 
			new Dictionary<string, float>();
		public UnityEvent _PreceedScoreUpdated;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void UpdateStats()
		{
			UpdateSituDict ();
			UpdateCampChessCountDict ();
			UpdateCampPreceedScore ();
		}

		void UpdateCampChessCountDict ()
		{
			List<string> cnames =_mgr.GetMiroPrefabsCampNames ();
			foreach (string cname in cnames) {
				_CampChessCount [cname] = 0;
			}
			/*
			int prefCnt = _mgr._MiroPrefabs.Count;
			for (int i = 0; i < prefCnt; i++) {
				MiroV1ModelSetting setting = _mgr._MiroPrefabs [i].GetComponent<MiroV1ModelSetting> ();
				if (_CampChessCount.ContainsKey (setting._CampName)) {
					_CampChessCount [setting._CampName] = 0;
				}
			}*/
				
			foreach (GameObject ctrlObj in _gridCtrl._Cells) {
				CellObjCtrl ctrl = ctrlObj.GetComponent<CellObjCtrl> ();
				MiroV1ModelSetting setting = CellObjCtrlUtils.GetModelSettingFromCtrl (ctrl);
				if (setting != null) {
					string cname = setting._CampName;
					if (!_CampChessCount.ContainsKey (cname)) {
						_CampChessCount [cname] = 0;
					}
					else {
						_CampChessCount [cname]++;
					}
				}
			}
		}

		void UpdateSituDict ()
		{
			

			foreach (GameObject ctrlObj in _gridCtrl._Cells) {
				CellObjCtrl ctrl = ctrlObj.GetComponent<CellObjCtrl> ();
				MiroV1ModelSetting setting = CellObjCtrlUtils.GetModelSettingFromCtrl (ctrl);
				string cname = "";
				if (setting != null) {
					cname = setting._CampName;
				}
				_ChessBoardSitu [ctrl] = cname;
			}
		}

		void UpdateCampPreceedScore ()
		{
			float cellCnt = (float)_ChessBoardSitu.Count;
			float sumCnt = 0.0f;
			foreach (KeyValuePair<string, int> item in _CampChessCount) {
				sumCnt += (float)item.Value;
			}
			float averageCnt = sumCnt / (float)_CampChessCount.Count;
			foreach (KeyValuePair<string, int> item in _CampChessCount) {
				float score = (float)item.Value - averageCnt;
				float pscore = _PreceedScoreONCount.Evaluate (score);
				_CampPreceedScores [item.Key] = pscore;
			}

			_PreceedScoreUpdated.Invoke ();
		}

		public float GetPreceedScore(string campName)
		{
			float s = 1.0f;
			if (_CampChessCount.ContainsKey (campName)) {
				s = _CampPreceedScores [campName];
			} else {
				s = 1.0f;
			}

			return s;
		}


	}
}
