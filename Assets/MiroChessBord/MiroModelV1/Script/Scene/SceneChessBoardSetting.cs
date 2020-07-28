using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChessBoardSetting : MonoBehaviour {

	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MiroV1Demo");
	}
	public void StartChessboardSetting()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MiroV1ChessboardSetting");
	}
}
