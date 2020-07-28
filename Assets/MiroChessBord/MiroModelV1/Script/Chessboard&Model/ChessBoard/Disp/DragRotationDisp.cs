using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class DragRotationDisp : MonoBehaviour {
		public ChessDragRotator _DragRoter;
		public CellObjCtrl _CCtrl;
		public float _zbias = -1.0f;

		public Animator _HighlightAnim;


		// Use this for initialization
		void Start () {
			
		}
			

		// Update is called once per frame
		void Update () {

			ControlHighlightAnim ();

			ControlHighlightColor ();
		}

		void ControlHighlightAnim ()
		{
			bool bEnabled = _DragRoter.enabled;
			bool bControlling = CellObjCtrlUtils.IsControllingObj (_CCtrl);
			bool bON = bEnabled && bControlling;
			//print (gameObject.name + " bON:" + bON);

			if (bON ) {
				_HighlightAnim.gameObject.SetActive (true);
			}
			else if(!bON){
				_HighlightAnim.gameObject.SetActive (false);
			}
		}

		private string _ColorMode = "UpDown";
		private Color _HighlightColor;
		public float _LerpSpd = 0.1f;
		public float _AlphaUpDown = 0.5f;
		public float _AlphaBoom = 0.8f;
		void ControlHighlightColor ()
		{
			SetHighlightColorAsChoosen ();
			SpriteRenderer sr = _HighlightAnim.GetComponentInChildren<SpriteRenderer> ();
			Color curColor = sr.color;
			Color lColor = Color.Lerp (curColor, _HighlightColor, _LerpSpd * Time.deltaTime);
			sr.color = lColor;
		}
		private void SetHighlightColorAsChoosen()
		{
			MiroV1ModelSetting modelSetting = CellObjCtrlUtils.GetModelSettingFromCtrl (_CCtrl);
			if(modelSetting==null)
			{
				return;
			}
			if (_ColorMode == "Boom") {
				_HighlightColor = modelSetting._colorSetting._ENMax;
				_HighlightColor.a = _AlphaBoom;
			} else if (_ColorMode == "UpDown") {
				_HighlightColor = modelSetting._colorSetting._ENEmpty;
				_HighlightColor.a = _AlphaUpDown;
			}
		}

		public void BoomHighlight()
		{
			_HighlightAnim.SetTrigger ("Boom");
			//SetBoomColor ();
		}


		public void ChooseHighlightColor_Boom ()
		{
			_ColorMode = "UpDown";
		}

		public void ChooseHighlightColor_UpDown()
		{
			_ColorMode = "Boom";
		}


			
		public void UpdateDragDirectionDisp ()
		{
			LineRenderer lr = GetComponent<LineRenderer> ();
			bool bDragging = _DragRoter.IsDragging ();
			//print ("bDragging:" + bDragging);
			if (!bDragging) {
				lr.enabled = false;
				return;
			}
			int draggingDir = _DragRoter.GetDragingDir ();
			CellObjCtrl ctrl = CellObjCtrlUtils.GetNbCellObjCtrlInAbsDir (_CCtrl, draggingDir);
			if (ctrl == null) {
				lr.enabled = false;
			}
			else {
				lr.enabled = true;
				Vector3 p0 = _CCtrl.transform.position + new Vector3 (0, 0, _zbias);
				Vector3 p1 = ctrl.transform.position + new Vector3 (0, 0, _zbias);
				lr.SetPosition (0, p0);
				lr.SetPosition (1, p1);
				lr.useWorldSpace = true;
			}
		}
	}
}
