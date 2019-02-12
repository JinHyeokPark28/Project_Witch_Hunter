using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type1 
{
	Sword = 101,
	Ammo = 201,
	Armor = 301,
	Use = 401,
	Powder = 501
}
public class Inventory : MonoBehaviour
{
	#region Private Variable
	private PlayerController _Player;                       // 플레이어 스크립트 호출할 변수

	private ItemManager _ItemManager;                        // 아이템매니저 스크립트 호출할 변수

	private Item item;                                      // 각 각 아이템이 가지고 있을 스크립트

	private List<InventorySlots> Slots = new List<InventorySlots>();                         // 슬롯 리스트 선언

	private List<Item> l_Items = new List<Item>();          // 슬롯아이템 리스트 선언.

	private bool EquipmentTabActivated;                     // true 일때 장비 창 탭 활성화

	private bool CombinationTabActivated;                   // true 일때 조합 창 탭 활성화.

	private SelectedTab selectedTab;
	#endregion
	#region Public Variable
	public static Inventory instance = null;                // 인벤토리 싱글톤

	public bool Activated = false;                          // true 시 인벤토리 활성화

	public GameObject m_Inventory;                          // 인벤토리를 담아 놓을 게임오브젝트

	public Transform Bags;                                  // 인벤토리 슬롯을 담아놓은 가방 변수

	public GameObject m_Equipment;                          // 장비창을 담아 놓을 게임 오브젝트

	public GameObject m_Combination;                        // 조합창을 담아 놓을 게임 오브젝트

	public Button equipmentButton;                          // 장비창으로 넘어갈 버튼

	public Button combinationButton;                        // 조합창으로 넘어갈 버튼


	#region EunmType
	public enum SelectedTab
	{
		Equipment,
		Combination,
		None
	}
	#endregion                                             //장비탭을 검사할 이넘타입

