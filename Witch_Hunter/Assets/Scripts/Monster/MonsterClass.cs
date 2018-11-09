using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//몬스터 클래스(기본)
//몬스터파싱
public class MonsterClass : MonoBehaviour
{
    #region 몬스터 공통 속성
    public int HP;
    public int Attack;  //함정도 있음
    public int index;
    public int Stage_Location;
    public bool IsBoss;
    public int Coin; 
    public bool isDead; //죽으면 true->마녀 죽으면 다시 리젠 못함
    #endregion
    #region 일반 몬스터 특별 속성
    public bool Recon;  //true면 정찰(false-고정형이면 무조건 원거리)
    public int MonsterType; //0이면 일반(근접공격-근거리) 1이면 사격-원거리 2면 강화형(HP 더 커짐)

    #endregion
    
    
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
