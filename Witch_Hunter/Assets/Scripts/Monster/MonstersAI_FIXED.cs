using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersAI_FIXED : MonoBehaviour
{
    //MonsterManager가 몬스터 생성하면서 각 몬스터에 맞는 csv파일 파싱해서 공격력, hp,몬스터 타입(정찰,고정,사격,근접..),속도 등 나눠줌
	#region Private Variable
	private GameManager _GameManager;
    public string Name;
    public int HP;  //몬스터 체력
    public int attack;  //몬스터 공격력,함정도 있음
    public int index;   //몬스터 인덱스
    public bool _CheckMode;									// true면 공격
    public float NormalSpeed = 2f;      //정찰 모드 시 속도
	public float _AttackSpeed = 4f;                         // 공격 속도(추적 모드때 속도)
	public float _CheckTime = 0;    //공격 쿨타임 재는 시간
	public float _CheckDelay = 2;   //쿨타임 한계시간
    public bool isDead; //죽으면 true(죽으면 코인줌)->마녀 죽으면 다시 리젠 못함
    public int Stage_Location;  //몬스터 출현 스테이지 
    public bool IsBoss; //몬스터 마녀인지 아닌지->나중에 삭제 가능성
    #region 일반 몬스터 특별 속성
    public bool Recon;  //true면 정찰(false-고정형이면 무조건 원거리)
    public int MonsterType; //0이면 일반(근접공격-근거리) 1이면 사격-원거리 2면 강화형(HP 더 커짐) 3-자폭
    public bool GetInfo;    //MonsterManager로부터 정보 받으면 true
    #endregion

    [SerializeField]
	public GameObject Target;
	#endregion

	#region public Variable
	public int Coin;
	#endregion

	#region Private Method
	private void Start(){
        Target = GameObject.FindGameObjectWithTag("Player");
		_GameManager = GameManager.GetGameManager;
	}
	private void Update(){
        if (Recon == false)
        {
            NotMovingMonsterAttack();
        }
        else
        {
            //고정형 아니면
            switch (MonsterType)
            {
                case 0: //일반(근접)
                    break;
                case 1: //일반(원거리=사격형)
                    break;
                case 2: //강화형(hp두배)
                    HP = 2 * HP;
                    break;
            }

        }
        if (Coin == 0)
        {
            Coin = Random.Range(1, 21);

            //w_Coin = Random.Range(100, 501);
        }

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
        switch (Recon) {
            case false:
                //고정형인 경우!!!
                //Y축검사->같은 층에 있는지
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
                break;
            case true:
                break;
          
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

	#region 고정형 타입 
     public void NotMovingMonsterAttack()
    {
        //고정형 원거리 몬스터 공격 타입
    }
	#endregion

}
