using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class ChessDragRotator : MonoBehaviour {
		public CellObjCtrl _cctrl;

		public HashSet<int> _banDirs = new HashSet<int>();
		public UnityEvent _StopDragging;

		[System.Serializable]
		public class EventInt: UnityEvent<int>{}
		public EventInt _DirChanged;


		public void AddBanDir(int dir)
		{
			_banDirs.Add(dir);
		}
		public void RemoveBanDir(int dir)
		{
			_banDirs.Remove (dir);
		}

		public void SetBanDirs(HashSet<int> dirs)
		{
			_banDirs = dirs;
		}

		public void ClearBanDirs()
		{
			_banDirs.Clear ();
		}

		void OnMouseDown()
		{
			
		}

		private int _RightDir =-1;
		public int GetDragingDir()
		{
			return _RightDir;
		}

		private bool _bDragging = false;
		void OnMouseDrag()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (CheckBlock ()) {
				return;
			}

			if (enabled) {
				_bDragging = true;
				//Debug.Log (_cctrl.name + " OnMouseDrag" + " rotate");

				Vector3 hitDir = GetHitDir ();

				int rightDir = _cctrl.GetDir();
				float minAngle = 100000.0f;
				for (int dir = 0; dir < 6; dir++) {
					if (_banDirs.Contains (dir)) {
						continue;
					}
					CellObjCtrl nbCtrl = 
						CellObjCtrlUtils.GetNbCellObjCtrlInAbsDir (_cctrl, dir);
					if (nbCtrl == null) {
						continue;
					}
					Vector3 nbDir = nbCtrl.transform.position - _cctrl.transform.position;
					nbDir.Normalize ();

					float angle = Vector3.Angle (hitDir, nbDir);
					if (angle < minAngle) {
						minAngle = angle;
						rightDir = dir;
					}
				}

				if (rightDir != _cctrl.GetDir()) {
					_cctrl.SetDir(rightDir);
					_cctrl.ChangeDir ();
					//print ("ChangeDir" + rightDir);

					_RightDir = rightDir;
					_DirChanged.Invoke (rightDir);
				}

			}
		}

		void OnMouseUp()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (CheckBlock ()) {
				return;
			}

			if (enabled) {
				_bDragging = false;
				_StopDragging.Invoke ();
			}
		}

		public bool IsDragging()
		{
			return _bDragging;
		}

		private Vector3 GetHitDir()
		{
			Vector3 hitDir = Vector3.zero;
			//Input.mousePosition
			var ray = 
				Camera.main.ScreenPointToRay(Input.mousePosition);

			float deltaDist = _cctrl.transform.position.z - Camera.main.transform.position.z;
			Vector3 Dir = ray.direction / ray.direction.z;
			Vector3 hitPos =ray.GetPoint (deltaDist);
			hitDir = (hitPos - _cctrl.transform.position).normalized;

			return hitDir;
		}



		private bool CheckBlock()
		{
			CellObjCtrl ctrl = GetComponentInParent<CellObjCtrl> ();
			return ctrl.IsBlocked ();
		}





	}
}