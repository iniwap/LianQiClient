using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartAgent;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1BulletEmitterBase : MonoBehaviour {
		public Transform _SourceTF = null;
		private Transform _Target = null;

		public bool _bON = false;
		public Transform _BulletParent = null;
		public GameObject _BulletPrefab;
		public GameObject _ENAbsorbPrefab;

		public Vector3 _StartOffset;
		public Vector2 _StartDir;
		public float _StartImplusePwr;
		public MiroV1ModelSetting _modelSetting;

		private MiroV1AbsorbPoint _Absorber = null;

		private GameObject _EmittedEN = null;

		private bool _EmittedENAlive = false;
		private bool _bArriveTgt = false;
		public UnityEvent _ArriveTgt;

		virtual public void Start()
		{
			_bArriveTgt = true;
			if (_SourceTF == null) {
				_SourceTF = transform;
			}
			GetModelSetting ();
		}

		//private GameObject _EmitENPrev = null;
		void Update()
		{
			if (_Target == null) {
				TurnOFF ();
			}

		}

		public bool IsAttacking()
		{
			return _bON && (_Absorber == null);
		}

		public bool HasEmitEN()
		{
			return _EmittedEN != null;
		}
		public bool IsEmitArriveTgt()
		{
			if (_bArriveTgt) {
				//Debug.Log (gameObject.name + " en Arrive target");
			} else {
				//Debug.Log (gameObject.name + " not arrive target");
			}
			return _bArriveTgt;
		}

		public bool IsEmitENArriveAndVanish()
		{
			bool bArrive = IsEmitArriveTgt ();
			bool bVanish = _EmittedENAlive;
			bool bFlag = bArrive && bVanish;
			return bFlag;
		}

		public void SetAbsorber(MiroV1AbsorbPoint ab)
		{
			//print (this + " SetAbsorber! " + ab);
			_Absorber = ab;
		}

		public bool IsAbsorberON()
		{
			return (_Absorber != null);
		}

		public void RmAbsorber()
		{
			_Absorber = null;
		}

		public bool IsON()
		{
			return _bON;
		}

		public bool TurnON()
		{
			_bON = true;
			return _bON;
		}

		public void TurnOFF()
		{
			_bON = false;
		}

		public void SetSourceTF(Transform srcTF)
		{
			_SourceTF = srcTF;
		}

		public Transform GetSourceTF()
		{
			return _SourceTF;
		}

		public void SetTarget(Transform tgt)
		{
			_Target = tgt;
			if (tgt != null) {
				//Debug.Log ("Tgt:" + tgt);
			}
		}


		public Transform GetTargets()
		{
			return _Target;
		}

		virtual public void Emit()
		{
			ClearEmits ();


			if (_bON) {
				GameObject Prefab = _BulletPrefab;
				Transform tgt = _Target;
				if (_Absorber != null) {
					Prefab = _ENAbsorbPrefab;
					tgt = _Absorber.transform;
				}

				_EmittedEN = Instantiate (Prefab) as GameObject;
				InitTF (_EmittedEN);
				SetEmitTarget (_EmittedEN, tgt);
				SetEmitColor (_EmittedEN);
				SetEmitTrailColor (_EmittedEN);
				if (_Absorber == null) {
					AddStartImpluse (_EmittedEN);
				}

				SA_Arrive arrive = 
					_EmittedEN.GetComponent<SA_Arrive> ();
				arrive._Arrive.AddListener (this.Arrived);

				_bArriveTgt = false;
			} 
		}

		void ClearEmits ()
		{
			foreach (Transform emTf in _Emits) {
				if (emTf != null) {
					Destroy (emTf.gameObject);
				}
			}
			_Emits.Clear ();
		}

		private List<Transform> _Emits = new List<Transform> ();
		public void Arrived()
		{
			if (_EmittedEN != null) {
				TrailRenderer tr = 
					_EmittedEN.GetComponentInChildren<TrailRenderer> ();
				if (tr != null) {
					Transform tf = tr.transform;
					tf.SetParent (transform);
					_Emits.Add (tf);
					//print ("_Emits.Add (tf);" + tf.name);
				}
			}

			_ArriveTgt.Invoke ();
			_bArriveTgt = true;
			//Debug.Log ("Arrived");
		}

		void InitTF (GameObject bullet)
		{
			if (_BulletParent == null) {
				_BulletParent = transform;
			}
			bullet.transform.SetParent (_BulletParent);
			float baseZOffset = bullet.transform.localPosition.z;
			Vector3 pos = 
				_SourceTF.TransformPoint (
					Vector3.zero );
			pos.z += baseZOffset;
			pos += _StartOffset;
			bullet.transform.position = pos;
		}

		void SetEmitTarget (GameObject bullet, Transform tgt)
		{
			TargetTransform ttf = bullet.GetComponent<TargetTransform> ();
			if (ttf == null) {
				ttf = bullet.AddComponent<TargetTransform> ();
			}
			ttf._Target = tgt;
		}

		void AddStartImpluse (GameObject bullet)
		{
			Rigidbody2D rb = bullet.GetComponent<Rigidbody2D> ();
			Vector2 Force = _StartImplusePwr * transform.TransformDirection (_StartDir.normalized);
			rb.AddForce (Force, ForceMode2D.Impulse);
		}
			
		void SetEmitColor (GameObject bullet)
		{
			SpriteRenderer sr = bullet.GetComponentInChildren<SpriteRenderer> ();
			sr.color = _modelSetting._colorSetting._ENMax;
		}

		void SetEmitTrailColor(GameObject bullet)
		{
			TrailRenderer tr = bullet.GetComponentInChildren<TrailRenderer> ();
			if (tr != null) {
				Lyu.TrailRendererCtrl.SetGradient (
					tr, _modelSetting._colorSetting._BulletTrailGrad);
			}
		}

		public Transform GetTarget()
		{
			return _Target;
		}
			
			
		[ContextMenu("GetModelSetting")]
		public void GetModelSetting ()
		{
			if (_modelSetting == null) {
				_modelSetting = GetComponentInParent<MiroV1ModelSetting> ();
			}
		}
	}
}
