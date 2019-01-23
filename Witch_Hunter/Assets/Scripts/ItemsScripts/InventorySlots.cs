using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour {

	public Text ItemID;
	public Text ItemCount;
	public Image ItemImage;
	private ItemManager _ItemManager;

	[SerializeField]
	public GetItem getItem;

	private List<InventorySlots> SlotList;

	private void Start()
	{
		SlotList = new List<InventorySlots>();
		_ItemManager = FindObjectOfType<ItemManager>();
  
	}
	public void AddItem(GetItem item)
	{
		ItemID.text = item.ItemID.ToString();
		if (getItem.Count > 0)
			ItemCount.text = "x " + item.Count.ToString();
		else
			ItemCount.text = "";

	}
	public void GenerateIcon()
	{ 
		for (int i = 0; i < SlotList.Count; i++)
		{
			Debug.Log(SlotList[i].ItemImage);
		}
	}
	public void RemoveSlot()
	{
	//	ItemID.text = "";
	//	ItemCount.text = "";
	//	ItemImage = null;
	}
}
