using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

	public int itemID;
	public int _count;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform.tag == "Player")
		{
		Inventory.instance.GetAnItem(itemID, _count);
		Destroy(this.gameObject);

		}
	}
}
