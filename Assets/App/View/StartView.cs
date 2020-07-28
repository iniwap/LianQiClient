using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartView : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//启动动画结束后，切换到帐号登陆界面
		ViewManagerEvent.sShowView data;
		data.fromView = ViewManagerEvent.VIEW_TYPE.START_VIEW;
		data.toView = ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW;
		ViewManagerEvent.EM().InvokeEvent(ViewManagerEvent.EVENT.SHOW_VIEW,(object)data);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
