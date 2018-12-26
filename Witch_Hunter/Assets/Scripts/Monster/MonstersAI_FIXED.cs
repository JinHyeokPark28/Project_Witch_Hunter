using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersAI_FIXED : MonoBehaviour
{
    //고정형 몬스터
    //MonsterManager가 몬스터 생성하면서 각 몬스터에 맞는 csv파일 파싱해서 공격력, hp,몬스터 타입(정찰,고정,사격,근접..),속도 등 나눠줌
    #region Private Variable
    private GameManager _GameManager;
    private GameObject Player;
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
    public bool IsBoss; //몬스터 마녀인지 아닌지->나중에 삭제 가능성
    #region 일반 몬스터 특별 속성
    public int _isMonstate = 0;                // 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드 3:죽음(IsDead==true)
    //고정형:0=경비모드,1:공격모드
    public bool Recon;  //몬스터가 움직일 수 있는 타입인지 true면 정찰(false-고정형이면 무조건 원거리)
    private Animator _Anim;
    public int Coin;
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
    #endregion
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄
    [SerializeField]
    public GameObject Target;
    #endregion
    #region 자식 오브젝트
    public GameObject SearchArea;   //플레이어 탐색하는 자식오브젝트 ChasingArea담을 오브젝트
    public GameObject AttackArea;   //플레이어와 접촉하면 몬스터가 공격하도록 하는 자식오브젝트 AttackingArea담을 오브젝
    //고정형 몬스터or함정인 경우 AttackArea 버림:(고정형0(평소><->1(발견&공격)상태만 왔다갔다함)
    #endregion

    #region 컴포넌트 변수
    public Rigidbody2D rigid;
    public SpriteRenderer SR;
    #endregion

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartXPos = gameObject.transform.position.x;
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        SR = gameObject.GetComponent<SpriteRenderer>();
        Target = GameObject.FindGameObjectWithTag("Player");
        _GameManager = GameManager.GetGameManager;
        HurtTime = WholeHurtTime;
        //몬스터 초기 _isMonState 값 줘야함
        _Anim = GetComponent<Animator>();
    }
    //정찰 모드 상태일때만
    IEnumerator Dead()
    {
        while (true)
        {
            DeadStart = true;
            _Anim.SetBool("Dead", true);
            _Anim.SetBool("Attack", false);
            _Anim.SetBool("Walk", true);
            if (Coin == 0)
            {
                Coin = Random.Range(1, 21);
                //w_Coin = Random.Range(100, 501);
            }
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Hurt == true)
        {
            HurtTime -= Time.deltaTime;
        }
        if (GetInfo == true)
        {
            #region HP>0일 경우
            if (HP > 0&&isDead==false)
            {
                #region 고정형 몬스터인 경우
                if (Recon == false)
                {
                    if (_isMonstate == 0)
                    {   //평소 모드(플레이어오나 안오나 살펴보는 모드)  ->좌우 살피도록
                        _Anim.SetBool("Attack", false);
                        Watching(); //고정형 몬스터가 플레이어 오나 안오나 살피는 함수
                    }
                    else if (_isMonstate == 1)
                    {
                        NotMovingMonsterAttack();
                        _Anim.SetBool("Attack", true);
                        _Anim.SetTrigger("SET");
                        print("ATTACK");
                        //플레이어 발견모드(이때 조준&&공격)
                    }
                    else if (_isMonstate == 3)
                    {
                        //죽었으면
                    }
                }
                #endregion
            }
            #endregion
        }
        #region HP>0일 경우
        if (HP <= 0)
        {
            print("DEAD");
            _isMonstate = 3;
            isDead = true;
        }
        #endregion

        #region 죽는 거 처리하는 부분
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

    #region 근접, 이동가능!(Recon=true) 몬스터의 이동 함수
    public void Moving()
    {
        if (_isMonstate == 0)   //다른 상태로 접어들었을때 꺼지기 위해서
        {
            if (isLeft == true)
            {
                //  isMonstate 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드
                //기본 왼쪽 형태
                //time.deltaTime으로 해서 더 느려짐
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                //transform.translate로 하니까 좌표이동함수라서 통과해버림->아님, 좌표 반전될때 자식 오브젝트가 상대적으로 
                // x=0이 아니라서 그럼
                // rigid.velocity = Vector2.left * NormalSpeed * Time.deltaTime;
                //velocity=한번에 계속 쭉 주는듯
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                //rigid.velocity = Vector2.left * NormalSpeed * Time.deltaTime;
            }
            //코루틴 안에서 또 while 붙이니까 0->1로 가는 과정에서 멈추는듯?(코루틴도 계속 돌아감, while도 계속 돌아감)
            // transform.Translate(Vector2.left * 1f * Time.deltaTime);  업데이트에서는 매 프레임마다 명령문이 실행되니까 매 프레임마다 
            //Transform.translate은 transform.position이랑 다를게 없음. 그저 매 프레임마다 실행되다 보니 자연스럽게 움직이는 것처럼 보이는 것뿐
            //case 0->1로 바로 전환 필요!!(현재는 부딪히면 바로 전환되는게 아니라 case 0끝낼때까지 기다리는듯?
            //해결책:코루틴 끄고 다시 시작해야 하는가??
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }
    #endregion
    #region 플레이어를 따라가는 함수
    void Chasing()
    {
        if (_isMonstate == 1 && isDead == false)
        {
            //쫓는 함수->movetowards로 하니까 갑자기 빨라짐
            if (Target.transform.position.x < transform.position.x)
            {
                //플레이어가 몬스터 왼편에 있을 때
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Target.transform.position.x > transform.position.x)
            {
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
    #region 공격 타이밍 체크하는 함수
    private void Check()
    {
        if (Time.time - _CheckTime > _CheckDelay)
        {
            _CheckTime = Time.time;
            Attack();
        }
    }
    #endregion

    #region 이동가능한 몬스터가 공격하는 함수
    void Attack()
    {
        if (Recon == true)
        {

        }
    }
    #endregion
    #region <고정형>이 좌우 살피는 함수
    void Watching()
    {
        if (_isMonstate == 0)
        {
            if (isLeft == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
    #region 몬스터가 다칠때 깜빡이는 함수
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
            // HP -= 10; 
        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            _CheckMode = true;  //true면 공격
            if (Target.GetComponent<PlayerController>().touched == false)
            {
                Target.GetComponent<PlayerController>().touched = true;
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
        if (_CheckMode == true) Check();
        //check():공격 타이밍 체크하는 함수
        if (_CheckMode == false) return;

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
    #region 고정형 원거리 몬스터가 플레이어 발견&공격하는 함수
    public void NotMovingMonsterAttack()
    {

        if (_isMonstate == 1)   //만약 발견&공격이면
        {
            //플레이어붙인 태그를 자동으로 찾게 되어있음
            //쫓는 함수->movetowards로 하니까 갑자기 빨라짐
            if (Target.transform.position.x < transform.position.x)
            {
                //플레이어가 몬스터 왼편에 있을 때
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (Target.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        //고정형 원거리 몬스터 공격 타입
    }
    #endregion

}
