using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexCoordInput : MonoBehaviour {

	public Hex _coord;

	public UnityEvent _coordChanged;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InputQ(string q)
	{
		int x = 0;
		bool bInput = int.TryParse (q, out x);
		if (bInput) {
			Hex newCoord = new Hex(x,_coord.r,_coord.s);
			_coord = newCoord;
			_coordChanged.Invoke ();
			PrintCoord ();
		}
	}

	public void InputR(string r)
	{
		int x = 0;
		bool bInput = int.TryParse (r, out x);
		if (bInput) {
			Hex newCoord = new Hex(_coord.q,x,_coord.s);
			_coord = newCoord;
			_coordChanged.Invoke ();
			PrintCoord ();
		}
	}

	public void InputS(string s)
	{
		int x = 0;
		bool bInput = int.TryParse (s, out x);
		if (bInput) {
			Hex newCoord = new Hex(_coord.q,_coord.r,x);
			_coord = newCoord;
			_coordChanged.Invoke ();
			PrintCoord ();
		}
	}

	void PrintCoord ()
	{
		print ("(" + _coord.q + "," + _coord.r + "," + _coord.s + ")");
	}
}
