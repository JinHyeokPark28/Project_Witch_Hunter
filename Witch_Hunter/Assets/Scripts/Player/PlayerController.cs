using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    #region 변수 목록
    public float Speed = 5;
    public float JumpSpeed;
    public bool CanJump;
    //true면 점프 가능, false면 점프 불가능
    public bool IsAttacking;    //공격하고 있으면 true
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
	public bool CheckNPC;       // true면 대화창 실행
	public bool CheckEnemy;     // true면 공격 가능
	public bool CheckChest;     // true면 상자 열기
	public bool CheckSave;		// true면 세이브 하기
	private Animator _Anim;
    private GameObject CollidedTreasureBox;
    private int SceneNum=-1;
    public bool SceneStart = true;  //씬 전환시 true면 시작 포인트에서 플레이어 시작
                                    //false면 backpoint에서 시작 ->씬 체인저 쪽에서 신호 줌
    private static bool playerExists; //이미 플레이어가 존재하면 true
                                      //선언된 함수 내에서만 접근이 가능하다.
    /*정적(static)변수:
    딱 1회만 초기화되고 프로그램 종료 시까지
    메모리 공간에 존재한다.
    출처: http://1924.tistory.com/30 */
                                    //이 스크립트를 쓰는 모든 오브젝트는 같은 playerExists를 씀
                                    // Use this for initialization
    #endregion
        //플레이어가 다른 씬으로 넘어갈 때 시작 포인트잡아줘야 함
        //플레이어 스크립트에서 시작 포인트 바로 잡도록 하기
        //start문에서 시작하자마자 포인트 잡기&다른 씬으로 넘어가면 거기에 있는 포인트 자동으로 잡아주기

        void Start()
    {
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
            //When loading a new level all objects in the scene are destroyed, then the objects in the new level are loaded
            //그 이전 레벨 씬의 오브젝트들 안없어지도록 만듬
            //근데 이것 하나만 쓰면 계속 오브젝트들 duplicate됌
        }
        else
        {
            Destroy(gameObject);
        }

        if (GameObject.FindGameObjectWithTag("SceneStartPoint") != null)
        {
            //처음 시작할 포인트 따로, 씬에서 되돌아올 포인트 따로 한 씬에 최소 2개 있어야함
            //A<->B로 왔다갔다 하고, 처음 시작할 때 A에서 시작한다고 하면
            //A에서 시작할 포인트 하나, B에서 시작할 포인트 하나, B에서 A로 갈때 다시 A에서 시작할 포인트 하나
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
        if (SceneNum != SceneManager.GetActiveScene().buildIndex)
        {
            //실제로 씬 바뀌었을 때 실행해야함
            if (SceneStart == true)
            {
                //씬 전환되는데 씬 도입부에서 시작해야 한다면(다음 씬에 돌입)

                if (GameObject.FindGameObjectWithTag("SceneStartPoint") != null)
                {
                    //그전 씬에 있던 포지션 잡아버림
                    transform.position = GameObject.FindGameObjectWithTag("SceneStartPoint").transform.position;
                }
                if (Camera.main.GetComponent<PlayerFollower>().Player == null)
                {
                    Camera.main.GetComponent<PlayerFollower>().Player = this.gameObject;
                }
            }
            else
            {
                //씬 전환되는데 씬 마지막 부분에서 시작해야 한다면(이전 씬에 돌입)
                if (GameObject.FindGameObjectWithTag("SceneBackPoint") != null)
                {
                    //그전 씬에 있던 포지션 잡아버림
                    transform.position = GameObject.FindGameObjectWithTag("SceneBackPoint").transform.position;
                }
                if (Camera.main.GetComponent<PlayerFollower>().Player == null)
                {
                    Camera.main.GetComponent<PlayerFollower>().Player = this.gameObject;
                }
            }
            SceneNum = SceneManager.GetActiveScene().buildIndex;
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
            print("Press A");
            _Anim.SetBool("P_Attack", true);
            _Anim.SetBool("IsRun", false);
            IsAttacking = true;
            if (CheckNPC == true)
            {

            }
            //npc만났을 경우
            else if (CheckChest == true)
            {

            }
            //보물상자
            else if (CheckSave == true)
            {

            }
            //세이브포인트
        }
        //공격 애니메이션 끝났을 때 P_Attack=false로 만들어주기
        if (IsAttacking == false)
        {
            _Anim.SetBool("P_Attack", false);
            // print("isattack:" + IsAttacking);
            
        }
        //무기 변경:총<->칼
        if (Input.GetKeyDown(KeyCode.D))
        {
            print("CHANGE_WEAPON");
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
        }
        if (collision.gameObject.tag == "AirGround")
        {
            //공중에 있는 발판에 부딪히면
            //스페이스바 누르고 위로 올라갈때(=중력에 의해 떨어지고 있는 상태가 아닐때) 발판에 부딪히면 발판 그대로 통과하도록 하기
            //->다시 떨어지는 동안은??->떨어지는 동안 발판에 걸친 상태면??->triggerExit??
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
            //GameObject.Find("HP & Coin").GetComponent<PlayerStatUIManager>().HPMinus = true;
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
        if (collision.gameObject.tag == "WitchBullet")
        {
            //마녀가 발사하는or 리스폰하는 투사체에 맞는 경우
            //체력감소
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
