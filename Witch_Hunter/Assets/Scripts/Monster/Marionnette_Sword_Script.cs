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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //마리오네트가 공격상태일 때만 소드 데미지가 들어감
        if (collision.gameObject.tag == "Player"&&
            Marionnette.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack")==true)
        {
            if (Player.GetComponent<PlayerController>().NowState != PlayerController.PlayerState.Hit)
            {
                Player.GetComponent<PlayerController>().isPlayerHit = true;
                Player.GetComponent<PlayerController>().HP -= Marionnette.GetComponent<MonsterAI_Moving>().attack;
                if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
                {

                    collision.GetComponent<Rigidbody2D>().velocity = new Vector2(2.5f, 2.5f);
                }
                else if (collision.gameObject.transform.position.x <= gameObject.transform.position.x)
                {
                    collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-2.5f, 2.5f);
                }
                //현재 맞은 상태가 아니라면
            }
        }
    }
  
}
