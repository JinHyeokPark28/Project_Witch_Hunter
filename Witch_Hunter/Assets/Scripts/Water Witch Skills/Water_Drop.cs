using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Water_Drop : MonoBehaviour
{
    //물방울 침수
    //물의 마녀가 소환하는 물방울에 달린 스크립트
    public bool HitGround = false;
    public Sprite HitSprite;
    //부딪히면 바꿀 스크립트
    private bool isStart = false;
   // public Vector2 CenterVector;
    private Vector2 StopVector;
    private GameObject Player;
    //중심과의 거리 최소 간격 5; 
    //플레이어 현재 위치 생각하면서 이미 기존에 나와있던 물방울들 위치 고려해야함
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        //if (GameObject.FindGameObjectsWithTag("WitchBullet").Length > 0)
        //{
 
        //    //만약 자기나오기 전의 다른 오브젝트가 존재한다면
        //}
        //else
        //{
        //    //만약 자기나오기 전의 다른 오브젝트가 존재하지 않는다면->자기가 첫번째라면
        //    //신경안쓰고 그냥 나오면 됨->맵 가로 길이도 생각해야함. 물방울 놓아진 곳이 맵 넘으면 안되게 고려해야함->Bounds 이용
        //    //겹치게 해야 될듯??플레이어 양쪽에 하나씩 물방울이 있는데 이 물방울 사이 간격이 물방울 하나가 들어가기 힘든 길이라면??
        //    this.transform.position = new Vector2(Player.transform.position.x, transform.position.y);
        //}
    }
    IEnumerator ChangeSprite()
    {
        isStart = true;
        StopVector = this.gameObject.transform.position;
        yield return new WaitForSeconds(0.1f);
        //while (true)
        //    if (HitGround == true)->x터짐
        //if로 해야하나? 
        //while(hitGround==true)로 하니 안들어감
        if (this.gameObject.GetComponent<PolygonCollider2D>() != null)
        {
            Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
        }
        GetComponent<SpriteRenderer>().sprite = HitSprite;
        this.gameObject.AddComponent<PolygonCollider2D>();
        //HitSprite_2 = @"/Resources/RuffImages/Metal_Slime_Temporary/Slime_2(56_75).png"as Sprite;
        //sr.sprite = "/Resources/RuffImages/Metal_Slime_Temporary/Slime_2(56_75).png" as sr.sprite;
        //나중에 이렇게 바꾸기
        //Destroy(this.gameObject);
     
    }

    // Update is called once per frame
    void Update()
    {
        //CenterVector = GetComponent<SpriteRenderer>().sprite.bounds.center;
        if (HitGround == true)
        {
            if (isStart == false)
            {
                StartCoroutine(ChangeSprite());
            }
            if (isStart == true)
            {
                transform.position = StopVector;
            }
            //스프라이트 처리 시작
            //나중에 할것->trigger인 상태에서 pivot이 그라운드 좌표와 오차 범위 내의 겹침->스프라이트 변경
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            //같은 좌표에 있으면 콜라이더 켜짐->콜라이더끼리 부딪힘->하나가 위로 올라감
            //바닥이랑 부딪힌다면->스프라이트 처리
            //중간에 떠있는 바닥은 어떻게??
            //바닥이랑 부딪히면 멈추도록
            HitGround = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Sword"|| collision.gameObject.tag == "Bullet")
        {
            if (Player.GetComponent<PlayerController>().IsAttacking == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
