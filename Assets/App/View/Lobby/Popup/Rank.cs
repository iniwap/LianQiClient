/*
 * 单个场的处理
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour {

	//上方
	public Text _myRank;
	public Text _rankPopupTitle;

	//左侧
	public Text _rankTime;
	public Text _rankType;
	public Image _rankAwardIcon;
	public Text[] _rankAwardList;

	//右侧
	public RankItem _rankItemPrefab;
	public GameObject _rankRoot;

	private List<RankItem> _rankList = new List<RankItem>();

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
	}
	void OnDisable(){   
		removeAllEvent();

		//隐藏删除
		for (int i = 0; i < _rankList.Count; i++) {
			Destroy (_rankList[i].gameObject);
		}
		_rankList.Clear ();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_RANK,onUpdateRank);

		//请求显示界面
		LobbyEvent.RankScopeType rst;
		rst.scope = LobbyEvent.RankScopeType.RANK_SCOPE_TYPE.RANK_AREA;
		rst.type = LobbyEvent.RankScopeType.RANK_TYPE.RANK_GOLD;

		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_RANK,(object)rst);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_RANK);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	public void OnClickCloseBtn(){
		this.gameObject.SetActive (false);
	}

	//----------------------------网络更新界面----------------------------------------------------
	public void onUpdateRank(object data){
		List<LobbyEvent.Rank> rankList = (List<LobbyEvent.Rank>)data;

		RectTransform rt = _rankRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x,rankList.Count*66 + 36);
		removeAllRank ();
		//根据type 展示所需要的排行榜 
		for (int i = 0; i < rankList.Count; i++) {
			LobbyEvent.RankScopeType.RANK_TYPE type = rankList[i].rst.type;

			string score = "";
			switch(rankList[i].rst.type){
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_CHARM:
				score = score + rankList[i].charm + " 魅力";
				break;
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_DIAMOND:
				score = score + rankList[i].diamond + " 晶核";
				break;
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_GOLD:
				score = score + rankList[i].gold + " 积分";
				break;
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_SCORE:
				score = score + rankList[i].score + " 段位";
				break;
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_WINRATE:
				score = score + rankList[i].win_rate + " 胜率";
				break;
			case LobbyEvent.RankScopeType.RANK_TYPE.RANK_EXP:
				score = score + rankList[i].exp + " 经验等级（大厅等）";
				break;
			}
			_rankList.Add (createRankItem (i+1,rankList[i].name,score));

			//是否有自己，有自己则设置自己的排名
			if(CommonUtil.Util.getIsSelf(rankList[i].userID)){
				_myRank.text = "你的排名："+(i+1);
			}
		}

		//设置时间
		_rankTime.text = DateTime.Today.ToString("yyyy年MM月dd日");
	}

	//-------------------------一些接口--------------------------------------------------------
	public RankItem createRankItem(int rank,string name,string score){
		RankItem rankItem = Instantiate(_rankItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		rankItem.transform.SetParent(_rankRoot.transform);

		rankItem.transform.localPosition = new Vector3(0,- (rank-1) * 66,0);

		rankItem.updateRankItem (rank,name,score);

		rankItem.transform.localScale = rankItem.transform.localScale * CommonUtil.Util.getScreenScale();

		return rankItem;
	}
	public void removeAllRank(){
		for (int i = 0; i < _rankList.Count; i++) {
			if (_rankList [i] != null) {
				Destroy (_rankList [i].gameObject);
			}
			_rankList [i] = null;
		}

		_rankList.Clear ();
	}
}
