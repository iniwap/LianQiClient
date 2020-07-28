using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour {
	public GameObject _loadingAni;

	public Dialog _dialogPrefab;
	private Dictionary<ViewManagerEvent.VIEW_TYPE, string> _viewMap;

	private static bool _isChangeScene = false;

	// Use this for initialization
	void Start () {

		ViewManagerEvent.EM().AddEvent(ViewManagerEvent.EVENT.SHOW_VIEW,onShowView);
		ViewManagerEvent.EM().AddEvent(ViewManagerEvent.EVENT.SHOW_DIALOG,onShowDialog);
		ViewManagerEvent.EM().AddEvent(ViewManagerEvent.EVENT.SHOW_LOADING_ANI,onShowLoadingAni);
		ViewManagerEvent.EM().AddEvent(ViewManagerEvent.EVENT.SHOW_SCENE,onShowScene);

		initViewMap ();
		hideAllView ();

		if (!_isChangeScene) {
			showViewFromTo (ViewManagerEvent.VIEW_TYPE.NONE_VIEW, ViewManagerEvent.VIEW_TYPE.START_VIEW);
		} else {
			_isChangeScene = false;
			showViewFromTo (ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW, ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW);
		}
	}
	
	// Update is called once per frame
	void Update () {
		ProtocolManager.getInstance().Update();//此处一直存在
	}
	void onShowView(object data){

		ViewManagerEvent.sShowView showView = (ViewManagerEvent.sShowView)data;
		showViewFromTo(showView.fromView,showView.toView);
	}

	void initViewMap(){
		_viewMap = new Dictionary<ViewManagerEvent.VIEW_TYPE, string>();
		_viewMap.Add (ViewManagerEvent.VIEW_TYPE.NONE_VIEW,"NoneView");
		_viewMap.Add (ViewManagerEvent.VIEW_TYPE.START_VIEW,"StartView");
		_viewMap.Add (ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW,"AccountView");
		_viewMap.Add (ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW,"LobbyView");
		_viewMap.Add (ViewManagerEvent.VIEW_TYPE.GAME_VIEW,"GameView");
	}

	void showViewFromTo(ViewManagerEvent.VIEW_TYPE fromView,ViewManagerEvent.VIEW_TYPE toView){

		//-----------------------处理所有控制器--------------------
		setAllControllerEvent(fromView,toView);

		//此处可以实现各种切换动画
		GameObject root = GameObject.Find("ViewManager");

		Transform tFrom = root.transform.Find(_viewMap[fromView]);
		if (tFrom != null) {
			GameObject viewFrom = tFrom.gameObject;
			string v = _viewMap [fromView];
			if (viewFrom != null) {
				viewFrom.SetActive (false);
			}
		}

		Transform tTo = root.transform.Find (_viewMap [toView]);
		if(tTo != null){
			GameObject viewTo = tTo.gameObject;
			string v2 = _viewMap[toView];
			if (viewTo != null) {
				viewTo.SetActive (true);
			}
		}

	}
	void hideAllView(){
		foreach (KeyValuePair<ViewManagerEvent.VIEW_TYPE, string> kv in _viewMap)
		{
			// kv.Key, kv.Value;
			GameObject view = GameObject.Find(kv.Value);
			if (view != null) {
				view.SetActive (false);
			}
		}
	}
	void setAllControllerEvent(ViewManagerEvent.VIEW_TYPE fromView,ViewManagerEvent.VIEW_TYPE toView){
		//remove
		switch(fromView){
		case ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW:
			AccountController.getInstance ().removeAllEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.GAME_VIEW:
			GameController.getInstance ().removeAllEvent ();
			RoomController.getInstance ().removeRoomGameEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW:
			LobbyController.getInstance ().removeAllEvent ();
			RoomController.getInstance ().removeRoomLobbyEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.START_VIEW:
			break;
		}

		//add
		switch(toView){
		case ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW:
			AccountController.getInstance ().addAllEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.GAME_VIEW:
			GameController.getInstance ().addAllEvent ();
			RoomController.getInstance ().addRoomGameEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW:
			LobbyController.getInstance ().addAllEvent ();
			RoomController.getInstance ().addRoomLobbyEvent ();
			break;
		case ViewManagerEvent.VIEW_TYPE.START_VIEW:
			break;
		}
	}

	public void onShowDialog(object data){
		ViewManagerEvent.s_ShowDialog d = (ViewManagerEvent.s_ShowDialog)data;

		Dialog dlg = Instantiate(_dialogPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		dlg.init (d.type,d.tip,d.hasClose,d.hasOk,d.hasCancel,d.callBack);
		dlg.transform.SetParent (this.transform);
		dlg.transform.position = this.transform.position;
		dlg.transform.localScale = dlg.transform.localScale * CommonUtil.Util.getScreenScale();
		dlg.openDialog (true);
	}
	public void onShowLoadingAni(object data){
		if(_loadingAni != null)
			_loadingAni.SetActive ((bool)data);
	}

	public void onShowScene(object data){
		ViewManagerEvent.sShowScene ss = (ViewManagerEvent.sShowScene)data;
		if (ss.fromScene == ViewManagerEvent.SCENE_TYPE.MACHINE_SCENE) {
			//此时应该去大厅，而不是登陆界面
			if(ss.toScene == ViewManagerEvent.SCENE_TYPE.APP_SCENE){
				SceneManager.LoadScene ("App");
				SceneManager.sceneLoaded += OnSceneLoaded;
			}
		}else if (ss.fromScene == ViewManagerEvent.SCENE_TYPE.APP_SCENE) {
			if(ss.toScene == ViewManagerEvent.SCENE_TYPE.MACHINE_SCENE){
				SceneManager.LoadScene ("StartScene");
			}
		}
	}
	void OnSceneLoaded(Scene scence, LoadSceneMode mod){
		SceneManager.sceneLoaded -= OnSceneLoaded;
		_isChangeScene = true;
	}
}
