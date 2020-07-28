/*
 * 游戏匹配等待界面
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class LianQiWait : MonoBehaviour {

	private int _inRoomGridLevel;
	private int _inRoomPlayerNum;
	public Image _plazaTagImg;
	public Text _plazaName;
	public Image[] _plazaStar;
	public Text _modeText;
	public Text _ruleText;

	public RoomPlayer _roomPlayerPrefab;

	public GameObject[] _roomPlayer;
	private RoomPlayer[] _realPlayer = new RoomPlayer[4];

	public Text _waitClock;
	private int _currentTime;

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
			if(_realPlayer [i] != null){
				Destroy (_realPlayer [i].gameObject);
			}
			_realPlayer [i] = null;
			showWaitImg (false,i);
		}

		addAllEvent();
	}
	void OnDisable(){   
		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){

	}
	void removeAllEvent(){
	}
	//------------------------------以下界面事件-----------------------------------------------
	public void OnClickLeaveRoomBtn(){
		//离开房间
		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.LEAVE_ROOM,false);
		CancelInvoke();
	}

	public void onUpdateRoomRule(RoomEvent.sC2V_RoomRule roomRule){

		_inRoomGridLevel = roomRule.gridLevel;
		_inRoomPlayerNum = roomRule.playerNum;

		_modeText.text = CommonUtil.Util.getModeStr (roomRule.type,roomRule.playerNum, roomRule.gridLevel);

		if (roomRule.type == CommonDefine.eCreateRoomType.ROOM_PLAZA) {

			string rr;
			if (roomRule.lmtRound != 0) {
				rr = "回合限制" + roomRule.lmtRound;
			} else {
				rr = "回合无限制";
			}

			if (roomRule.lmtTurnTime != 0) {
				rr = rr + " 思考时间" + roomRule.lmtTurnTime + "秒";
			} else {
				rr = rr + " 思考时间无限制";
			}

			_ruleText.text = rr;
			_ruleText.gameObject.SetActive(true);

			_plazaName.gameObject.SetActive (true);
			_plazaName.text = roomRule.plazaName;
			//这里实现起来太繁琐了，这个标志几乎没有意义
			_plazaTagImg.gameObject.SetActive (true);
			_plazaTagImg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAZA_TAG + roomRule.tag);

			for (int i = 0; i < _plazaStar.Length; i++) {
				_plazaStar [i].gameObject.SetActive (false);
				if (i < (int)(roomRule.star)) {
					_plazaStar [i].gameObject.SetActive (true);
				}
			}
			if ((int)roomRule.star + 0.5 == roomRule.star) {
				_plazaStar [(int)roomRule.star].gameObject.SetActive (true);
				_plazaStar [(int)roomRule.star].sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAZA_HALF_STAR);
			}
		}else if(roomRule.type == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA){
			//经典模式，不显示任何多余信息
			_ruleText.gameObject.SetActive(false);
			_plazaName.gameObject.SetActive (false);
			_plazaTagImg.gameObject.SetActive (false);

			for (int i = 0; i < _plazaStar.Length; i++) {
				_plazaStar [i].gameObject.SetActive (false);
			}
		}
	}

	public void onPlayerEnter(object data){
		Room.Player player = (Room.Player)data;
		int local = CommonUtil.Util.getLocalBySeat (player.seat) ;

		//ADD PLAYER
		_realPlayer[local] = createPlayer(player);

		showWaitImg (false,local);

		if (local == (int)CommonDefine.SEAT.SELF) {

			if (_inRoomPlayerNum == 2) {
				showWaitImg (true, (int)CommonDefine.SEAT.TOP);
			} else if (_inRoomPlayerNum == 3) {
				showWaitImg (true, (int)CommonDefine.SEAT.RIGHT);
				showWaitImg (true, (int)CommonDefine.SEAT.LEFT);
			} else if (_inRoomPlayerNum == 4) {
				showWaitImg (true, (int)CommonDefine.SEAT.RIGHT);
				showWaitImg (true, (int)CommonDefine.SEAT.LEFT);
				showWaitImg (true, (int)CommonDefine.SEAT.TOP);
			}

			_currentTime = 0;

			//开启等待计时
			InvokeRepeating("updateClock", 0.0f, 1.0f);	
		}
	}

	public void onPlayerState(object data){
		//场模式，进来自动准备，一般不需要更新用户状态
		RoomEvent.sC2V_PlayerState ps = (RoomEvent.sC2V_PlayerState)data;

		if (ps.state == CommonDefine.PLAYER_STATE.STATE_TYPE_ROOMREADY &&
		    ps.local == (int)CommonDefine.SEAT.SELF) {
		
		} else {
			//修改别人的状态为准备
		}
	}
	public void onPlayerLeave(object data){

		int local = (int)data;

		showWaitImg (true,local);

		Destroy (_realPlayer [local].gameObject);
		_realPlayer [local] = null;
	}
	public void updateClock(){
		_currentTime++;
		_waitClock.text = ""+_currentTime + "s";
	}
	public void hideLianQiWaitPanel(){
		CancelInvoke();
		this.gameObject.SetActive(false);
	}
	//----------------一些接口-------------------------------------
	RoomPlayer createPlayer(Room.Player data){

		int local = CommonUtil.Util.getLocalBySeat (data.seat);
		RoomPlayer player = Instantiate(_roomPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		player.transform.SetParent(_roomPlayer[local].transform);

		int total = data.win + data.lose + data.draw;
		float winRate = 0.0f;
		if (total != 0) {
			winRate = 1.0f * data.win / (data.win + data.lose + data.draw);
		}
		player.updateRoomPlayer(RoomPlayer.eRoomPlayerType.GAME_WAIT,
			local,data.vip,winRate,data.head,data.isOwner,data.name);

		player.transform.localPosition = new Vector3(0,0,0);

		player.transform.localScale = player.transform.localScale * CommonUtil.Util.getScreenScale();


		return player;
	}
	void showWaitImg(bool show, int local){
		_roomPlayer [local].transform.Find ("WaitImg").gameObject.SetActive(show);

	}
}
