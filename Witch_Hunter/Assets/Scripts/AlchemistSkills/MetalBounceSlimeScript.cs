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
       
	}
    #region 바닥이랑 부딪혔을 때 스프라이트 바꾸기 시작
    IEnumerator ChangeSprite()
    {
        while (true)
        {
            isStart = true;
            Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
            GetComponent<SpriteRenderer>().sprite = HitSprite;
            this.gameObject.AddComponent<PolygonCollider2D>();
           
            yield return new WaitForSeconds(.5f);
            Destroy(this.gameObject);
        }
    }
    #endregion
    // Update is called once per frame
    void Update () {
        //바닥이랑 부딪힌다면
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
            //바닥이랑 부딪히면 멈추도록
            HitGround = true;
        }
    }
}
