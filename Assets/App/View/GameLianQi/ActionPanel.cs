/**
 * 显示、处理所有非棋盘本身的 游戏事件
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Sprites;

public class ActionPanel : MonoBehaviour {


	public Text _clock;
	public Text _round;
	public Button _btnPass;

	private int _lmtTurnTime;
	private int _leftTime;

	public enum eActionStep{
		STEP_NOT_TURN,
		STEP_PLAY,
		STEP_WAIT_PLAY_RESULT,
		//STEP_MOVE_OR_PASS,
		STEP_WAIT_MOVE_RESULT,
		STEP_PASS,
		STEP_WAIT_PASS_RESULT,
	};

	private eActionStep _currentStep;

	// 初始化
	void Start(){

	}
	void Update(){

	}

	void OnDestroy(){
		//remove all event
		removeAllEvent();
	}
	void OnEnable(){
		addAllEvent ();
	}
	void OnDisable(){
		removeAllEvent();
	}


	//----------------------------------------------------------------
	void addAllEvent(){

		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_FAIL, onActionFail);

		GameEvent.EM ().AddEvent (GameEvent.EVENT.CLOCK,onShowClock);


	}
	void removeAllEvent(){

		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_FAIL);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.CLOCK);	
	}

	//----------------------------界面事件发出----------------------------
	public void  OnMoveBtn(){
		//不在move(即可以pass阶段)步骤，不能移动
		/*
		if (_currentStep != eActionStep.STEP_MOVE_OR_PASS) {
			return;
		}
		*/
		if (_currentStep != eActionStep.STEP_PASS) {
			return;
		}

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_MOVE,null);

		setCurrentStep(eActionStep.STEP_WAIT_MOVE_RESULT);

	}
	public void  OnPassBtn(){
		_btnPass.interactable  = false;

		// 这里需要判断当前处于哪个阶段来决定是落子还是结束回合
		//不在play步骤，不能落子
		if (_currentStep == eActionStep.STEP_PLAY) {
			
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_PLAY,null);

			setCurrentStep (eActionStep.STEP_WAIT_PLAY_RESULT);

			return;
		}else if (_currentStep == eActionStep.STEP_PASS) {

			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_PASS,null);
			setCurrentStep (eActionStep.STEP_WAIT_PASS_RESULT);

			return;
		}/*else if (_currentStep == eActionStep.STEP_MOVE_OR_PASS) {

			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_MOVE,null);
			_currentStep = eActionStep.STEP_WAIT_MOVE_RESULT;

			return;
		}
		*/
	}
		
	//-----------------------------接收消息处理------------------------
	public void onInitActionPanel(object data){
		RoomEvent.sC2V_RoomRule rr = (RoomEvent.sC2V_RoomRule)data;
		_round.text = "回合 1" + "/" + rr.lmtRound;

		_round.gameObject.SetActive (false);

		_lmtTurnTime = rr.lmtTurnTime;
		_leftTime = _lmtTurnTime;
		if (_lmtTurnTime == 0) {
			//不限制每手时长
			_clock.gameObject.SetActive(false);
		}

		setCurrentStep (eActionStep.STEP_NOT_TURN);

	}
	public void onShowAbandonPass(object data){

	}
	public void onShowPass(object data){

		GameEvent.sC2V_ShowTurn st;
		st.local = (int)data;
		st.isPassTurn = true;
		st.isTimeOut = false;

		//
		//string lmtRound = CommonUtil.Util.split(_round.text,'/')[1];
		string currentRound = CommonUtil.Util.split(_round.text,'/')[0];
		currentRound = CommonUtil.Util.split(currentRound,' ')[1];
		st.round = System.Convert.ToInt32(currentRound) + 1;//因为是自己点了pass，客户端自动＋1
		st.banDirList = new List<int> ();

		showTurn(true,st);

		//设置非自己轮
		setCurrentStep (eActionStep.STEP_NOT_TURN);
	}

	public void showTurn(bool isPass,GameEvent.sC2V_ShowTurn st){
		if (!isPass) {
			if ((int)st.local == (int)CommonDefine.SEAT.SELF) {
				setCurrentStep (eActionStep.STEP_PLAY);
			} else {
				setCurrentStep (eActionStep.STEP_NOT_TURN);
			}
		}

		//设置按钮图为对应的出手方
		_btnPass.image.sprite = CommonUtil.Util.getSpriteByLocal(st.local,CommonDefine.ResPath.LQ_PASS_BTN);
		if ((int)st.local == (int)CommonDefine.SEAT.SELF) {
			_btnPass.interactable = true;

			for (int i = 0; i < 6; i++) {
				banDir (false,i);
			}

		} else {
			_btnPass.interactable = false;
		}

		setPassBtnTxt("Play");

		if (!isPass) {
			//ON TURN
			//设置禁用方向
			for (int i = 0; i < st.banDirList.Count; i++) {
				if (st.banDirList [i] != -1) {
					banDir (true, st.banDirList [i]);
				}
			}
		}

		if (st.isPassTurn) {
			updateRound (st.round);
		}

		if (_lmtTurnTime != 0) {
			if (IsInvoking ("updateClock")) {
				CancelInvoke();
			}
			_leftTime = _lmtTurnTime;// 重置限手时间

			InvokeRepeating("updateClock", 0.0f, 1.0f);	
		}
	}
	public void onShowTurn(object data){
		GameEvent.sC2V_ShowTurn st = (GameEvent.sC2V_ShowTurn)data;
		showTurn (false,st);
	}
	public void onShowMove(object data){
		setCurrentStep (eActionStep.STEP_PASS);
	}
	public void onShowPlay(object data){

		GameEvent.sC2V_PlayOrMove pm = (GameEvent.sC2V_PlayOrMove)data;

		if (pm.local == (int)CommonDefine.SEAT.SELF) {
			_btnPass.interactable = true;
		}

		//落子之棋盘不能操作，无论是否是自己
		for (int i = 0; i < 6; i++) {
			banDir (true,i);
		}

		setPassBtnTxt("Pass");

		setCurrentStep(eActionStep.STEP_PASS);
	}

	public void onActionFail(object data){
		switch ((GameEvent.EVENT)data) {
		case GameEvent.EVENT.ABANDON:
			break;
		case GameEvent.EVENT.DRAW:
			break;
		case GameEvent.EVENT.PLAY:
			_btnPass.interactable = true;
			setCurrentStep(eActionStep.STEP_PLAY);
			break;
		case GameEvent.EVENT.PASS:
			setCurrentStep(eActionStep.STEP_PASS);
			_btnPass.interactable = true;
			break;
		case GameEvent.EVENT.MOVE:
			//_currentStep = eActionStep.STEP_MOVE_OR_PASS;
			setCurrentStep(eActionStep.STEP_PASS);
			break;
		}

		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.TO_LQP_ACTION_FAIL,data);
	}

	public void onShowClock(object data){
		//GameEvent.sC2V_ShowClock sc = (GameEvent.sC2V_ShowClock)data;
		//此处是其他时间，每手限制时间通过房间规则实现
	}

	public void onUpdateFirstHandSeat(object data){
		//开局所有方向可用
		int firstHand = CommonUtil.Util.getLocalBySeat((int)data);

		for (int i = 0; i < 6; i++) {
			if (firstHand != (int)CommonDefine.SEAT.SELF) {
				banDir (true,i);
			}
		}

	}

	//--------------------------------------------------------

	public void updateClock(){

		_clock.text = ""+_leftTime + "s";
		//通知lq panel
		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.TO_LQP_CLOCK,(object)_leftTime);

		if (_leftTime == 0) {
			// 超时了
			//自动pass
			CancelInvoke();
			return;
		}

		--_leftTime;
	}
	public void updateRound(int round){
		//更新回合数

		int lmtRound = System.Convert.ToInt32(CommonUtil.Util.split(_round.text,'/')[1]);

		//不限制回合场，不显示回合提示
		if (lmtRound == 0)
			return;

		_round.text = "回合 " + round + "/" + lmtRound;
		//最后10回合一直提示
		if (lmtRound - round <= 10) {
			//一直显示
			_round.gameObject.SetActive(true);
		} else {
			if(round % 10 == 0){
				//每10个回合显示一次
				_round.gameObject.SetActive(true);
			}else{
				_round.gameObject.SetActive(false);
			}
		}
	}

	private void banDir(bool ban,int dir){
		GameEvent.sV2V_ActionBanDir abd;
		abd.ban = ban;
		abd.dir = dir;

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_BAN_DIR,(object)abd);
	}

	private void setPassBtnTxt(string txt){
		Text btnTxt = _btnPass.GetComponentInChildren<Text>();
		btnTxt.text = txt;
	}
	private void setCurrentStep(eActionStep step){

		_currentStep = step;
		GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_STEP,(int)_currentStep);
	}
}
