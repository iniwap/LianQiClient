using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1BlackDotBase : MonoBehaviour {

		public int _Precedence = 0;

		private List<MiroV1BulletEmitterBase> _attackers = 
			new List<MiroV1BulletEmitterBase>();
	
		[Range(0,24)]
		public int _HPMax = 1;

		[Range(0,24)]
		public int _HP = 0;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		public void Update () {
			LimitHP ();
			UpdateHPDisp ();
			UpdateAttackersInfo ();
		}

		virtual public void UpdateHPDisp()
		{
			
		}

		public void UpdateAttackersInfo()
		{
			if (_attackers.Count > _HPMax) {
				int count = _attackers.Count - _HPMax;
				_attackers.RemoveRange (_HPMax, count);
			}
			for (int i = _attackers.Count - 1; i >= 0; i--) {
				if (_attackers [i].GetTarget()!=transform) {
					_attackers.RemoveAt (i);
					continue;
				}
				if (!_attackers [i].IsON ()) {
					_attackers.RemoveAt (i);
					continue;
				}
			}

		}

		public int GetAttackersCount()
		{
			return _attackers.Count;
		}

		public bool IsAttackerMax()
		{
			return _attackers.Count >= _HPMax;
		}

		public bool ContainAttacker(MiroV1BulletEmitterBase em)
		{
			return _attackers.Contains (em);
		}
		public void RemoveAttacker(MiroV1BulletEmitterBase em)
		{
			_attackers.Remove (em);
		}

		public bool AddAttacker(MiroV1BulletEmitterBase em)
		{
			if (_attackers.Contains (em)) {
				return true;
			}
			if (_attackers.Count >= _HPMax)
				return false;
			
			_attackers.Add(em);
			return true;
		}

		public List<MiroV1BulletEmitterBase> GetAttackers()
		{
			return _attackers;
		}


		public void ClearAttackers()
		{
			_attackers.Clear ();
		}
			
		virtual public void Grow()
		{
			_HPMax++;
			LimitHP ();
		}

		virtual public void Shrink()
		{
			_HPMax--;
			LimitHP ();
		}

		virtual public void Recover()
		{
			_HP++;
			LimitHP ();
		}

		virtual public void RecoverMax()
		{
			_HP = _HPMax;
			LimitHP ();
		}

		virtual public void Break()
		{
			_HP --;
			LimitHP ();
		}

		protected void LimitHP ()
		{
			_HPMax = Mathf.Clamp (_HPMax, 0, 24);
			_HP = Mathf.Clamp (_HP, 0, _HPMax);
		}

	}
}
