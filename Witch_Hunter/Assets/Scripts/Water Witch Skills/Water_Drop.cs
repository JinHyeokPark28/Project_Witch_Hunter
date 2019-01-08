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
    private GameObject MyWitch;
    public bool GotoWitch = false;  //true면 마녀한테로 가서 사라짐
    private float MovingSpeed = .5f;   //마녀한테로 갈때 움직이는 속도
    // Use this for initialization
    void Start()
    {
        MyWitch = GameObject.FindGameObjectWithTag("Witch");
        Player = GameObject.FindGameObjectWithTag("Player");
       
    }
    IEnumerator ChangeSprite()
    {
        isStart = true;
        StopVector = this.gameObject.transform.position;
        yield return new WaitForSeconds(0.1f);
        if (this.gameObject.GetComponent<PolygonCollider2D>() != null)
        {
            Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
        }
        GetComponent<SpriteRenderer>().sprite = HitSprite;
        this.gameObject.AddComponent<PolygonCollider2D>();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (HitGround == true)
        {
            if (isStart == false)
            {
                StartCoroutine(ChangeSprite());
            }
            if (isStart == true)
            {
                if (GotoWitch == false)
                {
                    transform.position = StopVector;
                }
                if (GotoWitch == true)
                {
                    Destroy(this.gameObject.GetComponent<Rigidbody2D>());
                    GetComponent<PolygonCollider2D>().isTrigger = true;
                    transform.position = Vector2.MoveTowards(this.gameObject.transform.position, MyWitch.transform.position, MovingSpeed);

                    if ((transform.position.x > MyWitch.transform.position.x - 2) && (transform.position.x < MyWitch.transform.position.x + 2))
                    {
                        if ((transform.position.y > MyWitch.transform.position.y - 2) && (transform.position.y < MyWitch.transform.position.y + 2))
                        {
                           MyWitch.GetComponent<WitchClass>().DROP_NUMBER += 1;
                            Destroy(this.gameObject);
                        }

                    }

                }
            }
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
