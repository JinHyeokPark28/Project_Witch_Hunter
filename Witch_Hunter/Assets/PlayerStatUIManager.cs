using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatUIManager : MonoBehaviour {
    //HPSlider가 가지는 스탯 관리 스크립트
    public Slider Healthy;
    public Text HPText;
    public int HPValue;     //총 체력
    public int NowHP;   //현재 체력
    public GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
        HPText = GameObject.Find("HPText").GetComponent<Text>();
        HPValue = (int)GetComponent<Slider>().maxValue;
        NowHP = HPValue;
	}
	
	// Update is called once per frame
	void Update () {
        HPText.text = "HP:" + NowHP;
        NowHP = (int)GetComponent<Slider>().value;
        if (Player.GetComponent<PlayerController>().touched == true)
        {
           
            print("NOW:" + NowHP);
            GetComponent<Slider>().value -= 10;
        }
        if (GetComponent<Slider>().value <=0)
        {
            Time.timeScale = 0;
        }

	}
}
