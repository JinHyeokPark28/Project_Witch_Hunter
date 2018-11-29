using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemist_Slime : MonoBehaviour {
    public GameObject LightBall;
   //연금술 마녀 옆에 늘 붙어있는 슬라임(3초 간격으로 투사체 날림). 무적
    /*
     * 메탈슬라임은 연금술 마녀가 페이즈 1이든 페이즈 2든
늘 마녀 옆에 붙어있는 거죠?

장한 [6:54 PM]
넵
맞아여

Screwby [6:54 PM]
근데 슬라임한테 공격해도 슬라임은 죽지는 않고 계속 투사체를 날리고요
     */
    //연금술 마녀의 페이즈1 스킬인 젤리니들 시전 때 나타나는 메탈슬라임에 붙여진 함수
    // Use this for initialization
    IEnumerator AttackingBall()
    {
        while (true)
        {
            Instantiate(LightBall, transform.position,new Quaternion(0,0,0,0));
            yield return new WaitForSeconds(3);
        }
    }
    void Start () {
        StartCoroutine(AttackingBall());
	}
	void Update () {
        
    }
}
