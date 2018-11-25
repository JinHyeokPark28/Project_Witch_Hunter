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

	public int atk;
	public int def;
	public int recover_hp;
	public int add_hp;
	public enum ItemType
	{
		Use, Equip, ETC
	}
	#endregion

	#region Public Method
	public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType,
				int _atk = 0, int _def = 0, int _add_hp = 0, int _recover_hp = 0, int _itemCount = 1)
	{
		itemID = _itemID;
		itemName = _itemName;
		itemDescription = _itemDes;
		itemType = _itemType;
		itemCount = _itemCount;
		itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;

		atk = _atk;
		def = _def;
		add_hp = _add_hp;
		recover_hp = _recover_hp;
	}
	#endregion
}
