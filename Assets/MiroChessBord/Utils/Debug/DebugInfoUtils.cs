using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Lyu
{
	public class DebugUtils
	{
		static public string GetTypeNameString(GameObject obj)
		{
			System.Type tyText= obj.GetType ();
			string text = tyText.ToString () + "_" +  obj.name;
			return text;
		}

	}


}
