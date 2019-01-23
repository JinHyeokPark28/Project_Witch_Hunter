using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryButton : MonoBehaviour {

	#region Private Variable
	private Inventory _inven;
	#endregion
	#region Private Method
	private void Start()
	{
		_inven = FindObjectOfType<Inventory>();
	}
	#endregion
	#region Public Method
	public void EquipmentOpen()
	{
		_inven.EquipmentButton();
	}
	public void CombinationOpen()
	{
		_inven.CombinationButton();
	}
	#endregion
}
