using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[RequireComponent(typeof(AudioSource))]
public class GameView : MonoBehaviour {

	public LianQiWait _lianQiWaitPanel;
	public LianQiPanel _lianQiPanel;
	public ActionPanel _actionPanel;
	public GameResult _gameResultPanel;
	public GameObject _gameRule;
	public GameObject _topBar;
	public ScrollRect _chatPanel;
	private Player[] _players = new Player[4];
	public Player _playerPrefab;
	public Dialog _dialogPrefab;
	public AudioSource _audioSource;

	RoomEvent.sC2V_RoomRule _roomRule;

	private string[] _chatMsg = { "先下手围墙！",
		"承让承认",
		"大哥，手下留情啊！",
		"瞧，乌龟都睡着了！",
		"哈哈哈哈哈哈哈",
		"你苏定了！",
		"不要走，决战到天亮"};

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy(){
		//remove all event
		removeAllEvent();
	}
	void OnEnable(){
		_lianQiPanel.gameObject.SetActive (false);
		_actionPanel.gameObject.SetActive (false);
		//_playerInfoPanel.SetActive (false);
		_chatPanel.gameObject.SetActive(false);
		_gameResultPanel.gameObject.SetActive (false);
		_lianQiWaitPanel.gameObject.SetActive (true);

		//投降和切换棋子信息不能点击
		enableTopBarButton(false);

		for(int i = 0;i<_players.Length;i++){
			if (_players [i] != null) {
				Destroy (_players[i].gameObject);
			}
			_players [i] = null;
		}

		addAllEvent ();

		RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.ENTER_ROOM_FINISH,null);
	}
	void OnDisable(){
		removeAllEvent();

		//清除动态生成的内容
		for(int i = 0;i<_players.Length;i++){
			if (_players [i] != null) {
				Destroy (_players[i].gameObject);
				_players [i] = null;
			}
		}
	}


	//----------------------------------------------------------------
	void addAllEvent(){
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_ROOMRULE, onUpdateRoomRule);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.PLAER_ENTER,onPlayerEnter);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.UPDATE_PLAER_STATE,onPlayerState);
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.SHOW_TALK_MSG, onShowTalkMsg);
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM, onPlayerLeave);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_GAME_START,onShowGameStart);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_DRAW,onReqDraw);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_DRAW_RESULT,onRespDraw);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_TURN,onShowTurn);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_ABANDON,onShowAbandon);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_PASS,onShowPass);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_ABANDON_PASS, onShowAbandonPass);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_PLAY,onShowPlay);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_MOVE,onShowMove);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_RESULT,onShowResult);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_FLAG,onShowFlag);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.SHOW_LIANQI, onShowLianQi);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.TO_GAMEVEIW_UPDATE_SCORE, onUpdateScore);
	}
	void removeAllEvent(){
		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.UPDATE_ROOMRULE);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.PLAER_ENTER);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.UPDATE_PLAER_STATE);
		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.SHOW_TALK_MSG);
		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_GAME_START);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_DRAW);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_DRAW_RESULT);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_TURN);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_ABANDON);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_PASS);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_ABANDON_PASS);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_PLAY);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_MOVE);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_RESULT);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_FLAG);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.SHOW_LIANQI);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.TO_GAMEVEIW_UPDATE_SCORE);
	}

	//----------------------------以下是界面事件发送-----------------------------
	public void  OnDrawBtn(){

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.DRAW,null);
	}
	public void  OnAbandonBtn(){
		showDialog(CommonDefine.eDialogEventType.GAME_ABANDON,"你向对手举起了白旗，是否真的要<size=60><color=green>投降</color></size>？",false,true,true,onClickDialogBtn);
	}
	public void OnPlayerClickReadyBtn(){
		RoomEvent.sV2C_PlayerAct data;
		data.act = RoomEvent.sV2C_PlayerAct.ACT_TYPE.ACT_READY;

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.PLAYER_ACT,(object)data);
	}

	public void OnPlayerClickSendChatBtn(int chatID){
		RoomEvent.sV2C_PlayerTalk data;
		data.content = ""+chatID;
		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.TALK_MSG,(object)data);

		//关闭聊天窗口
		_chatPanel.gameObject.SetActive(false);
	}
	public void OnClickOpenChatPanel(){
		_chatPanel.gameObject.SetActive( !_chatPanel.IsActive ());
	}

	//棋子生命值显示等
	public void OnClickHintBtn(){
		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.TO_LQP_SWITCH_HINT,null);
	}
	//-------------------网络消息，刷新界面--------------------------------------
	void onShowTalkMsg(object data){
		
		RoomEvent.sC2V_PlayerTalk pt = (RoomEvent.sC2V_PlayerTalk)data;
		_players [pt.local].showChatMsg (_chatMsg[Convert.ToInt32(pt.content)]);

		//播放声音
		CommonUtil.Util.playTalkSnd(Convert.ToInt32(pt.content),_audioSource);
	}
	void onUpdateRoomRule(object data){
		_roomRule = (RoomEvent.sC2V_RoomRule)data;
		Text rule = _gameRule.GetComponentInChildren<Text>() ;

		string text = "";
		text += _roomRule.playerNum + "人 ";
		text += _roomRule.gridLevel + "阶 ";

		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			rule.text = CommonUtil.Util.getModeStr (_roomRule.type,_roomRule.playerNum, _roomRule.gridLevel) + " 经典模式";
		} else if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA){
			rule.text = CommonUtil.Util.getModeStr (_roomRule.type,_roomRule.playerNum, _roomRule.gridLevel) + " " + _roomRule.plazaName;
		}else if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_ROOM){
			rule.text = CommonUtil.Util.getModeStr (_roomRule.type,_roomRule.playerNum, _roomRule.gridLevel) + " 房间模式";
		}else if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_TEAM){
			rule.text = CommonUtil.Util.getModeStr (_roomRule.type,_roomRule.playerNum, _roomRule.gridLevel) + " 组队模式";
		}

		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA
			||_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			_lianQiWaitPanel.onUpdateRoomRule (_roomRule);
		} else {
			_lianQiWaitPanel.gameObject.SetActive (false);
			_lianQiPanel.gameObject.SetActive (true);
		}

		//初始化联棋界面
		_lianQiPanel.onInit (data);
	}

	//更新玩家当前棋子数
	void onUpdateScore(object data){
		int[] score = (int[])data;
		for (int i = 0; i < _players.Length; i++) {
			if (_players [i] != null) {
				_players [i].updateScore (score [i]);
			}
		}
	}

	void onPlayerEnter(object data){
		Room.Player player = (Room.Player)data;
		int local = CommonUtil.Util.getLocalBySeat (player.seat) ;

		if (_players [CommonUtil.Util.getLocalBySeat (player.seat)] != null) {
			
			Destroy (_players[CommonUtil.Util.getLocalBySeat(player.seat)].gameObject);
			_players [CommonUtil.Util.getLocalBySeat (player.seat)] = null;
		}

		_players[CommonUtil.Util.getLocalBySeat(player.seat)] = createPlayer (player);

		if (_roomRule.playerNum == 2) {
			//对家跳到下家位置
			if(local == (int)CommonDefine.SEAT.TOP){
				_players[local].transform.position = this.transform.Find ("PlayerInfoPanel/Player" + (int)CommonDefine.SEAT.RIGHT).transform.position;
			}
		} else if (_roomRule.playerNum == 3) {
			//上家位置调到对家
			if(local == (int)CommonDefine.SEAT.LEFT){
				_players[local].transform.position = this.transform.Find ("PlayerInfoPanel/Player" + (int)CommonDefine.SEAT.TOP).transform.position;	
			}
		}

		if(CommonUtil.Util.getLocalBySeat (player.seat) != (int)CommonDefine.SEAT.SELF){
			Vector3 prep = _players [local].transform.position;
			float c = prep.x;
			float offset = (c - _players [local].transform.right.x)/0.75f*0.125f;
			_players [local].transform.position =  new Vector3(prep.x - offset,prep.y,0.0f);
		}

		//进来之后自动准备
		if (player.state == CommonDefine.PLAYER_STATE.STATE_TYPE_SITDOWN) {
			OnPlayerClickReadyBtn ();
		}

		//更新等待界面
		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA
			||_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			_lianQiWaitPanel.onPlayerEnter (data);
		}
	}

	void onPlayerState(object data){
		RoomEvent.sC2V_PlayerState ps = (RoomEvent.sC2V_PlayerState)data;

		_players [ps.local].updateState (ps.state);
		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA
			||_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			_lianQiWaitPanel.onPlayerState (data);
		}
	}
	void onPlayerLeave(object data){
		//_players[local].gameObject.SetActive(false);

		if (_players [(int)data] != null) {
			Destroy (_players [(int)data].gameObject);
			_players [(int)data] = null;
		}

		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA
			||_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			_lianQiWaitPanel.onPlayerLeave (data);
		}
	}
	void onShowGameStart(object data){
		// 对局开始
		_lianQiPanel.gameObject.SetActive (true);
		_actionPanel.gameObject.SetActive (true);

		if (_roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA
			||_roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
			_lianQiWaitPanel.hideLianQiWaitPanel ();
		}

		//顶部按钮可以点击了
		enableTopBarButton(true);

		//设置先手
		_lianQiPanel.onUpdateFirstHandSeat (data);
		_actionPanel.onUpdateFirstHandSeat (data);
		_actionPanel.onInitActionPanel ((object)_roomRule);
	}
	void onReqDraw(object data){
		GameEvent.sC2V_Draw dr = (GameEvent.sC2V_Draw)data;
	}
	void onRespDraw(object data){
		GameEvent.sC2V_Draw dr = (GameEvent.sC2V_Draw)data;
	}
	void onShowTurn(object data){
		//int local = (int)data;

		//通知action panel
		_actionPanel.onShowTurn (data);
		_lianQiPanel.onLQShowTurn (data);
	}
	void onShowPass(object data){
		_actionPanel.onShowPass (data);
		_lianQiPanel.onLQShowPass (data);
	}
	void onShowAbandonPass(object data){
		_lianQiPanel.onShowAbandonPass (data);
		_actionPanel.onShowAbandonPass (data);
	}
	void onShowAbandon(object data){
		showDialog (CommonDefine.eDialogEventType.GAME_ABANDON ,_players[(int)data].getPlayerName() + "投降了！投降了！投降了！",true,false,false,null);
	}
	void onShowPlay(object data){
		//通知action panel
		_actionPanel.onShowPlay (data);
		_lianQiPanel.onLQShowPlay (data);
	}
	void onShowMove(object data){
		_actionPanel.onShowMove (data);
		_lianQiPanel.onLQShowMove (data);
	}

	void onShowResult(object data){
		//其他面板是否也需要处理？
		_gameResultPanel.showGameResult ((GameEvent.sV2V_ShowGameResult)data);
	}
	void onShowFlag(object data){
		
	}
	void onShowLianQi(object data){
		List<GameEvent.Chess> cb = data as List<GameEvent.Chess>;//(List<Game.Chess>)data;
		_lianQiPanel.onShowLianQi(cb);
	}

	//----------------------一些辅助--------------------------
	public void enableTopBarButton(bool enable){
		Button[] btns = _topBar.GetComponentsInChildren<Button>();
		for (int i = 0; i < btns.Length; i++) {
			if (btns [i].name.Equals("HintBtn")
				|| btns [i].name.Equals("AbandonBtn")) {
				btns [i].interactable = enable;
			}
		}
	}
	public Player createPlayer(Room.Player data){
		Player player = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		player.transform.SetParent(this.transform.Find("PlayerInfoPanel"));

		bool self = CommonUtil.Util.getLocalBySeat (data.seat) == (int)CommonDefine.SEAT.SELF;
		player.updatePlayer(self,data.isOwner,data.seat,data.head,data.sex,data.name);

		//位置根据local来
		player.transform.position = this.transform.Find ("PlayerInfoPanel/Player" + CommonUtil.Util.getLocalBySeat (data.seat)).transform.position;

		//player.transform.localScale = player.transform.localScale * CommonUtil.Util.getScreenScale();

		return player;
	}

	public void onClickDialogBtn(CommonDefine.eDialogBtnType btn,CommonDefine.eDialogEventType type){
		if (btn == CommonDefine.eDialogBtnType.DIALOG_BTN_OK) {
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ABANDON,null );
		}
	}
	public void showDialog(CommonDefine.eDialogEventType type, string tip,bool hasClose,bool hasOk,bool hasCancel,
		Action<CommonDefine.eDialogBtnType,CommonDefine.eDialogEventType> callBack){

		Dialog dlg = Instantiate(_dialogPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		dlg.init (type,tip,hasClose,hasOk,hasCancel,callBack);
		dlg.transform.SetParent (this.transform);
		dlg.transform.position = this.transform.position;

		dlg.transform.localScale = dlg.transform.localScale * CommonUtil.Util.getScreenScale();

		dlg.openDialog (true);
	}
}
