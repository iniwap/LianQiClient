/*
 * 天赋配置界面
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleJson;

public class TalentPanel : MonoBehaviour {

	//用户信息
	public Text _userGold;
	public Text _userDiamond;

	public Image _currentTalentIcon;
	public Text _currentTalentName;
	public Text _currentTalentDes;
	private CommonDefine.eTalentType _talentType;
	private CommonDefine.eTalentType _prevTalentType;
	private bool isBaseSelected;

	public Button _baseTalentBtn;
	public Button _highTalentBtn;

	public Button _installBtn;
	public Button _uninstallBtn;

	public Button _toLeftSwitchBtn;
	public Button _toRightSwitchBtn;
	public GameObject _talentItemRoot;
	public TalentItem _talentPrefab;
	public float _talentInteral;
	private List<TalentItem> _talentItemList = new List<TalentItem>();

	private int _canInstallId;
	private int _openSlotId;
	public Text _currentAndTotal;


	//消耗详情
	public GameObject _baseTalentCostInfoRoot;
	public GameObject _highTalentCostInfoRoot;
	public TalentCostItem _talentCostPrefab;
	public float _talentCostInteral;
	private List<TalentCostItem> _talentCostItemList = new List<TalentCostItem>();
	public Text _totalCost;

	//只是保存天赋数据
	private struct TalentData{
		public CommonDefine.eTalentType talentType;
		public string name;
		public string des;
		public int cost;// 装备增加消耗
	};

	//用来保存可配置天赋槽
	private struct TalentSlot{
		public CommonDefine.eTalentType talentType;
		public int id;
		public CommonDefine.TalentSlotState btnState;
	}

	private List<TalentData> _talentList;
	private List<TalentSlot> _talentSlotList;

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
		initTalentPanel ();

		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_USER_INFO,null);
	}
	void OnDisable(){   
		removeAllEvent();
		//隐藏删除
		for (int i = 0; i < _talentItemList.Count; i++) {
			Destroy (_talentItemList[i].gameObject);
		}
		_talentItemList.Clear ();

		for (int i = 0; i < _talentCostItemList.Count; i++) {
			Destroy (_talentCostItemList[i].gameObject);
		}
		_talentCostItemList.Clear ();
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_USER_INFO,onUpdateUserInfo);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.RESP_OPEN_TALENTSLOT,onOpenTalentSlot);
	}
	void removeAllEvent(){
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_USER_INFO);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.RESP_OPEN_TALENTSLOT);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	//基础天赋
	public void OnClickBaseTalentBtn(){
		if(isBaseSelected){
			return;
		}

		_talentType = CommonDefine.eTalentType.TALENT_A1;//每次进来默认显示A1
		_prevTalentType = _talentType;

		isBaseSelected = true;

		//基础天赋，不显示高亮按钮

		_currentAndTotal.text = (int)_talentType + "/" + ((int)CommonDefine.eTalentType.TALENT_B2 - (int)CommonDefine.eTalentType.TALENT_A1 + 1);

		_installBtn.gameObject.SetActive (false);
		_uninstallBtn.gameObject.SetActive (false);

		_toLeftSwitchBtn.interactable = false;
		_toRightSwitchBtn.interactable = true;

		removeAllTalent ();

		RectTransform rt = _talentItemRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x,((int)CommonDefine.eTalentType.TALENT_B3 -(int)CommonDefine.eTalentType.TALENT_A1 )*_talentInteral);
		//根据_talentList 生成界面
		for(int i = (int)CommonDefine.eTalentType.TALENT_A1;i < (int)CommonDefine.eTalentType.TALENT_B3;i++){
			_talentItemList.Add (createTalentItem (_talentList[i - 1].talentType,
				i - 1,
				_talentList[i - 1].name,
				_talentList[i - 1].des,
				CommonDefine.TalentSlotState.TALENT_INSTALLED));

		}

		onUpdateSelectTalent ();

	}
	//高级天赋
	public void OnClickHighTalentBtn(){
		if(!isBaseSelected){
			return;
		}

		_talentType = CommonDefine.eTalentType.TALENT_B3;//每次进来默认显示b3
		_prevTalentType = _talentType;
		_canInstallId = -1;

		isBaseSelected = false;
		_installBtn.gameObject.SetActive (false);
		_uninstallBtn.gameObject.SetActive (false);

		_toLeftSwitchBtn.interactable = false;
		_toRightSwitchBtn.interactable = true;

		_currentAndTotal.text = ((int)_talentType - (int)CommonDefine.eTalentType.TALENT_B3 + 1) + "/" + ((int)CommonDefine.eTalentType.TALENT_C1 - (int)CommonDefine.eTalentType.TALENT_B3 + 1);


		if(!isBaseSelected && getIfSlotInstalled(_talentType)){
			_uninstallBtn.gameObject.SetActive (true);
		}

		removeAllTalent ();

		RectTransform rt = _talentItemRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x,((int)CommonDefine.eTalentType.TALENT_C1 -(int)CommonDefine.eTalentType.TALENT_B3 )*_talentInteral);

		//_talentSlotList 生成界面
		for (int i = 0;i < _talentSlotList.Count;i++) {
			CommonDefine.eTalentType ttp = _talentSlotList[i].talentType;
			string name = "";
			string des = "";
			if (ttp != CommonDefine.eTalentType.TALENT_NONE) {
				name = _talentList[(int)ttp - 1].name;
				des = _talentList[(int)ttp - 1].des;
			}
			_talentItemList.Add (createTalentItem (ttp, _talentSlotList[i].id, name,des,_talentSlotList[i].btnState));

		}

		//if(self.talent != )

		onUpdateSelectTalent ();
	}

	public void OnClickBuyBtn(int id){
		//0  增加金币/积分  1增加钻石

	}
	public void OnClickBackBtn(){
		//
		//切换到主大厅界面
		LobbyEvent.s_V2VShowLobbyPanel lp;
		lp.from = LobbyEvent.LOBBY_PANEL.LOBBY_TALENT_PANEL;
		lp.to = LobbyEvent.LOBBY_PANEL.LOBBY_LOBBY_PANEL;
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_LOBBY_PANEL,(object)lp);
	}

	public void OnClickInstallBtn(bool isInstall){
		if (isInstall) {
			//安装
			//装备当前选中的天赋
			if (getIfSlotInstalled(_talentType)) {
				//不能装备已经装备的
				CommonUtil.Util.showDialog ("温馨提示","【"+_talentList[(int)_talentType - 1].name + "】已经装备过了！");
			} else {
				_installBtn.gameObject.SetActive(false);
				_uninstallBtn.gameObject.SetActive(true);

				if (_canInstallId == -1)
					return;

				if (_talentSlotList [_canInstallId].talentType == CommonDefine.eTalentType.TALENT_NONE
					&& _talentSlotList [_canInstallId].btnState == CommonDefine.TalentSlotState.TALENT_CAN_INSTALL ) {


					TalentSlot tst = _talentSlotList [_canInstallId];

					tst.talentType = _talentType;
					tst.id = _canInstallId;
					tst.btnState = CommonDefine.TalentSlotState.TALENT_INSTALLED;
					_talentSlotList [_canInstallId] = tst;

					_talentItemList [_canInstallId].updateTalentItem (this,
						_talentType,
						_canInstallId,
						_talentList[(int)_talentType - 1].name,
						_talentList[(int)_talentType - 1].des,
						CommonDefine.TalentSlotState.TALENT_INSTALLED);

					saveTalentCfg (true);

					onUpdateTalentCost ();
				}

				_canInstallId = -1;
			}
		} else {
			//卸载
			for(int i = 0;i < _talentSlotList.Count;i++){
				if (_talentSlotList [i].talentType == _talentType
				   && _talentSlotList [i].btnState == CommonDefine.TalentSlotState.TALENT_INSTALLED) {
					//卸载这个天赋
					TalentSlot tst = _talentSlotList [i];
					tst.btnState = CommonDefine.TalentSlotState.TALENT_CAN_INSTALL;
					tst.talentType = CommonDefine.eTalentType.TALENT_NONE;
					_talentSlotList [i] = tst;

					_talentItemList [_talentSlotList [i].id].updateTalentItem (this,
						CommonDefine.eTalentType.TALENT_NONE,
						i,
						"",
						"",
						CommonDefine.TalentSlotState.TALENT_CAN_INSTALL);

					_installBtn.gameObject.SetActive(false);
					_uninstallBtn.gameObject.SetActive(false);

					saveTalentCfg (true);

					onUpdateTalentCost ();

					break;
				}
			}
		}
	}

	public void OnClickTalentLockBtn(int id){
		//开槽

		_openSlotId = id;

		LobbyEvent.sV2C_OpenTalentslot ot;
		ot.type = CommonDefine.eOpenByType.OPEN_BY_DIAMOND;//默认只能钻石开槽。如果需要别的，这里需要弹窗，并在选择后请求

		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.REQ_OPEN_TALENTSLOT,(object)ot);
	}
	public void OnClickTalentCanInstallBtn(int id){
		//装备当前选中的天赋
		if (getIfSlotInstalled(_talentType)) {
			// 此处需要切换
			for (int i = (int)CommonDefine.eTalentType.TALENT_B3; i <= (int)CommonDefine.eTalentType.TALENT_C1; i++) {
				if (!getIfSlotInstalled (_talentList [i - 1].talentType)) {

					//切换到这个
					_talentType = _talentList [i - 1].talentType;

					onUpdateSelectTalent ();
					hightlightSelect ();
					checkSwitchTalentBtn ();

					_installBtn.gameObject.SetActive(true);
					_uninstallBtn.gameObject.SetActive(false);

					_canInstallId = id;

					break;
				}

			}
		} else {
			
			_installBtn.gameObject.SetActive(true);
			_uninstallBtn.gameObject.SetActive(false);

			_canInstallId = id;
			//此处最好对该按钮做一定的动画或者高亮操作
		}

	}
	public void OnClickTalentHasInstallBtn(int id){
		//显示已经装备的天赋
		_installBtn.gameObject.SetActive(false);
		_uninstallBtn.gameObject.SetActive (false);


		if(!isBaseSelected){
			_uninstallBtn.gameObject.SetActive (true);
		}

		if (isBaseSelected) {
			_talentType = _talentList[id].talentType;
		} else {
			_talentType = _talentSlotList [id].talentType;
		}

		checkSwitchTalentBtn ();

		onUpdateSelectTalent ();
	}

	public void OnClickHightlightBtn(int index){
		_talentType = (CommonDefine.eTalentType)(index + CommonDefine.eTalentType.TALENT_B3);
		onUpdateSelectTalent ();
	}

	public void OnClickSwitchTalentBtn(bool left){

		if (left) {
			if (isBaseSelected) {
				if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_A1) {
					_toLeftSwitchBtn.interactable = false;
					_toRightSwitchBtn.interactable = true;
				} else {
					_talentType = (CommonDefine.eTalentType)((int)_talentType - 1);
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = true;

				}
			} else {
				//
				if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_B3) {
					_toLeftSwitchBtn.interactable = false;
					_toRightSwitchBtn.interactable = true;
				} else {
					_talentType = (CommonDefine.eTalentType)((int)_talentType - 1);
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = true;

				}
			}
		} else {
			if (isBaseSelected) {
				if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_B2) {
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = false;
				} else {
					_talentType = (CommonDefine.eTalentType)((int)_talentType + 1);
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = true;

				}
			} else {
				//
				if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_C1) {
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = false;
				} else {
					_talentType = (CommonDefine.eTalentType)((int)_talentType + 1);
					_toLeftSwitchBtn.interactable = true;
					_toRightSwitchBtn.interactable = true;
				}
			}
		}

		_installBtn.gameObject.SetActive(false);
		_uninstallBtn.gameObject.SetActive (false);

		if(!isBaseSelected){
			//_uninstallBtn.gameObject.SetActive (true);//切换默认丢失焦点，不显示安装卸载
		}

		checkSwitchTalentBtn ();
		onUpdateSelectTalent ();
	}
	//--------------------------以下网络数据更新界面------------------------------

	void onUpdateUserInfo(object data){
		//假设这里传递了整个account信息，实际上没有必要，也可以另外定义，其他均为相同做法
		SelfData self = (SelfData)data;

		// 刷新所有用户相关的信息
		//比如金币显示，昵称，头像等等
		_userGold.text = ""+self.gold;
		_userDiamond.text = ""+self.diamond;


		//需要处理槽数和目前可安装+已安装不相等的情况
		checkTalentSlotNum(self.talent);
		OnClickHighTalentBtn ();

		onUpdateTalentCost ();
	}

	void onOpenTalentSlot(object data){

		LobbyEvent.s_C2V_UpdateTalentForOpenSlot utfos = (LobbyEvent.s_C2V_UpdateTalentForOpenSlot)data;

		if (utfos.ret == CommonDefine.eRespResultType.SUCCESS) {
			//开槽成功
			TalentSlot tst = _talentSlotList[_openSlotId];
			tst.btnState = CommonDefine.TalentSlotState.TALENT_CAN_INSTALL;
			_talentSlotList [_openSlotId] = tst;

			saveTalentCfg(false);
			//刷新界面数据
			_userGold.text = "" + utfos.gold;
			_userDiamond.text = "" + utfos.diamond;
			//utfos.currentTalentNum

			_talentItemList [_openSlotId].updateTalentItem (this,CommonDefine.eTalentType.TALENT_NONE,_openSlotId,"","",
				CommonDefine.TalentSlotState.TALENT_CAN_INSTALL);

			//此处失去选中焦点
			_installBtn.gameObject.SetActive(false);
			_uninstallBtn.gameObject.SetActive (false);

		} else {
			
		}
	}
	//----------------一些接口------------------------------------------------
	public void onHideTalentPanel(bool hide){
		this.gameObject.SetActive (!hide);
		if (hide) {
			/*
			for (int i = 0; i < _realPlaza.Count; i++) {
				Destroy (_realPlaza [i].gameObject);
			}

			_realPlaza.Clear ();
			*/
		}
	}

	void onUpdateSelectTalent(){
		
		//_talentType
		_currentTalentIcon.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.TALENT_ICON_BTN + (int)_talentType);
		_currentTalentName.text = _talentList[(int)_talentType - 1].name;
		_currentTalentDes.text = "技能说明： "+_talentList[(int)_talentType - 1].des;

		hightlightSelect ();

	}

	void onUpdateTalentCost(){
		//BASE 固定显示
		removeAllTalentCost ();

		int totalCost = 0;
		//基础天赋消耗
		RectTransform rt = _baseTalentCostInfoRoot.gameObject.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x,((int)CommonDefine.eTalentType.TALENT_B3 -(int)CommonDefine.eTalentType.TALENT_A1 )*_talentCostInteral);

		for(int i = (int)CommonDefine.eTalentType.TALENT_A1;i <= (int)CommonDefine.eTalentType.TALENT_B2;i++){
			_talentCostItemList.Add (createTalentCostItem (_baseTalentCostInfoRoot,
				(CommonDefine.eTalentType)i,
				i-1,
				_talentList[i - 1].name,
				_talentList[i - 1].cost));
			totalCost += _talentList[i - 1].cost;
		}

		//高级天赋消耗

		RectTransform rt1 = _highTalentCostInfoRoot.gameObject.GetComponent<RectTransform>();
		rt1.sizeDelta = new Vector2(rt1.sizeDelta.x,((int)CommonDefine.eTalentType.TALENT_C1 -(int)CommonDefine.eTalentType.TALENT_B3 )*_talentCostInteral);

		int offset = 0;
		for (int i = 0; i < _talentSlotList.Count; i++) {
			if (_talentSlotList [i].btnState == CommonDefine.TalentSlotState.TALENT_INSTALLED) {
				//
				_talentCostItemList.Add (createTalentCostItem (_highTalentCostInfoRoot,
					_talentSlotList [i].talentType,
					i - offset,
					_talentList [(int)_talentSlotList [i].talentType - 1].name,
					_talentList [(int)_talentSlotList [i].talentType - 1].cost));
				
				totalCost += _talentList [(int)_talentSlotList [i].talentType - 1].cost;
			} else {
				offset++;
			}
		}


		//总消耗数
		_totalCost.text = "" + totalCost;
	}

	void initTalentPanel (){
		isBaseSelected = true;

		//test  remove all cfg
		//CommonUtil.Util.delAllPlayerPrefs();

		loadTalentCfg ();

	}

	void loadTalentCfg(){

		//加载天赋配置文件
		JsonObject talentJsonCfg = CommonUtil.Util.getTalentConfig();
		_talentList = new List<TalentData> ();
		for(int i = (int)CommonDefine.eTalentType.TALENT_A1;i <= (int)CommonDefine.eTalentType.TALENT_C1;i++){

			TalentData td;
			td.talentType = (CommonDefine.eTalentType)i;

			System.Object v;
			System.Object name;
			System.Object des;
			System.Object cost;

			if (talentJsonCfg.TryGetValue (""+i, out v)) {
				JsonObject tt = (JsonObject)v;
				tt.TryGetValue ("name", out name);
				td.name = (string)name;
				tt.TryGetValue ("des", out des);
				td.des = (string)des;

				tt.TryGetValue ("cost", out cost);
				td.cost = System.Convert.ToInt32(cost) ;

				_talentList.Add (td);
			}
		}

		//加载配置信息
		_talentSlotList = new List<TalentSlot> ();
		for (int i = (int)CommonDefine.eTalentType.TALENT_B3; i <= (int)CommonDefine.eTalentType.TALENT_C1; i++) {
			TalentSlot ts;
			ts.talentType = CommonDefine.eTalentType.TALENT_NONE;
			ts.id = i - (int)CommonDefine.eTalentType.TALENT_B3;
			ts.btnState = CommonDefine.TalentSlotState.TALENT_LOCK;
			_talentSlotList.Add (ts);
		}

		string btnStateStr = CommonUtil.Util.getPlayerPrefs (CommonDefine.CONST.TALENT_SLOT_STATE, "");
		if (btnStateStr != "") {
			string [] btnStates = btnStateStr.Split (',');
			foreach (string st in btnStates) {
				string[] data = st.Split ('#');

				for(int i = 0;i < _talentSlotList.Count;i++){
					if (_talentSlotList [i].id == int.Parse (data [1])) {
						TalentSlot tst = _talentSlotList [i];
						tst.talentType = (CommonDefine.eTalentType)int.Parse (data [0]);
						tst.btnState = (CommonDefine.TalentSlotState)(int.Parse (data [2]));
						_talentSlotList [i] = tst;
					}
					
				}
			}
		}
	}

	public void removeAllTalent(){
		for (int i = 0; i < _talentItemList.Count; i++) {
			if (_talentItemList [i] != null) {
				Destroy (_talentItemList [i].gameObject);
			}
			_talentItemList [i] = null;
		}

		_talentItemList.Clear ();
	}

	public void removeAllTalentCost(){
		for (int i = 0; i < _talentCostItemList.Count; i++) {
			if (_talentCostItemList [i] != null) {
				Destroy (_talentCostItemList [i].gameObject);
			}
			_talentCostItemList [i] = null;
		}

		_talentCostItemList.Clear ();
	}
	public TalentItem createTalentItem(CommonDefine.eTalentType type,int id,string name,string des,CommonDefine.TalentSlotState btnState){
		TalentItem talentItem = Instantiate(_talentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		talentItem.transform.SetParent(_talentItemRoot.transform);

		talentItem.transform.localPosition = new Vector3(0,- id * _talentInteral,0);

		talentItem.updateTalentItem (this,type,id, name,des,btnState);

		talentItem.transform.localScale = talentItem.transform.localScale * CommonUtil.Util.getScreenScale();

		return talentItem;
	}

	public TalentCostItem createTalentCostItem(GameObject root,CommonDefine.eTalentType type,int id,string name,int cost){
		TalentCostItem talentItem = Instantiate(_talentCostPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		talentItem.transform.SetParent(root.transform);

		talentItem.transform.localPosition = new Vector3(0,- id * _talentCostInteral,0);

		talentItem.updateTalentItem (type, name,cost);

		talentItem.transform.localScale = talentItem.transform.localScale * CommonUtil.Util.getScreenScale();

		return talentItem;
	}

	public void saveTalentCfg(bool sholudRefreshUserTalent){
		//"2#1#1,4#2#2,..."

		string cfg = "";
		for (int i = 0;i< _talentSlotList.Count;i++) {
			cfg = cfg + (int)_talentSlotList[i].talentType + "#" + _talentSlotList[i].id +"#"+ (int)_talentSlotList[i].btnState + ",";
		}

		if (cfg == "")
			return;
		
		cfg = cfg.Substring (0,cfg.Length - 1);

		CommonUtil.Util.setPlayerPrefs (CommonDefine.CONST.TALENT_SLOT_STATE,cfg);

		if (sholudRefreshUserTalent) {
			LobbyEvent.s_V2C_UpdateUserTalent uut;
			List<CommonDefine.eTalentType> tl = new List<CommonDefine.eTalentType>();

			for (int i = 0; i < _talentSlotList.Count; i++) {
				if (_talentSlotList [i].btnState == CommonDefine.TalentSlotState.TALENT_INSTALLED) {
					tl.Add (_talentSlotList [i].talentType);
				}
			}

			uut.talentList = tl;

			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.UPDATE_USER_TALENT,(object)uut);
		}
	}

	void hightlightSelect(){
		if (isBaseSelected) {
			_currentAndTotal.text = "" + (int)_talentType + "/" + ((int)CommonDefine.eTalentType.TALENT_B2 - (int)CommonDefine.eTalentType.TALENT_A1 + 1);
		} else {
			_currentAndTotal.text = "" + ((int)_talentType - (int)CommonDefine.eTalentType.TALENT_B3 + 1) + "/" + ((int)CommonDefine.eTalentType.TALENT_C1 - (int)CommonDefine.eTalentType.TALENT_B3 + 1);
		}
	}
	bool getIfSlotInstalled(CommonDefine.eTalentType ttp){
		for(int i = 0;i < _talentSlotList.Count;i++ ){
			if(_talentSlotList[i].talentType == ttp ){
				if (_talentSlotList [i].btnState == CommonDefine.TalentSlotState.TALENT_INSTALLED) {
					return true;
				}
				return false;
			}
		}
		return false;
	}

	void checkSwitchTalentBtn(){

		_toLeftSwitchBtn.interactable = true;
		_toRightSwitchBtn.interactable = true;

		if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_A1) {
			_toLeftSwitchBtn.interactable = false;
		} else if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_B3) {
			_toLeftSwitchBtn.interactable = false;
		} else if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_B2) {
			_toRightSwitchBtn.interactable = false;
		} else if ((int)_talentType == (int)CommonDefine.eTalentType.TALENT_C1) {
			_toRightSwitchBtn.interactable = false;
		}
	}
		
	void checkTalentSlotNum(int num){
		int cnt = 0;
		for(int i = 0;i< _talentSlotList.Count;i++){
			if(_talentSlotList[i].btnState != CommonDefine.TalentSlotState.TALENT_LOCK){
				cnt++;
			}
		}

		if (cnt == num)
			return;

		if (cnt < num) {
			//数据应该被清理了或者删除过应用

		}else{
			//这种出了异常问题，不是清理数据

		}

		cnt = num;

		//简单处理，全部重置，需要重新配置
		_talentSlotList.Clear();
		for (int i = (int)CommonDefine.eTalentType.TALENT_B3; i <= (int)CommonDefine.eTalentType.TALENT_C1; i++) {
			TalentSlot ts;
			ts.talentType = CommonDefine.eTalentType.TALENT_NONE;
			ts.id = i - (int)CommonDefine.eTalentType.TALENT_B3;
			if (cnt > 0) {
				ts.btnState = CommonDefine.TalentSlotState.TALENT_CAN_INSTALL;
				cnt--;
			} else {
				ts.btnState = CommonDefine.TalentSlotState.TALENT_LOCK;
			}
			_talentSlotList.Add (ts);
		}
	}
}
