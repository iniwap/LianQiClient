using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1WeaponBase : MonoBehaviour {
		[Range(0,9)]
		public int _ATMax = 0;
		[Range(0,9)]
		public int _AT = 0;
		private float _ATMaxF = 0.0f;
		private float _ATF = 0.0f;
		public float _LerpSpd = 5.0f;


		// Update is called once per frame
		void Update () {
			UpdateATMaxF ();
			_AT = Mathf.Clamp (_AT, 0, _ATMax);
			UpdateATF ();
			UpdateByATMax (_ATMax);
			UpdateByATMaxFloat (_ATMaxF);
			UpdateBYAT (_AT);
			UpdateByATFloat (_ATF);
			UpdateAfterOther ();
		}
		protected void UpdateATMaxF()
		{
			float dt = Time.deltaTime;
			_ATMaxF = Mathf.Lerp (_ATMaxF, (float)_ATMax, dt * _LerpSpd);
		}

		protected void UpdateATF()
		{
			float dt = Time.deltaTime;
			_ATF = Mathf.Lerp (_ATF, (float)_AT, dt * _LerpSpd);
		}

		virtual public int GetValidAT()
		{
			return _AT;
		}

		virtual public void UpdateByATMax(int ATMax)
		{
			Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) + 
				"UpdateByATMax(int ATMax= " + ATMax + ")");
		}

		virtual public void UpdateByATMaxFloat(float ATMaxFloat)
		{
			Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) + 
				"UpdateByATMaxFloat(int ATMaxFloat= " + ATMaxFloat + ")");
		}

		virtual public void UpdateBYAT(int AT)
		{
			Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) + 
				"UpdateByAT(int AT = " + AT + ")");
		}

		virtual public void UpdateByATFloat(float ATF)
		{
			Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) +
				" UpdateByATFloat(float ATF = " + ATF + ")");
		}

		virtual public void UpdateAfterOther()
		{
			
		}

		[ContextMenu("Shrink")]
		virtual public void Shrink()
		{
			//Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) + " Shrink!");
		}

		[ContextMenu("Scatter")]
		virtual public void Scatter()
		{
			Debug.Log (Lyu.DebugUtils.GetTypeNameString(gameObject) + " Scatter!");
		}

		virtual public void AttachLineRendererPathCtrl(Lyu.LineRendererPathCtrl lrCtrl)
		{
			
		}

		virtual public void DettachLineRendererPathCtrl()
		{
			
		}

		virtual public void TurnDynamics(bool bON)
		{
		}

		public int GetAT()
		{
			return _AT;
		}

		protected float GetATF()
		{
			return _ATF;
		}

		public virtual List<MiroV1BulletEmitterBase> GetEmitters()
		{
			MiroV1BulletEmitterBase[] emsa = 
				GetComponentsInChildren<MiroV1BulletEmitterBase> ();
			List<MiroV1BulletEmitterBase> ems = new List<MiroV1BulletEmitterBase> (emsa);
			return ems;
		}

		public List<Animator> _EmittersAnims = new List<Animator>();
		public virtual bool Emit()
		{
			if (_EmittersAnims.Count == 0) {
				Animator[] anims = GetComponentsInChildren<Animator> ();

				_EmittersAnims = new List<Animator> (anims);
			}

			bool bEmitted = false;

			foreach (Animator an in _EmittersAnims) {
				an.SetTrigger ("Emit");
				bEmitted = true;
			}
			return bEmitted;
		}

	}


}
