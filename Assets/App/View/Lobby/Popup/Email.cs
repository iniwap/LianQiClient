/*
 * 邮件弹窗界面的处理
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Email : MonoBehaviour {

	public GameObject _emailContainer;
	public Text _noEmail;

	public Text _emailReadInfo;
	public Text _emailContent;
	public Text _emailTime;
	public Text _emailAuthor;

	// 如果是奖励邮件
	public Image _awardIcon;
	public Text _awardCnt;
	public Button _awardBtn;//领取奖励按钮
	public Button _allAwardBtn;//一键领取奖励按钮
	public GameObject _awardPanel;


	public EmailTitleItem _emailTitleItemPrefab;
	public GameObject _emailRoot;

	private List<EmailTitleItem> _emailList = new List<EmailTitleItem>();

	private int _currentSelectEmailID;
	private bool _hasGettingAllAward;

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
		//隐藏删除
		for (int i = 0; i < _emailList.Count; i++) {
			Destroy (_emailList[i].gameObject);
		}
		_emailList.Clear ();
		_hasGettingAllAward = false;

		removeAllEvent();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_PRIVATEMSG,onUpdateEmail);//刷新邮件列表
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_UPDATE_EMAIL_RESULT,onShowUpdateEmailResult);//显示邮件操作结果

		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_PRIVATE_MSG,null);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_PRIVATEMSG);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_UPDATE_EMAIL_RESULT);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	public void OnClickCloseBtn(){
		this.gameObject.SetActive (false);
	}
	public void OnClickGetAwardBtn(){
		//
		if(_currentSelectEmailID != -1){
			LobbyEvent.sV2C_ReqUpdateEmail re;
			re.id = _emailList[_currentSelectEmailID].getEmailID();
			re.type = CommonDefine.eUpdateEmailType.GET_AWARD ;
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.REQ_UPDATE_EMAIL,(object)re);
		}
	}

	public void OnClickGetAllAwardBtn(){
		//这里记录 是否在一键领取
		_hasGettingAllAward = true;

		for(int i = 0;i < _emailList.Count;i++){
			if (_emailList [i].getContent ().hasAward
			   && !_emailList [i].getContent ().hasGottenAward) {
				LobbyEvent.sV2C_ReqUpdateEmail re;
				re.id = _emailList [i].getEmailID ();
				re.type = CommonDefine.eUpdateEmailType.GET_AWARD;

				LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.REQ_UPDATE_EMAIL, (object)re);
				break;
			}
		}

		_allAwardBtn.interactable = false;
	}

	//----------------------------网络更新界面--------------------------
	public void onUpdateEmail(object data){
		List<LobbyEvent.PrivateMsg> privateMsgList = (List<LobbyEvent.PrivateMsg>)data;

		// 设置左侧 滑动范围大小
		RectTransform rt = _emailRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x,privateMsgList.Count*160);

		removeAllEmail ();
		for (int i = 0; i < privateMsgList.Count; i++) {
			//生成保存信息
			string time = Convert.ToDateTime(privateMsgList[i].send_time).ToString("yyyy/MM/dd");

			_emailList.Add (createEmailItem (i,privateMsgList[i].id,
				privateMsgList[i].author,
				CommonUtil.EmailContent.deserialize( privateMsgList[i].content),
				privateMsgList[i].has_read != 0,
				time,
				privateMsgList[i].title));
		}

		checkAllAwardBtnCanShow ();

		updateDefaultSelect ();

		updateReadUnRead ();

	}
	public void onShowUpdateEmailResult(object data){
		LobbyEvent.sV2C_ReqUpdateEmail re = (LobbyEvent.sV2C_ReqUpdateEmail)data;

		int index = -1;

		for (int i = 0; i < _emailList.Count; i++) {
			if (_emailList [i].getEmailID () == re.id) {
				index = i;
				break;
			}
		}
		if (index == -1)
			return;
		
		if (re.type == CommonDefine.eUpdateEmailType.READ) {
			//设置已读
			_emailList [index].updateHasRead();

		} else if (re.type == CommonDefine.eUpdateEmailType.DEL) {
			//删除
			Destroy(_emailList [index].gameObject);
			_emailList.RemoveAt (index);
			//需要刷新整个界面
			//将默认选择移动到第一个，设置当前选择
			updateDefaultSelect();

		} else if (re.type == CommonDefine.eUpdateEmailType.GET_AWARD) {
			//恭喜获得xx 提示
			ViewManagerEvent.s_ShowDialog d;
			d.callBack = onClickDialogBtn;
			d.hasCancel = false;
			d.hasClose = true;
			d.hasOk = false;
			d.tip = "温馨提示";
			CommonUtil.EmailContent content = _emailList [index].getContent ();
			if (content.type == CommonUtil.EmailContent.AWARD_TYPE.GOLD) {
				d.tip = "恭喜获得" + content.awardCnt + "积分！祝您游戏愉快～";	
			} else if (_emailList [index].getContent ().type == CommonUtil.EmailContent.AWARD_TYPE.PROP) {
				d.tip = "恭喜获得永久皮肤" + "吃遍天下" + "！祝您游戏愉快～";	
			}

			d.type = CommonDefine.eDialogEventType.LOBBY_EMAIL_GET_AWARD_RESULT;

			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_DIALOG, (object)d);

			_emailList [index].getContent().hasGottenAward = true;
			//更新
			_awardBtn.interactable = false;
			Text txt = _awardBtn.GetComponentInChildren<Text> ();
			txt.text = "已领取";

			//all btn 
			Invoke("checkIsGettingAllAward", 0.001f);
		}
		updateReadUnRead ();
	}

	//-------------------------------一些接口--------------------------
	public void selectEmailTitleItem(int emailID){
		int id = -1;
		for (int i = 0; i < _emailList.Count; i++) {
			if (_emailList [i].getEmailID () != emailID) {
				_emailList [i].unselectItem ();
			} else {
				id = i;
				_emailList [i].selectItem ();
			}
		}

		if (id == -1)
			return;

		_currentSelectEmailID = id;

		if (!_emailList [_currentSelectEmailID].getHasRead ()) {
			LobbyEvent.sV2C_ReqUpdateEmail re;
			re.id = _emailList [_currentSelectEmailID].getEmailID ();
			re.type = CommonDefine.eUpdateEmailType.READ;
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.REQ_UPDATE_EMAIL, (object)re);
		}

		//右侧信息刷新
		CommonUtil.EmailContent content = _emailList [id].getContent();
		_emailContent.text = content.content;
		_emailAuthor.text = _emailList [id].getAuthor();
		_emailTime.text = _emailList [id].getDate ();

		if (content.hasAward) {
			_awardPanel.SetActive (true);

			/*
		public AWARD_TYPE type;
		public int prop_id;//如果有
		public string awardIcon;//可以从道具列表中查到相关图标
			*/

			if (content.hasGottenAward) {
				_awardBtn.interactable = false;
				Text txt = _awardBtn.GetComponentInChildren<Text> ();
				txt.text = "已领取";
			} else {
				_awardBtn.interactable = true;
			}

			_awardCnt.text = "x"+content.awardCnt;

			//_awardIcon.sprite = CommonUtil.Util.getPropIconByPropID (content.prop_id);

		}else{
			_awardPanel.SetActive (false);
		}
	}
	public EmailTitleItem createEmailItem(int index,int id,string author,CommonUtil.EmailContent content,
		bool hasRead,string date,string title){

		EmailTitleItem emailItem = Instantiate(_emailTitleItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		emailItem.transform.SetParent(_emailRoot.transform);

		emailItem.transform.localPosition = new Vector3(0,- index * 160 - 40,0);

		emailItem.updateEmailTitleItem (this,id,author,content,title,date,hasRead);

		emailItem.transform.localScale = emailItem.transform.localScale * CommonUtil.Util.getScreenScale();

		return emailItem;
	}

	//设置是否存在未领取的奖励
	public void checkAllAwardBtnCanShow(){

		bool hasAward = false;

		for (int i = 0; i < _emailList.Count; i++) {

			CommonUtil.EmailContent content = _emailList[i].getContent();
			if(content.hasAward){
				hasAward = true;
				break;
			}
		}

		if (hasAward) {
			_allAwardBtn.gameObject.SetActive (true);
		} else {
			_allAwardBtn.gameObject.SetActive (false);
		}

		// 是否已经全部领取
		bool hasAllGotten = true;

		for (int i = 0; i < _emailList.Count; i++) {

			CommonUtil.EmailContent content = _emailList[i].getContent();
			if(content.hasAward && !content.hasGottenAward){
				hasAllGotten = false;
				break;
			}
		}
		if (hasAllGotten) {
			//_allAwardBtn.interactable = false;
			_allAwardBtn.gameObject.SetActive (false);
		} else {
			_allAwardBtn.interactable = true;
		}
	}
	void updateReadUnRead(){

		int all = _emailList.Count;
		int unread = 0;
		for (int i = 0; i < all; i++) {
			if (!_emailList [i].getHasRead ()) {
				unread++;
			}
		}
		_emailReadInfo.text = "未读邮件"+unread + "/" + all;
	}
	void updateDefaultSelect(){
		if(_emailList.Count > 0){
			_emailList [0].OnClickTitleBtn ();//默认选中第0个
			_emailContainer.SetActive(true);
			_noEmail.gameObject.SetActive(false);
		}else{
			_currentSelectEmailID = -1;
			_emailContainer.SetActive(false);
			_noEmail.gameObject.SetActive(true);
		}
	}

	void onClickDialogBtn(CommonDefine.eDialogBtnType btn,CommonDefine.eDialogEventType type){
		if (type == CommonDefine.eDialogEventType.LOBBY_EMAIL_GET_AWARD_RESULT) {

		}
	}
	void checkIsGettingAllAward(){
		bool hasAllGotten = true;
		if(_hasGettingAllAward){
			for(int i = 0;i < _emailList.Count;i++){
				if (_emailList [i].getContent ().hasAward
					&& !_emailList [i].getContent ().hasGottenAward) {
					LobbyEvent.sV2C_ReqUpdateEmail re2;
					re2.id = _emailList [i].getEmailID ();
					re2.type = CommonDefine.eUpdateEmailType.GET_AWARD;

					LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.REQ_UPDATE_EMAIL, (object)re2);

					hasAllGotten = false;

					break;
				}
			}
			if(hasAllGotten){
				_hasGettingAllAward = false;
			}
		}

		checkAllAwardBtnCanShow();
	}
	public void removeAllEmail(){

		for (int i = 0; i < _emailList.Count; i++) {
			if (_emailList [i] != null) {
				Destroy (_emailList [i].gameObject);
			}
			_emailList [i] = null;
		}

		_emailList.Clear ();
	}
}
