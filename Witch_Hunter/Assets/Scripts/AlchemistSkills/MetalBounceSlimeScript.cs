using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MetalBounceSlimeScript : MonoBehaviour {
    public bool HitGround = false;
    public Sprite HitSprite;
    private bool isStart = false;
    // Use this for initialization
    void Start () {
        //만약 부딪히기 전에 미리 바꾸고 싶다->그럼 자식 콜라이더 하나 트리거로 만들어서 바닥과 가까워지면 이미지 바꾸도록 하기

	}
    IEnumerator ChangeSprite()
    {
        //while (true)
        //    if (HitGround == true)->x터짐
        //if로 해야하나?
        //while(hitGround==true)로 하니 안들어감
        while (true)
        {
            isStart = true;
            Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
            GetComponent<SpriteRenderer>().sprite = HitSprite;
            this.gameObject.AddComponent<PolygonCollider2D>();
            //HitSprite_2 = @"/Resources/RuffImages/Metal_Slime_Temporary/Slime_2(56_75).png"as Sprite;
            //sr.sprite = "/Resources/RuffImages/Metal_Slime_Temporary/Slime_2(56_75).png" as sr.sprite;
            //나중에 이렇게 바꾸기
            yield return new WaitForSeconds(.5f);
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (HitGround == true)
        {
            if (isStart == false)
            {
                StartCoroutine(ChangeSprite());
            }
            //스프라이트 처리 시작
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            //바닥이랑 부딪힌다면->스프라이트 처리
            //중간에 떠있는 바닥은 어떻게??
            //바닥이랑 부딪히면 멈추도록
            HitGround = true;
        }
    }
}
