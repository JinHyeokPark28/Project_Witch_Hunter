using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	private Item item;

	#region Public Variable
	public Image icon;
	public Text itemName_Text;
	public Text itemCount_Text;
	public GameObject selected_Item;

	private void Start()
	{
		item = GetComponent<Item>();
	}
	public void Additem(Item _item)
	{
		itemName_Text.text = _item.itemName;
		//icon.sprite = _item.itemIcon;
		if(item.itemID == 401 || item.itemID == 402 || item.itemID == 403 ||
		item.itemID == 404 || item.itemID == 501 || item.itemID == 502)
		{
			if (_item.itemCount > 0)
				itemCount_Text.text = "x " + _item.itemCount.ToString();
			else
				itemCount_Text.text = "";
		}

	}
	public void RemoveItem()
	{
		itemName_Text.text = "";
		itemCount_Text.text = "";
		icon.sprite = null;
	}
	#endregion

}
