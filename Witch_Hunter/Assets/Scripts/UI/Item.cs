using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item{
	#region Public Variable
	public int itemID;              //아이템의 고유 ID값, 중복 불가능
	public string itemName;         //아이템의 이름, 중복가능
	public string itemDescription;  // 아이템 설명
	public int itemCount;           // 소지 개수
	public Sprite itemIcon;         // 아이템의 아이콘
	public ItemType itemType;       // 아이템 타입
	#endregion

	#region Public Method
	public enum ItemType
	{
		Use, Equip, ETC
	}
	public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1)
	{
		itemID = _itemID;
		itemName = _itemName;
		itemDescription = _itemDes;
		itemType = _itemType;
		itemCount = _itemCount;
		itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;
	}
	#endregion
}
