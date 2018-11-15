using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class WitchClass : MonoBehaviour
{
    #region 몬스터 공통 속성
    public string Name;
    public int HP;  //100으로 나누어지게!!
    private int WholeHP=0;    //hp 최댓값
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
            }
            if(HP <= WholeHP * 0.75)
            {
                Phase = 2;
            }
            if (HP <= WholeHP * 0.5)
            {
                Phase = 3;
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

        }
    }
    #endregion
    #region 페이즈2(체력 175%~50%)
    public void Phase_2()
    {
        if (Phase == 2)
        {

        }
    }
    #endregion
    #region 페이즈1(체력 50%)
    public void Phase_3()
    {
        if (Phase == 3)
        {

        }
    }
    #endregion
}
