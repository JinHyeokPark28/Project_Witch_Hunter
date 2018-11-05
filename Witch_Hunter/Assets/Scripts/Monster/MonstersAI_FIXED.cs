using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersAI_FIXED : MonoBehaviour
{
	#region Private Variable
	private GameManager _GameManager;
	public bool _CheckMode;									// true면 공격

	public float _AttackSpeed = 4f;                         // 공격 속도

	public float _CheckTime = 0;

	public float _CheckDelay = 2;

	[SerializeField]
	public GameObject Target;
	#endregion

	#region public Variable
	public int Coin;

	public int w_Coin;
	#endregion

	#region Private Method
	private void Start(){

		_GameManager = GameManager.GetGameManager;
	}
	private void Update(){

		Coin = Random.Range(1, 21);

		w_Coin = Random.Range(100, 501);

	}
	private void Check()
	{
		if (Time.time - _CheckTime > _CheckDelay)
		{
			_CheckTime = Time.time;
			Attack();
		}
	}
	private void Attack(){

		if (Target.transform.position.x > transform.position.x)
		{
			print("체크");
			Target.GetComponent<Rigidbody2D>().AddForce(Vector2.right * _AttackSpeed * 2, ForceMode2D.Impulse);
		}
		if (Target.transform.position.x < transform.position.x)
		{
			print("가즈아");
			Target.GetComponent<Rigidbody2D>().AddForce(Vector2.left * _AttackSpeed * 2, ForceMode2D.Impulse);
			_GameManager.m_GetGold(Coin);
		}
		
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag == "Player"){
			_CheckMode = true;
		}
	}
	private void OnTriggerStay2D(Collider2D col)
	{
		if (_CheckMode == true) Check();
		if (_CheckMode == false) return;
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if(col.transform.tag == "Player"){
			_CheckMode = false;
		}
	}
	private void OnCollisionStay2D(Collision2D other)
	{
		// 공격 받았을 때 데미지 처리.
	}

	#endregion 

	#region public Method
	#endregion

}
