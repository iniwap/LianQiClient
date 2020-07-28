/*
 * 对局结算面板
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour {
	public Image _levelIcon;
	public Text _levelText;
	public Text _baseScoreText;
	public Image _titleImg;
	public Text _poolScore;
	public Text _abandonScore;

	public PlayerScore _playerScorePrefab;
	public GameObject _playerContainer;
	private List<PlayerScore> _playerScore = new List<PlayerScore>();


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

		for (int i = 0; i < _playerScore.Count; i++) {
			_playerScore [i] = null;
		}

		addAllEvent();
	}
	void OnDisable(){ 
		for (int i = 0; i < _playerScore.Count; i++) {
			if (_playerScore [i] != null) {
				Destroy (_playerScore[i].gameObject);	
			}
			_playerScore [i] = null;
		}
		_playerScore.Clear ();  

		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		
	}
	void removeAllEvent(){
		
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	public void OnClickCloseBtn(){
		//此处原则上也应该返回大厅
		OnClickBackBtn();
	}

	public void OnClickShareBtn(){
		// 启动分享
	}
	public void OnClickBackBtn(){
		//返回大厅
		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.LEAVE_ROOM,true);
	}

	//----------------------------通过gameview操作，无需通过事件----------------------------------------------------
	public void showGameResult(GameEvent.sV2V_ShowGameResult result){

		this.gameObject.SetActive (true);

		int poolGold = 0;
		for(int i = 0;i < result.gameResult.Count;i++){
			_playerScore.Add (createPlayerScore(i,result.roomType,result.gameResult[i]));

			if (CommonUtil.Util.getLocalBySeat(result.gameResult [i].seat) == (int)CommonDefine.SEAT.SELF ) {
				if (result.gameResult [i].score > 0) {
					_titleImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.GAME_RESULT_TITLE_WIN);
				} else if (result.gameResult [i].score < 0) {
					_titleImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.GAME_RESULT_TITLE_LOSE);
				} else {
					//_titleImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.GAME_RESULT_TITLE_DRAW);
					_titleImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.GAME_RESULT_TITLE_LOSE);
					//不存在平手
				}

				//投降为输
				if (result.gameResult [i].hasAbandon) {
					_titleImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.GAME_RESULT_TITLE_LOSE);
				}
			}

			poolGold += result.gameResult[i].multi * result.baseScore;
		}

		//有一种情况，就是大家的得分都相等，认为是平手 


		_poolScore.text = "奖池积分  " + poolGold;
		if (result.poolGold != poolGold) {
			_abandonScore.text = "" + (result.poolGold - poolGold);
			_abandonScore.gameObject.SetActive (true);
		} else {
			_abandonScore.gameObject.SetActive (false);
		}

		updateLevel (result.level,result.baseScore);
	}
		
	//-------------------------一些接口--------------------------------------------------------
	PlayerScore createPlayerScore(int index,CommonDefine.eCreateRoomType roomType,
		GameEvent.sV2V_ShowGameResult.sGameResult rst){

		PlayerScore playerScore = Instantiate(_playerScorePrefab, new Vector3(0, 0, 0), Quaternion.identity);
		playerScore.transform.SetParent(_playerContainer.transform);

		playerScore.transform.localPosition = new Vector3(0,150 - index * 100,0);

		bool self = CommonUtil.Util.getLocalBySeat (rst.seat) == (int)CommonDefine.SEAT.SELF;
		bool showOwner = false;
		if (roomType == CommonDefine.eCreateRoomType.ROOM_ROOM 
			|| roomType == CommonDefine.eCreateRoomType.ROOM_TEAM) {

			if (rst.isOwner) {
				showOwner = true;
			} else {
				showOwner = false;
			}
		}

		playerScore.updatePlayScore (self,index+1,rst.seat,showOwner,rst.head,rst.name,
			rst.area,rst.kill,rst.score,rst.multi,rst.hasAbandon);

		playerScore.transform.localScale = playerScore.transform.localScale * CommonUtil.Util.getScreenScale();

		return playerScore;
	}
	void updateLevel(CommonDefine.ePlazaLevelType level,int baseScore){
		string pname = "新手场";
		if(level == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_LOW){
			pname = "新手场";
		}else if(level == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_MIDDLE){
			pname = "进阶场";
		}else if(level == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_HIGH){
			pname = "高手场";
		}
		_levelText.text = pname;
		_levelIcon.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.LEVEL_ICON + (int)level);
		_baseScoreText.text = "" + baseScore + "积分";
	}
}
