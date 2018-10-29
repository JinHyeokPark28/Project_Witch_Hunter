using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어의 움직임을 제외한 나머지 키-탭,enter키 설정
public class PlayerSettingController : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Monsters;
    public bool InfoScreen;
    public bool MapOn;
    //true면 지도 켜짐
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Monsters = GameObject.FindGameObjectsWithTag("Enemy");
    }
    // Update is called once per frame
    void Update()
    {
        InfoMapManager();
        ESCcontrol();
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {

        }
        
    }
    #region 지도 켜고 끄기
    void InfoMapManager()
    {
        //MoveObjMgr로 지도창 관리 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanJump == true)
            {
                if (MapOn == true)
                {
                    MapOn = false;
                }
                else if(MapOn==false&&InfoScreen==false)
                {
                    MapOn = true;
                }
            }
        }
        if (MapOn == true)
        {
            Player.GetComponent<PlayerController>().enabled = false;
            for(int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].GetComponent<EnemyChaser>().enabled = false;
            }
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(true);
        }
        else if (MapOn == false)
        {
            Player.GetComponent<PlayerController>().enabled = true;
            for (int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].GetComponent<EnemyChaser>().enabled = true;
            }
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(false);
        }
    }
    #endregion
    #region ESC켜기
    public void ESCcontrol()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Player.GetComponent<PlayerController>().CanJump == true)
        {
            if (InfoScreen == false && MapOn == false)
            {
                Debug.Log("Stop");
                InfoScreen = true;
            }
            else
            {
                InfoScreen = false;
            }
            //지면에 붙어있을 때 esc키를 누르면->정보창 뜨도록
        }
        if (InfoScreen == true)
        {
            Player.GetComponent<PlayerController>().enabled = false;
            for (int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].GetComponent<EnemyChaser>().enabled = false;
            }
        }
        else if (InfoScreen==false) {
            Player.GetComponent<PlayerController>().enabled = true;
            for (int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].GetComponent<EnemyChaser>().enabled = true;
            }
        }
    }
    #endregion
}
