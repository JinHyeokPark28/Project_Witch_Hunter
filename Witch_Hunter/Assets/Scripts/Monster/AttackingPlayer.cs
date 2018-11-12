using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터에 붙은 AttackingArea에 붙은 스크립트
//플레이어와 이 오브젝트와 접촉하면 부모오브젝트 monster에 신호 보냄(공격모드로 전환하라고)
public class AttackingPlayer : MonoBehaviour {
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄

    public GameObject ParentMonster;
    public float PosX;
    //현재 x좌표(상대좌표)
    // Use this for initialization
    void Start () {
        PosX = transform.localPosition.x;   //locapPosition으로 해야 부모와의 상대좌표로 나옴
        ParentMonster = this.gameObject.transform.parent.gameObject;
        ParentMonster.GetComponent<MonstersAI_FIXED>().AttackArea = this.gameObject;
        

    }

    // Update is called once per frame
    void Update () {
        //몬스터가 오른쪽으로 몸 돌리면 공격 범위도 위치가 달라짐
        if (ParentMonster.GetComponent<MonstersAI_FIXED>().isLeft == false)
        {
            transform.localPosition = new Vector2(-PosX, transform.localPosition.y);
        }
        else
        {
            transform.localPosition = new Vector2(PosX, transform.localPosition.y);

        }
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        //코드 꼬일수도 있음->정찰범위,공격범위 겹치는 부분에서 계속 실시간으로 monState값이 바뀔수도?->방지 필요
        if (collision.gameObject.tag == "Player")
        {
            //공격범위에 플레이어 진입 할 경우 정찰 범위 콜라이더 꺼놓음
            ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea.GetComponent<BoxCollider2D>().enabled = false;
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌(공격모드=1)
            ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //공격범위에 플레이어 진입 할 경우 정찰 범위 콜라이더 꺼놓음->됌
            if (ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea.GetComponent<BoxCollider2D>().enabled == false)
            {
                ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea.GetComponent<BoxCollider2D>().enabled = true;
            }
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌(공격모드=1)
        }
    }
}
