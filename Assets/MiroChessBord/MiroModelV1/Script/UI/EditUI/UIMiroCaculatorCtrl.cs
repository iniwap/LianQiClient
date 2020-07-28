using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class UIMiroCaculatorCtrl : MonoBehaviour {

		public Toggle[] Tgs;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void TurnALL(bool bON)
		{
			//MiroV1Caculator.TurnAll (bON);
			foreach (Toggle tg in Tgs) {
				tg.isOn = bON;
			}
		}

		public void TurnFarm2(bool bON)
		{
			if (bON) {
				CCFarm2.TurnAllON ();
			} else {
				CCFarm2.TurnAllOFF ();
			}
		}

		public void TurnHole2(bool bON)
		{
			if (bON) {
				CCHole2.TurnAllON ();
				CCAbsorb.TurnAllON ();
			} else {
				CCHole2.TurnAllOFF ();
				CCAbsorb.TurnAllOFF ();
			}
		}

		public void TurnAnteller(bool bON)
		{
			if (bON) {
				CCAnteller.TurnAllON ();
			} else {
				CCAnteller.TurnAllOFF ();
			}
		}

		public void TurnAid(bool bON)
		{
			if (bON) {
				CCAid.TurnAllON ();
			} else {
				CCAid.TurnAllOFF ();
			}
		}

		public void TurnRing(bool bON)
		{
			if (bON) {
				CCRing.TurnAllON ();
			} else {
				CCRing.TurnAllOFF ();
			}
		}

		public void TurnEN(bool bON)
		{
			if (bON) {
				CCEN.TurnAllON ();
			} else {
				CCEN.TurnAllOFF ();
			}
		}

		public void TurnMainWP(bool bON)
		{
			if (bON) {
				CCMainWP.TurnAllON ();
			} else {
				CCMainWP.TurnAllOFF ();
			}
		}

		public void TurnRecover(bool bON)
		{
			if (bON) {
				CCRecover.TurnAllON ();
			} else {
				CCRecover.TurnAllOFF ();
			}
		}

		public void TurnHurt(bool bON)
		{
			if (bON) {
				CCHurt.TurnAllON ();
			} else {
				CCHurt.TurnAllOFF ();
			}
		}


	}
}