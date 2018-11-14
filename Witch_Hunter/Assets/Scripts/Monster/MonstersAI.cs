using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonstersAI : MonoBehaviour
{
	#region Private Variable
    //정찰-0,1
	private int RangerMode = 0;                 // 정찰 모드 변수 0 : left, 1 : right, 2 : Idle

	private int _isMonstate = 0;                // 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드

	private float _MoveSpeed = 3f;              // 적 움직임 속도

	private bool _isChasing;                    // 추적 모드

	private bool _InAttack;                     // 공격 모드

	public Transform Target;                    // 타겟

	private Rigidbody2D _Rigid;                 // 리지드 함수

	#endregion

	#region Public Variable
	#endregion

	#region Private Method
	private void Start()
	{
		StartCoroutine("ReconCheck");


		_Rigid = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
		Recon();
	}

	#region 몬스터 관리

	private IEnumerator ReconCheck()
	{
		RangerMode = Random.Range(0, 3);

		if (RangerMode == 2)
		{
			// 애니메이션
		}

		else
		{
			// 애니메이션
		}

		yield return new WaitForSeconds(3f);

		StartCoroutine("ReconCheck");
	}

	private void Recon()
	{

		switch (_isMonstate)
		{
			case 0: // 정찰모드
				Vector3 moveVelocity = Vector3.zero;
				if (RangerMode == 0)
				{
					moveVelocity = Vector3.left;
					transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				if (RangerMode == 1)
				{
					moveVelocity = Vector3.right;
					transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
				break;
			case 1: // 추격모드
				Chase();
				break;
			case 2: // 공격 모드
				Attack();
				break;
		}
	}
	private void Attack()
	{
		if (transform.tag.Equals("Ghost"))
		{
			if (Target.transform.position.x > transform.position.x)
			{
				_Rigid.AddForce(-Vector2.right * _MoveSpeed, ForceMode2D.Impulse);
			}
			else if (Target.transform.position.x < transform.position.x)
			{
				_Rigid.AddForce(-Vector2.left * _MoveSpeed, ForceMode2D.Impulse);
			}
		}
	}
	private void Chase()
	{
		Vector3 moveVelocity;
		if (Target.transform.position.x < gameObject.transform.position.x)
		{
			moveVelocity = Vector3.left;
			transform.rotation = Quaternion.Euler(0, 0, 0);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else if (Target.transform.position.x > gameObject.transform.position.x)
		{
			moveVelocity = Vector3.right * _MoveSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(0, 180, 0);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else
		{
			moveVelocity = Vector3.zero;
		}
	}

	#endregion


	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Player") {
			_isChasing = true;
		}
	}
	private void OnTriggerStay2D(Collider2D col)
	{
		if (_isChasing) _isMonstate = 1;
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (_InAttack == false) {
			if (col.transform.tag == "Player")
			{
				_isMonstate = 0;
				_isChasing = false;
			}
		}
	}
	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.tag == "Player" && _isChasing)
		{
			_InAttack = true;
		}
	}
	private void OnCollisionStay2D(Collision2D col)
	{
		if (_InAttack) _isMonstate = 2;
	}
	private void OnCollisionExit2D(Collision2D col)
	{
		if (_InAttack)
		{
			_isMonstate = 1;
			_InAttack = false;
		}
	}
	#endregion

	#region Public Method
	#endregion
}


