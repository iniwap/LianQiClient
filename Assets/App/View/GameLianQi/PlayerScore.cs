/*
 * 单条用户得分详情
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {
	public Image _headBg;
	public Image _head;
	public Image _owner;
	public Image _scoreBg;

	public Image _rankIcon;//只会存在一个
	public Text _rankText;

	public Text _name;
	public Text _areaText;//领地text
	public Text _killText;
	public Text _scoreText ;
	public Text _multText;
	public Image _multi;

	public Image _abandonFlag;
	public Text _abandonText;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickMoreDetailBtn(){
		//详情真的有意义吗？
	}

	public void updatePlayScore(bool isSelf,int rank,int seat,bool showOwner,
		string head,string name,int area,int kill,int score,int multi,bool hasAbandon){

		if (isSelf) {
			_scoreBg.gameObject.SetActive (true);
		} else {
			_scoreBg.gameObject.SetActive (false);
		}

		if (showOwner) {
			_owner.gameObject.SetActive (true);
		} else {
			_owner.gameObject.SetActive (false);
		}

		_headBg.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.ROOM_HEAD_BG + seat);
		_head.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.ROOM_HEAD + seat);

		if (rank > 3) {
			_rankIcon.gameObject.SetActive(false);
			_rankText.gameObject.SetActive (true);

			_rankText.text = ""+rank;
		} else {
			_rankIcon.gameObject.SetActive(true);
			_rankIcon.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.RANK_NUM_ICON + rank);
			_rankText.gameObject.SetActive (false);
		}

		_name.text = name;
		_areaText.text = "<color=#aaaaff>领地</color><color=yellow>  "  + area + "</color>";
		_killText.text = "<color=#aaaaff>消灭</color><color=yellow>  "  + kill + "</color>";

		string scoreStr = "";
		if (score < 0) {
			scoreStr = scoreStr + score;
		} else if (score >= 0) {
			scoreStr = scoreStr + "+" + score;
		}
		_scoreText.text = "<color=#aaaaff>积分</color><color=yellow>  "  + scoreStr + "</color>";

		if (multi > 1) {
			_multText.text = "x" + multi;
		} else {
			_multi.gameObject.SetActive (false);
		}

		if (hasAbandon) {
			_abandonFlag.gameObject.SetActive (true);
			_abandonText.gameObject.SetActive (true);

			_abandonText.text = "+" + score;

			_scoreText.text = "<color=#aaaaff>积分</color><color=yellow>  --</color>";

		} else {
			_abandonFlag.gameObject.SetActive (false);
			_abandonText.gameObject.SetActive (false);
		}
	}
}
