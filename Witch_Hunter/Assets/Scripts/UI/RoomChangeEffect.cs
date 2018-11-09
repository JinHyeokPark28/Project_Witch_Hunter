using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomChangeEffect : MonoBehaviour {
    //방 입구(문)마다 주어지는 스크립트
    //플레이어가 이 오브젝트 지나면 자동으로 black_Fade오브젝트 활성화시켜서 페이드인 애니메이션 자동으로 동작함
    // Use this for initialization
    public GameObject Player;
    Image img;
    Animator Fade;
	void Start () {
        img = GameObject.Find("RoomChanger").transform.Find("Canvas").transform.Find("Black_Fade").GetComponent<Image>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Fade = GameObject.Find("RoomChanger").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        
        //유니티 오브젝트 비활성화 되도 거기에 붙은 컴포넌트는 비활성화된것이 아님!!
        if (img.enabled == false)
        {
            Debug.Log("FALSE");
            //false가 되면서 img상태도 true로 바뀐것
            Fade.SetBool("FadeTurnOn", false);
            GameObject.Find("RoomChanger").transform.Find("Canvas").transform.Find("Black_Fade").gameObject.SetActive(false);
        }
       
    }
    //플레이어가 이 스크립트가 담긴 오브젝트에 접촉->FadeIn.anim실행하려면 FadeTurnOn==true가 되어야함
    //FadeIn.anim실행을 위해 Fadein.anim이 담긴 애니메이터 컨트롤러RoomChanger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("true");
            GameObject.Find("RoomChanger").transform.Find("Canvas").transform.Find("Black_Fade").gameObject.SetActive(true);
            Fade.SetBool("FadeTurnOn", true);

        }
    }
}
