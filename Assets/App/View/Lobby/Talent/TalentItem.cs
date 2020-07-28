/*
 * 单条天赋
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentItem : MonoBehaviour {

	private TalentPanel _talentPanel;

	public Button _talentLockBtn;
	public Button _talentCanInstallBtn;
	public Button _talentHasInstalledBtn;

	private CommonDefine.TalentSlotState _btnState;
	private CommonDefine.eTalentType _talent;
	private int _id;
	private string _name;
	private string _des;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void updateTalentItem( TalentPanel talentPanel, CommonDefine.eTalentType type,int id,string name,string des,CommonDefine.TalentSlotState btnState){
		_talent = type;
		_id = id;
		_name = name;
		_des = des;
	
		_talentPanel = talentPanel;

		updateBtnState (btnState);
	}

	public void updateBtnState(CommonDefine.TalentSlotState btnState){

		_btnState = btnState;

		switch (btnState) {
		case CommonDefine.TalentSlotState.TALENT_LOCK:
			_talentLockBtn.gameObject.SetActive (true);
			_talentCanInstallBtn.gameObject.SetActive (false);
			_talentHasInstalledBtn.gameObject.SetActive (false);
			_talentLockBtn.image.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.TALENT_LOCK_BTN);
			break;
		case CommonDefine.TalentSlotState.TALENT_CAN_INSTALL:
			_talentLockBtn.gameObject.SetActive (false);
			_talentCanInstallBtn.gameObject.SetActive (true);
			_talentHasInstalledBtn.gameObject.SetActive (false);
			_talentCanInstallBtn.image.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.TALENT_CANINSTALL_BTN);
			break;
		case CommonDefine.TalentSlotState.TALENT_INSTALLED:
			_talentLockBtn.gameObject.SetActive (false);
			_talentCanInstallBtn.gameObject.SetActive (false);
			_talentHasInstalledBtn.gameObject.SetActive (true);

			//更新为对应的天赋图标

			_talentHasInstalledBtn.image.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.TALENT_ICON_BTN + (int)_talent);

			break;
		}
	}

	public void onTalentLockBtnClick(){
		if(_btnState == CommonDefine.TalentSlotState.TALENT_LOCK){
			_talentPanel.OnClickTalentLockBtn(_id);
		}
	}

	public void onTalentCanInstallBtnClick(){
		if(_btnState == CommonDefine.TalentSlotState.TALENT_CAN_INSTALL){
			_talentPanel.OnClickTalentCanInstallBtn(_id);
		}
	}

	public void onTalentInstalledBtnClick(){
		if(_btnState == CommonDefine.TalentSlotState.TALENT_INSTALLED){
			_talentPanel.OnClickTalentHasInstallBtn(_id);
		}
	}

	public CommonDefine.eTalentType getTalentType(){
		return _talent;
	}

	public string getTalentName(){
		return _name;
	}

	public string getTalentDes(){
		return _des;
	}

	public int getTalentID(){
		return _id;
	}

	public CommonDefine.TalentSlotState getTalentBtnState(){
		return _btnState;
	}
}
