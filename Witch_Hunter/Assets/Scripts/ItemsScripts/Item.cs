﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	private ItemManager _ItemManager;
	private Inventory _Inven;

	public int ItemName;
	public int ItemCount;
	public string ItemType;

	private void Start()
	{
		_Inven = FindObjectOfType<Inventory>();
		_ItemManager = FindObjectOfType<ItemManager>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			_Inven.PutinInventory(ItemName);

			this.gameObject.SetActive(false);
		}
	}
}
