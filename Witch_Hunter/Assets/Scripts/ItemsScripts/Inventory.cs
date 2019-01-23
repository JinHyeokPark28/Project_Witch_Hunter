using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

	#region Private Variable
	public ItemManager _ItemManager;                        // 아이템매니저 스크립트 호출할 변수

	private Item item;										// 각 각 아이템이 가지고 있을 스크립트

	private InventorySlots[] Slots;                         // 슬롯 배열처리

	private List<Item> l_Items = new List<Item>();			// 슬롯에다가 넣어줄 아이템 리스트 선언.

	private bool Activated;                                 // true 시 인벤토리 활성화
	
	private bool EquipmentTabActivated;                     // true 일때 장비 창 탭 활성화

	private bool CombinationTabActivated;                   // true 일때 조합 창 탭 활성화.

	private SelectedTab selectedTab = SelectedTab.Equipment;
	#endregion
	#region Public Variable
	public static Inventory instance = null;                // 인벤토리 싱글톤

	public GameObject m_Inventory;                          // 인벤토리를 담아 놓을 게임오브젝트

	public Transform Bags;									// 인벤토리 슬롯을 담아놓은 가방 변수

	public Button equipmentButton;							// 장비창으로 넘어갈 버튼

	public Button combinationButton;                        // 조합창으로 넘어갈 버튼


	#region EunmType
	public enum SelectedTab
	{
		Equipment,
		Combination
	}
	#endregion												//장비탭을 검사할 이넘타입

	#endregion
	#region Private Method
	private void Start()
	{
		instance = this;
		Slots = Bags.GetComponentsInChildren<InventorySlots>();
		_ItemManager = FindObjectOfType<ItemManager>();
		item = FindObjectOfType<Item>();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			Activated = !Activated;

			// 인벤토리가 활성화 되었을때
			if (Activated == true)
			{
				RemoveSlot();
				m_Inventory.SetActive(true);
			}
			// 인벤토리가 비 활성화 되었을 때
			else
			{
				m_Inventory.SetActive(false);
			}
		}
	}

	#endregion
	#region Public Method

	// 장비창을 눌렀을 때 거기에 담긴 가방 오브젝트를 실행시켜주는 함수
	// 장비창을 눌렀을 때 장비창에 들어갈 타입들의 아이템들을 넣어준다.
	// 단, 물약은 동일하게 들어간다.
	// *스위치문으로 변경 예정.
	public void EquipmentButton()
	{
		EquipmentTabActivated = true;
		CombinationTabActivated = false;
	}
	public void CombinationButton()
	{
		EquipmentTabActivated = false;
		CombinationTabActivated = true;
	}
	// 인벤토리가 실행되면 자동적으로 빈 슬롯을 없애줄 함수
	public void RemoveSlot()
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			Slots[i].RemoveSlot();
			Slots[i].gameObject.SetActive(false);
		}
	}
	public void ShowItems()
	{
		l_Items.Clear();
		//RemoveSlot();

		switch(selectedTab)
		{
			case SelectedTab.Equipment:
				for (int i = 0; i < l_Items.Count; i++)
				{
					if(item.ItemName == _ItemManager.getItem[i].ItemID)
					{
						//Slots[i].getItem = 
					}
				}
				break;
			case SelectedTab.Combination:
				break;
		}
	}
	// 아이템을 확인해서 아이템을 인벤토리에 넣어준다.s
	public void PutinInventory(int ItemID, int ItemCount = 1)
	{
		// ItemManager가 가지고 있는 리스트의 길이를 검사합니다.
		for (int i = 0; i < _ItemManager.getItem.Count; i++)
		{
			// Item이 가지고 있는 ItemManager의 아이디와 아이템 매니저의 비교합니다.
			if (ItemID == _ItemManager.getItem[i].ItemID)
			{
				for (int k = 0; k < Slots.Length; k++)      // 슬롯의 크기를 검사한다.
				{
					if (Slots[k].getItem == null)           // 슬롯안에 아무것도 들어가있지 않다면
					{
						List<GetItem> _temp = new List<GetItem>(_ItemManager.getItem);
						// 슬롯안에 아이템을 추가해줍니다. 
						Slots[k].getItem = _temp[i];
						Slots[k].gameObject.SetActive(true);
						Debug.Log(Slots[k].getItem.ItemID);
						break;
					}
					else
						return;                             // 슬롯이 다 찼다면 다시 되돌아갑니다.
				}
			}
			//else if (ItemID != _ItemManager.getItem[i].ItemID) return;
		}
		// 아이템을 타입별로 검사합니다.
	}
	#endregion
}
