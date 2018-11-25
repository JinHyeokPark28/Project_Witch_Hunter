using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {


	#region Private Variable
	private PlayerStatManager thePlayerStat;
	#endregion

	#region Public Variable
	static public DatabaseManager instance;
	
	#endregion

	#region Private Method
	private void Awake()
	{
		if(instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
	}
	public void Start()
	{
		thePlayerStat = FindObjectOfType<PlayerStatManager>();

		itemList.Add(new Item(10001, "붉은 포션", "체력을 30%를 서서히 채워준다.", Item.ItemType.Use, 0,0,0,3));
		itemList.Add(new Item(10002, "노란 포션", "체력을 즉시 회복한다.", Item.ItemType.Use, 0,0,0,100));
		itemList.Add(new Item(10003, "보라 포션", "체력을 30% 채우고 최대 체력을 10 높인다.", Item.ItemType.Use,0,0,10,3));
		itemList.Add(new Item(10004, "백색 포션", "체력을 즉시 회복하고 최대 체력을 10 높인다.", Item.ItemType.Use,0,0,10,100));
		itemList.Add(new Item(10101, "푸른 가루", "붉은 포션의 조합아이템", Item.ItemType.ETC));
		itemList.Add(new Item(10102, "초록 가루", "붉은 포션의 조합아이템", Item.ItemType.ETC));
		itemList.Add(new Item(20001, "롱 소드", "기사단의 기본 무기", Item.ItemType.Equip ,10));
		itemList.Add(new Item(20101, "제식 갑옷", "기사단의 기본 방어 장비", Item.ItemType.Equip,0,5));
		itemList.Add(new Item(20102, "티셔츠", "티셔츠", Item.ItemType.Equip, 0, 1));
		itemList.Add(new Item(20201, "권총", "기사단 기본 총기", Item.ItemType.Equip,20));

	}
	#endregion
	#region 코루틴
	IEnumerator RecoverHP()
	{
		thePlayerStat.currentHP += 3;

		yield return new WaitForSeconds(1);
		StartCoroutine(RecoverHP());

		StartCoroutine(StopCoroutine());

	}
	IEnumerator StopCoroutine()
	{
		yield return new WaitForSeconds(10);
	}
	#endregion
	#region Public Method
	public void UseItem(int _itemID)									//아이템 사용
	{
		switch(_itemID)
		{
			case 10001:
				// 코루틴 만들어서 1초에 3씩 10초동안 총 30 회복하게 만들기.
				if (thePlayerStat.hp >= thePlayerStat.currentHP + 50)
					StartCoroutine(RecoverHP());
				else thePlayerStat.currentHP = thePlayerStat.hp;

				break;
			case 10002:
				thePlayerStat.currentHP += 100;
				break;
			case 10003:
				thePlayerStat.currentHP += 3;
				thePlayerStat.hp += 10;
				break;
			case 10004:
				thePlayerStat.currentHP += 100;
				thePlayerStat.hp += 10;
				break;
			case 10101:
				break;
			case 10102:
				break;
			case 20001:
				break;
			case 20101:
				break;
			case 20201:
				break;

		}
	}
	public List<Item> itemList = new List<Item>();
	#endregion
}
