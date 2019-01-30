using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour {

	public Text ItemID;
	public Text ItemCount;
	public Image ItemImage;
	private ItemManager _ItemManager;
	private Item _Item;

	[SerializeField]
	public GetItem getItem;

	private List<InventorySlots> SlotList = new List<InventorySlots>();

	private void Start()
	{
		_ItemManager = FindObjectOfType<ItemManager>();
		_Item = FindObjectOfType<Item>();
		ItemImage.sprite = null;
	}
	public void AddItem(Item item)
	{
		ItemID.text = getItem.Name.ToString();
		//_Item.gameObject.GetComponent<SpriteRenderer>().sprite = ItemImage.sprite;
		if (getItem.Count > 1)
			ItemCount.text = "x " + getItem.Count.ToString();
		else
			ItemCount.text = "";

	}
	public void RemoveSlot()
	{
		for (int i = 0; i < SlotList.Count; i++)
		{
			if (SlotList[i].ItemID.text == null)
			{
				SlotList[i].ItemID.text = "";
				SlotList[i].ItemCount.text = "";
				SlotList[i].ItemImage = null;
			}
			else
				return;
		}
	}
}
