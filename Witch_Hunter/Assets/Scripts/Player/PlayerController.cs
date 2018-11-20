using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region 변수 목록
    public float Speed = 5;
    public float JumpSpeed;
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
    public bool GunShot;
    //false 면 칼로 공격
    public bool SceneChanged = false;   //SceneChanger로부터 true값 받으면 새로 포지션 찾기
	private Animator _Anim;
    private GameObject CollidedTreasureBox;
    // Use this for initialization
    #endregion
    //플레이어가 다른 씬으로 넘어갈 때 시작 포인트잡아줘야 함
    //플레이어 스크립트에서 시작 포인트 바로 잡도록 하기
    //start문에서 시작하자마자 포인트 잡기&다른 씬으로 넘어가면 거기에 있는 포인트 자동으로 잡아주기
    void Start() {
        if (GameObject.FindGameObjectWithTag("SceneStartPoint") != null)
        {
            gameObject.transform.position = GameObject.FindGameObjectWithTag("SceneStartPoint").transform.position;
        }
		_Anim = GetComponent<Animator>();
       
    }
    IEnumerator Flash()
    {
        while (FlashActive == true)
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0f);
            yield return new WaitForSeconds(0.25f);
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
            yield return new WaitForSeconds(0.25f);
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0f);
            yield return new WaitForSeconds(0.25f);
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Update is called once per frame
    void Update() {
        DontDestroyOnLoad(this.gameObject);
        if (SceneChanged == true)
        {
            //실제로 씬 바뀌었을 때 실행해야함
            if (GameObject.FindGameObjectWithTag("SceneStartPoint") != null)
            {
                transform.position = GameObject.FindGameObjectWithTag("SceneStartPoint").transform.position;
                SceneChanged = false;
            }
        }
        UpAttack = false;
        DownState = false;
          PlayerMove();
                ChangeWeapon();
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

                //무기 변경:총<->칼
                if (Input.GetKeyDown(KeyCode.D))
                {

                }
       
      
        

     }

    #region 플레이어 움직임
    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
			// 스프라이트 애니메이션 넣기
			_Anim.SetBool("IsRun", true);
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
		{
			_Anim.SetBool("IsRun", true);
			transform.Translate(Vector2.right * Speed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else
		{
			_Anim.SetBool("IsRun", false);
		}
        //점프키
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump == true && Time.timeScale == 1)
            {
                CanJump = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, JumpSpeed);
            }
            else
            {
            }
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
           // print("CANJUMP:" + CanJump);
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
	#region 숫자키관리(아이템 사용키)
	void NumberKeyManager()
    {
        //숫자 키 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //print("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           // print("2");

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
           // print("3");

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
          //  print("4");

        }

    }
    #endregion
    #region 적과 접촉
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //여기서 꼬였음!!!
            GameObject.Find("HP & Coin").GetComponent<PlayerStatUIManager>().HPMinus = true;
            //hp가 깎이는건 touched=false일 때 깎임!!!!
            if (touched == true)
            {
            }
            else
            {
                touched = true;
                FlashActive = true;
            }
        }
        if (GameObject.Find("NPCSymbol") != null)
        {

            if (collision.gameObject.name == "NPCSymbol")
            {
                GameObject.Find("NPC").transform.Find("NPCText").gameObject.SetActive(true);
            }
        }
      
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //키 습득

        //콜라이더 함수와 getkeydown같이 놓으면 나중에 위험
        //보물상자와 부딪히는 경우(보물상자 여는 경우)
        if (collision.gameObject.transform.tag == "TreasureBox")
        {
            CollidedTreasureBox = collision.gameObject;
            print("CollidedTreasure");
            OpeningTreasureBox();
        } 
        //Rigidbody2d기본은 멈추면 작동안해서 Sleeping모드로 들어감. 그래서 오브젝트가 움직이지 않으면 Rigidbody2d에 관련된 스크립트,함수들도 작동안함->해결법:rigidbody의 SleepingMode를 NeverSleep로 켜줘야함
        if (collision.gameObject.tag == "NPC")
        {
            print("NPC");
           
            if (Input.GetKeyDown(KeyCode.A))
            {
                //if(collision.gameObject.name== "WoundedSoldier")
                //{
                //    병사와 만났을 경우
                //    print("SOLDIER");
                //}
                  //if (GameObject.Find("Canvas").transform.Find("NPCImage").gameObject.activeInHierarchy == false)
                    //{
                    //    GameObject.Find("Canvas").transform.Find("NPCImage").gameObject.SetActive(true);
                    //}
                    // if (GameObject.Find("Canvas").transform.Find("NPCImage").gameObject.activeInHierarchy == true)
                    //{
                    //    GameObject.Find("Canvas").transform.Find("NPCImage").gameObject.SetActive(false);
                    //}
            }
        }
        else if (collision.gameObject.tag == "PlayerSavePoint")
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Save();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPCSymbol")
        {
            GameObject.Find("NPC").transform.Find("NPCText").gameObject.SetActive(false);
        }
    }
    #endregion
    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            GunShot=! GunShot;
        }
    }
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
        else
        {

        }
    }
    #endregion
    #region 보물상자 열 때
    //콜라이더 함수에서 보물상자와 충돌 했을 때 처리하는 함수
    //만약 얘도 에러나면 update 쪽에서 A키 눌렸을 때 true인 bool함수 만들어서 하기!!
    void OpeningTreasureBox()
    {
        print("dd");
        if (Input.GetKeyDown(KeyCode.A))
        {
             print("AA");
            Destroy(CollidedTreasureBox);
            CollidedTreasureBox = null;
        }
    }
    #endregion
    #region 저장하는 함수
    void Save()
    {
        //아이템은 맨 마지막에 1이상이면 넣도록
        //DBManager에서 다 저장되있어서 따로 csv할 필요 없음
        //위치(position,씬넘버),보스 죽었는지 안죽었는지, 골드,죽인 몬스터 수,플레이 타임, 인벤토리 창, 장비창
    }
    #endregion

}