	#endregion
	#region Private Method
	private void Start()
	{
		selectedTab = SelectedTab.None;
		instance = this;
		_ItemManager = FindObjectOfType<ItemManager>();
		_Player = FindObjectOfType<PlayerController>();
		item = FindObjectOfType<Item>();
		l_Items = new List<Item>();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			Activated = !Activated;

			// 인벤토리가 활성화 되었을때
			if (Activated == true)
			{
				m_Inventory.SetActive(true);
				m_Equipment.SetActive(false);
				m_Combination.SetActive(false);
				_Player.MinimapObject.SetActive(false);
				_Player.OptionObject.SetActive(false);
				RemoveSlot();
			}
			// 인벤토리가 비 활성화 되었을 때
			else
			{
				m_Inventory.SetActive(false);
				m_Equipment.SetActive(false);
				m_Combination.SetActive(false);
				_Player.MinimapObject.SetActive(false);
				_Player.OptionObject.SetActive(false);
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
		selectedTab = SelectedTab.Equipment;

		// 각각 탭 클릭했을 때 액티브 되는 불형 변수
		EquipmentTabActivated = true;
		CombinationTabActivated = false;

		if (EquipmentTabActivated == true)
		{
			m_Equipment.SetActive(true);
			m_Combination.SetActive(false);
			ShowItems();
		}
		else
		{
			m_Equipment.SetActive(false);
			m_Combination.SetActive(false);
		}
	}
	public void CombinationButton()
	{
		selectedTab = SelectedTab.Combination;
		EquipmentTabActivated = false;
		CombinationTabActivated = true;

		if (CombinationTabActivated == true)
		{
			m_Equipment.SetActive(false);
			m_Combination.SetActive(true);
			ShowItems();
		}
		else
		{
			m_Equipment.SetActive(false);
			m_Combination.SetActive(false);
		}
	}
	// 인벤토리가 실행되면 자동적으로 빈 슬롯을 없애줄 함수
	public void RemoveSlot()
	{
		for (int i = 0; i < Slots.Count; i++)
		{
			Slots[i].RemoveSlot();
			Slots[i].gameObject.SetActive(false);
		}
	}
	// 각 버튼을 누르면 장비창에는 장비 관련된 아이템만, 조합창에는 조합관련된 아이템만 보이게 하기 위한 메소드
	// 리스트 
	public void ShowItems()
	{
		RemoveSlot();

		switch (selectedTab)
		{
			case SelectedTab.Equipment:										// 장비창 버튼을 눌렀을 때
				for (int i = 0; i < Slots.Count; i++)
				{
					if (Slots[i].getItem == null)                           // 인벤토리 내의 슬롯에 정보가 담겨져 있다면 아이템의 타입값을 검사한 후 활성화 시켜준다.
					{
						if (Slots[i].getItem.Type == "Sword" || Slots[i].getItem.Type == "Armor" || Slots[i].getItem.Type == "Ammo" || Slots[i].getItem.Type == "Use")
						{
							Slots[i].gameObject.SetActive(true);
						}
						else if (Slots[i].getItem.Type == "Powder")
						{
							Slots[i].gameObject.SetActive(false);
						}
					}
					if (Equipment.instance.Sword.text != null)
					{
						Slots[i].gameObject.SetActive(false);
					}
					else if (Equipment.instance.Armor.text != null)
					{
						Slots[i].gameObject.SetActive(false);
					}
				}
				break;
			case SelectedTab.Combination:									// 조합창 버튼을 눌렀을 때
				for (int i = 0; i < Slots.Count; i++)
				{
					if (Slots[i].getItem != null)
					{
						if (Slots[i].getItem.Type == "Use" || Slots[i].getItem.Type == "Powder")
						{
							Slots[i].gameObject.SetActive(true);
						}
						else if(Slots[i].getItem.Type == "Sword" || Slots[i].getItem.Type == "Armor" || Slots[i].getItem.Type == "Ammo")
						{
							Slots[i].gameObject.SetActive(false);
						}
					}
				}
				break;
		}

		//if (EquipmentTabActivated == true)                              // 장비창 버튼이 눌렸을 때
		//{
		//	for (int i = 0; i < Slots.Length; i++)
		//	{
		//		if (Slots[i].getItem != null)                           // 인벤토리 내의 슬롯에 정보가 담겨져 있다면 아이템의 타입값을 검사한 후 활성화 시켜준다.
		//		{
		//			if (item.ItemType == "Sword" || item.ItemType == "Armor" || item.ItemType == "Ammo" || item.ItemType == "Use")
		//			{
		//				Slots[i].gameObject.SetActive(true);
		//			}
		//			else if (item.ItemType == "Powder")
		//			{
		//				Slots[i].gameObject.SetActive(false);
		//			}
		//		}
		//	}
		//}
		//if (CombinationTabActivated == true)
		//{
		//	for (int i = 0; i < Slots.Length; i++)
		//	{
		//		if (Slots[i].getItem != null)
		//		{
		//			if (item.ItemType == "Use" || item.ItemType == "Powder")
		//			{
		//				Slots[i].gameObject.SetActive(true);
		//			}
		//			else if (item.ItemType == "Sword" || item.ItemType == "Armor" || item.ItemType == "Ammo")
		//			{
		//				Slots[i].gameObject.SetActive(false);
		//			}
		//		}
		//	}
		//}

	}
	private void ItemNumbering()
	{
		for (int i = 0; i < Slots.Count; i++)
		{
			if (Slots[i].getItem.Count > 1) // 1개 이상있을때 검사해서 만약에 무기, 방어구, 총알이 아니라면 1개만 있게 만들고 물약과 파우더라면 개수를 늘려주기.
			{
				if (Slots[i].getItem.Type == "Sword" || Slots[i].getItem.Type == "Armor" || Slots[i].getItem.Type == "Ammo")
				{
					Slots[i].getItem.Count = 1;
				}
				else if (Slots[i].getItem.Type == "Use" || Slots[i].getItem.Type == "Powder")
				{
					if(Slots[i].getItem.Count >= 1)
					Slots[i].getItem.Count += 1;
				}
			}
		}
	}
	// 아이템을 확인해서 아이템을 인벤토리에 넣어준다.s
	public void PunInInventory(int ItemID, int ItemCount = 1)
	{
		// itemmanager가 가지고 있는 리스트의 길이를 검사합니다.
		for (int i = 0; i < _ItemManager.getItem.Count; i++)
		{
			// item이 가지고 있는 itemmanager의 아이디와 아이템 매니저의 비교합니다.
			if (ItemID == _ItemManager.getItem[i].ItemID)
			{
#pragma warning disable CS0162 // 접근할 수 없는 코드가 있습니다.
				for (int k = 0; k < l_Items.Count; k++)      // 슬롯의 크기를 검사한다.
#pragma warning restore CS0162 // 접근할 수 없는 코드가 있습니다.
				{
					Debug.Log(item);
					if(l_Items[k].getItem.ItemID == ItemID)
					{
						if (l_Items[k].ItemType == "Use" && l_Items[k].ItemType == "Powder")
						{
							l_Items[k].ItemCount += ItemCount;
						}
						else
						{
							Slots[k].AddItem(l_Items[k].getItem);
						}
					//	if (_ItemManager.getItem[i].ItemID == itemid)           // 아이템 매니저안의 아이템아이디와 획득하려는 아이템 아이디가 같다면 슬롯에 넣어줍니다.
					//	{
					//		//GetItem _temp = _ItemManager.getItem[i];
					//		//// 슬롯안에 아이템을 추가해줍니다. 
					//		Slots[k] = l_Items
					//		Slots[k].AddItem(Slots[k].getItem);
					//		Debug.Log("slot.getitem : " + Slots[k].getItem.ItemID);
					//		return;
					//	}
					//	else
					//		break;                             // 슬롯이 다 찼다면 다시 되돌아갑니다.
					}
					Slots[k].AddItem(l_Items[k].getItem);
				}
			}
		}
	}
	public void GetanItem()
	{
		for (int i = 0; i < Slots.Count; i++)
		{
			if (Slots[i].getItem.Type == "Sword" && Slots[i].getItem != null)
			{
				Equipment.instance.Sword.text = Slots[i].getItem.Name.ToString();
				Equipment.instance.SwordImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
				Equipment.instance.Attack.text = "Attack  : " + Slots[i].getItem.Stat.ToString();
			}
			else if (Slots[i].getItem.Type == "Armor" && Slots[i].getItem != null)
			{
				Equipment.instance.Armor.text = Slots[i].getItem.Name.ToString();
				Equipment.instance.ArmorImage.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
				Equipment.instance.Defend.text = "Armor  : " + Slots[i].getItem.Stat.ToString();
			}
			RemoveSlot();
		}
		return;
	}
	#endregion
}
