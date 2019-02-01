using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소울파이어가 생성한 불꽃이 쏘는 작은 불꽃들&&마리오네트 사수가 쏘는 총알에 달린 스크립트
//바운즈 벗어나면 자동삭제
public class MiniFlameScript : MonoBehaviour {
    private GameObject Player;
    public int DamageNum=5;  //플레이어가 입는 피해 수치
    public float Speed = 2.5f;
    public Vector2 CameraPos;
    public float ScreenWidth;
    // Use this for initialization
    void Start () {
        ScreenWidth = Camera.main.GetComponent<Camera>().orthographicSize / Screen.height * Screen.width;
        if (this.gameObject.name== "Test_M_Bullet")
        {
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
            {
                if(GameObject.FindGameObjectsWithTag("Enemy")[i].name== "Marionnette_S")
                {
                    DamageNum = GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<MonstersAI_FIXED>().attack;
                }
            }
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        StartCoroutine(AutoDeath());
	}
    #region 플레이어와 안부딪히고 그대로 전진했을 때, 일정 시간 뒤 소멸하게 하는 함수
    IEnumerator AutoDeath()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            Destroy(this.gameObject);
        }
    }
    #endregion

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector2.up * Time.deltaTime * Speed);
        CameraPos = Camera.main.transform.position;
        if (transform.position.y < CameraPos.y - Camera.main.GetComponent<Camera>().orthographicSize)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y > CameraPos.y + Camera.main.GetComponent<Camera>().orthographicSize)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x < CameraPos.x - ScreenWidth)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x > CameraPos.x +ScreenWidth)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어와 부딪혔을 경우
        if (collision.gameObject.tag == "Player"&& 
            collision.gameObject.GetComponent<PlayerController>().NowState != PlayerController.PlayerState.Hit)
        {
            Player.GetComponent<PlayerController>().HP -= DamageNum;
            Destroy(this.gameObject);
        }
    }
}
