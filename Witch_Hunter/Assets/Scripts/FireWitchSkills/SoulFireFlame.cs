using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireFlame : MonoBehaviour {
    //불의 마녀 페이즈 1인 소울 파이어로 인해 랜덤으로 생성되는 불꽃에 달린 스크립트
    public GameObject smallFlame;
    //얘가 생성할 불꽃(플레이어를 따라가는거) 
    private GameObject Player;
    private int HitNum=0; //플레이어에게 맞은 횟수 5되면 없어짐
	// Use this for initialization
    IEnumerator MakingSmallFlame()
    {
        while (Player!=null)
        {
            //플레이어의 1초 전 위치를 탐색하는 변수
            Vector2 LatePlayerPos = new Vector2(Player.transform.position.x, Player.transform.position.y);
            yield return new WaitForSeconds(1f);
            if (smallFlame != null)
            {
                Instantiate(smallFlame, this.transform.position, Quaternion.Euler(0, 0, -Mathf.Atan2(LatePlayerPos.x - transform.position.x, LatePlayerPos.y - transform.position.y) * Mathf.Rad2Deg));
            }
        }
    }
    
	void Start () {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        StartCoroutine(MakingSmallFlame());
        if (smallFlame == null)
        {
            print("작은 불꽃이 없습니다");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (HitNum >= 5)
        {
            Destroy(this.gameObject);
        }
        
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Sword") || (collision.gameObject.tag == "Bullet"))
        {
            if (Player.GetComponent<PlayerController>().IsAttacking == true)
            {
                print("HIT");
                HitNum++;
            }
        }
    }
}
