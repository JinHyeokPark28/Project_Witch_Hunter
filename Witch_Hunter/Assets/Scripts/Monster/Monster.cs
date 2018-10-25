using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IMonster {
	#region Private Variable
	private bool _IsRecon;          // 정찰모드

	private bool _OnChase;          // 추격 모드 

	private bool _InAttack;         // 공격 모드

	private int _RangerMode;		// 정찰모드 0(IDLE), 1(LEFT), 2(RIGHT)
	[SerializeField]
	private float _MoveSpeed = 2f;  // 몬스터 속도 변수

	[SerializeField]
	private Transform Target;
	#endregion

	#region Public Variable
	#endregion

	#region Private Method
	private void Start() {
	}
	#endregion

	#region Public Method
	public void Recon() {
		switch (_IsRecon) {
			case true:
				_RangerMode = Random.Range(0, 3);
				Vector3 moveVelocity;
				if(_RangerMode == 0){
					moveVelocity = Vector3.zero;
				}
				if(_RangerMode == 1){
					moveVelocity = Vector3.left;
					transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				if(_RangerMode == 2){
					moveVelocity = Vector3.right;
					transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				break;
			case false:
				if(_OnChase == true){
					Chase();
					if(_InAttack == true){
						Attack();
					}
				}
				break;
		}
	}
	public void Attack() {
		// 공격하는 스크립트 짜기
	}
	public void Chase() {
		Vector3 moveVelocity = Vector3.zero;
		if (Target.transform.position.x < transform.position.x) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
			moveVelocity = Vector3.left;
		}
		if (Target.transform.position.x > transform.position.x) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
			moveVelocity = Vector3.right;
		}
		transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.tag.Equals("Player") && transform.tag.Equals("Chase")) {
			if (_OnChase == false) _OnChase = true;
			if (_OnChase == true) return;
		}

	}
	private void OnTriggerStay2D(Collider2D other) {
		if(other.transform.tag.Equals("Player") && transform.tag.Equals("Chase") && transform.tag.Equals("Attack")){
			if (_InAttack == false) _InAttack = true;
			if (_InAttack == true) return;
		}
	}
	private void OnTriggerExit2D(Collider2D other) {
		if(other.transform.tag.Equals("Player") && transform.tag.Equals("Chase")){
			if (_OnChase == true) _OnChase = false;
			if (_OnChase == false) return;
		}
		if (other.transform.tag.Equals("Player") && transform.tag.Equals("Chase") && transform.tag.Equals("Attack")) {
			if (_InAttack == true) _InAttack = false;
			if (_InAttack == false) return;
		}
	}
	#endregion

}
