/*
 * 邮件标题
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailTitleItem : MonoBehaviour {

	private int _emailID = -1;
	private bool _isSelect = false;
	private CommonUtil.EmailContent _content;// 这里存放解析出来的内容
	private string _author;
	private bool _hasRead;

	public Image _bg;
	public Text _title;
	public Text _date;
	public Text _notRead;

	private Email _email;//用于传出点击事件
	private string _titleStr;

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

	public void OnClickTitleBtn(){
		if(_isSelect)return;

		_isSelect = true;
		_bg.gameObject.SetActive(true);
		_email.selectEmailTitleItem(_emailID);
	}

	//-------------------------------一些接口--------------------------
	public void updateEmailTitleItem(Email email,int id,string author,
		CommonUtil.EmailContent content,string title,string date,bool hasRead){

		_emailID = id;
		_email = email;
		_title.text = title;
		_date.text = date;
		_author = author;
		_content = content;
		_notRead.gameObject.SetActive (!hasRead);
		_hasRead = hasRead;
		_titleStr = title;
	}
	public void updateHasRead(){
		_notRead.gameObject.SetActive(false);
		_hasRead = true;
	}
	public void updateContent(CommonUtil.EmailContent content){
		_content = content;
	}
	public void unselectItem(){
		_isSelect = false;
		_bg.gameObject.SetActive(false);

		//_title.text = "<color=#261601FF>"+ _titleStr + "</color>";
		//_title.fontSize = 48;
		//_title.alignment = TextAnchor.MiddleLeft;
	}
	public void selectItem(){
		//_title.text = "<color=#261601FF>"+ _titleStr + "</color>";
		//_title.fontSize = 36;
		//_title.alignment = TextAnchor.MiddleCenter;
	}
	public int getEmailID(){
		return _emailID;
	}
	public CommonUtil.EmailContent getContent(){
		return _content;
	}
	public string getAuthor(){
		return _author;
	}
	public string getDate(){
		return _date.text;
	}
	public bool getHasRead(){
		return _hasRead;
	}
}
