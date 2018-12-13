using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]

public class Item : ItemParser
{
	#region Public Variable
	public int itemID;					//아이템의 고유 ID값, 중복 불가능
	public string itemName;				//아이템의 이름, 중복가능
	public string itemDescription;		// 아이템 설명
	public double itemState;            // 아이템 스탯
	public int itemAddHP;				// 아이템 추가 능력치
	public int itemType;				// 아이템 타입
	public int itemCount;				// 아이템 개수
	public Item[] ItemList;
	
	#endregion

	#region Public Method
	private void Start()
	{
		base.Start();
		MakingItem(1);
	}
	#endregion

	public void MakingItem(int i)
	{
		
		ItemList = new Item[CellLength - i];
		for (int x = i; x < CellLength; x++)
		{
			int j = 0;
			ItemList[j] = new Item();   //이거 안해줘서 에러났음;;
										//클래스 꼭 하나씩 초기화 시키자;;
			ItemList[j].itemID = Convert.ToInt32(data[x, 0]);
			ItemList[j].itemName = data[x, 1];
			ItemList[j].itemDescription = data[x, 2];
			ItemList[j].itemState = Convert.ToDouble(data[x, 3]);
			ItemList[j].itemAddHP = Convert.ToInt32(data[x, 4]);
			ItemList[j].itemType = Convert.ToInt32(data[x, 5]);
			ItemList[j].itemCount = Convert.ToInt32(data[x, 6]);
			if (j < CellLength - i)
			{
				j++;
			}
			else
			{
				break;
			}

		}

	}
	public void UseItem(int _itemID)                                    //아이템 사용
	{
		switch (_itemID)
		{
			case 10001:
				break;
			case 10002:
				break;
			case 10003:
				break;
			case 10004:
				break;
			case 10101:
				break;
			case 10102:
				break;

		}
	}
}
