using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 ChasingArea오브젝트에 붙은 스크립트
public class DetectingPlayer : MonoBehaviour {
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄
    public GameObject ParentMonster;
    public float PosX;
    //현재 x좌표(상대좌표)
    private int ParentState;
    public float ExitTime = 0;  //플레이어가 추적 영역 빠져나간 시간
    public float WaitTime = 5;  //플레이어가 기다릴 시간
    public bool PlayerExit = true;    //플레이어와 접촉하면 false 빠져나가면 true
	// Use this for initialization
	void Start () {
        ParentMonster = this.gameObject.transform.parent.gameObject;
        PosX = transform.localPosition.x;
        if (ParentMonster.GetComponent<MonstersAI_FIXED>() != null)
        { 
            ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea = this.gameObject;
        }
        else if(ParentMonster.GetComponent<MonsterAI_Moving>()!=null)
        {
            ParentMonster.GetComponent<MonsterAI_Moving>().SearchArea = this.gameObject;
        }
        //StartCoroutine(TimeChecker());
        if (WaitTime == 0)
        {
            WaitTime = 5;
        }
    }

    // Update is called once per frame
    void Update () {
        #region 고정형 몬스터인 경우
        if (ParentMonster.GetComponent<MonsterAI_Moving>() == null)
        {
            #region 몬스터 상태 전환(플레이어가 발견 영역에서 빠져나간뒤 추적 하는 동안 시간 초과
            //플레이어가 이 발견 영역&&공격 영역&&몬스터 콜라이더 영역에 충돌하지 않은 상태여야 한다->쫓는 상태이긴 함
            if (ParentMonster.GetComponent<MonstersAI_FIXED>().NowMonstate == MonstersAI_FIXED._IsMonstate.AttackState
                && PlayerExit == true)
            {
                ExitTime += Time.deltaTime;
                if (ExitTime >= WaitTime)
                {
                    print("EXIT");
                    //ParentState = 0;    //안됨:왜?->얜 그냥 _isMonState값 만을 받는 변수라서
                    ParentMonster.GetComponent<MonstersAI_FIXED>().NowMonstate = MonstersAI_FIXED._IsMonstate.ReconState;
                    ExitTime = 0;
                }
            }
            #endregion
        }
        #endregion
        else if (ParentMonster.GetComponent<MonsterAI_Moving>() != null)
        {
            //움직일 수 있는 몬스터 인경우
            if (ParentMonster.GetComponent<MonsterAI_Moving>().NowMonstate==MonsterAI_Moving._IsMonstate.ChasingState
                && PlayerExit == true)
            {
                ExitTime += Time.deltaTime;
                if (ExitTime >= WaitTime)
                {
                    print("Moving_EXIT");
                    ParentMonster.GetComponent<MonsterAI_Moving>().NowMonstate = MonsterAI_Moving._IsMonstate.ReconState;
                    ExitTime = 0;
                }
            }
        }
        if (PlayerExit == true)
        {
        }
        if (PlayerExit == false)
        {
            ExitTime = 0;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌(추적모드=1)
            if (ParentMonster.GetComponent<MonstersAI_FIXED>() != null)
            {
                ParentMonster.GetComponent<MonstersAI_FIXED>().NowMonstate = MonstersAI_FIXED._IsMonstate.AttackState;
            }
            else if (ParentMonster.GetComponent<MonsterAI_Moving>() != null)
            {
                if (ParentMonster.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DELAY") == false)
                {
                    ParentMonster.GetComponent<MonsterAI_Moving>().NowMonstate = MonsterAI_Moving._IsMonstate.ChasingState;
                }

            }
            PlayerExit = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerExit = true;
        }
    }
}
