/*
 * 单个场的处理
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour {
	public GameObject[] _feedBackList;
	public Text[] _feedBackTextList;

	public Image _selectedBg;
	public Text _inputText;
	private string[] _feedBackTextStrList = {
		"BUG报错",
		"账号问题",
		"奖励发放",
		"充值问题",
		"改善建议",
		"发射弹幕"
	};

	private CommonDefine.eFeedbackType _feedType;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy(){
		removeAllEvent();
	}
	void OnEnable(){
		addAllEvent();
	}
	void OnDisable(){   
		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		//LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_USER_INFO,onUpdataUserInfo);
	}
	void removeAllEvent(){
		//LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_USER_INFO);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	public void OnClickCloseBtn(){
		this.gameObject.SetActive (false);
	}

	public void OnClickSubmitBtn(){
		if(_inputText.text.Length != 0 && _inputText.text.Length <= 200){
			//发送反馈
			LobbyEvent.sV2C_Feedback fb;
			fb.type = _feedType;
			fb.content = _inputText.text;

			LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.REQ_FEEDBACK,(object)fb);
		}
		this.gameObject.SetActive (false);
	}
	public void OnClickFBBtn(int id){
		//背景图设置到这个位置
		//反馈类型为id
		_feedType = (CommonDefine.eFeedbackType)id ;
		_selectedBg.transform.localPosition = _feedBackList[id].transform.localPosition;
		_inputText.text = "11";

		for(int i =0;i<6;i++){
			_feedBackTextList[i].text = "<color=#261601FF>"+ _feedBackTextStrList[i] + "</color>";
		}

		//_feedBackTextList[id].text = "<color=#FFE8AEFF>"+ _feedBackTextStrList[id] + "</color>";
	}
}
