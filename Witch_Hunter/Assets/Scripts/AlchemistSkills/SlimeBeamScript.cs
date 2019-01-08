using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBeamScript : MonoBehaviour {
    private Vector2 PlayerPosition;
    public float Speed = 1f;
    //연금술 마녀 옆에 붙어있는 슬라임이 소환하는 투사체에 붙어있는 스크립트
    //리스폰 되자마자 플레이어 향해서 날아감&rotation바뀌어야 함
    public int Attack = 5;
	// Use this for initialization
    IEnumerator SelfDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            Destroy(this.gameObject);
        }
        //씬 벗어나면 죽도록
    }
	void Start () {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //플레이어 향해 쭉 날아가도록. 리스폰 된 순간 플레이어 좌표 받아오기
        StartCoroutine(SelfDestroy());
        //이 좌표에서 플레이어 포지션 좌표방향으로 회전하는 명령어
        transform.rotation=Quaternion.Euler(0, 0, -Mathf.Atan2(PlayerPosition.x - transform.position.x, PlayerPosition.y - transform.position.y) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector2.up * Time.deltaTime * Speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //플레이어와 부딪힌다면
            //플레이어 HP 깎는 스크립트
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<PlayerController>().HP -= Attack;
           
        }
    }
}
