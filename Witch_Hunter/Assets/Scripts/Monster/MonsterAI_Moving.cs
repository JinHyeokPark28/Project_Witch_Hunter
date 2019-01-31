using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Anima2D;
public class MonsterAI_Moving : MonoBehaviour
{
    //리스폰 포인트에 달린 몬스터매니저 스크립트 로부터 정보 받으면 getinfo=true
    //움직일 수 있는, 일반 몬스터 스크립트
    //몬스터 기본 스프라이트 형태:왼쪽 바라봄
    #region private 변수
    private bool Recon = false;  //몬스터가 움직일 수 있는 타입인지 true면 정찰(false-고정형이면 무조건 원거리)
    private Animator _Anim;
    private int Coin;
    private bool DeadStart = false;
    private float StartXPos;    //시작할 때 position의 X좌표 값
    private bool ResetStartXPos = false;    //추적모드->일반상태로 되돌아왔을 때 해야 할것
                                            //추적모드->일반상태시 true로 바뀌어서 새로 StartXPos잡도록 함. 그 뒤엔 다시 false
    private GameObject Player;
    private Rigidbody2D rigid;
    #endregion
    #region CSV 파서(리스폰 포인트에 달린 MonsterManager_Plus)로부터 값 받는 변수들
    public string MonsterName;
    public int HP;  //몬스터 체력
    public int attack;  //몬스터 공격력,함정도 있음
    public int index;   //몬스터 인덱스
    public float NormalSpeed = 2f;      //정찰 모드 시 속도
    public float _AttackSpeed = 4f;      // 공격 속도(때릴 때 속도):근접,이동가능
    public float _CheckTime = 0;    //공격 쿨타임 재는 시간->원거리,고정에 쓰임
    public float _CheckDelay = 2;   //쿨타임 한계시간
    public bool isDead; //죽으면 true(죽으면 코인줌)->마녀 죽으면 다시 리젠 못함
    public int Stage_Location;  //몬스터 출현 스테이지 
    public  enum _IsMonstate { ReconState =0,ChasingState,AttackState,DeadState};
    public _IsMonstate NowMonstate =_IsMonstate.ReconState;
              // 0 : 정찰 모드 , 1: 추격 모드, 2 : 공격 모드 3:죽음(IsDead==true)
                                               //고정형:0=경비모드,1:공격모드
    #endregion
    public int MonsterType; //0이면 일반(근접공격-근거리) 1이면 사격-원거리 2면 강화형(HP 더 커짐) 3-자폭
    public bool GetInfo;    //MonsterManager로부터 정보 받으면 true
    private bool isLeft = true; //왼쪽으로 가는 중이면 true
    //이동형 몬스터는 씬 시작 시 자신의 X좌표를 받아 StartXPos에 넣고, 왼쪽으로 움직이다 X좌표가 StartXPos보다 일정 값(현재 5)
    //보다 작으면 isLeft=false로 전환
    private bool Hurt;   //플레이어에게 맞으면 잠시동안 스프라이트 깜빡이도록 함. 이때 Hurt==true이고 이 동안은 플레이어에게
                         //공격받아도 일시적으로 무적 상태이다
    #region 자식 오브젝트
    private List<GameObject> Bodyparts = new List<GameObject>();
    public GameObject SearchArea;   //플레이어 탐색하는 자식오브젝트 ChasingArea담을 오브젝트
    public GameObject AttackArea;   //플레이어와 접촉하면 몬스터가 공격하도록 하는 자식오브젝트 AttackingArea담을 오브젝
                                    //고정형 몬스터or함정인 경우 AttackArea 버림:(고정형0(평소><->1(발견&공격)상태만 왔다갔다함)
    #endregion
    // Use this for initialization
    #region 죽을 때 시작되는 코루틴
    IEnumerator Dead()
    {
        while (true)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            DeadStart = true;
            _Anim.SetBool("IsDead", true);
            _Anim.SetBool("Attack", false);
            //  _Anim.SetBool("Walk", false);
            yield return new WaitForSeconds(.5f);
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
    #region 몬스터가 다칠때 깜빡이는 함수
    IEnumerator GetHurt()
    {
        while (Hurt == true)
        {
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
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rigid = gameObject.GetComponent<Rigidbody2D>();
        StartXPos = gameObject.transform.position.x;
        _Anim = GetComponent<Animator>();
        Bodyparts.Clear();
        Bodyparts.Add(this.transform.Find("Head").gameObject);
        Bodyparts.Add(this.transform.Find("Body").gameObject);
        Bodyparts.Add(this.transform.Find("Right_Hand").gameObject);
        Bodyparts.Add(this.transform.Find("Left_Hand").gameObject);
        Bodyparts.Add(this.transform.Find("Left_Foot").gameObject);
        Bodyparts.Add(this.transform.Find("Ringt_Foot").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetInfo == true)
        {
        #region HP>0 이상인 상태
            if (HP>0)
            {
                #region 정찰모드(NowMonstate==0)
                if (NowMonstate==_IsMonstate.ReconState)
                {
                   
                    _Anim.SetBool("Walk", true);
                    _Anim.SetBool("Attack", false);
                    _Anim.SetBool("IsDead", false);
                    Moving();
                    #region 이동형 몬스터의 방향 지정함수. 시작한 지점의 X좌표보다 더 왼쪽으로 5 갔으면 방향 바꾸기
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
                else if (NowMonstate==_IsMonstate.ChasingState)  //플레이어 발견->추적모드&&추적 범위 콜라이더와 플레이어 충돌
                {
                    _Anim.SetBool("IsDead", false);
                    //여기서 플레이어 방향 못잡음
                    _Anim.SetBool("Walk", true);
                    _Anim.SetBool("Attack", false);
                    Chasing();
                }
                #endregion
                #region 공격 모드
                else if (NowMonstate==_IsMonstate.AttackState)  //공격 모드. 
                {
                    //Check();
                    _Anim.SetBool("Walk", false);
                    _Anim.SetBool("Attack", true);
                    if (this.gameObject.name == "Marionnette")
                    {
                        _Anim.SetTrigger("Attack_Delay");
                    }
                }
                #endregion
            }
            #endregion
        #region HP<=0이라면
            else if (HP <= 0)
            {
                NowMonstate = _IsMonstate.DeadState;
            }
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
    }
    #region 근접, 이동가능!(Recon=true) 몬스터의 이동 함수
    public void Moving()
    {
        if (NowMonstate==_IsMonstate.ReconState)   //다른 상태로 접어들었을때 꺼지기 위해서
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
        }
    }
    #endregion
    #region 플레이어 추적 함수(_isMonState==1)
    void Chasing()
    {
        if (NowMonstate==_IsMonstate.ChasingState && isDead == false)
        {
            //쫓는 함수->movetowards로 하니까 갑자기 빨라짐
            if (Player.transform.position.x < transform.position.x)
            {
                //플레이어가 몬스터 왼편에 있을 때
                print("player is on left");
                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                print("player is on right");

                transform.Translate(Vector3.left * NormalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
  

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.gameObject.GetComponent<SceneChanger>() != null)
        //{
        //    //만약 씬 전환하는 부분이 부딪혔다면 움직이지 말고 공격만 하기
        //    StopChasing = true;
        //}
        if (((col.gameObject.transform.tag == "Sword")&&(Player.GetComponent<PlayerController>().NowState == PlayerController.PlayerState.Attack))
            || (col.gameObject.transform.tag == "Bullet"))        //칼이나 총알에 맞으면
        {
            if (NowMonstate != _IsMonstate.DeadState && HP > 0)
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
    private void OnTriggerStay2D(Collider2D col)
    {

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        //if (col.gameObject.GetComponent<SceneChanger>() != null)
        //{
        //    //씬 전환하는 부분에서 빠져 나왔으면
        //    StopChasing = false;
        //}

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
    private void OnCollisionStay2D(Collision2D other)
    {

    }
}
