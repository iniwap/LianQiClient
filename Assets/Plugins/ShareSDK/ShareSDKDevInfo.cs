using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet
	{
		public SinaWeiboDevInfo sinaweibo;
		public Facebook facebook;
		public Twitter twitter;
		public Email email;
		public ShortMessage shortMessage;
		public Instagram instagram;
		public QQ qq;
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;

		#if UNITY_ANDROID
		#elif UNITY_IPHONE		
		public Copy copy;
		public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
		public QQSeries qqSeries;								//iOS端QQ系列,  可直接配置QQ系列两个子平台		[仅支持iOS端]
		#endif

	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}

	[Serializable]
	public class SinaWeiboDevInfo : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.SinaWeibo;
		public string SortId = "4";
		public string AppKey = "568898243";
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public bool ShareByAppClient = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SinaWeibo;
		public string app_key = "568898243";
		public string app_secret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string redirect_uri = "http://www.sharesdk.cn";
		public string auth_type = "both";	//can pass "both","sso",or "web"  
		#endif
	}

	[Serializable]
	public class QQ : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.QQ;
		public string SortId = "2";
		public string AppId = "1106301431";
		public string AppKey = "9TQjV0uGhMLzvRX4";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "1106301431";
		public string app_key = "9TQjV0uGhMLzvRX4";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#endif
	}
		
	[Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public string userName = "gh_afb25ac019c9@app";
		public string path = "/page/API/pages/share/share";
		public bool BypassApproval = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class WeChatFavorites : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFavorites;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class Facebook : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "8";
		public const int type = (int) PlatformType.Facebook;
		public string ConsumerKey = "107704292745179";
		public string ConsumerSecret = "38053202e1a5fe26c80c753071f0b573";
		public string RedirectUrl = "http://mob.com/";
		public bool ShareByAppClient = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Facebook;
		public string api_key = "107704292745179";
		public string app_secret = "38053202e1a5fe26c80c753071f0b573";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		public string display_name = "ShareSDK";//如果需要使用客户端分享，必填且需与FB 后台配置一样
		#endif
	}

	[Serializable]
	public class Twitter : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "9";
		public const int type = (int) PlatformType.Twitter;
		public string ConsumerKey = "LRBM0H75rWrU9gNHvlEAA2aOy";
		public string ConsumerSecret = "gbeWsZvA9ELJSdoBzJ5oLKX0TU09UOwrzdGfo9Tg7DjyGuMe8G";
		public string CallbackUrl = "http://mob.com";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Twitter;
		public string consumer_key = "LRBM0H75rWrU9gNHvlEAA2aOy";
		public string consumer_secret = "gbeWsZvA9ELJSdoBzJ5oLKX0TU09UOwrzdGfo9Tg7DjyGuMe8G";
		public string redirect_uri = "http://mob.com";
		#endif
	}

	[Serializable]
	public class Email : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "12";
		public const int type = (int) PlatformType.Mail;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Mail;
		#endif
	}

	[Serializable]
	public class ShortMessage : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "13";
		public const int type = (int) PlatformType.SMS;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SMS;
		#endif
	}
		
	[Serializable]
	public class Instagram : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "26";
		public const int type = (int) PlatformType.Instagram;
		public string ClientId = "ff68e3216b4f4f989121aa1c2962d058";
		public string ClientSecret = "1b2e82f110264869b3505c3fe34e31a1";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Instagram;
		public string client_id = "ff68e3216b4f4f989121aa1c2962d058";
		public string client_secret = "1b2e82f110264869b3505c3fe34e31a1";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}


	
	[Serializable]
	public class Bluetooth : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "34";
		public const int type = (int) PlatformType.Bluetooth;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Bluetooth;
		#endif
	}

	[Serializable]
	public class Copy : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.Copy;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Copy;
		#endif
	}

	[Serializable]
	public class WechatSeries : DevInfo 
	{	
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "Wechat" ,class "WechatMoments" or class "WechatFavorite"
		//对于安卓端，请在类Wechat,WechatMoments或WechatFavorite中配置相关信息↑	
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WechatPlatform;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class QQSeries : DevInfo 
	{	
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "QQ" and  class "QZone"
		//对于安卓端，请在类QQ或QZone中配置相关信息↑	
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQPlatform;
		public string app_id = "1106301431";
		public string app_key = "9TQjV0uGhMLzvRX4";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#endif
	}

}
