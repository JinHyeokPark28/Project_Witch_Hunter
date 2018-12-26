using UnityEngine;
using System.Collections;

public class MonsterAI_Moving : MonoBehaviour
{
    //리스폰 포인트에 달린 몬스터매니저 스크립트 로부터 정보 받으면 getinfo=true
    //움직일 수 있는, 일반 몬스터 스크립트
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄

    public string Name;
    public int HP;  //몬스터 체력
    public int attack;  //몬스터 공격력,함정도 있음
    public int index;   //몬스터 인덱스
    public bool _CheckMode;		// true면 공격
    public float NormalSpeed = 2f;      //정찰 모드 시 속도
    public float _AttackSpeed = 4f;      // 공격 속도(때릴 때 속도):근접,이동가능
    public float _CheckTime = 0;    //공격 쿨타임 재는 시간->원거리,고정에 쓰임
    public float _CheckDelay = 2;   //쿨타임 한계시간
    public bool isDead; //죽으면 true(죽으면 코인줌)->마녀 죽으면 다시 리젠 못함
    public int Stage_Location;  //몬스터 출현 스테이지 
    public int _isMonstate = 0;                // 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드 3:죽음(IsDead==true)
    //고정형:0=경비모드,1:공격모드
    private bool Recon=false;  //몬스터가 움직일 수 있는 타입인지 true면 정찰(false-고정형이면 무조건 원거리)
    private Animator _Anim;
    private int Coin;
    public int MonsterType; //0이면 일반(근접공격-근거리) 1이면 사격-원거리 2면 강화형(HP 더 커짐) 3-자폭
    public bool GetInfo;    //MonsterManager로부터 정보 받으면 true
    public bool isLeft = true; //왼쪽으로 가는 중이면 true
    public float HurtTime;  //무적인 시간(Time.deltaTime으로 더해주고, 만약이게 0보다 작으면 Hurt=false
    public float WholeHurtTime = 0.5f;  //무적인 시간 전체
    public bool Hurt;   //플레이어에게 맞으면 잠시동안 스프라이트 깜빡이도록 함. 이때 Hurt==true이고 이 동안은 플레이어에게
                        //공격받아도 일시적으로 무적 상태이다
    private bool DeadStart = false;
    private float StartXPos;    //시작할 때 position의 X좌표 값
    private bool ResetStartXPos = false;    //추적모드->일반상태로 되돌아왔을 때 해야 할것
                                            //추적모드->일반상태시 true로 바뀌어서 새로 StartXPos잡도록 함. 그 뒤엔 다시 false
    private GameObject Player;
    private Rigidbody2D rigid;
    #region 자식 오브젝트
    public GameObject SearchArea;   //플레이어 탐색하는 자식오브젝트 ChasingArea담을 오브젝트
    public GameObject AttackArea;   //플레이어와 접촉하면 몬스터가 공격하도록 하는 자식오브젝트 AttackingArea담을 오브젝
                                    //고정형 몬스터or함정인 경우 AttackArea 버림:(고정형0(평소><->1(발견&공격)상태만 왔다갔다함)
    #endregion
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rigid = gameObject.GetComponent<Rigidbody2D>();
        StartXPos = gameObject.transform.position.x;
        HurtTime = WholeHurtTime;
        _Anim = GetComponent<Animator>();

    }
    #region 죽을 때 시작되는 코루틴
    IEnumerator Dead()
    {
        while (true)
        {
            DeadStart = true;
            _Anim.SetBool("IsDead", true);
            _Anim.SetBool("Attack", false);
          //  _Anim.SetBool("Walk", false);
            yield return new WaitForSeconds(5);
            #region 죽을 때 떨어지는 코인 갯수 정하기
            if (Coin == 0)
            {
                Coin = Random.Range(1, 21);
                //w_Coin = Random.Range(100, 501);
            }
            #endregion
            Destroy(gameObject);
        }
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        if (GetInfo == true)
        {
        #region HP>0 이상인 상태
            if (HP>0)
            {
                
                #region 정찰모드(_isMonState==0)
                if (_isMonstate == 0)
                {
                    _Anim.SetBool("Walk", true);
                    _Anim.SetBool("Attack", false);
                    _Anim.SetBool("IsDead", false);
                    Moving();
                    #region 방향 지정함수 시작한 지점.x좌표보다 더 왼쪽으로 5 갔으면 방향 바꾸기
                    if (isLeft == true)
                    {
                        if (transform.position.x < StartXPos - 3)
                        {
                            isLeft = false;
                        }
                    }
                    else
                    //isLeft==false;    //오른쪽으로 가던 중
                    {
                        if (transform.position.x > StartXPos)
                        {
                            isLeft = true;
                        }
                    }
                    #endregion
                    #region 시작지점 정해주는 함수
                    if (ResetStartXPos == true)
                    {
                        print("RESET_X_POSITION");
                        StartXPos = transform.position.x;
                        ResetStartXPos = false;
                    }
                    #endregion
                }
                #endregion
                #region 추적 모드
                //정찰 함수 주기->왔다갔다 해야하니까 코루틴으로 줘야할듯?
                else if (_isMonstate == 1)  //플레이어 발견->추적모드&&추적 범위 콜라이더와 플레이어 충돌
                {
                    _Anim.SetBool("IsDead", false);
                    //여기서 플레이어 방향 못잡음
                    _Anim.SetBool("Walk", true);
                    _Anim.SetBool("Attack", false);
                    Chasing();
                }
                #endregion
                #region 공격 모드
                else if (_isMonstate == 2)  //공격 모드. 
                {
                    //Check();
                    _Anim.SetBool("Walk", false);
                    _Anim.SetBool("Attack", true);
                }
                #endregion
            }
            #endregion
        #region HP<=0이라면
            else if (HP <= 0)
            {
                isDead = true;
                _isMonstate = 3;
            }
            if (isDead == true)
            {
                if (DeadStart == false)
                {
                    //죽으면
                    StartCoroutine(Dead());
                    //animation.die불러오기
                }
            }
        #endregion
        }
    }
    #region 근접, 이동가능!(Recon=true) 몬스터의 이동 함수
    public void Moving()
    {
        if (_isMonstate == 0)   //다른 상태로 접어들었을때 꺼지기 위해서
        {
            if (isLeft == true)
            {
                //기본 왼쪽 형태
                //time.deltaTime으로 해서 더 느려짐
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                //rigid.velocity = Vector2.left * NormalSpeed * Time.deltaTime;
            }
        }
        else
        {
            //원본에는 주석 해제
           // rigid.velocity = Vector2.zero;
        }
    }
    #endregion

    #region 플레이어 추적 함수(_isMonState==1)
    void Chasing()
    {
        if (_isMonstate == 1 && isDead == false)
        {
            //쫓는 함수->movetowards로 하니까 갑자기 빨라짐
            if (Player.transform.position.x < transform.position.x)
            {
                //플레이어가 몬스터 왼편에 있을 때
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
    #region 몬스터가 다칠때 깜빡이는 함수(다치는건 트리거에 걸어놓음)
    void GetHurt()
    {
        if (Hurt == true)
        {
            //그냥 여기서 if문으로 다 써버리니까 조건 중복되서 들어감
            if (HurtTime <= 0f)
            {
                HurtTime = WholeHurtTime;
                Hurt = false;
            }
        }
        else if (Hurt == false)
        {
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            _CheckMode = true;  //true면 공격
            if (Player.GetComponent<PlayerController>().touched == false)
            {
                Player.GetComponent<PlayerController>().touched = true;
                //나중에 여기서 플레이어 hp깎는 것도 만들기
            }
        }
        if ((col.gameObject.transform.tag == "Sword") || (col.gameObject.transform.tag == "Bullet"))        //칼이나 총알에 맞으면
        {
            if (Player.GetComponent<PlayerController>().IsAttacking == true)
            {
                print("M+_HURT");
                GetHurt();
                HP -= 10;
                print("HP:" + HP);
                //나중에 플레이어가 착용한 무기의 공격력 적용하도록
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            _CheckMode = false;
            // 공격 받았을 때 데미지 처리.
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {

    }
}
