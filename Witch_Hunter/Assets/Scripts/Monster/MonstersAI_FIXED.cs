using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class MonstersAI_FIXED : MonoBehaviour
{
    //고정형 몬스터
    //MonsterManager가 몬스터 생성하면서 각 몬스터에 맞는 csv파일 파싱해서 공격력, hp,몬스터 타입(정찰,고정,사격,근접..),속도 등 나눠줌
    #region Private Variable
    private GameManager _GameManager;
    private GameObject Player;
    //몬스터 체력이 0 이하일 경우, 아직 죽는 모션 함수가 실행되었는지 아닌지 판단하는 bool함수
    //체력이 0 이하일 때 DeadStart가 false이면 코루틴으로 함수 실행하고 DeadStart=true로 돌린다
    private bool DeadStart = false;
    private Animator _Anim;
    //몬스터의 체력이 0 이하인 경우 Coin 값이 바뀐다. 그 순간 플레이어가 소지하고 있는 Coin에 값 전달하고 죽음
    private int Coin = 0;
    private bool isLeft = true; //왼쪽을 바라보고있으면 true
    #endregion
    #region 리스폰 포인트의 MonsterManager_Plus로부터 값을 받는 변수들
    public string MonsterName;
    public int HP;  //몬스터 체력
    public int attack;  //몬스터 공격력,함정도 있음
    public int index;   //몬스터 인덱스
    public float NormalSpeed = 2f;      //정찰 모드 시 속도
    public float _AttackSpeed = 4f;      // 공격 속도(때릴 때 속도):근접,이동가능
    public float _CheckTime = 0;    //공격 쿨타임 재는 시간->원거리,고정에 쓰임
    public float _CheckDelay = 2;   //쿨타임 한계시간
    public int Stage_Location;  //몬스터 출현 스테이지 
    public bool Recon;  //몬스터가 움직일 수 있는 타입인지 true면 정찰(false-고정형이면 무조건 원거리)
    public int MonsterType; //0이면 일반(근접공격-근거리) 1이면 사격-원거리 2면 강화형(HP 더 커짐) 3-자폭
    public bool GetInfo;    //MonsterManager로부터 정보 받으면 true
    public float HurtTime;  //무적인 시간(Time.deltaTime으로 더해주고, 만약이게 0보다 작으면 Hurt=false
    public float WholeHurtTime = 0.5f;  //무적인 시간 전체
    private bool Hurt;   //플레이어에게 맞으면 잠시동안 스프라이트 깜빡이도록 함. 이때 Hurt==true이고 이 동안은 플레이어에게
                         //공격받아도 일시적으로 무적 상태이다
    public enum _IsMonstate { ReconState = 0, AttackState, DeadState=3 };
    public _IsMonstate NowMonstate = _IsMonstate.ReconState;
    //고정형:0=경비모드,1:공격모드 3:죽음(IsDead==true)

    #endregion

    #region 자식 오브젝트
    private List<GameObject> Bodyparts = new List<GameObject>();
    public GameObject SearchArea;   //플레이어 탐색하는 자식오브젝트 ChasingArea담을 오브젝트
    public GameObject AttackArea;   //플레이어와 접촉하면 몬스터가 공격하도록 하는 자식오브젝트 AttackingArea담을 오브젝
                                    //고정형 몬스터or함정인 경우 AttackArea 버림:(고정형0(평소><->1(발견&공격)상태만 왔다갔다함)
    #endregion
    #region 마리오네트 사수의 총알
    public GameObject M_Bullet;
    public bool ShootStart = false;
    //총 쏘면 true로 바꿔줌->애니메이션 끝나면 true
    //bool로 하니까 안됨
    private Vector2 BulletRespawnPos = new Vector2(-1, -0.3f);
    //총알 리스폰할 마리오네트와의 로컬 포지션 지점
    #endregion

    #region 컴포넌트 변수 
    public Rigidbody2D rigid;
    public SpriteRenderer SR;
    #endregion
    #region 몬스터 죽었을 때
    IEnumerator Dead()
    {
        while (true)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            DeadStart = true;
            _Anim.SetBool("IsDead", true);
            _Anim.SetBool("Attack", false);
            if (Coin == 0)
            {
                Coin = Random.Range(1, 21);
                //w_Coin = Random.Range(100, 501);
            }
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
        }
    }
    #endregion
    #region 몬스터 좌우 방향 시간에 따라 결정하는 함수
    IEnumerator DirectionMaker()
    {
        while(NowMonstate==_IsMonstate.ReconState)
        {
            isLeft = true;
            yield return new WaitForSeconds(2);
            isLeft = false;
            yield return new WaitForSeconds(2);
        }
    }
    #endregion
    #region 총알 쏘는 함수
    IEnumerator Marionnette_Shooting()
    {
        //들어감
        while (true)
        {
            if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("SHOT") == true)
            {
                print("shotshot");
                if (isLeft == true)
                {
                    Instantiate(M_Bullet, new Vector2(transform.position.x + BulletRespawnPos.x, transform.position.y + BulletRespawnPos.y), Quaternion.Euler(0, 0, 90));
                }
                else if (isLeft == false)
                {
                    Instantiate(M_Bullet, new Vector2(transform.position.x - BulletRespawnPos.x, transform.position.y + BulletRespawnPos.y), Quaternion.Euler(0, 0, 270));
                }
                yield return new WaitForSeconds(1f);
            }
            if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("delay") == true)
            {

            }
            yield return null;
        }

    }
    #endregion
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        SR = gameObject.GetComponent<SpriteRenderer>();
        _GameManager = GameManager.GetGameManager;
        HurtTime = WholeHurtTime;
        //몬스터 초기 _isMonState 값 줘야함
        _Anim = GetComponent<Animator>();
        StartCoroutine(DirectionMaker());
        Bodyparts.Clear();
        if(this.gameObject.name== "Marionnette_S")
        {
            //마리오네트 사수라면
            Bodyparts.Add(transform.Find("Body").gameObject);
            Bodyparts.Add(transform.Find("Head").gameObject);
            Bodyparts.Add(transform.Find("Left_Foot").gameObject);
            Bodyparts.Add(transform.Find("Right_Foot").gameObject);
            Bodyparts.Add(transform.Find("Right_Hand").gameObject);
            Bodyparts.Add(transform.Find("Left_Hand").gameObject);
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
            if ((MonsterName == "Marionette_S") && (M_Bullet == null))
            {
                print("bb");
                M_Bullet = Resources.Load<GameObject>("Test_M_Bullet");
                //나중에 폴더 넣어서 관리
            }
            #region HP>0일 경우
            if (HP > 0 && NowMonstate != _IsMonstate.DeadState)
            {

                if (NowMonstate == _IsMonstate.ReconState)
                {   //평소 모드(플레이어오나 안오나 살펴보는 모드)  ->좌우 살피도록
                    _Anim.SetBool("Attack", false);
                    Watching(); //고정형 몬스터가 플레이어 오나 안오나 살피는 함수
                }
                else if (NowMonstate == _IsMonstate.AttackState)
                {
                    if(this.gameObject.name == "Marionnette_S")
                    {
                        if (ShootStart == false)
                        {
                            StartCoroutine(Marionnette_Shooting());
                            ShootStart = true;
                        }
                    }
                    FixedMonsterLooking();
                    _Anim.SetBool("Attack", true);

                    _Anim.SetTrigger("SET");
                    //플레이어 발견모드(이때 조준&&공격)
                }
                else if (NowMonstate == _IsMonstate.DeadState)
                {
                    //죽었으면
                    //밑에서 처리
                }
            }
            #endregion
        }
        #region HP<=0일 경우
        if (HP <= 0)
        {
            print("DEAD");
            NowMonstate = _IsMonstate.DeadState;
        }
        #endregion

        #region 죽는 거 처리하는 부분
        if (NowMonstate == _IsMonstate.DeadState)
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
   
    #region 고정형 원거리 몬스터가 플레이어 발견&공격하는 함수
    public void FixedMonsterLooking()
    {

        if (NowMonstate == _IsMonstate.AttackState)   //현재 몬스터 동작 상태=공격상태인지 체크
        {
            //플레이어붙인 태그를 자동으로 찾게 되어있음
            //플레이어가 몬스터 왼편에 있을 때
           

            if (Player.transform.position.x < transform.position.x)
            {
                isLeft = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //플레이어가 몬스터 오른쪽에 있을 때
            if (Player.transform.position.x > transform.position.x)
            {
                isLeft = false;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        //고정형 원거리 몬스터 공격 타입
    }
    #endregion
    #region <고정형>이 좌우 살피는 함수
    void Watching()
    {
        if (NowMonstate == _IsMonstate.ReconState)
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
    IEnumerator GetHurt()
    {
        while (Hurt == true)
        {
            for(int i = 0; i < Bodyparts.Count; i++)
            {
                Bodyparts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 0, 0);
            }
            yield return new WaitForSeconds(0.05f);
            for (int i = 0; i < Bodyparts.Count; i++)
            {
                Bodyparts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 1, 1);
            }
            yield return new WaitForSeconds(0.05f);
            for (int i = 0; i < Bodyparts.Count; i++)
            {
                Bodyparts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 0, 0);
            }
            yield return new WaitForSeconds(0.05f);
            for (int i = 0; i < Bodyparts.Count; i++)
            {
                Bodyparts[i].GetComponent<SpriteMeshInstance>().color = new Color(1, 1, 1);
            }
            yield return new WaitForSeconds(0.05f);
            Hurt = false;

        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D col)
    {
        //몬스터와 플레이어가 부딪혔을 경우
        if ((col.gameObject.transform.tag == "Sword") || (col.gameObject.transform.tag == "Bullet"))        //칼이나 총알에 맞으면
        {
            if (Player.GetComponent<PlayerController>().NowState ==PlayerController.PlayerState.Attack)
            {
                if (NowMonstate != _IsMonstate.DeadState)
                {

                    Hurt = true;
                    StartCoroutine(GetHurt());
                    if (col.gameObject.tag == "Sword")
                    {
                        HP -= Player.GetComponent<PlayerController>().SwordDamage;
                        print("m_hp:" + HP);
                    }
                    if (col.gameObject.tag == "Bullet")
                    {
                        HP -= Player.GetComponent<PlayerController>().BulletDagmage;
                        print("m_hp:" + HP);
                    }
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
       

    }
    private void OnTriggerExit2D(Collider2D col)
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnCollisionStay2D(Collision2D other)
    {

    }

}
