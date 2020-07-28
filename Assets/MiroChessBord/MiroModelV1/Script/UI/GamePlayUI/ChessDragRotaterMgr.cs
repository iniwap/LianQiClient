using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ChessDragRotaterMgr : MonoBehaviour {

		public ChessDragRotator [] _DratRoters;

		//private List<int> _BanDirs = new List<int> ();

		public void GetAllChessDragRotator()
		{
			_DratRoters = GetComponentsInChildren<ChessDragRotator> ();
		}

		public void AddBanDir(int dir)
		{
			foreach (var rtr in _DratRoters) {
				rtr.AddBanDir (dir);
			}
		}

		public void RemoveBanDir(int dir)
		{
			foreach (var rtr in _DratRoters) {
				rtr.RemoveBanDir (dir);
			}
		}

		public void SetBanDirs(HashSet<int> banDirs)
		{
			foreach (var rtr in _DratRoters) {
				rtr.SetBanDirs (banDirs);
			}
		}

		public void SetBanDirs(List<int> banDirs)
		{
			foreach (var rtr in _DratRoters) {
				rtr.ClearBanDirs ();
				foreach (int ivalue in banDirs) {
					rtr.AddBanDir (ivalue);
				}
			}
		}

		[ContextMenu("TurnONAll")]
		public void TurnONAll()
		{
			foreach (var rtr in _DratRoters) {
				rtr.enabled = true;
			}
		}

		[ContextMenu("TurnOFFAll")]
		public void TurnOFFAll()
		{
			foreach (var rtr in _DratRoters) {
				rtr.enabled = false;
			}
		}

		public void Turn(bool bON)
		{
			foreach (var rtr in _DratRoters) {
				rtr.enabled = bON;
				//print (rtr.name + " ON?" + bON);
			}
		}

		public void TurnOFF(bool bOFF)
		{
			
			Turn (!bOFF);
		}

		//public void EnableDragRotatorFor(CellObjCtrl

	}
}
