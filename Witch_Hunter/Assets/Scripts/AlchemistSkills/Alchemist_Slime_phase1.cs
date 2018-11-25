using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemist_Slime_phase1 : MonoBehaviour {
    public Rigidbody2D rg;
    public float MovingSpeed = 150f;
    //연금술 마녀의 페이즈1 스킬인 젤리니들 시전 때 나타나는 메탈슬라임에 붙여진 함수
	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody2D>();
        StartCoroutine(SlimeMoving());
	}
	IEnumerator SlimeMoving()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
	// Update is called once per frame
	void Update () {
        rg.velocity = Vector2.left * MovingSpeed * Time.deltaTime;

    }
}
