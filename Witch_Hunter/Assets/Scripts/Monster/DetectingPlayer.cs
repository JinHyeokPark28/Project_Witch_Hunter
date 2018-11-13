using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 ChasingArea오브젝트에 붙은 스크립트
public class DetectingPlayer : MonoBehaviour {
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄
    public GameObject ParentMonster;
    public float PosX;
    //현재 x좌표(상대좌표)
	// Use this for initialization
	void Start () {
        //자동으로 부모 오브젝트 찾게 해줌
        ParentMonster = this.gameObject.transform.parent.gameObject;
        PosX = transform.localPosition.x;
        ParentMonster.GetComponent<MonstersAI_FIXED>().SearchArea = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //부모 오브젝트에서 오일러쓰면 localPosition도 그 바뀐 오일러값에 따라 달라짐
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //한번 범위로 진입하면 바로 추적모드로 바뀜->범위 벗어나면?
        if (collision.gameObject.tag == "Player")
        {
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌(추적모드=1)
            ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 1;
        }
    }
}
