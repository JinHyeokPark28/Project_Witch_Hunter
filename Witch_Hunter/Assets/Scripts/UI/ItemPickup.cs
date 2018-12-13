using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {
    
    private Inventory theInven;
	public int itemID;
	public int _count;

    private void Start()
    {
        theInven = FindObjectOfType<Inventory>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform.tag.Equals("Player"))
		{
        theInven.GetAnItem(itemID, _count);
		Destroy(this.gameObject);
		}
	}
}
