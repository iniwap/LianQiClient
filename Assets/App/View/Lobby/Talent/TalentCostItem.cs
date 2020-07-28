/*
 * 单条天赋
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentCostItem : MonoBehaviour {

	private TalentPanel _talentPanel;

	public Text _costName;
	public Text _costNum;
	private CommonDefine.eTalentType _talent;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void updateTalentItem(CommonDefine.eTalentType talent,string talentName,int cost){

		_talent = talent;

		if ((int)talent > (int)CommonDefine.eTalentType.TALENT_B2) {
			_costName.text = "<color=#FFE8AEFF>高级天赋 " + talentName +"</color>";
			_costNum.text = "<color=#FFE8AEFF>消耗+" + cost+"</color>";
		} else {
			_costName.text = "<color=#261601>基础天赋 " + talentName +"</color>";
			_costNum.text = "<color=#261601>消耗+" + cost + "</color>";
		}
	}

	public CommonDefine.eTalentType getTalentType(){
		return _talent;
	}
}
