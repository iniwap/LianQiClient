using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardSetting : MonoBehaviour {

	public ChessBoardSetting _SettingPrefab;

	public int _EdgeSize = 4;

	public void SetEdgeSize(float esize)
	{
		_EdgeSize = (int)esize;
		_SettingPrefab._EdgeSize = _EdgeSize;
	}

}
