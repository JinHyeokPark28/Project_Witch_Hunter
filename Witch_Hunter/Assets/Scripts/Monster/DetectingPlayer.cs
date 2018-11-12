using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 ChasingArea오브젝트에 붙은 스크립트
public class DetectingPlayer : MonoBehaviour {
    public GameObject ParentMonster;
	// Use this for initialization
	void Start () {
        //자동으로 부모 오브젝트 찾게 해줌
        ParentMonster = this.gameObject.transform.parent.gameObject;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //부모 오브젝트인 몬스터 오브젝트에게 플레이어 발견했다고 신호줌
            ParentMonster.GetComponent<MonstersAI_FIXED>()._isMonstate = 1;
        }
    }
}
