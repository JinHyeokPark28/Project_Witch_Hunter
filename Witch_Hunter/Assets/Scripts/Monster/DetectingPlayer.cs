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
    public bool TimeCheck = true;  //더이상 시간 체크할 필요 없다면 false
	// Use this for initialization
	void Start () {
        //자동으로 부모 오브젝트 찾게 해줌
        ParentMonster = this.gameObject.transform.parent.gameObject;
        PosX = transform.localPosition.x;
        ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea = this.gameObject;
        //StartCoroutine(TimeChecker());
    }
    //플레이어 발견&&추적(_isMonState==1)인 상태에서 플레이어와 접촉 안한 상태
    /*IEnumerator TimeChecker()
    {
        print("Time Checker Function Start");
        //왜 여기서 안돌아가나? UPdate에 두니까 됌->왜???????->while 안들어가고 바로 나가는듯
        //해결책:update에 계속 돌리기->근데 그러기엔 안그래도 update쪽에서 매 프레임마다 나가고 코루틴도 update만큼은 아니지만
        //자원 크게 잡아먹어서 무리->그냥 함수 따로 만들어서 update에서 불러오기 하는 걸로 하기
        while (ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate == 1 && PlayerExit == true)
        {
            print("0"); //->얘도 안찍힘 
            yield return new WaitForSeconds(5);
            ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 0;
        }
    }*/

    // Update is called once per frame
    void Update () {
        //부모 오브젝트에서 오일러쓰면 localPosition도 그 바뀐 오일러값에 따라 달라짐
        #region 몬스터 상태 전환(플레이어가 발견 영역에서 빠져나간뒤 추적 하는 동안 시간 초과
        //플레이어가 이 발견 영역&&공격 영역&&몬스터 콜라이더 영역에 충돌하지 않은 상태여야 한다->쫓는 상태이긴 함
        if(ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate == 1 && PlayerExit == true)
        {
            //조건 만족할 !동안!에만 지속되어야 한다->if로는 왔다갔다함
            //한번 이어지면 끝까지 이어져야 한다. 중간에 조건 불만족하면 exitTime=0으로 되어야함
            if (TimeCheck == true)
            {
                //ExitTime은 if문이 맞을때 들어온다->계속 쌓임(조건 만족:더해짐->조건 안만족:안더해짐(그대로있음)->조건만족:더해짐)
                ExitTime += Time.deltaTime;
                if (ExitTime >= WaitTime) 
                {
                    print("YES"); 
                    //ParentState = 0;    //안됨:왜?->얜 그냥 _isMonState값 만을 받는 변수라서
                    ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 0;
                    print("STATE:" + ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate);
                    TimeCheck = false;
                    ExitTime = 0;
                }
            }
            
        }
        else
        {
            TimeCheck = false;
        }
        /*if ((PlayerExit == false) || (ParentState == 2))
        {
            //플레이어 다시 들어오면 exitTIme계산 안함
            ExitTime = 0;
        }*/
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //한번 범위로 진입하면 바로 추적모드로 바뀜->범위 벗어나면?
        if (collision.gameObject.tag == "Player")
        {
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌(추적모드=1)
            ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 1;
            PlayerExit = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //플레이어가 추적 영역 빠져나간 경우
            PlayerExit = true;
            print("EXIT");
        }
    }
}
