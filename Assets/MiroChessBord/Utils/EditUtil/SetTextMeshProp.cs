using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetTextMeshProp : MonoBehaviour {

		public Color _textColor;
		public float _CharacterSize = 0.02f;
		public TextMesh[] texMesh;
		public int _SetNameUpLevel = 0;

		public Vector3 _RightDir = Vector3.right;

		[ContextMenu("GetAllTextMeshes")]
		public void GetAllTextMeshes()
		{
			texMesh = GetComponentsInChildren<TextMesh> ();
		}

		[ContextMenu("UpdateAllProps")]
		public void UpdateAllProps()
		{
			UpdateColor ();
			UpdateCharacterSize ();
			SetTextAsObjName ();
			SetDirection ();
		}

		[ContextMenu("UpdateColor")]
		public void UpdateColor()
		{
			foreach (TextMesh tm in texMesh) {
				tm.color = _textColor;
			}
		}

		[ContextMenu("UpdateCharacterSize")]
		public void UpdateCharacterSize()
		{
			foreach (TextMesh tm in texMesh) {
				tm.characterSize = _CharacterSize;
			}
		}

		[ContextMenu("SetTextAsObjName")]
		public void SetTextAsObjName()
		{
			foreach (TextMesh tm in texMesh) {
				Transform tf = tm.transform;
				int L = _SetNameUpLevel;
				while (L > 0) {
					tf = tf.parent;
					L--;
				}
				tm.text = tf.gameObject.name;
			}
		}

		[ContextMenu("SetDirection")]
		public void SetDirection()
		{
			foreach (TextMesh tm in texMesh) {
				Vector3 TargetDir
				 	= tm.transform.InverseTransformDirection (_RightDir);
				Quaternion Rot = Quaternion.FromToRotation (Vector3.right, TargetDir);
				tm.transform.rotation *= Rot;
			}
		}

	}
}
