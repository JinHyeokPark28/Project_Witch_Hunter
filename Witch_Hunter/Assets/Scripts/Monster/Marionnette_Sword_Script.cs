using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본 마리오네트의 칼에 붙어 있는 스크립트
public class Marionnette_Sword_Script : MonoBehaviour {
    private GameObject Marionnette;
    private GameObject Player;
	// Use this for initialization
	void Start () {
        Marionnette = this.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        print("Mar:" + Marionnette);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"&&Marionnette.GetComponent<MonsterAI_Moving>().NowMonstate==MonsterAI_Moving._IsMonstate.AttackState)
        {
            if (Player.GetComponent<PlayerController>().NowState != PlayerController.PlayerState.Hit)
            {
                Player.GetComponent<PlayerController>().isPlayerHit = true;
                Player.GetComponent<PlayerController>().HP -= Marionnette.GetComponent<MonsterAI_Moving>().attack;
                //현재 맞은 상태가 아니라면
            }
        }
    }
}
