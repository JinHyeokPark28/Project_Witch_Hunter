using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Ab_Monster
{
	#region Private Variable
	private int RangerMode = 0;                 // 정찰 모드 변수 0 : left, 1 : right, 2 : Idle

	private int _isMonstate = 0;                // 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드

	private float _MoveSpeed = 3f;              // 적 움직임 속도

	private bool _isChasing;                    // 추적 모드

	private bool _InAttack;                     // 공격 모드

	private bool _CheckAttack;                  // 공격하고 있는지 체크하는 함수;

	public Transform Target;                    // 타겟

	private Rigidbody2D _Rigid;                 // 리지드 함수

	private int HP = 0;                         // 몬스터 HP

	private int m_Damage = 0;                     // 몬스터 데미지

	public List<string> m_Monster = new List<string>(); // 몬스터 리스트

	#endregion

	#region Public Variable
	public Text Money;
	#endregion

	#region Private Method
	private void Start()
	{
		m_Monster.Add("Ghost");
		StartCoroutine("ReconCheck");

		GameObject Chase = GameObject.Find("Chase");

		_Rigid = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
		Recon();
		print(_isMonstate);
	}

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




	private string GetThousandCommaText(int data)							// 돈 천단위 콤마;
	{
		return string.Format("{0:#,###}", data);
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag == "Player"){
			_isChasing = true;
		}
	}
	private void OnTriggerStay2D(Collider2D col)
	{
		if (_isChasing) _isMonstate = 1;
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (_InAttack == false){
			if(col.transform.tag == "Player")
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
			Item();
		}
	}
	#endregion

	#region Public Method
	public override void Recon()
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
	public override void Attack()
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
	public override void Chase()
	{
		Vector3 moveVelocity;
		if (Target.transform.position.x < gameObject.transform.position.x)
		{
			moveVelocity = Vector3.left;
			transform.rotation = Quaternion.Euler(0, 180, 0);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else if (Target.transform.position.x > gameObject.transform.position.x)
		{
			moveVelocity = Vector3.right * _MoveSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(0, 0, 0);
			transform.position += moveVelocity * _MoveSpeed * Time.deltaTime;
		}
		else
		{
			moveVelocity = Vector3.zero;
		}
	}

	public override void Item()
	{

		int m_Money = Random.Range(0, 21);

		Money.text = GetThousandCommaText(m_Money).ToString();
	}


	#region 몬스터 관리

	#endregion

	public override void Hp()
	{
	}

	public override void Damage()
	{
	}
}
#endregion


