using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	
	public class MiroModelV1 : MonoBehaviour {
		public bool _bUpdateMainBodyHPByBlackDots = true;
		public bool _bDieOnHP0 = true;
		public int _DispHP = 0;
		private int _hpPrev = 0;

		public GameObject _BreakPS;
		public MainBodySlot _MainBody = 
			new MainBodySlot ();
		public List<BlackAntellerSlot> _Antellers = 
			new List<BlackAntellerSlot> ();
		/*
		public List<RingBallSlot> _RingBalls =
			new List<RingBallSlot> ();
			*/
		public List<BlackDotSlot> _BlackDots = 
			new List<BlackDotSlot> ();
		public bool _bTurnMainWeaponByEN = true;
		public ENGeneratorSlot _ENGenerator = 
			new ENGeneratorSlot();
		public ENContainerSlot _ENContainer = 
			new ENContainerSlot();
		public List<WeaponSlot> _WeaponSlots =
			new List<WeaponSlot> ();
		private int _atPrev = 0;
		private int _DispAT = 0;
		private MiroModelV1 _attackTgt = null;
		public BlackHoleSlot _BlackHole = 
			new BlackHoleSlot();

		public MiroV1MainBodyAnchors _bodyAnchors;

		public Transform _DefaultAttackedTF;
		public MiroV1AidBlackDots _6DirAidDots;


		private bool _bUpdateAttackTarget = false;

		public float _MainBodyUpdatePeriod = 0.1f;
		public float _BodyAnchorsUpdatePeriod = 0.1f;
		public float _AntellersUpdatePeriod = 0.1f;
		public float _BlackDotsUpdatePeriod = 0.1f;
		public float _ENOrgansUpdatePeriod = 0.1f;
		public float _AttackTargetUpdatePeriod = 0.1f;
		public float _ATUpdatePeriod = 0.1f;
		public float _EmittersUpdatePeriod = 0.1f;
		public float _PumpUpdatePeriod = 0.1f;
		public float _BlackHoleUpdatePeriod = 0.1f;
		public float _MainBodyDispUpdatePeriod = 0.1f;

		private int _BirthID = 0;
		private static int _BirthCount = 0;

		public static void ResetBirthCount()
		{
			_BirthCount = 0;
		}

		public static int GetBirthCount()
		{
			return _BirthCount;
		}

		public MiroModelV1()
		{
			_BirthID = _BirthCount;
			_BirthCount++;
		}
		public int GetBirthID()
		{
			return _BirthID;
		}


		// Use this for initialization
		void Start () {
			InvokeRepeating ("MainBodyUpdate", 0.05f, _MainBodyUpdatePeriod);
			InvokeRepeating ("BodyAnchorsUpdate", 0.1f, _BodyAnchorsUpdatePeriod);
			InvokeRepeating ("AntellersUpdate", 0.15f, _AntellersUpdatePeriod);
			InvokeRepeating ("BlackDotsUpdate", 0.2f, _BlackDotsUpdatePeriod);
			InvokeRepeating ("ENOrgansUpdate", 0.25f, _ENOrgansUpdatePeriod);
			InvokeRepeating ("AttackTargetUpdate", 0.3f, _AttackTargetUpdatePeriod);
			InvokeRepeating ("ATUpdate", 0.35f, _ATUpdatePeriod);
			InvokeRepeating ("EmittersUpdate", 0.4f, _EmittersUpdatePeriod);
			InvokeRepeating ("PumpUpdate", 0.45f, _PumpUpdatePeriod);
			InvokeRepeating ("BlackHoleUpdate", 0.5f, _BlackDotsUpdatePeriod);
			InvokeRepeating ("MainBodyDispUpdate", 0.55f, _MainBodyDispUpdatePeriod);



			if (_Antellers.Count != 2) {
				InitAntellerSlots ();
			}

			/*
			if (_RingBalls.Count < 4) {
				int cnt = _RingBalls.Count;
				for (int i = cnt-1; i < 4; i++) {
					_RingBalls.Add (new RingBallSlot ());
				}
			}
			*/

			if (_WeaponSlots.Count != 6) {
				InitWeaponSlots ();
			}

			for (int i = 0; i < 6; i++) {
				_WeaponSlots [i]._Direction = (Direction)i;
			}

			Find6DirDotsCtrl ();


		}




		// Update is called once per frame
		void Update () {
			/*
			MainBodyUpdate ();
			BodyAnchorsUpdate ();
			AntellersUpdate ();
			BlackDotsUpdate ();
			ENOrgansUpdate ();
			AttackTargetUpdate ();
			ATUpdate ();
			EmittersUpdate ();
			PumpUpdate ();
			BlackHoleUpdate ();
			MainBodyDispUpdate ();
			*/
		}

		void MainBodyDispUpdate ()
		{
			if (_bUpdateMainBodyHPByBlackDots) {
				UpdateMainBodyHPByBlackDots ();
			}
		}

		void BlackHoleUpdate ()
		{
			_BlackHole.Update ();
		}

		void PumpUpdate ()
		{
			if (_GrowPumpTrigger) {
				GrowUpPump ();
				_GrowPumpTrigger = false;
			}

			if (_ShrinkPumpTrigger) {
				ShrinkPump ();
				_ShrinkPumpTrigger = false;
			}
		}

		void EmittersUpdate ()
		{
			MiroV1BulletEmitterBase[] ems = GetComponentsInChildren<MiroV1BulletEmitterBase> ();
			foreach (MiroV1BulletEmitterBase em in ems) {
				if (em.GetTarget () == null) {
					em.SetTarget (_DefaultAttackedTF);
				}
			}
		}

		void ATUpdate ()
		{
			int at = GetAT ();
			bool atChanged = (at != _atPrev);
			if (atChanged) {
				_DispAT = at;
			}
			_bUpdateAttackTarget = atChanged;
			_atPrev = at;
		}

		void AttackTargetUpdate ()
		{
			if (_bUpdateAttackTarget) {
				UpdateAttackTarget ();
				_bUpdateAttackTarget = false;
			}
		}

		void ENOrgansUpdate ()
		{
			_ENGenerator.Update ();
			UpdateMainWeaponByEN ();
			_ENContainer.Update ();
		}

		void BlackDotsUpdate ()
		{
			int hp = 0;
			foreach (BlackDotSlot dot in _BlackDots) {
				dot.Update ();
				hp += dot._blackDot._HP;
			}

			if (_hpPrev != hp) {
				_DispHP = hp;
			}

			if (_bDieOnHP0) {
				if (_hpPrev > 0 && _DispHP == 0) {
					Die ();
				}
			}
			_hpPrev = hp;


			for (int i = _BlackDots.Count - 1; i > 0; i--) {
				BlackDotSlot dotSlot = _BlackDots [i];
				if (dotSlot._blackDot == null) {
					_BlackDots.RemoveAt (i);
				}
			}
		}

		void AntellersUpdate ()
		{
			foreach (BlackAntellerSlot anteller in _Antellers) {
				anteller.Update ();
			}
		}

		void BodyAnchorsUpdate ()
		{
			_MainBody.AdjustBodyAnchors (_bodyAnchors);
		}

		void MainBodyUpdate ()
		{
			_MainBody.Update ();
		}

		public void SetDispHP(int dispHp)
		{
			_DispHP = dispHp;
		}

		public void SetDispAT(int dispAT)
		{
			_DispAT = dispAT;
		}

		public void SetFwdAbsorbingSrcTF(Transform tf)
		{
			_Antellers [0]._anteller.SetSrcTF (tf);
		}
		public void SetBwdAbsorbingSrcTF(Transform tf)
		{
			_Antellers [1]._anteller.SetSrcTF (tf);
		}

		public void GetBKDictionary (
			List<MiroV1BulletEmitterBase> ems,
			ref Dictionary<int, List<MiroV1BlackDotBase>> dicBkDots, 
			ref List<int> keys)
		{
			foreach (BlackDotSlot bds in _BlackDots) {
				int precd = bds._blackDot._Precedence;
				if (!dicBkDots.ContainsKey (precd)) {
					dicBkDots.Add (precd, new List<MiroV1BlackDotBase> ());
					keys.Add (precd);
				}

				/*
				bool bContain = false;
				foreach (MiroV1BulletEmitterBase em in ems) {
					if (bds._blackDot.ContainAttacker (em)) {
						bContain = true;
						break;
					}
				}*/

				// to debug
				bds._blackDot.UpdateAttackersInfo ();
				bool bAttackMax = bds._blackDot.IsAttackerMax ();
				//int hp = bds._blackDot._HP;

				if (!bAttackMax ) {
					dicBkDots [precd].Add (bds._blackDot);
				}
			}
			keys.Sort ();
		}	
		void UpdateMainWeaponByEN ()
		{
			if (_bTurnMainWeaponByEN) {
				if (_ENGenerator._EN > 0 && !_WeaponSlots [0]._Active) {
					_WeaponSlots [0]._Active = true;
				}
				_WeaponSlots [0]._ATMax = _ENGenerator._EN;
				_WeaponSlots [0]._AT = _ENGenerator._EN;
			}
		}

		public void CeaseMainWeapon()
		{
			_WeaponSlots [0]._ATMax = _ENGenerator._EN;
			_WeaponSlots [0]._AT = _ENGenerator._EN;
			_WeaponSlots [0]._ShrinkTrigger = true;
			Debug.Log (this.name + "Turn AT 0");
		}

		public void TurnOFFMainWeapon()
		{
			_WeaponSlots [0]._Active = false;
		}
	

		// debug
		Dictionary<MiroV1BlackDot, MiroV1BulletEmitterBase> _BDonEM = 
			new Dictionary<MiroV1BlackDot, MiroV1BulletEmitterBase>();
		public void UpdateAttackTarget()
		{
			if (_attackTgt == null)
				return;

			// get ON emitters
			List<MiroV1BulletEmitterBase> _onEmitters = GetONEmitters();

			// remove emitters recorded in target blackdots
			foreach (MiroV1BulletEmitterBase em in _onEmitters) {
				// remove from black dot
				foreach (BlackDotSlot bds in _attackTgt._BlackDots) {
					bds._blackDot.RemoveAttacker (em);
				}
			}

			foreach (BlackDotSlot bds in _attackTgt._BlackDots) {
				bds._blackDot.UpdateAttackersInfo ();
			}

			// get target blackdots dictionary and sorted keys
			Dictionary<int, List<MiroV1BlackDotBase>> dicBkDots = 
				new Dictionary<int, List<MiroV1BlackDotBase>>();
			List<int> keys = new List<int> ();
			_attackTgt.GetBKDictionary (_onEmitters, ref dicBkDots, ref keys);

			// aim each emitter to nearest black dot
			foreach (MiroV1BulletEmitterBase em in _onEmitters) {
				// add to black dot
				_attackTgt.ConfigTargetBlackDotTFFor (em, keys, dicBkDots);
			}
		}



		public void ConfigTargetBlackDotTFFor(
			MiroV1BulletEmitterBase em, 
			List<int> keys, 
			Dictionary<int, List<MiroV1BlackDotBase>> dicBkDots)
		{
			Transform tgt = null;
			bool IsEmitting = em.IsON();
			if (!IsEmitting) {
				return;
			}

			for (int i = 0; i < keys.Count; i++) {
				int precd = keys [i];
				List<MiroV1BlackDotBase> dots = dicBkDots [precd];

				for (int j = dots.Count - 1; j >= 0; j--) {
					var dot = dots [j];
					/*
					if (dot.ContainAttacker (em)) {
						em.SetTarget (dot.transform);
						dot.AddAttacker (em);
						return;
					}*/

					bool bAttackMax = dot.IsAttackerMax ();
					if (bAttackMax) {
						dots.RemoveAt (j);
					}
				}

				MiroV1BlackDotBase nearestBD = GetNearestBlackDot (em, dots);
				if (nearestBD != null) {
					tgt = nearestBD.transform;
					break;
				}
			}
			if (tgt == null) {
				tgt = transform;
			}

			em.SetTarget (tgt);
			MiroV1BlackDotBase bdot = 
				tgt.GetComponent<MiroV1BlackDotBase> ();
			if (bdot != null) {
				bdot.AddAttacker (em);
			}

		}

		static MiroV1BlackDotBase GetNearestBlackDot (
			MiroV1BulletEmitterBase em,
			List<MiroV1BlackDotBase> dots)
		{
			MiroV1BlackDotBase nearestBD=null;
			float distMin = 1000000.0f;
			foreach (MiroV1BlackDotBase bd in dots) {
				if (bd == null) {
					continue;
				}
				Vector3 posBDot = bd.transform.position;
				Vector3 posEM = em.transform.position;
				Vector3 offset = posEM - posBDot;
				offset.z = 0.0f;
				float dist = offset.magnitude;
				if (dist < distMin) {
					distMin = dist;
					nearestBD = bd;
				}
			}

			return nearestBD;
		}



		public void SetAttackTarget(MiroModelV1 other)
		{
			_attackTgt = other;
		}


		public void ReleaseAllAttackers()
		{
			foreach (BlackDotSlot bds in _BlackDots) {
				bds._blackDot.ClearAttackers ();
				List<MiroV1BulletEmitterBase> atkrs = 
					bds._blackDot.GetAttackers ();
				foreach (MiroV1BulletEmitterBase atkr in atkrs) {
					atkr.SetTarget(null);
					atkr.TurnOFF ();
				}
			}
		}

		public void CeaseAttacking()
		{
			MiroV1BulletEmitterBase[] ems = 
				GetComponentsInChildren<MiroV1BulletEmitterBase> ();
			foreach (MiroV1BulletEmitterBase atkr in ems) {
				atkr.SetTarget(null);
				//atkr.TurnOFF ();
			}
		}

		/*
		public void NotAttackTarget(MiroModelV1 other)
		{
			List<MiroV1BulletEmitterBase> _emitters = GetEmitters ();

			foreach (MiroV1BulletEmitterBase em in _emitters) {
				Transform tgt = 
					em.GetTarget ();
				MiroV1BlackDotBase bdot = 
					tgt.GetComponent<MiroV1BlackDotBase> ();

				em.SetTarget (null);
			}

		}*/

		/*
		private HashSet<MiroV1BlackDotBase> _AttackedBKDots =
			new HashSet<MiroV1BlackDotBase> ();
		public void AddAttacked(MiroV1BlackDotBase bkDot)
		{
			_AttackedBKDots.Add (bkDot);
		}
		*/

		private List<MiroV1.MiroV1BulletEmitterBase> GetEmitters()
		{
			List<MiroV1BulletEmitterBase> _emitters = new List<MiroV1BulletEmitterBase> ();
			foreach (WeaponSlot wps in _WeaponSlots) {
				MiroV1WeaponBase weapon = wps._Weapon;
				MiroV1BulletEmitterBase[] ems = weapon.GetComponentsInChildren<MiroV1BulletEmitterBase> ();
				foreach (MiroV1BulletEmitterBase em in ems) {
					_emitters.Add (em);
				}
			}
			return _emitters;
		}

		private List<MiroV1.MiroV1BulletEmitterBase> GetONEmitters()
		{
			List<MiroV1BulletEmitterBase> _emitters = 
				new List<MiroV1BulletEmitterBase> ();
			foreach (WeaponSlot wps in _WeaponSlots) {
				MiroV1WeaponBase weapon = wps._Weapon;
				MiroV1BulletEmitterBase[] ems = 
					weapon.GetComponentsInChildren<MiroV1BulletEmitterBase> ();
				foreach (MiroV1BulletEmitterBase em in ems) {
					if (em.IsON ()) {
						_emitters.Add (em);
					}
				}
			}
			return _emitters;
		}

		public void SetMainBody(GameObject mainBodyObj)
		{
			MiroV1MainBodyBase mbody = 
				mainBodyObj.GetComponent<MiroV1MainBodyBase> ();
			_MainBody._mainbody = mbody;
		}

		public void SetFwdAnteller(GameObject antellerObj)
		{
			MiroV1AntellerBase anteller = 
				antellerObj.GetComponent<MiroV1AntellerBase> ();
			_Antellers [0]._anteller = anteller;
		}

		public void SetBwdAnteller(GameObject antellerObj)
		{
			MiroV1AntellerBase anteller = 
				antellerObj.GetComponent<MiroV1AntellerBase> ();
			_Antellers [1]._anteller = anteller;
		}

		public void NewBlackDot(MiroV1BlackDotBase bkDot)
		{
			bool bExist = false;
			foreach (BlackDotSlot bkdSlot in _BlackDots) {
				if(bkdSlot._blackDot == bkDot)
				{
					bExist = true;
				}
			}
			if (!bExist) {
				BlackDotSlot newBkDotSlot = new BlackDotSlot ();
				newBkDotSlot._blackDot = bkDot;
				_BlackDots.Add (newBkDotSlot);
			}
		}

		public void RemoveBlackDot(MiroV1BlackDot bkDot)
		{
			foreach (BlackDotSlot bkdSlot in _BlackDots) {
				if(bkdSlot._blackDot == bkDot)
				{
					_BlackDots.Remove (bkdSlot);
					break;
				}
			}
		}

		public int GetAidBlackDotRDir(MiroV1BlackDot bdot)
		{
			MiroV1AidBlackDots aidBlackDots =
				GetComponentInChildren<MiroV1AidBlackDots> ();

			int bdotDir = 
				aidBlackDots.GetBlackDotDir (bdot);

			return bdotDir;
		}
			
		/*
		public void SetRingBall0(GameObject ringballObj)
		{
			MiroV1RingBallBase ringball = 
				ringballObj.GetComponent<MiroV1RingBallBase> ();
			_RingBalls [0]._RingBall = ringball;
		}

		public void SetRingBall1(GameObject ringballObj)
		{
			MiroV1RingBallBase ringball = 
				ringballObj.GetComponent<MiroV1RingBallBase> ();
			_RingBalls [1]._RingBall = ringball;
		}

		public void SetRingBall2(GameObject ringballObj)
		{
			MiroV1RingBallBase ringball = 
				ringballObj.GetComponent<MiroV1RingBallBase> ();
			_RingBalls [2]._RingBall = ringball;
		}

		public void SetRingBall3(GameObject ringballObj)
		{
			MiroV1RingBallBase ringball = 
				ringballObj.GetComponent<MiroV1RingBallBase> ();
			_RingBalls [3]._RingBall = ringball;
		}

		public void SetRingBall4(GameObject ringballObj)
		{
			MiroV1RingBallBase ringball = 
				ringballObj.GetComponent<MiroV1RingBallBase> ();
			_RingBalls [4]._RingBall = ringball;
		}
		*/

		public void SetENContainer(GameObject encontainerObj)
		{
			MiroV1ENContainerBase cont = 
				encontainerObj.GetComponent<MiroV1ENContainerBase> ();
			_ENContainer._Container = cont;
		}
		public void SetENGenerator(GameObject engeneratorObj)
		{
			MiroV1ENGeneratorBase gen = 
				engeneratorObj.GetComponent<MiroV1ENGeneratorBase> ();
			_ENGenerator._Generator = gen;
		}

			
		public void SetWeaponFwd(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [0]._Weapon = wp;
		}

		public void SetWeaponFwdRight(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [1]._Weapon = wp;
		}

		public void SetWeaponBwdRight(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [2]._Weapon = wp;
		}

		public void SetWeaponBwd(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [3]._Weapon = wp;
		}

		public void SetWeaponBwdLeft(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [4]._Weapon = wp;
		}

		public void SetWeaponFwdLeft(GameObject wpObj)
		{
			MiroV1WeaponBase wp = wpObj.GetComponent<MiroV1WeaponBase> ();
			_WeaponSlots [5]._Weapon = wp;
		}

		public void SetBlackHole(GameObject bhObj)
		{
			BlackHoleBase bk = bhObj.GetComponent<BlackHoleBase> ();
			_BlackHole._BlackHole = bk;
		}

		public int GetDispHP()
		{
			return _DispHP;
		}

		public int GetHP()
		{
			int hp = 0;
			foreach (BlackDotSlot dot in _BlackDots) {
				hp += dot._blackDot._HP;
			}
			return hp;
		}

		public int GetMaxHP()
		{
			int mhp = 0;
			foreach (BlackDotSlot dot in _BlackDots) {
				mhp += dot._blackDot._HPMax;
			}
			return mhp;
		}

		public int GetAttackersCount()
		{
			int cnt = 0;
			foreach (BlackDotSlot dot in _BlackDots) {
				cnt += dot._blackDot.GetAttackersCount ();
			}
			return cnt;
		}

		public List<MiroV1BlackDotBase> GetAliveBlackDots()
		{
			List<MiroV1BlackDotBase> _bdots = new List<MiroV1BlackDotBase> ();
			foreach (BlackDotSlot dot in _BlackDots) {
				if (dot._blackDot!=null && dot._blackDot._HP > 0 ) {
					_bdots.Add (dot._blackDot);
				}
			}
			return _bdots;
		}

		private bool _bAttacking = false;

		public int GetDispAT()
		{
			return _DispAT;
		}

		public int GetAT()
		{
			int at = 0;
			foreach (WeaponSlot wp in _WeaponSlots) {
				if (wp._Active) {
					at += wp._Weapon.GetValidAT ();
				}
			}
			//print (gameObject.name + " at:" + at);
			_bAttacking = at > 0;
			return at;
		}

		public int GetMaxAT()
		{
			int at = 0;
			foreach (WeaponSlot wp in _WeaponSlots) {
				if (wp._Active) {
					at += wp._Weapon._ATMax;
				}
			}

			return at;
		}

		public bool IsAttacking()
		{
			return _bAttacking;
		}

		public List<MiroV1WeaponBase> GetAttackingWeapons()
		{
			List<MiroV1WeaponBase> wps = new List<MiroV1WeaponBase> ();
			foreach (WeaponSlot wp in _WeaponSlots) {
				if (wp._Active) {
					wps.Add (wp._Weapon);
				}
			}
			return wps;
		}

		public MiroV1WeaponBase GetWeaponInDir(int relativeDir)
		{
			return _WeaponSlots [relativeDir]._Weapon;
		}

		void InitAntellerSlots ()
		{
			_Antellers.Clear ();
			for (int i = 0; i < 3; i++) {
				BlackAntellerSlot ant = new BlackAntellerSlot ();
				_Antellers.Add (ant);
			}
		}

		void InitWeaponSlots ()
		{
			_WeaponSlots.Clear ();
			for (int i = 0; i < 6; i++) {
				_WeaponSlots.Add (new WeaponSlot ());
			}
		}

		[ContextMenu("Find6DirDotsCtrl")]
		void Find6DirDotsCtrl ()
		{
			if (_6DirAidDots == null) {
				_6DirAidDots = GetComponentInChildren<MiroV1AidBlackDots> ();
			}
		}

		public MiroV1AidBlackDots Get6DirDotsCtrl()
		{
			return _6DirAidDots;
		}
			
		public void GrowUPAidDotAtDir(int dir)
		{
			MiroV1BlackDotBase bkDot = 
				_6DirAidDots.GetBlackDot (dir);
			bkDot.Grow ();
			bkDot.Recover ();
		}

		public void ShrinkAidDotAtDir(int dir)
		{
			MiroV1BlackDotBase bkDot = 
				_6DirAidDots.GetBlackDot (dir);
			bkDot.Shrink ();
		}

		public void BreakAidDotAtDir(int dir)
		{
			MiroV1BlackDotBase bkDot = 
				_6DirAidDots.GetBlackDot (dir);
			bkDot.Break ();
		}

		private MiroV1HPDisp _hpDisp;
		[ContextMenu("TurnONHighlightHPDist")]
		public void TurnONHighlightHPDist()
		{
			if (_hpDisp == null) {
				_hpDisp = GetComponentInChildren<MiroV1HPDisp> ();
			}
			_hpDisp.TurnONKeepHighlighting ();
		}

		private HashSet<MiroModelV1> _absorbers = new HashSet<MiroModelV1>();
		public void AddAbsorber(MiroModelV1 modelAbsorber)
		{
			bool bAiding = IsAiding ();

			if (!bAiding) {
				AddAbsorbingAttacking (modelAbsorber);
				//print (modelAbsorber + " AddAbsorbingAttacking");
			} else {
				AddAbsorbingAssisting (modelAbsorber);
			}
		}

		void AddAbsorbingAssisting(MiroModelV1 modelAbsorber)
		{
			MiroModelV1 modelAidee = _Pump.GetModelAidee ();
			if (!modelAidee._absorbers.Contains (modelAbsorber)) {
				modelAidee._absorbers.Add (modelAbsorber);
			}

			int aidingDir = _Pump.GetAidingRDir ();
			MiroV1WeaponBase weapon = modelAidee.GetWeaponInDir (aidingDir);

			List<MiroV1BulletEmitterBase> allEms = weapon.GetEmitters ();
			List<MiroV1BulletEmitterBase> OnNotAbsorbedEMs =
				GetOnNotAbsorbedEmittersFrom (allEms);

			List<MiroV1AbsorbPoint> absPs = 
				modelAbsorber._BlackHole._BlackHole.GetAbsorbingPos ();
			List<MiroV1AbsorbPoint> hungryAPs = GetHungryAbsorbingPointsFrom (absPs);

			if (OnNotAbsorbedEMs.Count > 0 && hungryAPs.Count > 0) {
				LinkEmittersToAbsorbingPoss (OnNotAbsorbedEMs, absPs);
			}
		}

		private List<MiroV1BulletEmitterBase> 
			GetOnNotAbsorbedEmittersFrom(List<MiroV1BulletEmitterBase> allEms)
		{
			List<MiroV1BulletEmitterBase> OnNotAbsorbedEMs = 
				new List<MiroV1BulletEmitterBase> ();
			for (int i = allEms.Count - 1; i >= 0; i--) {
				bool bON = allEms [i].IsON ();
				bool bAbsorbed = allEms [i].IsAbsorberON ();
				if (bON && !bAbsorbed) {
					OnNotAbsorbedEMs.Add (allEms[i]);
				}
			}
			return OnNotAbsorbedEMs;
		}

		public int GetAbsorbedAmt()
		{
			List<MiroV1BulletEmitterBase> onEms = 
				GetChdONEmitters ();

			int absorbedAmt = 0;
			foreach (MiroV1BulletEmitterBase em in onEms) {
				if (em.IsAbsorberON ()) {
					absorbedAmt++;
				}
			}

			return absorbedAmt;
		}

		public int GetAssistingAT()
		{
			bool bAiding = IsAiding ();
			if (!bAiding) {
				return 0;
			}
			int en = _ENGenerator._EN;
			int absorbed = GetAbsorbedAmt ();
			int assistingAT = en - absorbed;
			assistingAT = (int)Mathf.Clamp (
				(float)assistingAT, 0.0f, float.PositiveInfinity);
			return assistingAT;
		}

		private List<MiroV1AbsorbPoint> GetHungryAbsorbingPointsFrom(
			List<MiroV1AbsorbPoint> absPs)
		{
			List<MiroV1AbsorbPoint> readyAbsPos = new List<MiroV1AbsorbPoint> ();
			foreach (var ap in absPs) {
				bool bEmLinked = ap.IsEmitterLinked ();
				if (!bEmLinked) {
					readyAbsPos.Add (ap);
				}
			}
			return readyAbsPos;
		}


		void AddAbsorbingAttacking (MiroModelV1 modelAbsorber)
		{
			if (!_absorbers.Contains (modelAbsorber)) {
				_absorbers.Add (modelAbsorber);
			}

			List<MiroV1BulletEmitterBase> ems = GetONNotAbsorbedEmitters ();
			if (modelAbsorber._BlackHole._BlackHole == null) {
				print ("modelAbsorber._BlackHole._BlackHole == null");
			}
			List<MiroV1AbsorbPoint> absPs = 
				modelAbsorber._BlackHole._BlackHole.GetAbsorbingPos ();
			
			LinkEmittersToAbsorbingPoss (ems, absPs);
		}

		void LinkEmittersToAbsorbingPoss(
			List<MiroV1BulletEmitterBase> ems,
			List<MiroV1AbsorbPoint> absPos)
		{
			foreach (MiroV1AbsorbPoint ab in absPos) {
				if (!ab.IsEmitterLinked ()) {
					if (ems.Count > 0) {
						ab.LinkEmitter (ems [ems.Count - 1]);
						ems.RemoveAt (ems.Count - 1);
					} else {
						ab.DelinkEmitters ();
					}
				}
			}

			/*
			foreach (MiroV1BulletEmitterBase em in ems) {
				foreach (MiroV1AbsorbPoint ab in absPos) {
					if (!ab.IsEmitterLinked ()) {
						ab.LinkEmitter (em);
						break;
					}
				}
			}*/
		}

		public void ReleaseAbsorbing()
		{
			if (_BlackHole._BlackHole == null) {
				return;
			}
			List<MiroV1AbsorbPoint> absPs = 
				_BlackHole._BlackHole.GetAbsorbingPos ();

			foreach (MiroV1AbsorbPoint abp in absPs) {
				abp.DelinkEmitters ();
			}
		}



		public List<MiroV1BulletEmitterBase> GetONNotAbsorbedEmitters()
		{
			List<MiroV1BulletEmitterBase> ems = GetChdONEmitters ();
			for (int i = ems.Count - 1; i >= 0; i--) {
				if (ems [i].IsAbsorberON ()) {
					ems.RemoveAt (i);
				}
			}
			return ems;
		}

		public List<MiroV1BulletEmitterBase> GetChdONEmitters()
		{
			List<MiroV1BulletEmitterBase> lstEMs = 
				new List<MiroV1BulletEmitterBase>();
			MiroV1BulletEmitterBase[] ems = 
				GetComponentsInChildren<MiroV1BulletEmitterBase> ();
			foreach(MiroV1BulletEmitterBase em in ems)
			{
				if(em.IsON())
				{
					lstEMs.Add(em);
				}
			}
			return lstEMs;
		}


		public Transform _PumpParentTF;
		public MiroV1PumpBase _Pump = null;
		public bool _GrowPumpTrigger = false;
		public bool _ShrinkPumpTrigger = false;
		public void AddPump(MiroV1PumpBase pump)
		{
			_Pump = pump;
			pump.transform.SetParent (_PumpParentTF);
		}

		public MiroV1PumpBase GetPump()
		{
			return _Pump;
		}

		[ContextMenu("GrowUpPump")]
		public void GrowUpPump()
		{
			if (_Pump != null) {
				_Pump.GrowUP ();
			}
		}

		[ContextMenu("ShrinkPump")]
		public void ShrinkPump()
		{
			if (_Pump != null) {
				_Pump.Shrink ();
				ShrinkNeighborBlackDot ();
			}
		}

		void ShrinkNeighborBlackDot ()
		{
			Transform akrTFB = _Pump.GetAnchorTFB ();
			Lyu.KeepOffset kepOff = akrTFB.GetComponent<Lyu.KeepOffset> ();
			if (kepOff._Anchor != null) {
				MiroV1BlackDotBase bkDot = kepOff._Anchor.GetComponent<MiroV1BlackDotBase> ();
				bkDot.Shrink ();
			}
		}

		[ContextMenu("DestroyPump")]
		public void DestroyPump()
		{
			if (_Pump != null) {
				ShrinkNeighborBlackDot ();
				Destroy (_Pump.gameObject);
			}
		}

		[ContextMenu("BreakPump")]
		public void BreakPump()
		{
			if (_Pump != null) {
				_Pump.Scatter ();
			}
		}

		[ContextMenu("RecoverPump")]
		public void RecoverPump()
		{
			if (_Pump != null) {
				_Pump.Recover ();
			}
		}

		[ContextMenu("ShrinkRing")]
		public void ShrinkRing()
		{

			MiroV1RingBallBase[] ringballs = GetComponentsInChildren<MiroV1RingBallBase> ();

			foreach (MiroV1RingBallBase rball in ringballs) {
				rball.ShringWholeRing ();
			}



		}


		public void ShrinkEveryRingBall()
		{
			MiroV1RingBallBase[] ringballs = GetComponentsInChildren<MiroV1RingBallBase> ();
			foreach (var b in ringballs) {
				b.Shrink ();
			}
		}

		public void DestroyEveryRingBall()
		{
			MiroV1RingBallBase[] ringballs = GetComponentsInChildren<MiroV1RingBallBase> ();
			foreach (var b in ringballs) {
				Destroy (b.gameObject);
				print ("Destroy:" + b.name);
			}
		}

		public void UpdateMainBodyHPByBlackDots()
		{
			int hpmax = 0;
			int hp = 0;
			foreach (BlackDotSlot bds in _BlackDots) {
				hpmax += bds._blackDot._HPMax;
				hp += bds._blackDot._HP;
			}

			_MainBody._HPMax = hpmax;
			_MainBody._HP = hp;
		}


		public void Die()
		{
			// Init Breaking Partiles
			GameObject ps = Instantiate (_BreakPS) as GameObject;
			ps.transform.position = transform.position;
			ps.transform.SetParent (transform.parent,true);

			// Delete Connections
			if (_Farm2 != null) {
				_Farm2.Scatter ();
			}

			Destroy (gameObject);
		}

		public void RecoverAllBlackDots()
		{
			foreach (BlackDotSlot bds in _BlackDots) {
				bds._RecoverTrigger = true;
			}
		}

		private MiroV1EF2Ctrl _Farm2 = null;
		public void SetFarm(MiroV1EF2Ctrl farm2)
		{
			_Farm2 = farm2;
		}

		public void DetachFarm()
		{
			_Farm2 = null;
		}

		public bool HasFarm()
		{
			return _Farm2 != null;
		}

		public void ShrinkFarm2()
		{
			if (_Farm2 != null) {
				_Farm2.Shrink ();
				_Farm2 = null;
			}
		}

		public bool HasPump()
		{
			return _Pump != null;
		}
		public bool IsAiding()
		{
			bool bAid = (_Pump!=null);
			if (bAid) {
				bAid = _Pump.IsGrown ();
			}
			return bAid;
		}

		public bool IsAiding(MiroModelV1 another)
		{
			bool bAid = (_Pump!=null);
			if (bAid) {
				bAid = _Pump.IsGrown ();
				MiroV1PumpBase pump = 
					_Pump.GetComponent<MiroV1PumpBase> ();
				bAid = bAid && (pump.GetModelAidee () == another);
			}
			return bAid;
		}


		// ---------- Chess Movement ---------------//
		public int _MovePwr = 0;
		public void AddMovePwr(int movePwr)
		{
			_MovePwr += movePwr;
			_MovePwr = Mathf.Clamp (_MovePwr, 0, 9999);
		}

		public void MovePwrInc1()
		{
			AddMovePwr (1);
		}

		public void ClearMovePwr()
		{
			_MovePwr = 0;
		}

		public int GetMovePwr()
		{
			return _MovePwr;
		}

		public int _RotLeftPwr = 0;
		public void AddRotLeftPwr(int pwr)
		{
			_RotLeftPwr += pwr;
			_RotLeftPwr = Mathf.Clamp (_RotLeftPwr, 0, 9999);
		}

		public void RotLeftPwrInc1()
		{
			AddRotLeftPwr (1);
		}

		public void ClearRotLeftPwr()
		{
			_RotLeftPwr = 0;
		}

		public int GetRotLeftPwr()
		{
			return _RotLeftPwr;
		}

		public int _RotRightPwr = 0;
		public void AddRotRightPwr(int pwr)
		{
			_RotRightPwr += pwr;
			_RotRightPwr = Mathf.Clamp (_RotRightPwr, 0, 9999);
		}

		public void RotRightPwrInc1()
		{
			AddRotRightPwr (1);
		}

		public void ClearRotRightPwr()
		{
			_RotRightPwr = 0;
		}

		public int GetRotRightPwr()
		{
			return _RotRightPwr;
		}

		public void ResetEmitterTrigger()
		{
			ENEmittersTrigger emTgr = GetComponentInChildren<ENEmittersTrigger> ();
			emTgr.ResetbAllOnEmitters ();

		}

		public void UpdateEmittersTrigger()
		{
			ENEmittersTrigger tgr = GetComponentInChildren<ENEmittersTrigger> ();
			tgr.GetEmitters ();
		}

		public bool IsAlive()
		{
			int attackerCount = GetAttackersCount ();
			int mhp = GetMaxHP ();
			if (mhp > 0) {
				bool bAlive = mhp > attackerCount;
				return bAlive;
			} else {
				return true;
			}

		}

		public int GetAbsorbingPwr()
		{
			bool bHoleON = _BlackHole.IsGrown ();
			if (bHoleON) {
				return 2;
			} else {
				return 0;
			}
		}

		public int GetAbsorbingAmt()
		{
			int amt = 0;
			if (_BlackHole.IsGrown ()) {
				amt = _BlackHole._BlackHole.GetAbsorbingAmt ();
			} else {
				amt = 0;
			}
			return amt;
		}



	}
}
