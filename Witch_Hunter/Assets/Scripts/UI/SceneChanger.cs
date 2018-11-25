using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour {
    public int NextSceneNumber;
    //플레이어가 다른 씬으로 넘어갈 때 시작 포인트잡아줘야 함
    //플레이어 스크립트에서 시작 포인트 바로 잡도록 하기
    //start문에서 시작하자마자 포인트 잡기&다른 씬으로 넘어가면 거기에 있는 포인트 자동으로 잡아주기
    public bool LoadBeforeScene;
    public bool CanGoback = true;   //보스 나오는 씬이면 false로 바꿔줌
    public bool reset = false;
    //로드하는 씬이 이 전단계인가 아님 다음 단계인가?
	// Use this for0 initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //플레이어에게 새 씬 시작되면 바로 포인트 잡아주라고 신호 보내기
        if (reset == true)
        {

        }
        if (CanGoback == false)
        {
            //못돌아가면 충돌해도 못지나감&&아무 씬 전환 없도록
            GetComponent<BoxCollider2D>().isTrigger = false;
            reset = false;
        }
        else
        {
            //보스 물리침or 보스 나오는 스테이지 아님->cangoBack==true->Trigger효과 enter해놓음
            GetComponent<BoxCollider2D>().isTrigger = true;
            reset = false;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanGoback == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (LoadBeforeScene == true)
                {
                    //전환하려는 씬이 줄거리 상 이전 단계의 씬이라면
                    collision.gameObject.GetComponent<PlayerController>().SceneStart = false;
                }
                else
                {
                    //전환하려는 씬이 줄거리상 다음 단계의 씬이라면
                    collision.gameObject.GetComponent<PlayerController>().SceneStart = true;
                }
                SceneManager.LoadScene(NextSceneNumber);

            }
        }
    }
}
