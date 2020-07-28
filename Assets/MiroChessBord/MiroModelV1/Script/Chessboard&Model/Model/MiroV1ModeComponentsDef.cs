using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiroV1
{
	public enum Direction
	{
		FORWARD,
		FORWARD_RIGHT,
		BACKWARD_RIGHT,
		BACKWARD,
		BACKWARD_LEFT,
		FORWARD_LEFT
	}

	public enum Distance
	{
		NEAR,
		MIDDLE,
		FAR,
		CENTER,
	}

	[System.Serializable]
	public class MainBodySlot
	{
		public MiroV1MainBodyBase _mainbody;
		[Range(0,100)]
		public int _HP = 0;

		[Range(0,100)]
		public int _HPMax = 0;

		public bool _GrowUpTrigger = false;
		public bool _ScatterTrigger = false;

		public void Update()
		{
			_HP = Mathf.Clamp (_HP, 0, _HPMax);

			if (_mainbody == null)
				return;
			
			_mainbody._HPMax = _HPMax;
			_mainbody._HP = _HP;

			if (_GrowUpTrigger) {
				_mainbody.GrowUp ();
				_GrowUpTrigger = false;
			}
			if (_ScatterTrigger) {
				_mainbody.Scatter ();
				_ScatterTrigger = false;
			}
		}

		public void AdjustBodyAnchors(MiroV1MainBodyAnchors bodyAnchors)
		{
			_mainbody.AdjustBodyAnchors(bodyAnchors);
		}
	}

	[System.Serializable]
	public class BlackAntellerSlot
	{
		public MiroV1AntellerBase _anteller;
		public bool _GrowUpTrigger = false;
		public bool _ShrinkTrigger = false;
		public bool _ScatterTrigger = false;

		public void Update()
		{
			if (_anteller == null)
				return;
			if (_GrowUpTrigger) {
				_anteller.GrowUp ();
				_GrowUpTrigger = false;
			}
			if (_ShrinkTrigger) {
				_anteller.Shrink ();
				_ShrinkTrigger = false;
			}
			if (_ScatterTrigger) {
				_anteller.Scatter ();
				_ScatterTrigger = false;
			}
		}
	}

	[System.Serializable]
	public class ENGeneratorSlot
	{
		[Range(0,9)]
		public int _EN = 0;
		[Range(0,9)]
		public int _ENMax = 0;

		public MiroV1ENGeneratorBase _Generator;
		public void Update()
		{
			_EN = Mathf.Clamp (_EN,0, _ENMax);
			if (_Generator == null)
				return;
			
			_Generator._ENMax = _ENMax;
			_Generator._EN = _EN;
		}
	}

	[System.Serializable]
	public class ENContainerSlot
	{
		[Range(0,54)]
		public int _AT = 0;

		public MiroV1ENContainerBase _Container;

		public void Update()
		{
			_Container._AT = _AT;
		}
	}

	[System.Serializable]
	public class WeaponSlot
	{
		public Direction _Direction;
		public bool _Active = false;
		public bool _ShrinkTrigger = false;
		public bool _ScatterTrigger = false;

		[Range(0,9)]
		public int _AT = 0;
		[Range(0,9)]
		public int _ATMax = 0;

		public MiroV1WeaponBase _Weapon = null;

		public void SetWeapon(MiroV1WeaponBase wp)
		{
			_Weapon = wp;	
		}

		public void Update()
		{
			
			_AT = Mathf.Clamp (_AT, 0, _ATMax);

			if (_Weapon != null) {
				_Weapon.UpdateAfterOther ();
				UpdateWeaponProps ();

				if (_ShrinkTrigger) {
					Shrink ();
				}
				if (_ScatterTrigger) {
					Scatter ();
				}
			}
		}

		void UpdateWeaponProps ()
		{
			_Weapon.gameObject.SetActive (_Active);
			_Weapon._ATMax = _ATMax;
			_Weapon._AT = _AT;
		}

		public void SetATImmediate(int at, int atmax)
		{
			_AT = at;
			_ATMax = atmax;
			_Weapon._ATMax = atmax;
			_Weapon._AT = at;
		}

		public void ActivateImmediate()
		{
			_Active = true;
			_Weapon.gameObject.SetActive (true);
			//_Weapon.UpdateAfterOther ();
		}
		public void DeactivateImmediate()
		{
			_Active = false;
			_Weapon.gameObject.SetActive (false);
		}

		public void Shrink ()
		{
			_Weapon.Shrink ();
			_ShrinkTrigger = false;
			_ATMax = 0;
			_AT = 0;
		}

		public void Scatter ()
		{
			_Weapon.Scatter ();
			_ScatterTrigger = false;
			_ATMax = 0;
			_AT = 0;
		}
	}


	[System.Serializable]
	public class RingBallSlot
	{
		public MiroV1RingBallBase _RingBall;

		[Range(0,24)]
		public int _HP;

		public bool _ShrinkRingTrigger = false;

		public void Update()
		{
			if (_RingBall != null) {
				_RingBall._HP = _HP;

				if (_ShrinkRingTrigger) {
					_RingBall.ShringWholeRing ();
					_ShrinkRingTrigger = false;
				}
			}
		}

	}

	[System.Serializable]
	public class BlackDotSlot
	{
		public MiroV1BlackDotBase _blackDot;

		/*
		[Range(0,24)]
		public int _HPMax = 1;

		[Range(0,24)]
		public int _HP = 1;
		*/

		public bool _GrowTrigger = false;
		public bool _ShrinkTrigger = false;
		public bool _BreakTrigger = false;
		public bool _RecoverTrigger = false;
		public bool _ShrinkRingTrigger = false;

		public bool _RecoverFull = true;

		public void Update()
		{
			if (_blackDot == null) {
				return;
			}
			/*
			_blackDot._HPMax = _HPMax;
			_blackDot._HP = _HP;
			*/

			if (_GrowTrigger) {
				_blackDot.Grow ();
				_GrowTrigger = false;
			}
			if(_ShrinkTrigger)
			{
				_blackDot.Shrink ();
				_ShrinkTrigger = false;
			}
			if (_BreakTrigger) {
				_blackDot.Break ();
				_BreakTrigger = false;
			}
			if (_RecoverTrigger) {
				if (_RecoverFull) {
					_blackDot.RecoverMax ();
				} else {
					_blackDot.Recover ();
				}
				_RecoverTrigger = false;
			}
			if (_ShrinkRingTrigger) {
				MiroV1RingBallBase rb = (MiroV1RingBallBase)_blackDot;
				if (rb != null) {
					rb.ShringWholeRing ();
				}
				_ShrinkRingTrigger = false;
			}
		}


	}


	[System.Serializable]
	public class BlackHoleSlot
	{
		public BlackHoleBase _BlackHole;

		public bool _GrowUpTrigger = false;
		public bool _ShrinkTrigger = false;
		public bool _bAbsorbing = false;

		public void Update()
		{
			if (_BlackHole == null)
				return;
			
			if (_bAbsorbing) {
				_BlackHole.AbsorbingON ();
			} else {
				_BlackHole.AbsorbingOFF ();
			}

			if (_GrowUpTrigger) {
				_BlackHole.GrowUp ();
				_GrowUpTrigger = false;
			}
			if (_ShrinkTrigger) {
				_BlackHole.Shrink ();
				_ShrinkTrigger = false;
			}


		}

		public void TurnAbsorbing(bool bAbsorbing)
		{
			if (bAbsorbing) {
				_BlackHole.AbsorbingON ();
			} else {
				_BlackHole.AbsorbingOFF ();
			}
		}

		public bool IsGrown()
		{
			if (_BlackHole == null)
				return false;
			else
				return _BlackHole.IsGrown ();
		}

		public void GrowUp()
		{
			if (_BlackHole != null) {
				_BlackHole.GrowUp ();
			}
		}

		public void Shrink()
		{
			if (_BlackHole != null) {
				_BlackHole.Shrink ();
			}
		}


	}
}
