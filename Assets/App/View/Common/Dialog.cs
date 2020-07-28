using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

	public Button _okBtn;//确定按钮，即同意
	public Button _closeBtn;//中间关闭按钮，即忽略关闭
	public Button _cancelBtn;//即拒绝按钮
	public Text  _tip;//显示的内容
	public CommonDefine.eDialogEventType _eventType;

	//也可以有提示框标题等

	//按钮回调
	Action<CommonDefine.eDialogBtnType,CommonDefine.eDialogEventType> _callBack = null;

	void Start(){
		
	}
	void Update(){

	}

	public void OnClickOKBtn(){
		if (_callBack != null) {
			_callBack (CommonDefine.eDialogBtnType.DIALOG_BTN_OK,_eventType);
		}

		openDialog (false);
	}

	public void OnClickCancelBtn(){
		if (_callBack != null) {
			_callBack (CommonDefine.eDialogBtnType.DIALOG_BTN_CANCEL,_eventType);
		}
		openDialog (false);
	}

	public void OnClickCloseBtn(){
		openDialog (false);
	}
	public void openDialog(bool open){
		
		this.gameObject.SetActive (open);

		if (!open) {
			Destroy (this.gameObject);
		}
	}

	public void init(CommonDefine.eDialogEventType type,string tip,bool hasClose,bool hasOk,bool hasCancel,
		Action<CommonDefine.eDialogBtnType,CommonDefine.eDialogEventType> callBack){

		_eventType = type;
		_callBack = callBack;
		_tip.text = tip;
		_okBtn.gameObject.SetActive (hasOk);
		_cancelBtn.gameObject.SetActive (hasCancel);
		_closeBtn.gameObject.SetActive (hasClose);
	}
}
