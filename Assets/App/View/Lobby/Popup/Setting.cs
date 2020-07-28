/*
 * 单个场的处理
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

	public Image _versionInfo;
	public Image _bgSndSelectedBg;
	public Image _effSndSelectedBg;

	public GameObject _bgSndOn;
	public GameObject _bgSndOff;

	public GameObject _effSndOn;
	public GameObject _effSndOff;

	public Text _bgSndOnText;
	public Text _bgSndOffText;
	public Text _effSndOnText;
	public Text _effSndOffText;

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

		//初始化声音设置
		initSndSetting();
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
	public void OnClickVersionBtn(){
		_versionInfo.gameObject.SetActive(!_versionInfo.IsActive());
	}
	public void OnClickDevBtn(){
		//开发者名单
	}
	public void OnClickFBBtn(){
		_versionInfo.gameObject.SetActive(false);
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_POPUP,(object)CommonDefine.ePopupType.FEEDBACK);
	}

	public void OnClickBgSndBtn(bool on){
		if (on) {
			_bgSndSelectedBg.transform.localPosition = _bgSndOn.transform.localPosition;
			_bgSndOnText.gameObject.SetActive (false);
			_bgSndOffText.gameObject.SetActive (true);
		} else {
			_bgSndSelectedBg.transform.localPosition = _bgSndOff.transform.localPosition;
			_bgSndOnText.gameObject.SetActive (true);
			_bgSndOffText.gameObject.SetActive (false);
		}
		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.OPEN_CLOSE_BG_SND,on);
	}

	public void OnClickEffSndBtn(bool on){
		if (on) {
			_effSndSelectedBg.transform.localPosition = _effSndOn.transform.localPosition;
			_effSndOnText.gameObject.SetActive (false);
			_effSndOffText.gameObject.SetActive (true);
		} else {
			_effSndSelectedBg.transform.localPosition = _effSndOff.transform.localPosition;
			_effSndOnText.gameObject.SetActive (true);
			_effSndOffText.gameObject.SetActive (false);
		}
		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.OPEN_CLOSE_EFFECT_SND,on);
	}
	void initSndSetting(){
		if (CommonUtil.Util.getSetting("BgSnd")) {
			_bgSndSelectedBg.transform.localPosition = _bgSndOn.transform.localPosition;
			_bgSndOnText.gameObject.SetActive (false);
			_bgSndOffText.gameObject.SetActive (true);
		} else {
			_bgSndSelectedBg.transform.localPosition = _bgSndOff.transform.localPosition;
			_bgSndOnText.gameObject.SetActive (true);
			_bgSndOffText.gameObject.SetActive (false);
		}

		if (CommonUtil.Util.getSetting("EffectSnd")) {
			_effSndSelectedBg.transform.localPosition = _effSndOn.transform.localPosition;
			_effSndOnText.gameObject.SetActive (false);
			_effSndOffText.gameObject.SetActive (true);
		} else {
			_effSndSelectedBg.transform.localPosition = _effSndOff.transform.localPosition;
			_effSndOnText.gameObject.SetActive (true);
			_effSndOffText.gameObject.SetActive (false);
		}
	}
}
