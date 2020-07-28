/**
 *大厅所有界面总管理 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LobbyView : MonoBehaviour {

	//这种界面通过接口直接操作，无须用事件
	public PlazaRoomPanel _plazaRoomPanel;//房间和场列表界面
	public LobbyPanel _lobbyPanel;//主大厅界面
	public TalentPanel  _talentPanel;//天赋配置

	private LobbyEvent.LOBBY_PANEL _currentLobbyPanel = LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL;

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

		//
		showCurrentLobbyPanel(true);
	}
	void OnDisable(){   
		removeAllEvent();
		showCurrentLobbyPanel(false);
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_PACKAGE,onUpdatePackage);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_SIGNIN,onUpdateSignIn);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_LUCKDRAW,onUpdateLuckDraw);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL,onShowLobbyPanel);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_PACKAGE);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_SIGNIN);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_LUCKDRAW);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL);
	}
	//--------------------------以下网络数据更新界面------------------------------
	public void onUpdatePackage(object data){
		
	}
	public void onUpdateSignIn(object data){
		
	}
	public void onUpdateLuckDraw(object data){
		
	}


	//---------------- 界面消息--------------------------
	public void onShowLobbyPanel(object data){
		LobbyEvent.s_V2VShowLobbyPanel lp = (LobbyEvent.s_V2VShowLobbyPanel)data;

		if(lp.from == LobbyEvent.LOBBY_PANEL.LOBBY_NONE_PANEL){

			_currentLobbyPanel = lp.to;

			switch (lp.to) {
			case LobbyEvent.LOBBY_PANEL.LOBBY_PLAZAROOM_PANEL:
				_plazaRoomPanel.onHidePlazaRoomPanel (false);
				_lobbyPanel.gameObject.SetActive (false);
				break;
			case  LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL:
				_lobbyPanel.gameObject.SetActive (true);
				_plazaRoomPanel.onHidePlazaRoomPanel (true);
				break;
			case LobbyEvent.LOBBY_PANEL.LOBBY_TALENT_PANEL:
				_talentPanel.onHideTalentPanel (false);
				_lobbyPanel.gameObject.SetActive (false);
				break;
			}

			return;
		}
		if(lp.to == LobbyEvent.LOBBY_PANEL.LOBBY_NONE_PANEL){
			_plazaRoomPanel.onHidePlazaRoomPanel (true);
			_lobbyPanel.gameObject.SetActive (false);
			return;
		}

		_currentLobbyPanel = lp.to;

		if (lp.from == LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL) {
			_lobbyPanel.gameObject.SetActive (false);
			switch (lp.to) {
			case LobbyEvent.LOBBY_PANEL.LOBBY_PLAZAROOM_PANEL:
				_plazaRoomPanel.onHidePlazaRoomPanel (false);
				break;
			case LobbyEvent.LOBBY_PANEL.LOBBY_TALENT_PANEL:
				_talentPanel.onHideTalentPanel (false);
				break; 
			}
		} else if(lp.from == LobbyEvent.LOBBY_PANEL.LOBBY_PLAZAROOM_PANEL) {
			_plazaRoomPanel.onHidePlazaRoomPanel(true);
			switch (lp.to) {
			case LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL:
				_lobbyPanel.gameObject.SetActive (true);
				break;
			}
		}else if(lp.from == LobbyEvent.LOBBY_PANEL.LOBBY_TALENT_PANEL) {
			_talentPanel.onHideTalentPanel(true);
			switch (lp.to) {
			case LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL:
				_lobbyPanel.gameObject.SetActive (true);
				break;
			}
		}
	}

	public void showCurrentLobbyPanel(bool show){
		if (show) {
			LobbyEvent.s_V2VShowLobbyPanel lp;
			lp.from = LobbyEvent.LOBBY_PANEL.LOBBY_NONE_PANEL;
			lp.to = _currentLobbyPanel;
			onShowLobbyPanel ((object)lp);
		} else {
			LobbyEvent.s_V2VShowLobbyPanel lp;
			lp.from = _currentLobbyPanel;
			lp.to = LobbyEvent.LOBBY_PANEL.LOBBY_NONE_PANEL;
			onShowLobbyPanel ((object)lp);
		}
	}
}
