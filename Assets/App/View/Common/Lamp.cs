using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour {

	public Text _lampText;

	private int _speed = 2;
	private int _currentLamp = 0;
	private List<CommonUtil.SysBroadCast> _lampStrList = new List<CommonUtil.SysBroadCast>();
	private int _timeCnt;
	private int _currentPlayCnt = 0;

	void Start(){
		
	}
	void Update(){
		_timeCnt++;

		_lampText.transform.localPosition = new Vector3 (_lampText.transform.localPosition.x - _speed,
			_lampText.transform.localPosition.y,
			_lampText.transform.localPosition.z);

		if (_lampText.transform.localPosition.x < -800) {
			_lampText.transform.localPosition = new Vector3 (1020,
				_lampText.transform.localPosition.y,
				_lampText.transform.localPosition.z);

			if((++_currentPlayCnt) >= _lampStrList [_currentLamp].playCnt){
				//删除该条
				_lampStrList.RemoveAt(_currentLamp);
				_currentPlayCnt = 0;
				if (_lampStrList.Count == 0) {
					//没有了
					this.gameObject.SetActive (false);
					_timeCnt = 0;
					_currentPlayCnt = 0;
					_currentPlayCnt = 0;
					return;
				}

				//下一条
				if (_currentLamp >= _lampStrList.Count - 1) {
					_currentLamp = 0;
					_lampText.text = _lampStrList[0].content;

				} else {
					//_currentLamp++;
					_lampText.text = _lampStrList[_currentLamp].content;
				}
			}
		}
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

	public void addLamp(CommonUtil.SysBroadCast lamp){
		_lampStrList.Add (lamp);
	}

	public int getTotal(){
		return _lampStrList.Count;
	}

	public void showLampById(int id){
		if (id >= _lampStrList.Count || id < 0)
			return;

		this.gameObject.SetActive (true);

		_timeCnt = 0;
		_currentPlayCnt = 0;
		_lampText.text = _lampStrList[id].content;
		_currentLamp = id;

		_lampText.transform.localPosition = new Vector3 (1020,
			_lampText.transform.localPosition.y,
			_lampText.transform.localPosition.z);
	}
	public void resetLamp(){
		_lampStrList.Clear ();

		_currentLamp = 0;
		_timeCnt = 0;
		_currentPlayCnt = 0;

	}
}
