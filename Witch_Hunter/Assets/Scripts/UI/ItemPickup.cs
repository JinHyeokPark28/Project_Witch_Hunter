using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

	public int itemID;
	public int _count;

	private void OnTriggerStay2D(Collider2D other)
	{
		Inventory.instance.GetAnItem(itemID, _count);
		Destroy(this.gameObject);
	}
}

