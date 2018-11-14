using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//보물 상자도 여기 들어가있음
public class PlayerStatUIManager : MonoBehaviour {
    //HP&coin이 가지는 스크립트!!!->각 변수에 해당하는 스크립트 잘 처리하기
    public Slider Healthy;
    public Text HPText;
    public int NowHP;
    public GameObject Player;
    public bool HPMinus;
    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
        if (HPText == null)
        {
            HPText = GameObject.Find("HPText").GetComponent<Text>();
        }
    }
	
	// Update is called once per frame
	void Update () {
       // print(Player.GetComponent<PlayerController>().touched);
        NowHP = (int)Healthy.GetComponent<Slider>().value;
        HPText.text = "HP:" + NowHP;
        if (HPMinus==true)
        {
            print("cc");
            Healthy.GetComponent<Slider>().value -= 10;
            HPMinus = false;
        }
       /* if (Player.GetComponent<PlayerController>().touched == true)
        {
            print("NOW:" + NowHP);
            Healthy.GetComponent<Slider>().value -= 10;
        }*/
        if (Healthy.GetComponent<Slider>().value <= 0)
        {
            Time.timeScale = 0;
        }

	}
    
}
