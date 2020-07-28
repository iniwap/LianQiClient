/*
 * 房间模式的用户头像
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : MonoBehaviour {

	public Image _vipIcon;
	public Text _vipLevel;
	public Text _winRate;
	public Image _headBg;
	public Image _head;
	public Image _ownerImg;
	public Text _name;
	public Text _multiple;
	public GameObject _vipEffect;

	private  int _local;
	private bool _isOwner;

	public GameObject _gameWaitVipPos;

	public enum eRoomPlayerType{
		ROOM_WIAT,// 房间玩家
		GAME_WAIT,//游戏等待时玩家
	};
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

	public void updateRoomPlayer(eRoomPlayerType type,int local,int vip,float winRate,string head,bool isOwner,string name){

		int vipLevel = CommonUtil.Util.getVipLevel (vip);
		_vipIcon.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.VIP_ICON + vipLevel);
		_vipLevel.text = "" + vipLevel;
		if (vipLevel >= 3) {
			_vipEffect.gameObject.SetActive (true);
		} else {
			_vipEffect.gameObject.SetActive (false);
		}

		_local = local;
		_name.text = name;

		_headBg.sprite = CommonUtil.Util.getSpriteByLocal (local, CommonDefine.ResPath.ROOM_HEAD_BG);
		if (head.Contains ("http")) {
			//网路图片
		} else {
			_head.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.ROOM_HEAD);
		}


		if (type == eRoomPlayerType.ROOM_WIAT) {
			
			_winRate.text = "胜率" + winRate + "%";
			_multiple.gameObject.SetActive (false);
			_ownerImg.gameObject.SetActive (isOwner);

			_isOwner = isOwner;
		} else if(type == eRoomPlayerType.GAME_WAIT){
			//
			_winRate.gameObject.SetActive(false);
			_multiple.gameObject.SetActive (false);
			_ownerImg.gameObject.SetActive (false);

			//vip信息需要移动到头像下方
			_vipIcon.transform.position = _gameWaitVipPos.transform.position;
		}
	}

	public void updateMultiple(int m){
		_multiple.gameObject.SetActive (true);
		_multiple.text = "x"+m+"倍";
	}

	public bool getIsOwner(){
		return _isOwner;
	}
	public void updatePlayerReady(){
		_headBg.sprite = CommonUtil.Util.getSpriteByLocal (_local, CommonDefine.ResPath.ROOM_READY_HEAD_BG);
	}
}
