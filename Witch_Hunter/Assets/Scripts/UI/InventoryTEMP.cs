using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTEMP : MonoBehaviour
{

	#region Private Variable
	private MenuManager _Menu;								// 메뉴 매니저를 불러오기 위함.

	private InventorySlot[] Slots;							// 슬롯 배열처리

	#endregion
	#region Public Variable
	public GameObject Bags;
	#endregion
	#region Private Method
	private void Start()
	{
		_Menu = GetComponent<MenuManager>();
	}
	private void Update()
	{
		//if(Input.GetKeyDown(KeyCode.LeftArrow))
		//{

		//}
		//else if(Input.GetKeyDown(KeyCode.RightArrow))
		//{

		//}
	}
	#endregion
	#region Public Method
	public void InventoryButton() 
	{
		Bags.SetActive(!Bags.activeSelf);
		RemoveSlot();
	}
	public void RemoveSlot()
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			Slots[i].RemoveItem();
			Slots[i].gameObject.SetActive(false);
		}
	}
	#endregion
}
