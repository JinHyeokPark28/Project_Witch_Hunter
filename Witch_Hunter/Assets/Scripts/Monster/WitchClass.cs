using UnityEngine;
using System.Collections;

public class WitchClass : MonoBehaviour
{
    #region 몬스터 공통 속성
    public int HP;
    public int Attack;  //함정도 있음
    public int index;
    public int Stage_Location;
    public bool isBoss;
    public int Coin;
    public bool isDead=true; //죽으면 true->마녀 죽으면 다시 리젠 못함
    #endregion
    #region 마녀 특별 속성
    public int Phase;
    public int WitchType;  //0이면 물속성, 1이면 불속성, 2면 바람속성
    public bool CanGoFinal; //세 마녀를 다 쓰러뜨리면 true가 되어서 최종보스인 광기의 마녀를 만날수 있다;
    public bool isFinalBoss;    //isBoss=true이고 최종보스이면 true
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
