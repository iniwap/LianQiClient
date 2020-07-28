using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENAbsorberBase : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		virtual public Transform GetAnchorTFA()
		{
			return transform;
		}
		virtual public Transform GetAnchorTFB()
		{
			return transform;
		}

		[ContextMenu("GrowUp")]
		virtual public void GrowUp()
		{

		}
		[ContextMenu("Shrink")]
		virtual public void Shrink()
		{
		}

		[ContextMenu("ScatterL")]
		virtual public void ScatterL()
		{

		}

		[ContextMenu("ScatterR")]
		virtual public void ScatterR()
		{
		}

		[ContextMenu("RecoverL")]
		virtual public void RecoverL()
		{
		}

		[ContextMenu("RecoverR")]
		virtual public void RecoverR()
		{
		}

		[ContextMenu("LBeating0")]
		virtual public void LBeating0()
		{
		}


		[ContextMenu("LBeating1")]
		virtual public void LBeating1()
		{

		}

		[ContextMenu("LBeating2")]
		virtual public void LBeating2()
		{
		}

		[ContextMenu("RBeating0")]
		virtual public void RBeating0()
		{
		}

		[ContextMenu("RBeating1")]
		virtual public void RBeating1()
		{
		}

		[ContextMenu("RBeating2")]
		virtual public void RBeating2()
		{
		}

		[ContextMenu("TurnOnNoiseWriggling")]
		virtual public void TurnOnNoiseWriggling()
		{
		}

		[ContextMenu("TurnOffNoiseWriggling")]
		virtual public void TurnOffNoiseWriggling()
		{
		}



	}
}
