/*
 * 单条排行
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour {

	public Image _rankItemIcon;
	public Text _rankItemName;
	public Text _rankItemScore;
	public Text _rankItemRankNum;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void updateRankItem(int rank,string name,string score){
		if (rank > 3) {
			_rankItemIcon.gameObject.SetActive(false);
			_rankItemRankNum.gameObject.SetActive (true);

			_rankItemRankNum.text = ""+rank;

			_rankItemName.fontSize = 40;
			_rankItemScore.fontSize = 40;
		} else {
			_rankItemIcon.gameObject.SetActive(true);
			_rankItemIcon.sprite = CommonUtil.Util.getSprite (CommonDefine.ResPath.RANK_NUM_ICON + rank);
			_rankItemRankNum.gameObject.SetActive (false);

			_rankItemName.fontSize = 48;
			_rankItemScore.fontSize = 48;
		}
		_rankItemName.text = name;
		_rankItemScore.text = score;
	}
}
