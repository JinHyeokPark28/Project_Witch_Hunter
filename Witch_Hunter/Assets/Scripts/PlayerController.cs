using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region 변수 목록
    public float Speed = 1f;
    public bool CanJump;
    //true면 점프 가능, false면 점프 불가능
    public bool UpAttack;
    //true면 위쪽으로 공격하고 있을 때(방향키 누르고 있을 때)
    public bool DownState;
    //아래 키 눌렀을 때 true
    public float HurtFlashTime;
    //적에 부딪히면 플레이어의 스프라이트가 깜빡거리는 시간
    public float HowLongFlash;
    //깜빡거린 시간
    public bool FlashActive;
    //true이면 플레이어의 스프라이트가 깜빡거린다
    public bool touched;
    //IEnumerator 함수 업데이트에서 여러번 호출되는 것을 방지, true일 때 while문 실행시키고 바로 false로 전환
    public bool InfoScreenOn;
    //true면 정보창 켜짐
    public bool MapOn;
    //true면 지도 켜짐
    public bool GamePause;
    //false면 게임 진행, true면 게임 멈춤


    // Use this for initialization
    #endregion
    void Start() {

    }
    IEnumerator Flash()
    {
        while (FlashActive == true)
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0f);
            yield return new WaitForSeconds(0.25f);
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Update is called once per frame
    void Update() {
        UpAttack = false;
        DownState = false;
        PlayerMove();
        InfoMapManager();
        NumberKeyManager();
        TouchEnemy();
        //공격,세이브,상호작용키
        if (Input.GetKeyDown(KeyCode.A))
        {
            //기본은 공격(else로 처리)

            //npc만났을 경우

            //보물상자

            //세이브포인트



        }

        //무기 변경
        if (Input.GetKeyDown(KeyCode.D))
        {

        }
        //속성 변경키
        if (Input.GetKeyDown(KeyCode.S))
        {

        }

        //결정 엔터키
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (InfoScreenOn == true)
            {

            }
        }
        
     }

    #region 플레이어 움직임
    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
        }
        if (DownState == false && Input.GetKey(KeyCode.UpArrow))
        {
            UpAttack = true;
        }
        else if (UpAttack == false && Input.GetKey(KeyCode.DownArrow))
        {
            DownState = true;
        }
        //점프키
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump == true && Time.timeScale == 1)
            {
                CanJump = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, Speed * 1.5f);
            }
            else
            {
            }
        }
        if (InfoScreenOn == true)
        {
            Time.timeScale = 0;
            GameObject.Find("Canvas").transform.Find("InfoBackGround").gameObject.SetActive(true);
        }
        else if (InfoScreenOn == false)
        {
            Time.timeScale = 1;
            GameObject.Find("Canvas").transform.Find("InfoBackGround").gameObject.SetActive(false);
        }
    }
    #endregion
    #region 지도 켜고 끄기
    void InfoMapManager()
    {
        //MoveObjMgr로 지도창 관리 
        if (CanJump == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (MapOn == true)
                {
                    MapOn = false;
                }
                else
                {

                    MapOn = true;
                }
            }
          
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (InfoScreenOn == false)
                {

                    InfoScreenOn = true;
                }
                else
                {
                    InfoScreenOn = false;
                }
            }

        }
        if (MapOn == true)
        {
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(true);

        }
        else if (MapOn == false)
        {
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(false);
        }
    }
    #endregion
    #region CanJump관리
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            //바닥에 부딪히면
            CanJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //바닥에 부딪히면
            CanJump = false;
        }
    }
    #endregion
    #region 숫자키관리
    void NumberKeyManager()
    {
        //숫자 키 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("2");

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("3");

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("4");

        }

    }
    #endregion
    #region 적과 접촉
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            touched = true;
            FlashActive = true;
        }
    }
    #endregion
    #region 다쳤을 때
    void TouchEnemy()
    {
       
        if (FlashActive == true)
        {
            HowLongFlash += Time.deltaTime;
            if (touched == true)
            {
                touched = false;
                StartCoroutine(Flash());
            }
            if (HowLongFlash >= HurtFlashTime)
            {
                StopCoroutine(Flash());
                FlashActive = false;
                HowLongFlash = 0;
            }
        }
    }
    #endregion

}
