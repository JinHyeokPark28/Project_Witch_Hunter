using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {


	#region Private Variable
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
	private void Start()
	{
		itemList.Add(new Item(10001, "체력 회복제", "체력을 30%를 서서히 채워준다.", Item.ItemType.Use));
	}
	#endregion
	#region Public Method
	public void UseItem(int _itemID)
	{
		switch(_itemID)
		{
			case 10001:
				break;
			case 10002:
				break;
		}
	}
	public List<Item> itemList = new List<Item>();
	#endregion
}
