using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Image _bg;
	public Image _headbg;
	public Button _head;
	public Image _nickBg;
	public Text _name;
	public Image _chatBg;
	public Text _chatMsg;
	public  Text _currentScore;

	//更多信息，财富、段位等
	public int _seat;
	private bool _isSelf;
	// 初始化
	void Start(){
	}
	void Update(){

	}

	public void onClickHead(){
		if (!_isSelf) {
			//show nick
			_nickBg.gameObject.SetActive (!_nickBg.IsActive());
		}
	}
	public string getPlayerName(){
		return _name.text;
	}

	public void updatePlayer(bool self,bool isOwner,int seat,string head,int sex,string name){
		_seat = seat;
		_isSelf = self;

		_headbg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAYER_BG + seat);
		_head.image.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAYER_HEAD + seat);
		_name.text = name;
		//_chatBg
		_chatBg.gameObject.SetActive(false);
		if (self) {
			_bg.gameObject.SetActive (true);
			this.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		} else {
			this.transform.localScale = new Vector3(0.75f,0.75f,1.0f);
			_bg.gameObject.SetActive (false);
		}

		_currentScore.text = "0";

		_nickBg.gameObject.SetActive (false);
	}
	public void showChatMsg(string content){
		_chatBg.gameObject.SetActive (true);
		_chatMsg.text = content;
	}
	public void updateScore(int score){

		// 这里可以实现分数变化动画
		_currentScore.text = ""+score;
	}

	public void updateState(CommonDefine.PLAYER_STATE state){
		// 这里只处理用户上线(游戏中)和离线
		if(state == CommonDefine.PLAYER_STATE.STATE_TYPE_OFFLINE){
			_head.interactable = false;
		}else if(state == CommonDefine.PLAYER_STATE.STATE_TYPE_PLAYING){
			_head.interactable = true;
		}
	}
}
