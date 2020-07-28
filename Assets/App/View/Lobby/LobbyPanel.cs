/*
 * 用于显示大厅主相关
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LobbyPanel : MonoBehaviour {


	//用户信息
	public Image _userHead;
	public Text _userName;
	public Text _userGold;
	public Text _userDiamond;

	//
	public Text _currentMode;
	public Text _upMode;
	public Text _downMode;

	public Image _hasMsg;
	public Image _hasFeedback;

	//all popup 
	public Email _email;
	public Feedback _feedback;
	public Rank _rank;
	public Setting _setting;

	public Button _up;
	public Button _down;

	private CommonDefine.eRoomClassicType _currentType = CommonDefine.eRoomClassicType.MODE_2_4; 

	//跑马灯
	public Lamp _lamp;

	// Use this for initialization
	void Start () {
		hideAllPopups();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy(){
		removeAllEvent();
	}
	void OnEnable(){
		addAllEvent();

		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_USER_INFO,null);
	}
	void OnDisable(){   
		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_USER_INFO,onUpdataUserInfo);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_HAS_EMAIL,onShowHasUnReadEmail);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_SYSMSG,onUpdateSysMsg);

		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_SYSMSG,onShowSysMsg);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_POPUP,onShowPopup);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_USER_INFO);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_HAS_EMAIL);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_SYSMSG);

		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_SYSMSG);

		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_POPUP);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	//快速开始
	public void OnClickJoinRoom(){

		RoomEvent.sV2C_JoinRoom data;
		//请根据界面结果填写
		int pn = 2;
		int gl = 4;

		if (_currentType == CommonDefine.eRoomClassicType.MODE_2_4) {
			pn = 2;
			gl = 4;
		}else if (_currentType == CommonDefine.eRoomClassicType.MODE_2_6) {
			pn = 2;
			gl = 6;
		}else if (_currentType == CommonDefine.eRoomClassicType.MODE_4_6) {
			pn = 4;
			gl = 6;
		}

		data.playerNum = pn;
		data.gridLevel = gl;
		data.plazaID = 0;//c填写
		data.pwd = "";
		data.roomId = 0;
		data.plazaName = "";
		data.tagId = 0;

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.JOIN_ROOM,(object)data);
	}

	public void OnClickChangeMode(bool up){
		if (up) {
			if (_currentType == CommonDefine.eRoomClassicType.MODE_2_4) {

				_currentMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,6);
				_currentType = CommonDefine.eRoomClassicType.MODE_2_6;

				_up.interactable = false;
				_down.interactable = true;

				_downMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,4);
			} else if (_currentType == CommonDefine.eRoomClassicType.MODE_4_6) {
				_up.interactable = true;
				_down.interactable = true;

				_currentMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,4);
				_currentType = CommonDefine.eRoomClassicType.MODE_2_4;

				_upMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,6);
				_downMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,4,6);
			}
		} else {
			if (_currentType == CommonDefine.eRoomClassicType.MODE_2_4) {

				_currentMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,4,6);
				_currentType = CommonDefine.eRoomClassicType.MODE_4_6;

				_up.interactable = true;
				_down.interactable = false;

				_upMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,4);
			} else if (_currentType == CommonDefine.eRoomClassicType.MODE_2_6) {
				_up.interactable = true;
				_down.interactable = true;

				_currentMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,4);
				_currentType = CommonDefine.eRoomClassicType.MODE_2_4;

				_upMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,2,6);
				_downMode.text = CommonUtil.Util.getModeStr (CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA,4,6);
			}
		}
	}

	public void OnClickOfflineModeBtn(){
        //不提供单机模式代码
        /*
		ViewManagerEvent.sShowScene ss;
		ss.fromScene = ViewManagerEvent.SCENE_TYPE.APP_SCENE;
		ss.toScene = ViewManagerEvent.SCENE_TYPE.MACHINE_SCENE;
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_SCENE,(object)ss);
        */
	}
	public void OnClickOnlineModeBtn(){
		LobbyEvent.s_V2VShowLobbyPanel lp;
		lp.from = LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL;
		lp.to = LobbyEvent.LOBBY_PANEL.LOBBY_PLAZAROOM_PANEL;
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL,(object)lp);
	}
	public void OnClickGetRoomList(){

		RoomEvent.sV2C_GetRoomList data;

		data.currentPage = 0;
		data.perCnt = 10;

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.GET_ROOM_LIST,(object)data);
	}

	//RIGHT BAR
	public void OnClickSettingBtn(){
		showPopup(CommonDefine.ePopupType.SETTING);
	}
	public void OnClickEmailBtn(){
		showPopup(CommonDefine.ePopupType.EMAIL);
	}
	public void OnClickRankBtn(){
		showPopup(CommonDefine.ePopupType.RANK);
	}
	public void OnClickFeedbackBtn(){
		showPopup(CommonDefine.ePopupType.FEEDBACK);
	}
	public void OnClickTalentBtn(){
		showPopup(CommonDefine.ePopupType.TALENT);
	}

	public void OnClickBuyBtn(int id){
		//0  增加金币/积分  1增加钻石

	}
	//--------------------------以下网络数据更新界面------------------------------

	void onUpdataUserInfo(object data){
		//假设这里传递了整个account信息，实际上没有必要，也可以另外定义，其他均为相同做法
		SelfData self = (SelfData)data;

		// 刷新所有用户相关的信息
		//比如金币显示，昵称，头像等等

		setHead(self.head,_userHead);//设置头像

		_userName.text = self.name;
		_userGold.text = ""+self.gold;
		_userDiamond.text = ""+self.diamond;
	}
	void onShowHasUnReadEmail(object data){
		_hasMsg.gameObject.SetActive((bool)data);
	}
	void onUpdateSysMsg(object data){
		List<LobbyEvent.SysMsg>  sysMsgList = (List<LobbyEvent.SysMsg> )data;

		if (sysMsgList.Count == 0) {
			_lamp.gameObject.SetActive (false);
		} else {
			_lamp.gameObject.SetActive (true);
			_lamp.resetLamp ();
			for (int i = 0; i < sysMsgList.Count; i++) {
				CommonUtil.SysBroadCast lamp = CommonUtil.SysBroadCast.deserialize (sysMsgList [i].content);
				_lamp.addLamp (lamp);//这里原则上也可以实现内容携带其他信息，诸如图标

				_lamp.showLampById (0);
			}
		}
	}
	void onShowSysMsg(object data){
		LobbyEvent.SysMsg  sysMsg = (LobbyEvent.SysMsg)data;
		_lamp.gameObject.SetActive (true);

		CommonUtil.SysBroadCast lamp = CommonUtil.SysBroadCast.deserialize (sysMsg.content);
		_lamp.addLamp (lamp);//这里原则上也可以实现内容携带其他信息，诸如图标

		_lamp.showLampById (_lamp.getTotal() - 1);

	}

	public void onShowPopup(object data){
		showPopup((CommonDefine.ePopupType)data);
	}

	//----------------一些接口------------------------------------------------
	public void hideAllPopups(){
		_email.gameObject.SetActive(false);
		_feedback.gameObject.SetActive(false);
		_rank.gameObject.SetActive(false);
		_setting.gameObject.SetActive(false);
	}
	public void showPopup(CommonDefine.ePopupType type){
		hideAllPopups();
		switch(type){
		case CommonDefine.ePopupType.EMAIL:
			_email.gameObject.SetActive(true);
			break;
		case CommonDefine.ePopupType.FEEDBACK:
			_feedback.gameObject.SetActive(true);
			break;
		case CommonDefine.ePopupType.RANK:
			_rank.gameObject.SetActive(true);
			break;
		case CommonDefine.ePopupType.SETTING:
			_setting.gameObject.SetActive(true);
			break;
		case CommonDefine.ePopupType.TALENT:
			LobbyEvent.s_V2VShowLobbyPanel lp;
			lp.from = LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL;
			lp.to = LobbyEvent.LOBBY_PANEL.LOBBY_TALENT_PANEL;
			LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL,(object)lp);
			break;
		}
	}

	public void setHead(string url,Image head){

		if (url.Contains ("http")) {
			//创建目录
			if (!Directory.Exists(CommonUtil.Util.IMAGE_CACHE_PATH))  
			{  
				Directory.CreateDirectory(CommonUtil.Util.IMAGE_CACHE_PATH);  
			}

			//判断头像是否存在
			if (!File.Exists (CommonUtil.Util.IMAGE_CACHE_PATH + url.GetHashCode ())) {  
				//如果之前不存在缓存文件  
				StartCoroutine (DownloadImage (url, head));  
			} else {

				StartCoroutine(LoadLocalImage(url, head));
			}
		} else {

			head.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.HEAD + url);
		}
	}

	private IEnumerator DownloadImage(string url, Image image)  
	{  

		WWW www = new WWW(url);  
		yield return www;  

		Texture2D tex2d = www.texture;  
		//将图片保存至缓存路径  
		byte[] pngData = tex2d.EncodeToPNG();  
		File.WriteAllBytes(CommonUtil.Util.IMAGE_CACHE_PATH + url.GetHashCode(), pngData);  

		Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));  
		image.sprite = m_sprite;  
	}  

	private IEnumerator LoadLocalImage(string url, Image image)  
	{  
		string filePath = "file://" + CommonUtil.Util.IMAGE_CACHE_PATH + url.GetHashCode ();  

		WWW www = new WWW(filePath);  
		yield return www;

		Texture2D texture = www.texture;  

		Sprite m_sprite = Sprite.Create(texture,new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));  
		image.sprite = m_sprite;  
	}
}
