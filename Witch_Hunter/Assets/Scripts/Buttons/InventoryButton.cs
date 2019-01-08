using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour {

	#region Private Variable
	private InventoryTEMP _Inven;
	#endregion
	#region Private Method
	private void Awake()
	{
		_Inven = GetComponent<InventoryTEMP>();
	}
	private void InventoryBtn()
	{
		
	}
	#endregion

}
