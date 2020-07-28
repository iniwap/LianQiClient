using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreView : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//add all need game event and call back
	}

	// Update is called once per frame
	void Update () {
		//如果显示商城界面的时候，销毁了大厅界面，这里就需要调用，如果没有即大厅的update起作用，此处不可调用
		//一般情况的需求，销魂只存在于大厅到游戏，游戏到大厅，为了节省内存空间。大厅里面的所有界面，并不需要切换就销毁
		//特别注意，登陆界面需要销毁，那里也update了，相当于update交给了大厅，后续大厅和游戏切换
		//所以此处不必调用
//		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE,null);
	}

	void OnDestroy(){
		//remove all event
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_STORE);
	}
	void OnEnable(){
		//may be add all event again
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_STORE,onUpdateStore);

		//通知c界面显示，需要更新商城
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_STORE,null);
	}
	void OnDisable(){   
		//may be romeve all event
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_STORE);
	}

	//------------------------------以下界面事件传出-----------------------------------------------
	public void OnClickBuyProduct(){
		//点击购买，后续实现
	}

	//--------------------------以下网络数据更新界面------------------------------
	public void onUpdateStore(object data){
		List<LobbyEvent.Store> storeList = data as List<LobbyEvent.Store>;
		//显示界面
	}

}
