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
			/*
			 * {"access_token":"8C81B379BCE72BC8913D84B4EC993070", "msg":"", "pay_token":"3B98C5EA33476B465CD43D04A28FB66C",
			 * "openid":"D408119B7AE5319F3B18BFAC47D1F579", "ret":0, "expires_in":7775999.99989396, 
			 * "pf_key":"8adff5cf6ef03c730ef79719ce48e2fc", "pf":"openmobile_ios"}
			*/
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
			/*
			 * {"figureurl_2":"http://qzapp.qlogo.cn/qzapp/1106301431/D408119B7AE5319F3B18BFAC47D1F579/100", 
			 * "figureurl_qq_2":"http://q.qlogo.cn/qqapp/1106301431/D408119B7AE5319F3B18BFAC47D1F579/100", 
			 * "yellow_vip_level":"0", "msg":"", "province":"\u6d59\u6c5f", "gender":"\u7537", 
			 * "is_yellow_year_vip":"0", "nickname":"Elif", "level":"0", "ret":0,
			 * "figureurl_qq_1":"http://q.qlogo.cn/qqapp/1106301431/D408119B7AE5319F3B18BFAC47D1F579/40", 
			 * "city":"\u676d\u5dde", "vip":"0", "figureurl":"http://qzapp.qlogo.cn/qzapp/1106301431/D408119B7AE5319F3B18BFAC47D1F579/30", 
			 * "figureurl_1":"http://qzapp.qlogo.cn/qzapp/1106301431/D408119B7AE5319F3B18BFAC47D1F579/50", "is_lost":0, "is_yellow_vip":"0"}
			*/

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
