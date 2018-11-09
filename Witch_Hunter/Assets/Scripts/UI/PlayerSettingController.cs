using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어의 움직임을 제외한 나머지 키-탭,enter키 설정
public class PlayerSettingController : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Monsters;
    public bool OptionScreen;
    public bool MapOn;
	public bool StateOn;
    //true면 지도 켜짐
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        InfoMapManager();
        ESCcontrol();
		StateContorl();

		if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {

        }
        if (MapOn == true&&OptionScreen==false && StateOn == false && GameObject.Find("Canvas").transform.Find("Inventory").gameObject.activeInHierarchy == true)
        {
            
        }
       // if(GameObject.Find("Canvas").transform.Find("NPCImage").gameObject.activeInHierarchy == true)
       // {
	   //
       // }
        else if(MapOn==false&& OptionScreen == false && StateOn == false && GameObject.Find("Canvas").transform.Find("Inventory").gameObject.activeInHierarchy == false)
        {
			
        }
        if (MapOn == true || OptionScreen == true ||StateOn == true)
        {
            Time.timeScale = 0;
        }
        else if (!MapOn && !OptionScreen && !StateOn) { Time.timeScale = 1; }
    }
    #region 지도 켜고 끄기
    void InfoMapManager()
    {
        //MoveObjMgr로 지도창 관리 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanJump == true&&OptionScreen==false&&StateOn==false)
            {
                if (MapOn == true)
                {
                    MapOn = false;
                    print("MApOFf");
                }
                else if(MapOn==false)
                {
                    MapOn = true;
                    print("MAPOn");
                }
            }
        }
        if (MapOn == true && OptionScreen == false && !StateOn)
        {
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(true);

        }
        else if (MapOn == false && OptionScreen == false && !StateOn)
        {
            GameObject.Find("Canvas").transform.Find("Map").gameObject.SetActive(false);

        }
    
    }
    #endregion
    #region ESC켜기
    public void ESCcontrol()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Player.GetComponent<PlayerController>().CanJump == true&&MapOn==false&&StateOn==false)
        {
            if (OptionScreen == false && MapOn == false && StateOn == false)
            {
                Debug.Log("Stop");
                OptionScreen = true;
            }
            else
            {
                OptionScreen = false;
            }
            //지면에 붙어있을 때 esc키를 누르면->정보창 뜨도록
        }
        if (OptionScreen == true && MapOn==false&&StateOn==false)
        {
            GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject.SetActive(true);
      
			GameObject.Find("Canvas").transform.Find("Slider").gameObject.SetActive(false);
        }
        else if (OptionScreen== false && MapOn == false && StateOn == false) {
            GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject.SetActive(false);
            
			GameObject.Find("Canvas").transform.Find("Slider").gameObject.SetActive(true);
		}
    }

	#endregion
	#region States 창
	public void StateContorl(){
		if (Input.GetKeyDown(KeyCode.P) && Player.GetComponent<PlayerController>().CanJump == true&&MapOn==false&&OptionScreen==false)
		{
			if (StateOn == false)
			{
				StateOn = true;
			}
			else
			{
				StateOn = false;
			}
			//지면에 붙어있을 때 P키 누르면->정보창 뜨도록
		}
		if (StateOn == true&&OptionScreen==false&&MapOn==false)
		{
			GameObject.Find("Canvas").transform.Find("States").gameObject.SetActive(true);
		}
		else if (StateOn == false && OptionScreen == false && MapOn == false)
		{
			GameObject.Find("Canvas").transform.Find("States").gameObject.SetActive(false);
		}
	}
}
	#endregion