using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class WitchClass : MonoBehaviour
{
    #region 몬스터 공통 속성
    public string Name;
    public int HP;  //100으로 나누어지게!!
    public int WholeHP=0;    //hp 최댓값
    public int Attack;  //함정도 있음
    public int index;
    public int Stage_Location;
    public bool isBoss;
    public int Coin;
    public bool isDead=true; //죽으면 true->마녀 죽으면 다시 리젠 못함
    public bool getInfo = false;    //witchManager로부터 정보 받으면 true
    #endregion 
    private bool Sending = false;   //isdead=true이면 witchManager에게 마녀 죽음 알림
    #region 마녀 특별 속성
    public int Phase;   //0(죽음),1(100%이하),2(75%이하),3(50%이하),4(광기만의 특별 페이즈),
    public int WitchType;  //0이면 물속성, 1이면 불속성, 2면 바람속성
    public bool isFinalBoss;    //isBoss=true이고 최종보스이면 true
    #endregion

    #region 연금술 마녀 속성
    public GameObject MetalSlime;   //마녀가 페이즈1 때 날리려는 슬라임
    public GameObject MetalBounce;
    private bool AllMake = false;
    private bool HoleNumber2;   //검은 구멍 두개 만들었으면 트루
                                //매 페이즈가 끝날 때마다 false로 바꿔 놓는게 나을듯.->어떻게???
    //매 페이즈마다 같이 써야 할것 같음
    #endregion
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (getInfo == true)    //witchMAnager로부터 정보를 받아왔다면
        {
            if (WholeHP == 0)   //만약 HP최대량에 대한 정보가 들어오지 않았다면
            {
                WholeHP = HP;   //HP최대량=처음 HP값
            }
            if ((HP > WholeHP * 0.75)&&(HP<=WholeHP))
            {
                //HP가 100%~75%라면
                Phase = 1;
                Phase_1();
            }
            if(HP <= WholeHP * 0.75)
            {
                Phase = 2;
                Phase_2();
            }
            if (HP <= WholeHP * 0.5)
            {
                Phase = 3;
                Phase_3();
            }
            else if (HP <= 0)
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
                    //메탈슬라임이 직선으로 날아감
                    {
                        if (AllMake == false)
                        {
                            print("make");
                            Instantiate(MetalSlime, new Vector2(transform.position.x,transform.position.y+2), new Quaternion(0, 0, 0, 0));
                            Instantiate(MetalSlime, transform.position, new Quaternion(0, 0, 0, 0));
                            Instantiate(MetalSlime, new Vector2(transform.position.x, transform.position.y- 2), new Quaternion(0, 0, 0, 0));
                            AllMake = true;
                            //allmake=true이면 더이상 안만듬->class에서 생성안하고 함수내에서 선언한 함순데 괜찮나?
                            //안되면 그냥 클래스에서 선언하기
                            //해결책?->static으로 변수 선언?->함수내에서 선언한 변수는 static으로 선언 불가
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
                    //연금술 마녀인경우:페이즈2
                    //2.메탈바운스-메탈슬라임이 공중에서 떨어지며 튕긴다.(위쪽에서)
                    //i++해서 2가 되면 더이상 프리팹 생성X->AllMake=true
                    if (HoleNumber2 == false)
                    {
                        Instantiate(MetalBounce, new Vector3(-2.5f, 11.5f, 0.1f), new Quaternion(0, 0, 0, 0));
                        Instantiate(MetalBounce, new Vector3(20, 11.5f, 0.1f), new Quaternion(0, 0, 0, 0));
                        HoleNumber2 = true;
                        //Instantiate(Object original, Vector3 position, Quaternion rotation);
                    }
                    break;
            }
        }
    }
    #endregion
    #region 페이즈3(체력 50%)
    public void Phase_3()
    {
        if (Phase == 3)
        {
            switch (index)
            {
                case 1:
                    //연금술 마녀인경우
                    {

                    }
                    break;
            }
        }
    }
    #endregion
}
