using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//share sdk 登陆
using cn.sharesdk.unity3d;

public class AccountView : MonoBehaviour {
	//
	public ShareSDK ssdk;
	private NowAccount.eLoginType _loginType = NowAccount.eLoginType.LOGIN_TYPE_NONE;
	private string _thirdOpenId = "";
	private string _thirdToken = "";
	private int _thirdExpireTime = 0;

	// Use this for initialization
	void Start () {
		//add all need game event and call back
		ssdk.authHandler = AuthResultHandler;
		ssdk.showUserHandler = GetUserInfoResultHandler;

		AccountEvent.EM().InvokeEvent(AccountEvent.EVENT.CONNECT_SERVER,(object)false);
	}

	// Update is called once per frame
	void Update () {
	}

	void OnDestroy(){
		//remove all event
	}
	void OnEnable(){
		//may be add again

		AccountEvent.EM().AddEvent(AccountEvent.EVENT.THIRD_PARTY_LOGIN,onThirdPartyLogin);
	}
	void OnDisable(){
		//may be romeve
		AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.THIRD_PARTY_LOGIN);
	}

	public void onClickYKBtn(){
		_loginType = NowAccount.eLoginType.LOGIN_TYPE_YK;
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.LOGIN,(object)(NowAccount.eLoginType.LOGIN_TYPE_YK));
	}
	public void onClickWXBtn(){
		_loginType = NowAccount.eLoginType.LOGIN_TYPE_WX;
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.LOGIN,(object)(NowAccount.eLoginType.LOGIN_TYPE_WX));
	}
	public void onClickQQBtn(){
		_loginType = NowAccount.eLoginType.LOGIN_TYPE_QQ;
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.LOGIN,(object)(NowAccount.eLoginType.LOGIN_TYPE_QQ));
	}

	///////////share sdk ///////////////
	void onThirdPartyLogin(object data){
		
		NowAccount.eLoginType loginType =  (NowAccount.eLoginType)data;

		if (loginType == NowAccount.eLoginType.LOGIN_TYPE_WX) {
			ssdk.Authorize (PlatformType.WeChat);
		}else if(loginType == NowAccount.eLoginType.LOGIN_TYPE_QQ){
			ssdk.Authorize (PlatformType.QQ);
		}
	}
	void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		UnityEngine.Debug.Log("AuthResultHandler"+MiniJSON.jsonEncode (result));

		if (state == ResponseState.Success)
		{
			_thirdOpenId = (string)result["openid"];
			_thirdToken = (string)result["access_token"];
			_thirdExpireTime = (int)(double)result ["expires_in"];
			//获取用户其他信息
			if (_loginType == NowAccount.eLoginType.LOGIN_TYPE_QQ) {
				ssdk.GetUserInfo(PlatformType.QQ);
			}else if(_loginType == NowAccount.eLoginType.LOGIN_TYPE_WX){
				ssdk.GetUserInfo(PlatformType.WeChat);	
			}
		}
		else if (state == ResponseState.Fail)
		{
			CommonUtil.Util.showDialog("温馨提示","授权错误! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
		}
		else if (state == ResponseState.Cancel) 
		{
			CommonUtil.Util.showDialog("温馨提示","您取消了登陆");
		}
	}

	void GetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		UnityEngine.Debug.Log("GetUserInfoResultHandler"+MiniJSON.jsonEncode (result));

		if (state == ResponseState.Success)
		{
			AccountEvent.ThirdPartyLoginResult ret;
			ret.head = (string)result["figureurl_qq_2"];
			ret.name = (string)result["nickname"];
			ret.sex = ((string)result["gender"]) == "女"?0:1;

			ret.openId = _thirdOpenId;
			ret.token = _thirdToken;
			ret.expireTime = _thirdExpireTime;

			AccountEvent.EM().InvokeEvent(AccountEvent.EVENT.THIRD_PARTY_LOGIN_RET,(object)ret);
		}
		else if (state == ResponseState.Fail)
		{
			CommonUtil.Util.showDialog("温馨提示","获取用户信息错误! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);

		}
		else if (state == ResponseState.Cancel) 
		{
			CommonUtil.Util.showDialog("温馨提示","您取消了登陆");
		}
	}
}
