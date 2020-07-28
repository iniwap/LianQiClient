/*
 * 用于显示联机大厅相关，区别于总大厅
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class PlazaRoomPanel : MonoBehaviour {
	private enum ePlazaRoomPanelType{
		PLAZA_PANEL,
		CREATE_PANEL,
		ROOM_PANEL,
	};

	public GameObject _createRoomPanel;
	public GameObject _plazaPanel;
	public GameObject _roomPanel;

	public Button _createBtn;
	public Button _joinBtn;

	public InputField _inputField;
	public Image _roomTitleBg;
	public Text _inputRoomID;
	public Text _currentRoomID;

	public Plaza _plazaPrefab;
	public float _plazaInteral;
	public RoomPlayer _roomPlayerPrefab;

	public GameObject _plazaRoot;
	public Image _plazaLevel;
	public Text _plazaLevelText;

	public Button _switchPlazaLBtn;
	public Button _switchPlazaRBtn;
	public Text _switchPlazaText;

	public Button _switchModeLBtn;
	public Button _switchModeRBtn;
	public Text _currentRoomModeText;

	public int[] _baseScoreList;

	private CommonDefine.ePlazaLevelType _currentPlazaLevel = CommonDefine.ePlazaLevelType.PLAZA_LEVEL_MIDDLE;
	private int _currentPlayerNum = 2;
	private int _currentGridLevel = 4;

	public Button _roomActionBtn;
	public Button _roomShareOrJoinBtn;//快速开始或者分享
	private CommonDefine.eRoomActionType _actionType  = CommonDefine.eRoomActionType.CAN_CREATE_ROOM;


	public Text _currentLmtRoundText;
	private int _currentLmtRound = 100;//10回合增加/减少
	public Text _currentLmtTurnTimeText;
	private int _currentLmtTurnTime = 20;//1秒增加减少

	public Button _addLmtRoundBtn;
	public Button _desLmtRoundBtn;
	public Button _addLmtTurnTimeBtn;
	public Button _desLmtTurnTimeBtn;
	//正在房间中
	public Image _inRoomLevel;
	public Text _inRoomLevelText;
	public Text _inRoomModel;
	public Text _inRoomLmtRound;
	public Text _inRoomLmtTurnTime;
	public Button _readyBtn;
	public Button _cancelReadyBtn;
	public Button _startBtn;
	private int _inRoomGridLevel;

	public GameObject[] _roomPlayer;
	private List<Plaza> _realPlaza = new List<Plaza>();
	private RoomPlayer[] _realPlayer = new RoomPlayer[4] ;


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
		for (int i = 0; i < 4; i++) {
		//	_realPlayer [i] = null;
		}

		addAllEvent();

		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_PLAZA,null);
	}
	void OnDisable(){   
		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_PLAZA,onUpdatePlaza);

		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM, onPlayerLeave);
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_ROOMRULE, onUpdateRoomRule);

		RoomEvent.EM().AddEvent(RoomEvent.EVENT.PLAER_ENTER,onPlayerEnter);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.UPDATE_PLAER_STATE,onPlayerState);
		//RoomEvent.EM ().AddEvent (RoomEvent.EVENT.SHOW_TALK_MSG, onShowTalkMsg);
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM, onPlayerLeave);
		RoomEvent.EM ().AddEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM, onDissolve);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_PLAZA);

		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM);
		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.UPDATE_ROOMRULE);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.PLAER_ENTER);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.UPDATE_PLAER_STATE);
		RoomEvent.EM ().RemoveEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM);
	}
	//------------------------------以下界面事件-----------------------------------------------
	public void OnClickBackBtn(){
		//切换到主大厅界面
		LobbyEvent.s_V2VShowLobbyPanel lp;
		lp.from = LobbyEvent.LOBBY_PANEL.LOBBY_PLAZAROOM_PANEL;
		lp.to = LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL;
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL,(object)lp);
	}
	public void OnClickShareBtn(){
		//mlink share room
	}

	//关闭创建房间弹窗
	public void OnClickCloseCreateRoomPanelBtn(){
		showPanel (ePlazaRoomPanelType.PLAZA_PANEL);
	}
	public void OnClickQuickStartBtn(){
		if (!_plazaPanel.activeSelf) {
			OnClickShareBtn ();
			return;
		}

		//请输入房号

		int currentRoomID = 0;
		int.TryParse(_currentRoomID.text, out currentRoomID);
		if (currentRoomID != 0) {
			//已经在房间中
			CommonUtil.Util.showDialog("系统提示","您已经在房间，无法创建房间");
			return;
		}

		int roomID = 0;
		int.TryParse(_inputRoomID.text, out roomID);
		if (roomID == 0) {
			//请输入房号
			CommonUtil.Util.showDialog("温馨提示","请输入房号");
			return;
		}
		if (_inputRoomID.text.Length != 6) {
			// 房间号不正确
			CommonUtil.Util.showDialog("温馨提示","请输入正确的房号");
			return;
		}

		RoomEvent.sV2C_JoinRoom data;

		data.playerNum = 0;
		data.gridLevel = 0;
		data.plazaID = 0;
		data.pwd = "";
		data.roomId = roomID;
		data.tagId = -1;
		data.plazaName = "";

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.JOIN_ROOM,(object)data);
	}
	public void OnClickTopCreateRoomBtn(){

		if (!_plazaPanel.activeSelf) {
			//
			//  这个时候需要判断是处于什么情况，分别有，创建房间，离开房间，解散房间
			switch (_actionType) {
			//case CommonDefine.eRoomActionType.CAN_CREATE_ROOM:
			//	OnClickTopCreateRoomBtn ();
			//	break;
			case CommonDefine.eRoomActionType.CAN_LEAVE_ROOM:
				//其他人离开就是普通离开房间
				RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.LEAVE_ROOM,false);
				break;
			case CommonDefine.eRoomActionType.CAN_DISSOLVE:
				//房主离开就是解散房间
				RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.LEAVE_ROOM,false);
				break;
			}
			return;
		}

		showPanel (ePlazaRoomPanelType.CREATE_PANEL);
	}
	public void OnClickCreateRoomBtn(){
		RoomEvent.sV2C_CreateRoom data;
		data.roomName = "";//设置成自己的昵称
		data.roomPassword = "";//不设置密码

		JsonObject rule = new JsonObject ();
		rule.Add ("playerNum",_currentPlayerNum );
		rule.Add ("gridLevel",_currentGridLevel);
		rule.Add ("rule","default");
		rule.Add ("gameTime",0);
		rule.Add ("lmtRound",_currentLmtRound);
		rule.Add ("lmtTurnTime",_currentLmtTurnTime);

		//rule.Add ("levelID",(int)_currentPlazaLevel);
		data.roomType = CommonDefine.eCreateRoomType.ROOM_ROOM;
		data.rule = rule.ToString();
		data.minScore = _baseScoreList[(int)_currentPlazaLevel - 1];
		data.maxScore = 0;//没有最大限制
		data.baseScore = _baseScoreList[(int)_currentPlazaLevel - 1];

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.CREATE_ROOM,data);
	}

	public void OnClickSwitchPlazaLevelBtn(int op){
		//OP = 0 降低，1 升高
		if (op == 0) {
			int level = (int)_currentPlazaLevel;
			if (level == 1) {
				return;
			}
			_currentPlazaLevel = (CommonDefine.ePlazaLevelType)(--level);
			if (_currentPlazaLevel == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_LOW) {
				_switchPlazaLBtn.interactable = false;
			} else {
				_switchPlazaLBtn.interactable = true;
			}
			_switchPlazaRBtn.interactable = true;

		} else if (op == 1) {
			int level = (int)_currentPlazaLevel;
			if (level == 3) {
				return;
			}
			_currentPlazaLevel = (CommonDefine.ePlazaLevelType)(++level);
			if (_currentPlazaLevel == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_HIGH) {
				_switchPlazaRBtn.interactable = false;
			} else {
				_switchPlazaRBtn.interactable = true;
			}
			_switchPlazaLBtn.interactable = true;
		}

		_plazaLevel.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAZA_ROOM_LEVEL + (int)_currentPlazaLevel);

		_plazaLevelText.text = "" + getBaseScore() + "\n积分";

		_switchPlazaText.text = "创建该等级房间，需要消耗<color=#f03a13>"+getBaseScore() +"</color>积分";
	}

	public void OnClickSwitchRoomModeBtn(int op){
		//OP = 0 降低，1 升高
		if (op == 0) {
			if (_currentPlayerNum == 4 && _currentGridLevel == 6) {
				_switchModeLBtn.interactable = false;
				return;
			}

			if (_currentPlayerNum == 2 && _currentGridLevel == 4) {
				_currentPlayerNum = 4;
				_currentGridLevel = 6;
				_switchModeLBtn.interactable = false;
			}else if (_currentPlayerNum == 2 && _currentGridLevel == 6) {
				_currentPlayerNum = 2;
				_currentGridLevel = 4;
			}
			_switchModeRBtn.interactable = true;

		} else if (op == 1) {
			if (_currentPlayerNum == 2 && _currentGridLevel == 6) {
				_switchModeRBtn.interactable = false;
				return;
			}

			if (_currentPlayerNum == 2 && _currentGridLevel == 4) {
				_currentPlayerNum = 2;
				_currentGridLevel = 6;
				_switchModeRBtn.interactable = false;
			}else if (_currentPlayerNum == 4 && _currentGridLevel == 6) {
				_currentPlayerNum = 2;
				_currentGridLevel = 4;
			}

			_switchModeLBtn.interactable = true;
		}

		_currentRoomModeText.text = CommonUtil.Util.getModeStr(CommonDefine.eCreateRoomType.ROOM_ROOM,_currentPlayerNum,_currentGridLevel);

	}

	public void OnClickChangeLmtRound(bool add){
		if (add) {
			if (_currentLmtRound < CommonDefine.CONST.MAX_LMT_ROUND) {
				_currentLmtRound += 10;	
			}
		} else {
			if (_currentLmtRound > CommonDefine.CONST.MIN_LMT_ROUND) {
				_currentLmtRound -= 10;
			}
		}

		_addLmtRoundBtn.interactable = true;
		_desLmtRoundBtn.interactable = true;
		if(_currentLmtRound >= CommonDefine.CONST.MAX_LMT_ROUND){
			_addLmtRoundBtn.interactable = false;
		}
		if(_currentLmtRound <= CommonDefine.CONST.MIN_LMT_ROUND){
			_desLmtRoundBtn.interactable = false;
		}
		_currentLmtRoundText.text = "" + _currentLmtRound;
	}

	public void OnClickChangeLmtTurnTime(bool add){
		
		if (add) {
			if (_currentLmtTurnTime < CommonDefine.CONST.MAX_LMT_TURNTIME) {
				_currentLmtTurnTime += 1;
			}
		} else {
			if (_currentLmtTurnTime > CommonDefine.CONST.MIN_LMT_TURNTIME) {
				_currentLmtTurnTime -= 1;
			}
		}

		_addLmtTurnTimeBtn.interactable = true;
		_desLmtTurnTimeBtn.interactable = true;
		if(_currentLmtTurnTime >= CommonDefine.CONST.MAX_LMT_TURNTIME){
			_addLmtTurnTimeBtn.interactable = false;
		}
		if(_currentLmtTurnTime <= CommonDefine.CONST.MIN_LMT_TURNTIME){
			_desLmtTurnTimeBtn.interactable = false;
		}
		_currentLmtTurnTimeText.text = "" + _currentLmtTurnTime;
	}

	public void OnClickReady(bool ready){
		//点击准备
		RoomEvent.sV2C_PlayerAct data;
		data.act = RoomEvent.sV2C_PlayerAct.ACT_TYPE.ACT_READY;

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.PLAYER_ACT,(object)data);

		_readyBtn.interactable = false;
	}

	public void OnClickStart(){
		//
		_startBtn.interactable = false;
		RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.START_GAME,false);
	}

	//--------------------------以下网络数据更新界面------------------------------
	public void onUpdatePlaza(object data){

		List<LobbyEvent.Plaza> plazaList = data as List<LobbyEvent.Plaza>;

		RectTransform rt = _plazaRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(plazaList.Count*_plazaInteral, rt.sizeDelta.y);

		removeAllPlaza ();

		for(int i = 0;i<plazaList.Count;i++){
			_realPlaza.Add( createPlaza (i,plazaList[i]));
		}
	}
	// 当前能执行的操作类型，分别有，可以创建房间，可以离开房间，可以解散房间
	public void updateRoomActionType(CommonDefine.eRoomActionType type){
		_actionType = type;

		switch (type) {
		case CommonDefine.eRoomActionType.CAN_CREATE_ROOM:
			_roomActionBtn.GetComponentInChildren<Text>().text = "创建房间";
			_roomShareOrJoinBtn.GetComponentInChildren<Text>().text = "加入房间";
			break;
		case CommonDefine.eRoomActionType.CAN_LEAVE_ROOM:
			_roomActionBtn.GetComponentInChildren<Text>().text = "离开房间";
			_roomShareOrJoinBtn.GetComponentInChildren<Text>().text = "分享房间";
			break;
		case CommonDefine.eRoomActionType.CAN_DISSOLVE:
			_roomActionBtn.GetComponentInChildren<Text>().text = "解散房间";
			_roomShareOrJoinBtn.GetComponentInChildren<Text>().text = "分享房间";
			break;
		case CommonDefine.eRoomActionType.CAN_NONE:
			//此时隐藏
			break;
		}
	}

	public void onUpdateRoomRule(object data){
		RoomEvent.sC2V_RoomRule roomRule = (RoomEvent.sC2V_RoomRule)data;
		updateRoomInfo (true);

		_inRoomLevel.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_ROOM_LEVEL + roomRule.roomLevel);

		_inRoomLevelText.text = "" + getBaseScore() + "积分";

		string model = "";//CommonUtil.Util.getModeStr(roomRule.playerNum,roomRule.gridLevel);

		if (roomRule.playerNum == 2) {
			model = model + "双人";
		}else if(roomRule.playerNum == 3){
			model = model + "三人";
		}else if(roomRule.playerNum == 4){
			model = model + "四人";
		}
		if (roomRule.gridLevel == 4) {
			model = model + "四阶";
		}else if(roomRule.gridLevel == 6){
			model = model + "六阶";
		}else if(roomRule.gridLevel == 8){
			model = model + "八阶";
		}

		_inRoomModel.text = model;
		_inRoomLmtRound.text = ""+roomRule.lmtRound;
		_inRoomLmtTurnTime.text = "" + roomRule.lmtTurnTime;

		_inRoomGridLevel = roomRule.gridLevel;

		_currentRoomID.text = "房间ID:"+roomRule.roomID;

		//创建房间成功
		showPanel (ePlazaRoomPanelType.ROOM_PANEL);
	}

	void onPlayerEnter(object data){
		Room.Player player = (Room.Player)data;
		int local = CommonUtil.Util.getLocalBySeat (player.seat) ;

		//ADD PLAYER
		_realPlayer[local] = createPlayer(player);

		showWaitImg (false,local);

		if (local == (int)CommonDefine.SEAT.SELF) {
			if (player.isOwner) {
				updateRoomActionType (CommonDefine.eRoomActionType.CAN_DISSOLVE);
			} else {
				updateRoomActionType (CommonDefine.eRoomActionType.CAN_LEAVE_ROOM);
			}

			for (int i = 0; i < _inRoomGridLevel; i++) {
				showWaitImg (true, CommonUtil.Util.getLocalBySeat (i));
			}
		}
	}

	void onPlayerState(object data){
		RoomEvent.sC2V_PlayerState ps = (RoomEvent.sC2V_PlayerState)data;

		if (ps.state == CommonDefine.PLAYER_STATE.STATE_TYPE_ROOMREADY &&
		    ps.local == (int)CommonDefine.SEAT.SELF) {
			_readyBtn.gameObject.SetActive (false);

			//只有owner的才能设置显示开局按钮
			if (_realPlayer [ps.local].getIsOwner ()) {
				_startBtn.gameObject.SetActive (true);
			} else {
				_cancelReadyBtn.gameObject.SetActive (true);
				_cancelReadyBtn.interactable = false;//暂时不支持取消准备
			}

			_startBtn.interactable = false;
		} else {
			//修改别人的状态为准备
		}

		_realPlayer [ps.local].updatePlayerReady ();

		if (ps.ifAllReady) {
			if (_realPlayer [(int)CommonDefine.SEAT.SELF].getIsOwner ()) {
				_startBtn.interactable = true;
			}
		}
	}
	void onPlayerLeave(object data){
		//_players[local].gameObject.SetActive(false);

		int local = (int)data;

		showWaitImg (true,local);

		//REMOVE PLAERY

		Destroy (_realPlayer [local].gameObject);
		_realPlayer [local] = null;
	}
	void onDissolve(object data){
		//房间解散，切回到创建面板
		updateRoomInfo(false);

		updateRoomActionType (CommonDefine.eRoomActionType.CAN_CREATE_ROOM);

		//隐藏全部
		for (int i = 0; i < 4; i++) {
			showWaitImg (false, i);
			if (_realPlayer [i] != null) {
				Destroy (_realPlayer [i].gameObject);
				_realPlayer [i] = null;
			}
		}
		_currentRoomID.text = "";

		showPanel (ePlazaRoomPanelType.PLAZA_PANEL);
	}
	//--------------子界面处理---------------------
	public void unselectPlazaExcept(int id){
		Plaza[] plazas = _plazaRoot.GetComponentsInChildren<Plaza>();
		for(int i = 0;i < plazas.Length;i++){
			if (plazas [i].getID() != id) {
				plazas [i].unselectPlaza ();
			}
		}
	}

	public void onClickJoinRoom(int playerNum,int gridLevel,int plazaID,string name,int tag){

		if (playerNum == 0) {

			//提示请选择人数和阶数

			return;
		}

		RoomEvent.sV2C_JoinRoom data;

		data.playerNum = playerNum;
		data.gridLevel = gridLevel;
		data.plazaID = plazaID;
		data.pwd = "";
		data.roomId = 0;
		data.plazaName = name;
		data.tagId = tag;

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.JOIN_ROOM,(object)data);
	}
	public void updateRoomInfo(bool ifInRoom){
		if (ifInRoom) {

			_readyBtn.gameObject.SetActive (true);
			_cancelReadyBtn.gameObject.SetActive (false);
			_startBtn.gameObject.SetActive (false);

			_readyBtn.interactable = true;
			_cancelReadyBtn.interactable = false;

			enbaleAllPlaza (false);

		} else {
			enbaleAllPlaza (true);
		}
	}
	public void onHidePlazaRoomPanel(bool hide){
		this.gameObject.SetActive (!hide);
		if (hide) {
			for (int i = 0; i < _realPlaza.Count; i++) {
				Destroy (_realPlaza [i].gameObject);
			}

			_realPlaza.Clear ();
		}
	}
	//----------------一些接口-------------------------------------

	public Plaza createPlaza(int index,LobbyEvent.Plaza data){

		Plaza plaza = Instantiate(_plazaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		plaza.transform.SetParent(_plazaRoot.transform);

		plaza.transform.localPosition = new Vector3(index*_plazaInteral + _plazaInteral/2,0,0);

		CommonUtil.RoomRule roomRule = CommonUtil.RoomRule.deserialize(data.rule);

		string rr;
		if(roomRule.lmtRound != 0){
			rr = "回合限制"+roomRule.lmtRound;
		}else{
			rr = "回合无限制";
		}

		if (roomRule.lmtTurnTime != 0) {
			rr = rr + " 思考时间"+roomRule.lmtTurnTime+"秒";
		} else {
			rr = rr + " 思考时间无限制";
		}

		plaza.updatePlaza (index,data.plazaid,data.name,data.star,data.des,rr,this);

		plaza.transform.localScale = plaza.transform.localScale * CommonUtil.Util.getScreenScale();

		return plaza;
	}
	public RoomPlayer createPlayer(Room.Player data){

		int local = CommonUtil.Util.getLocalBySeat (data.seat);
		RoomPlayer player = Instantiate(_roomPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		player.transform.SetParent(_roomPlayer[local].transform);

		int total = data.win + data.lose + data.draw;
		float winRate = 0.0f;
		if (total != 0) {
			winRate = 100 * 1.0f * data.win / (data.win + data.lose + data.draw);
		}
		player.updateRoomPlayer(RoomPlayer.eRoomPlayerType.ROOM_WIAT,
			local,data.vip,winRate,data.head,data.isOwner,data.name);

		player.transform.localPosition = new Vector3(0,0,0);

		player.transform.localScale = player.transform.localScale * CommonUtil.Util.getScreenScale();

		return player;
	}
	public  void showWaitImg(bool show, int local){
		_roomPlayer [local].transform.Find ("RoomWaitImg").gameObject.SetActive(show);

	}
	public void enbaleAllPlaza(bool enable){
		Plaza[] pls = _plazaRoot.transform.GetComponentsInChildren<Plaza> ();
		for (int i = 0; i < pls.Length; i++) {
			pls [i].enablePlaza (enable);
		}
	}
	public void removeAllPlaza(){
		for (int i = 0; i < _realPlaza.Count; i++) {
			if (_realPlaza [i] != null) {
				Destroy (_realPlaza [i].gameObject);
			}
			_realPlaza [i] = null;
		}

		_realPlaza.Clear ();
	}
	public int getBaseScore(){
		int baseScore = 50;
		if (_currentPlazaLevel == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_LOW) {
			baseScore = 50;
		}else if(_currentPlazaLevel == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_MIDDLE){
			baseScore = 500;
		}else if(_currentPlazaLevel == CommonDefine.ePlazaLevelType.PLAZA_LEVEL_HIGH){
			baseScore = 5000;
		}

		return baseScore;
	}
	private void showPanel(ePlazaRoomPanelType type){
		switch(type){
		case  ePlazaRoomPanelType.CREATE_PANEL:
			_createRoomPanel.SetActive (true);
			_plazaPanel.SetActive (false);
			_roomPanel.SetActive (false);

			_inputField.gameObject.SetActive (false);
			_roomTitleBg.gameObject.SetActive(false);

			break;
		case ePlazaRoomPanelType.PLAZA_PANEL:
			_createRoomPanel.SetActive (false);
			_plazaPanel.SetActive (true);
			_roomPanel.SetActive (false);

			_inputField.gameObject.SetActive (true);
			_roomTitleBg.gameObject.SetActive(false);
			break;
		case ePlazaRoomPanelType.ROOM_PANEL:
			_createRoomPanel.SetActive (false);
			_plazaPanel.SetActive (false);
			_roomPanel.SetActive (true);

			_inputField.gameObject.SetActive (false);
			_roomTitleBg.gameObject.SetActive(true);
			break;
		}
	}
}
