using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour {
	#region Private Variable
	private bool inputKey = true;
	private PlayerStatManager _StatManager;
	private Equipment theEquip;
	private DatabaseManager theDatabase;
	private GameManager theGame;
	#endregion

	#region Public Variable
	public bool activated = false;
	public Text[] text;                 //정보 텍스트
	public Image Player_Image;
	public GameObject Go;
	public Slider Healthy;
    public int S_dam;
    public int G_dam;
    public int armor;
	public int NowHP;
    #endregion
   
	#region Private Method
	private void Start()
	{
        theGame = FindObjectOfType<GameManager>();
		_StatManager = FindObjectOfType<PlayerStatManager>();
		theEquip = FindObjectOfType<Equipment>();
		theDatabase = FindObjectOfType<DatabaseManager>();
	}
	private void Update()
	{
		if(inputKey)
		{
			if(Input.GetKeyDown(KeyCode.P))
			{
				activated = !activated;
				if(activated)
				{
					Go.SetActive(true);
				}
				else
				{
					Go.SetActive(false);
				}
			}
			if(activated)
			{
				HPControl();
				SwordDamage();
				GunDamage();
				Armor();
				Gold();
				Hits();
				CheckTime();
			}
		}
	}
	#endregion

	#region Public Method
	public void HPControl()
	{
		NowHP = (int)Healthy.GetComponent<Slider>().value;
		text[0].text = "HP : " + NowHP;
	}
	public void SwordDamage()
	{
        text[1].text = PlayerStatManager.instance.atk.ToString();
    }
	public void GunDamage()
	{
        
	}
	public void Armor()
	{

		text[3].text = _StatManager.def.ToString();

	}
	public void Gold()
	{

		text[4].text = "Gold : " + theGame.m_Gold.ToString("N0");
	}
	public void Hits()
	{
	
	}
	public void CheckTime()
	{
		int curTime = 0;
		int min = 0;
		int hour = 0;
		int	second = (int)(Time.time - curTime);

		if (second > 59)
		{
			curTime = (int)Time.time;
			second = 0;
			min++;

			if (min > 59)
			{
				min = 0;
				hour++;
			}
		}


		text[6].text = "Time : " + string.Format("{0:00} : {1:00} : {2:00}", hour, min, second);

	}
	#endregion

}
