using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {

	static public DatabaseManager _DatabaseManager;

	private void Awake()
	{
		if(_DatabaseManager != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this.gameObject);
			_DatabaseManager = this;
		}
	}
	public List<Item> itemList = new List<Item>();
	private void Start()
	{
		itemList.Add(new Item(10001, "빨간 포션", "체력을 어느정도 천천히 채워주는 물약", Item.ItemType.Equip));
		itemList.Add(new Item(10002, "하얀 포션", "체력을 단숨에 채워주는 물약", Item.ItemType.Equip));
		itemList.Add(new Item(10003, "수습기사의 장검", "수습 기사 서약식때 받을수 있는 검", Item.ItemType.Equip));
	}

}
