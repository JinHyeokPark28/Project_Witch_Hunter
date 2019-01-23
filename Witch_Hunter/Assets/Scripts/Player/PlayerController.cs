using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anima2D;
public class PlayerController : MonoBehaviour
{
    #region 변수 목록
    public int HP = 50;
    public float Speed = 5;
    public float JumpSpeed;
    public bool CanJump;
    //true면 점프 가능, false면 점프 불가능
    public bool JumpUp;
    //뛰었음&&공중으로 올라가는 상태(rigidbody.velocity.y>0)
    private bool isPlayerHit;
    public bool IsHitAnimActive = false;
   public enum PlayerState{Dead=-10,Idle=0,Hit=1,Run=2,Jump=3,Attack=4};
    public PlayerState NowState = PlayerState.Idle;
    public bool GunShot=false;
    public int SwordDamage = 5;
    public int BulletDagmage = 5;
    //false 면 칼로 공격
    public bool CheckNPC;       // true면 대화창 실행
    public bool CheckEnemy;     // true면 공격 가능
    private bool CheckChest;     // true면 상자 열기
    public bool CheckSave;		// true면 세이브 하기
    private bool ContactingSavePoint = false;
    private Animator _Anim;
    private int SceneNum = -1;
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
    private Rigidbody2D RG;
    private GameObject MySword;
    private GameObject BackSword;
    private List<GameObject> BodyParts = new List<GameObject>();
    #endregion
    #region 눌러진 키 관리
    private bool isAKeyPressed;
    private bool isMoveKeyPressed;
    #endregion
    //플레이어가 다른 씬으로 넘어갈 때 시작 포인트잡아줘야 함
    //플레이어 스크립트에서 시작 포인트 바로 잡도록 하기
    //start문에서 시작하자마자 포인트 잡기&다른 씬으로 넘어가면 거기에 있는 포인트 자동으로 잡아주기
    #region 다치면 스프라이트 깜빡이는 코루틴
    IEnumerator HitSpriteFlashing()
    {
        while(NowState==PlayerState.Hit)
        {
            print("hit_FLASH");
            for(int i = 0; i < BodyParts.Count; i++)
            {
                BodyParts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 0, 0);
            }
            yield return new WaitForSeconds(0.05f);

            for (int i = 0; i < BodyParts.Count; i++)
            {
                BodyParts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 1, 1);
            }
            yield return new WaitForSeconds(0.05f);
            print("HIT_END");
        }
    }
    #endregion
    void Start()
    {
        if (MySword == null)
        {
            MySword = this.gameObject.transform.Find("TestSword").gameObject;
            print(MySword);
        }
        if (BackSword == null)
        {
            BackSword = this.gameObject.transform.Find("Sword").gameObject;
        }
        BodyParts.Clear();
        BodyParts.Add(transform.Find("body").gameObject);
        BodyParts.Add(transform.Find("head").gameObject);
        BodyParts.Add(transform.Find("leftarm").gameObject);
        BodyParts.Add(transform.Find("rightarm").gameObject);
        BodyParts.Add(transform.Find("warm").gameObject);
        BodyParts.Add(transform.Find("left").gameObject);
        BodyParts.Add(transform.Find("right").gameObject);
        BodyParts.Add(transform.Find("Sword").gameObject);
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
        RG = this.gameObject.GetComponent<Rigidbody2D>();
        if (GameObject.FindGameObjectWithTag("SceneStartPoint") != null)
        {
            //처음 시작할 포인트 따로, 씬에서 되돌아올 포인트 따로 한 씬에 최소 2개 있어야함
            //A<->B로 왔다갔다 하고, 처음 시작할 때 A에서 시작한다고 하면
            //A에서 시작할 포인트 하나, B에서 시작할 포인트 하나, B에서 A로 갈때 다시 A에서 시작할 포인트 하나
            gameObject.transform.position = GameObject.FindGameObjectWithTag("SceneStartPoint").transform.position;
        }
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckingPlayerState();
        if (HP <= 0)
        {
            _Anim.SetBool("Dead", true);
        }
        else
        {
            _Anim.SetBool("Dead", false);
            ChangeWeapon();
            ManagingSword();
            NumberKeyManager();
            VectorMovingInScenes();
            if (isPlayerHit == false)
                //&& _Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Hit") == false)
            {
                _Anim.SetInteger("State", 1);
                JumpManaging();
                PlayerMove();
                PressingAKey();
            }
            if (isPlayerHit == true)
            {
                _Anim.SetInteger("State", 0);
                if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Hit") == true)
                {
                    StartCoroutine(HitSpriteFlashing());
                    _Anim.SetTrigger("Auto");
                    _Anim.SetInteger("State", 1);
                    print("hit");
                    isPlayerHit = false;
                }
            }
           
           
        
            if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Hit") == true)
            {
                IsHitAnimActive = true;
            }
            if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Hit") == false)
            {
                IsHitAnimActive = false;
            }
        }
    }
    #region 칼 움직임 활성화 관리
    //게임 시작할 때 실제 몬스터한테 닿는 칼 비활성화 시켜줘야 공격할 때만 활성화됨
    void ManagingSword()
    {
        if (NowState != PlayerState.Attack)
        {
            if (MySword.activeInHierarchy == true)
            {
                print("FALSE");
                MySword.SetActive(false);
            }
            if (BackSword.activeInHierarchy == false)
            {
                BackSword.SetActive(true);
            }
        }
        else if (NowState == PlayerState.Attack)
        {
            if(GunShot == false)
            {
                BackSword.SetActive(false);
                //검으로 공격할 때는 캐릭터 등에 있는 검 안보이고, 휘두르는 검만 보이도록
                if (MySword.activeInHierarchy == false)
                {
                    print("dd");
                    MySword.SetActive(true);
                }
            }
        }
    }
    #endregion
    #region 플레이어 상태 구분
    void CheckingPlayerState()
    {
        if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Die") == true)
        {
            NowState = PlayerState.Dead;
        }
        else if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Hit") == true)
        {
            NowState = PlayerState.Hit;
        }
        else if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack") == true)
        {
            //들어감
            //공격 애니메이션 끝나면 자동적으로 isAttacking을 false로 만들어줌
            NowState = PlayerState.Attack;
        }
        else if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Run") == true)
        {
            NowState = PlayerState.Run;
        }
        else if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump") == true)
        {
            NowState = PlayerState.Jump;
        }
        else
        {
            NowState = PlayerState.Idle;
        }
    }
    #endregion
    #region A키 눌렀을 때(공격&NPC상호작용)
    void PressingAKey()
    {
        if (CheckChest == false && CheckNPC == false && ContactingSavePoint == false)
        {
            //보물상자와 접촉 안했단 조건 하에서
            if (Input.GetKeyDown(KeyCode.A))
            {
                _Anim.SetInteger("State", 4);
                //세이브포인트
            }
        }
    }
    #endregion
    #region 플레이어 움직임
    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 스프라이트 애니메이션 넣기
            _Anim.SetInteger("State", 2);
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _Anim.SetInteger("State", 2);
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //점프키
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump == true && Time.timeScale == 1)
            {
                _Anim.SetInteger("State", 3);
                print("jump");
                CanJump = false;
                RG.velocity = new Vector2(0, JumpSpeed);
                //GetComponent<Rigidbody2D>().velocity = new Vector2(0, JumpSpeed);
            }
            else
            {
            }
        }

    }
    #endregion
    #region 뛰었을 때 발판 그대로 통과하도록하는 함수
    void JumpManaging()
    {
        if (CanJump == false && RG.velocity.y > 0f)
        {
            //뛰고 있는 중&&위로 올라갈 때는 공중에 있는 발판에 부딪혀도 레이어 충돌하지 않도록       
            JumpUp = true;

        }
        //발판 태그은 AirGround로 맞춰줘야함
        else
        {
            JumpUp = false;
        }
        if (JumpUp == false && this.gameObject.GetComponent<BoxCollider2D>().isTrigger == true)
        {
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if (JumpUp == true && this.gameObject.GetComponent<BoxCollider2D>().isTrigger == false)
        {
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
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
    #region CanJump관리 및 OnCollision ,OnTrigger관리
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어와 적이 부딪힐 시 플레이어가 밀려나도록 하는 함수
        if ((collision.gameObject.tag == "Enemy") &&
            (collision.gameObject.name != "ChasingArea") && (collision.gameObject.name != "AttackingArea")&&HP>0)
        {
            
            if ((collision.gameObject.GetComponent<MonsterAI_Moving>() != null)&& 
                (collision.gameObject.GetComponent<MonsterAI_Moving>().NowMonstate!=MonsterAI_Moving._IsMonstate.DeadState))
            {
                if (isPlayerHit == false)
                {
                    HP -= collision.gameObject.GetComponent<MonsterAI_Moving>().attack;
                    isPlayerHit = true;
                }
                print("P_HP:" + HP);
            }
            if ((collision.gameObject.GetComponent<MonstersAI_FIXED>() != null)&&
                (collision.gameObject.GetComponent<MonstersAI_FIXED>().NowMonstate!=MonstersAI_FIXED._IsMonstate.DeadState))
            {
                if (isPlayerHit == false)
                {
                    HP -= collision.gameObject.GetComponent<MonstersAI_FIXED>().attack;
                    isPlayerHit = true;
                }
                print("P_HP:" + HP);
            }
            if (collision.gameObject.transform.position.x < gameObject.transform.position.x)
            {

                RG.velocity = new Vector2(2.5f, 2.5f);
            }
            else if (collision.gameObject.transform.position.x >= gameObject.transform.position.x)
            {
                RG.velocity = new Vector2(-2.5f, 2.5f);
            }
        }
        if (collision.gameObject.tag == "WitchBullet")
        {
            if (collision.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                RG.velocity = new Vector2(2.5f, 2.5f);
            }
            else if (collision.gameObject.transform.position.x >= gameObject.transform.position.x)
            {
                RG.velocity = new Vector2(-2.5f, 2.5f);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "AirGround")
        {
            //공중에 뜨는 상태면
            CanJump = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WitchBullet")
        {
            if (isPlayerHit == false)
            {
                isPlayerHit = true;
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
            if (collision.gameObject.GetComponent<TreasureBox>().isOpen == false)
            {
                CheckChest = true;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    collision.gameObject.GetComponent<TreasureBox>().isOpen = true;
                }
            }
        }
        //Rigidbody2d기본은 멈추면 작동안해서 Sleeping모드로 들어감. 그래서 오브젝트가 움직이지 않으면 Rigidbody2d에 관련된 스크립트,함수들도 작동안함->해결법:rigidbody의 SleepingMode를 NeverSleep로 켜줘야함

        //보물상자와 충돌하지 않은 상태라면
        if (collision.gameObject.tag == "NPC")
        {

            CheckNPC = true;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (CheckChest == false)
                {
                    //NPC 처리
                }
            }
        }
        if (collision.gameObject.tag == "PlayerSavePoint")
        {
            ContactingSavePoint = true;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (CheckChest == false && CheckNPC == false)
                {
                    //NPC 처리
                    Save();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPCSymbol")
        {
            GameObject.Find("NPC").transform.Find("NPCText").gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "NPC")
        {
            CheckNPC = false;
        }
        if (collision.gameObject.tag == "TreasureBox")
        {
            CheckChest = false;
        }
        if (collision.gameObject.tag == "PlayerSavePoint")
        {
            ContactingSavePoint = false;
        }
    }

    #endregion


    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GunShot = !GunShot;
        }
    }
  
    #region 저장하는 함수
    void Save()
    {
        //아이템은 맨 마지막에 1이상이면 넣도록
        //DBManager에서 다 저장되있어서 따로 csv할 필요 없음
        //위치(position,씬넘버),보스 죽었는지 안죽었는지, 골드,죽인 몬스터 수,플레이 타임, 인벤토리 창, 장비창
    }
    #endregion
    #region 새로운 씬 진입시 플레이어 시작 좌표 변경하는 함수
    void VectorMovingInScenes()
    {

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
    }
    #endregion
}
