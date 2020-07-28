/*
 * 单个场的处理
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plaza : MonoBehaviour {
	public Image _plazaBg;
	public Image _tagImg;
	public Text _plazaName;
	public Image _plazaIcon;
	public Image[] _plazaStars;
	public Text _plazaDes;
	public Button _twoPlayerBtn;
	public Button _fourPlayerBtn;
	public Button _joinBtn;
	public Text _plazaRule;

	private int _playerNum = 0;
	private int _plazaId;
	private int _gridLevel = 0;
	private int _id ;
	private bool _hasSelected;
	private PlazaRoomPanel _plazaRoomPanel;
	// Use this for initialization
	void Start () {
		//需要默认一个选中按钮 人数
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
	}
	//--------------------------一些操作------------------------------------------------------
	void addAllEvent(){
		//LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_USER_INFO,onUpdataUserInfo);
	}
	void removeAllEvent(){
		//LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_USER_INFO);
	}
	//------------------------------以下界面事件传出-----------------------------------------------
	public void  OnClickTwoPlayerBtn(){
		if (_playerNum == 2)
			return;
		
		_playerNum = 2;
		_gridLevel = 4;

		//设置高亮，4人按钮非选中
		_twoPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_S_BTN);
		_fourPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_N_BTN);
		_plazaRoomPanel.unselectPlazaExcept (_id);

		_joinBtn.interactable = true;
		_hasSelected = true;
	}

	public void  OnClickFourPlayerBtn(){
		if (_playerNum == 4)
			return;
		
		_playerNum = 4;
		_gridLevel = 6;

		//设置4高亮，2人按钮非选中
		_twoPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_N_BTN);
		_fourPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_S_BTN);

		_plazaRoomPanel.unselectPlazaExcept (_id);

		_joinBtn.interactable = true;
		_hasSelected = true;
	}
	public void OnClickJoinRoomBtn(){
		_plazaRoomPanel.onClickJoinRoom(_playerNum,_gridLevel,_plazaId,_plazaName.text,_id%4);
	}

	public void updatePlaza(int index,int plazaid,string name,float star,string des,string roomRule, PlazaRoomPanel plazaRoomPanel){
		_plazaId = plazaid;
		_id = index;
		_plazaRoomPanel = plazaRoomPanel;

		_tagImg.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_TAG + index%4);
		_plazaName.text = name;
		_plazaIcon.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_ICON + plazaid);
		for(int i = 0;i<_plazaStars.Length;i++){
			_plazaStars [i].gameObject.SetActive (false);
			if (i < (int)(star)) {
				_plazaStars [i].gameObject.SetActive (true);
			}
		}
		if ((int)star + 0.5 == star) {
			_plazaStars [0].gameObject.SetActive (true);
			_plazaStars [0].sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.PLAZA_HALF_STAR);
		}


		_plazaDes.text = des;
		_plazaRule.text = roomRule;

		//默认选中第一个
		if(_id == 0){
			OnClickTwoPlayerBtn ();
		}
	}

	public void unselectPlaza(){

		if (_playerNum == 0)
			return;

		_playerNum = 0;
		_gridLevel = 0;

		_twoPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_N_BTN);
		_fourPlayerBtn.image.sprite = CommonUtil.Util.getSprite(CommonDefine.ResPath.PLAZA_MODEL_N_BTN);

		_joinBtn.interactable = false;
		_hasSelected = false;
	}

	public int getID(){
		return _id;
	}
	public void enablePlaza(bool enable){
		if (enable) {
			_twoPlayerBtn.interactable = true;
			_fourPlayerBtn.interactable = true;
			if (_hasSelected) {
				_joinBtn.interactable = true;
			}
		} else {
			_twoPlayerBtn.interactable = false;
			_fourPlayerBtn.interactable = false;
			_joinBtn.interactable = false;
		}
	}
	//--------------------------以下网络数据更新界面------------------------------


}
