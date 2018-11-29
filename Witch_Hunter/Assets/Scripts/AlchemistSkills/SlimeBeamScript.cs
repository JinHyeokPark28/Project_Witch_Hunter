using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBeamScript : MonoBehaviour {
    public Vector2 PlayerPosition;
    public Rigidbody2D rg;
    public float Speed = 1f;
    //연금술 마녀 옆에 붙어있는 슬라임이 소환하는 투사체에 붙어있는 스크립트
    //리스폰 되자마자 플레이어 향해서 날아감&rotation바뀌어야 함
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
        rg = this.gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(SelfDestroy());
        //이 좌표에서 플레이어 포지션 좌표방향으로 회전하는 명령어
        transform.rotation=Quaternion.Euler(0, 0, -Mathf.Atan2(PlayerPosition.x - transform.position.x, PlayerPosition.y - transform.position.y) * Mathf.Rad2Deg);
        //http://lhh3520.tistory.com/278
        
    }

    // Update is called once per frame
    void Update () {
        //걍 이거 안하고 플레이어 좌표와 내 좌표 사이의 각 구함
        //rotation구해서 돌리고 계속 움직이도록 하기
        transform.Translate(Vector2.up * Time.deltaTime * Speed);
        //rg.velocity = transform.TransformDirection(transform.InverseTransformDirection(Vector2.right));
        //rg.velocity = Vector2.up * Time.deltaTime * 50;
        //rigidbody에서 velocity로 움직이게 하니까 오브젝트의 로테이션 상관없이 world기준으로 움직여버림
        //transform.position = Vector2.MoveTowards(transform.position, PlayerPosition, Speed);
        //여기 안에 moveToWards넣으니까 너무 프레임드랍 심함
        //movetowards는 position형태라 rigidbody=vector2.~~~형태로 못씀
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //플레이어와 부딪힌다면
            //플레이어 HP 깎는 스크립트
            Destroy(this.gameObject);
        }
        //if (collision.gameObject.tag == "Ground")
        //{
        //    //땅 만나면 그대로 통과하기->됌
        //    print("Collide");
        //    Physics2D.IgnoreCollision(collision,this.gameObject.GetComponent<Collider2D>());
        //}
    }
}
