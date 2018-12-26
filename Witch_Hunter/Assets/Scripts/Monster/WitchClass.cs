using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WitchClass : MonoBehaviour
{
    public GameObject WitchMngr;
    private BoxCollider2D Bounds;  //소울 파이어 소환할 때 맵 안에서만 있게 하려고
    #region 몬스터 공통 속성
    private GameObject Player;
    public string Name;
    public int HP;  //100으로 나누어지게!!
    public int WholeHP = 0;    //hp 최댓값
    public int Attack;  //함정도 있음
    public int index;
    public int Stage_Location;
    public bool isBoss;
    public int Coin;
    public bool isDead = true; //죽으면 true->마녀 죽으면 다시 리젠 못함
    public bool getInfo = false;    //witchManager로부터 정보 받으면 true
    public int Phase_before;    //현재 실행되는 페이즈와 비교하는 변수
    //얘와 phase값이 다르면 phase가 변한 것으로 취급함
    #endregion 
    private bool Sending = false;   //isdead=true이면 witchManager에게 마녀 죽음 알림
    #region 마녀 특별 속성
    public int Phase;   //0(죽음),1(100%이하),2(75%이하),3(광기만의 특별 페이즈),
    public int WitchType;  //0이면 물속성, 1이면 불속성, 2면 바람속성
    public bool isFinalBoss;    //isBoss=true이고 최종보스이면 true
    #endregion
    #region 연금술 마녀 속성
    public GameObject MetalSlime;   //연금술 마녀 옆에 늘 붙어있는 슬라임(3초 간격으로 투사체 날림). 무적
    public GameObject MetalBounce;
    public GameObject NormalMarionnette;
    public GameObject ArcherMarionnette;
    private bool AllMake = false;
    private bool AllMake_2 = false;
    private bool AllMake_3 = false;

    //매 페이즈마다 같이 써야 할것 같음
    #endregion
    #region 물 마녀 속성
    public GameObject WaterBall;
    private GameObject AquaWave_Left;
    private GameObject AquaWave_Right;
    private bool DoNotPhaseUp = false;  //hp회복되도 페이즈 그대로인걸로
    private bool FindWave = false;  //찾으면 true
    private bool Water_Skill1_Done;
    public bool WaveActive = false; //true면 아무거나 켜져있는것, 웨이브매니저 꺼지기 전에 얘 false로 다시 만들어줌
    private bool ActivateRecovery = false;
    public int DROP_NUMBER=0; //슬라임쪽에서 +1씩 더해줌. 3되면 회복시작
    #endregion
    #region 불의 마녀 속성
    public GameObject Flame;
    public int MadeFlameNumber = 0;
    public GameObject FireRay;
    #endregion
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        WitchMngr = GameObject.FindGameObjectWithTag("WitchManager");
        Bounds = GameObject.FindGameObjectWithTag("Bounds").GetComponent<BoxCollider2D>();
        Phase_before = 1;
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Waves").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("Waves")[i].name == "Waves_Left")
            {
                AquaWave_Left = GameObject.FindGameObjectsWithTag("Waves")[i];
            }
            else
            {
                AquaWave_Right = GameObject.FindGameObjectsWithTag("Waves")[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (getInfo == true)    //witchMAnager로부터 정보를 받아왔다면
        {
            //페이즈가 바뀌었는지 아닌지 체크하는 함수
            if (Phase_before != Phase && Phase != 1)
            {
                //여기서 두번 들어간다->왜?,phase가 1을 받기전에 한번 걸림
                AllMake = false;
                AllMake_2 = false;
                AllMake_3 = false;
                Phase_before = Phase;
            }
            if (WholeHP == 0)   //만약 HP최대량에 대한 정보가 들어오지 않았다면
            {
                WholeHP = HP;   //HP최대량=처음 HP값
            }
            if (index == 2)
            {
                //물의 마녀라면
                if (FindWave == false)
                {
                    AquaWave_Left = this.gameObject.transform.Find("Waves_Left").gameObject;
                    AquaWave_Right = this.gameObject.transform.Find("Waves_Right").gameObject;
                    FindWave = true;
                }
            }
            if (index != 5 && HP > 0)
            {

                //광기 제외한 나머지 마녀들의 페이즈

                if ((HP > WholeHP * 0.5f))
                {
                    //HP가 100%~75%라면
                    if (DoNotPhaseUp ==false)
                    {
                        Phase = 1;
                        Phase_1();
                        DoNotPhaseUp = true;
                    }
                }
                if (DoNotPhaseUp == true)
                {
                    if (HP > 0)
                    {
                        Phase = 2;
                        Phase_2();
                    }
                }
            }
            else if (index == 5)
            {
                //광기의 마녀if (WholeHP == 0)   //만약 HP최대량에 대한 정보가 들어오지 않았다면
                //여기에 만들든 따로 스크립트를 짜든(위치 매니저쪽에서 또 따로 처리 해줘야함) 우선 얘는 나중에 만들기

            }
            //페이즈3는 광기 특별 페이즈
            if (HP <= 0)
            {
                Phase = 0;  //페이즈 죽음
                isDead = true;
            }
        }
        if (isDead == true && Sending == false)
        {
            GameObject.FindObjectOfType<WitchManager>().KillWitchNumber += 1;
            Sending = true; //마녀매니저에게 보냈으면 더이상 보내지 않기
        }
    }
    #region 페이즈1(체력 100%~75%)
    public void Phase_1()
    {
        if (Phase == 1)
        {

            switch (index)
            {
                case 1:
                    //연금술 마녀인경우:페이즈1-젤리니들
                    //메탈슬라임이 자식 오브젝트로 없으면 새로 하나 만들기
                    {
                        JellyNeedle();
                        RespawningMarionnette();
                    }
                    break;
                case 2:
                    {
                        //물의 마녀 인 경우
                        if (Water_Skill1_Done == false)
                        {
                            StartCoroutine(WaterDrop());
                            Water_Skill1_Done = true;   //우선 들어갔으니 한번은 실행됨
                        }
                        if (WaveActive == false)
                        {
                           StartCoroutine(WaveWave());
                            WaveActive = true;
                        }
                        else
                        {
                            //들어감
                        }

                    }
                    break;
                case 3:
                    //불의 마녀 페이즈1
                    {
                        if (MadeFlameNumber==0)
                        {
                            StartCoroutine(SoulFire());
                        }
                    }
                    break;
            }
        }
    }
    #endregion
    #region 페이즈2(체력 75%~50%)
    public void Phase_2()
    {   //우선 클래스에서 선언하고 나중에 고치기
        //GameObject MetalSlime = GameObject.Instantiate(Resources.Load(Application.dataPath + "/Assets/Resources/RuffImages/Metal_Slime_Temporary/TestMetalBounce.png", GameObject);
        //bool AllMake=false;
        if (Phase == 2)
        {
            switch (index)
            {

                case 1:
                    {
                        RespawningMarionnette();
                        MetalBounceSkill();
                        //연금술 마녀인경우:페이즈2
                        //2.메탈바운스-메탈슬라임이 공중에서 떨어지며 튕긴다.(위쪽에서)
                        //i++해서 2가 되면 더이상 프리팹 생성X->AllMake=true
                        break;
                    }
                case 2:
                    {
                        if (Water_Skill1_Done == false)
                        {
                            StartCoroutine(WaterDrop());
                            Water_Skill1_Done = true;   //우선 들어갔으니 한번은 실행됨
                        }
                        if (WaveActive == false)
                        {
                            // StartCoroutine(WaveWave());
                            WaveActive = true;
                        }
                        if (GameObject.FindGameObjectsWithTag("WitchBullet").Length >= 3)
                        {
                            if (ActivateRecovery == false)
                            {
                                CheckingDrops();
                            }
                            if (DROP_NUMBER == 3)
                            {
                                HP += 20;
                                DROP_NUMBER = 0;
                            }
                        }

                    }
                    break;
            }
        }
    }
    #endregion
    #region 페이즈3(체력 50%)->광기만의 페이즈
    public void Phase_3()
    {
        if (Phase == 3)
        {

        }
    }
    #endregion
    #region 연금술 마녀 스킬들(패턴들)
    #region 젤리니들
    void JellyNeedle()
    {
        //메탈슬라임이 투사체를 3초 간격으로 날림
        //메탈슬라임 안죽음(무적)
        if (this.gameObject.transform.Find("MetalSlime") == null)
        {
            if (AllMake == false)
            {
                Instantiate(MetalSlime, new Vector2(transform.position.x + 1, transform.position.y + 2), new Quaternion(0, 0, 0, 0));
                AllMake = true;
                //allmake=true이면 더이상 안만듬->class에서 생성안하고 함수내에서 선언한 함순데 괜찮나?
                //안되면 그냥 클래스에서 선언하기
                //해결책?->static으로 변수 선언?->함수내에서 선언한 변수는 static으로 선언 불가
            }
        }
    }
    #endregion
    #region 마리오네트 소환(연금술 페이즈1,2)
    void RespawningMarionnette()
    {
        //페이즈==1때는 리스폰 포인트 최소 2개, 페이즈2때는 리스폰 포인트 최소 3개 활성화 안되어있으면 터짐!!

        if (NormalMarionnette == null)
        {
            print("일반 마리오네트 없음. 마녀에 추가시켜야 함");
        }
        if (ArcherMarionnette == null)
        {
            print("사수 마리오네트 없음");
        }
        if (GameObject.FindGameObjectsWithTag("Respawn").Length >= 3)
        {
            //리스폰 포인트가 instatiate명령어 갯수보다 적으면 터지는 현상 방지
            if (Phase == 1)
            {
                //여기서 int선언해봤자 소용없음. 매 프레임마다 새로운 i를 만들어서. 그냥 클래스에서 선언해야함
                //일반 마리오네트 2마리 소환
                if (AllMake_2 == false)
                {
                    //이거 두번 돌려짐 왜?
                    int i = 0;
                    int x = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                    int y = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                    while (y == x)
                    {
                        y = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                        if (y != x)
                        {
                            break;
                        }
                    }
                    Instantiate(NormalMarionnette, GameObject.FindGameObjectsWithTag("Respawn")[x].transform.position, new Quaternion(0, 0, 0, 0));
                    i++;
                    Instantiate(NormalMarionnette, GameObject.FindGameObjectsWithTag("Respawn")[y].transform.position, new Quaternion(0, 0, 0, 0));
                    i++;
                    //여기서 위에 젤리니들이랑 겹칠수도 있음(AllMake==true)하는 과정에서
                    AllMake_2 = true;
                }
            }
            else if (Phase == 2)
            {
                if (AllMake_2 == false)
                {
                    int x = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                    int y = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                    int z = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                    while (y == x)
                    {
                        y = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                        if (y != x)
                        {
                            break;
                        }
                    }
                    while (z == y)
                    {
                        z = UnityEngine.Random.Range(0, GameObject.FindGameObjectsWithTag("Respawn").Length);
                        if (z != x)
                        {
                            break;
                        }
                    }
                    Instantiate(NormalMarionnette, GameObject.FindGameObjectsWithTag("Respawn")[x].transform.position, new Quaternion(0, 0, 0, 0));

                    Instantiate(NormalMarionnette, GameObject.FindGameObjectsWithTag("Respawn")[y].transform.position, new Quaternion(0, 0, 0, 0));
                    Instantiate(ArcherMarionnette, GameObject.FindGameObjectsWithTag("Respawn")[y].transform.position, new Quaternion(0, 0, 0, 0));
                    AllMake_2 = true;
                }
            }
        }
    }
    #endregion
    #region 메탈바운스
    void MetalBounceSkill()
    {
        if (Phase == 2)
        {
            if (AllMake_3 == false)
            {

                Instantiate(MetalBounce, new Vector3(-2.5f, 11.5f, 0.1f), new Quaternion(0, 0, 0, 0));
                Instantiate(MetalBounce, new Vector3(20, 11.5f, 0.1f), new Quaternion(0, 0, 0, 0));
                AllMake_3 = true;
                //Instantiate(Object original, Vector3 position, Quaternion rotation);

            }
        }
    }
    #endregion
    #endregion
    #region 물의 마녀 스킬들(패턴들)
    #region 물방울 공격
    IEnumerator WaterDrop()
    {

        yield return new WaitForSeconds(1);
        while (true)
        //while (Water_Skill1_Done ==false)
        {
            if (WaterBall != null)
            {
                
                //페이즈1,2시에 사용되는 침수스킬
                //물방울 플레이어 위치로 위에서 떨어짐-3개 소환
                //플레이어가 뒤로 못지나가고, 없애려면 플레이어가 공격해서 없애야함
                //페이즈 2때는 바닥에 떨어진 물방울이 3개가 되면 물의 마녀 회복 스킬 됨
                GameObject drop1=Instantiate(WaterBall, new Vector2(Player.transform.position.x, 18), new Quaternion(0, 0, 0, 0));
                yield return new WaitForSeconds(2);
                GameObject drop2=Instantiate(WaterBall, new Vector2(Player.transform.position.x, 18), new Quaternion(0, 0, 0, 0));
                yield return new WaitForSeconds(2);
                GameObject drop3=Instantiate(WaterBall, new Vector2(Player.transform.position.x, 18), new Quaternion(0, 0, 0, 0));
                yield return new WaitForSeconds(15);
            }

        }
    }
    #endregion
    //비활성화 된 웨이브에게 자식들 오브젝트들 깨우라고&작동 시키라고 신호보내는 함수
    #region 파도 공격 함수
    IEnumerator WaveWave()
    {
        //여기 안에 if문 넣으니까 계속 터짐
        while(true)
        {
            int num = Random.Range(0, 2);   //왼쪽이랑 오른쪽 중에 어느걸 킬지 결정하도록 random돌림
            
            if (num == 0)
            {
                AquaWave_Left.SetActive(true);
            }

            else if (num == 1)
            {
                AquaWave_Right.SetActive(true);
            }
            yield return new WaitForSeconds(10f);
        }

        //yield끝나면서 코루틴 빠져나감

    }
    #endregion
    #region 회복 함수
    void CheckingDrops()
    {
        ActivateRecovery = true;
        int x = 0;
        List<GameObject> Drops = new List<GameObject>();
        Drops.Clear();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("WitchBullet").Length; i++)
        {
            if(GameObject.FindGameObjectsWithTag("WitchBullet")[i].name== "Water Drop")
            {
                x++;
                Drops.Add(GameObject.FindGameObjectsWithTag("WitchBullet")[i]);
                
            }
        }
        if (x >= 3)
        {
            for(int y = 0; y < Drops.Count; y++)
            {
                Drops[y].GetComponent<Water_Drop>().GotoWitch = true;
                
            }
        }
        ActivateRecovery = false;
    }
    #endregion
    //침수에 의해 생긴 물방울을 흡수, 일정    체력을 회복한다.
    //2페이즈 때만 활성화
    
    void Water_Recovery()
    {
        if (Phase == 2)
        {
            
        }
    }


    #endregion
    #region 불의 마녀 스킬들
    IEnumerator SoulFire()
    {
        while (MadeFlameNumber!=2)
        {
            int x = Random.Range((int)Bounds.bounds.max.x, (int)Bounds.bounds.min.x);
            int y = Random.Range((int)Bounds.bounds.max.y, (int)Bounds.bounds.min.y);
            yield return new WaitForSeconds(1f);
            Instantiate(Flame, new Vector2(x,y),new Quaternion(0,0,0,0));
            MadeFlameNumber++;
            x=Random.Range((int)Bounds.bounds.max.x, (int)Bounds.bounds.min.x);
            y=Random.Range((int)Bounds.bounds.max.y, (int)Bounds.bounds.min.y);
            yield return new WaitForSeconds(1f);
            Instantiate(Flame, new Vector2(x,y), new Quaternion(0, 0, 0, 0));
            MadeFlameNumber++;
           // StopCoroutine(SoulFire());    ->이거랑 while(true)로 돌리니 안됨
        }
    }
    #endregion
    #region 다쳤을 때(플레이어에게 공격 먹었을 때)
    //플레이어와 마녀 부딪혔을 때 그 충돌만 isTrigger로 처리하고싶음(안밀리게)
    //특정 오브젝트과 충돌만 isTrigger처리 어떻게???
    //방법1. 콜라이더 두개 붙이기(안떨어지도록 하나는 그대로 collider,다른 하나는 trigger)
    //방법2. 부딪힌 동안에만 RIgidbody2d:kinematic으로 해서 중력값 안받게 처리. 그리고 콜라이더 trigger로 처리(점프하며 공격할때)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().IsAttacking == true)
        {
            print("WITCH_HURT");
            HP -= 10;
        }
    }
    #endregion
}
