using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parent_monster : MonoBehaviour {
	#region Private Variable
	// 에니메이터 변수 
	private Animator _Anim;

	// 벡터3 변수
	private Vector3 _Move;

	// 리지드 변수
	private Rigidbody2D _Rigid;

	// 정찰 모드 변수
	private int RangerMode = 0;

	// 이동 속도 변수
	private float _MoveSpeed = 2f;

	// 추적 시작 시간 변수
	private float _CheckTime = 2;

	// 추적 모드 변수
	private int TraceMode = 0;

	// 추적 변수
	private bool _isTracing;

	// 스위치 변수
	private int _isMonstate = 0;

	// 타겟 설정 변수
	public Transform Target;
	#endregion

	#region Public Variable
	#endregion

	#region Private Method
	private void Start() {
		_Anim = GetComponent<Animator>();

		_Rigid = GetComponent<Rigidbody2D>();
		StartCoroutine("Recon");
		Target = GameObject.Find("Player").transform;
	}
	private void FixedUpdate() {
		GhostState();
		Debug.Log(_isTracing);
	}
	IEnumerator Recon() {
		RangerMode = Random.Range(0, 3);

		if (RangerMode == 2)
			_Anim.SetBool("Halo", true);
		else
			_Anim.SetBool("Halo", false);

		yield return new WaitForSeconds(3f);

		StartCoroutine("Recon");
	}
	private void GhostState() {

		switch (_isMonstate) {
			case 0: // 정찰모드
				Vector3 moveVelocity = Vector3.zero;
				if (RangerMode == 0) {
					moveVelocity = Vector3.left;
					transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				if (RangerMode == 1) {
					moveVelocity = Vector3.right;
					transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
				break;
			case 1: // 추격모드
				if (_isTracing == true) {
					Trace();
				}
				break;
			case 2: // 공격모드
				break;
		}
	}
	private void Trace() {
		Vector3 moveVelocity;
		if (Target.transform.position.x < gameObject.transform.position.x) {
			moveVelocity = Vector3.left;
			transform.rotation = Quaternion.Euler(0, 180, 0);
			_Anim.SetBool("Halo", true);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else if (Target.transform.position.x > gameObject.transform.position.x) {
			moveVelocity = Vector3.right * _MoveSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(0, 0, 0);
			_Anim.SetBool("Halo", true);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else {
			moveVelocity = Vector3.zero;
			_isMonstate = 0;
			_Anim.SetBool("Halo", false);
		}
	}
	private void OnTriggerEnter2D(Collider2D other) {

	}
	private void OnTriggerStay2D(Collider2D other) {
		if (other.transform.tag.Equals("Player") && transform.Find("Chase")) {
			_isMonstate = 1;
			_isTracing = true;
		}
		if (other.transform.tag.Equals("Player") && transform.Find("Attack")) {
			_isMonstate = 2;
		}
	}
	private void OnTriggerExit2D(Collider2D other) {
		if (other.transform.tag.Equals("Player") && transform.Find("Chase")) {
			_isMonstate = 0;
			_isTracing = false;
		}
	}
	#endregion

	#region Public Method
	#endregion

}
