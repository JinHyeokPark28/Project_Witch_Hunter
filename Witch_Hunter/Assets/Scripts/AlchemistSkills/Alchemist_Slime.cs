using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemist_Slime : MonoBehaviour {
    public GameObject LightBall;
   //연금술 마녀 옆에 늘 붙어있는 슬라임(3초 간격으로 투사체 날림). 무적
 
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
