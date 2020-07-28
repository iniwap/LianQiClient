using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedEmyptBackground : MonoBehaviour {
    public Display _Display;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        _Display.clickEmypt();
    }
}
