using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {

	#region Private Variable
	private PlayerStatManager thePlayerState;
	private Inventory theInven;
	private OKOrCancel theOOC;
	private const int WEAPON = 0, GUN = 1, ARMOR = 2;
	private const int S_DAM = 0, G_DAM = 1, DEF = 2;
	private bool inputKey = true;
	#endregion

	#region Public Variable
	public GameObject Go_OOC;
	public Text[] text;							// 장비 설명
	public Image[] img_slots;                   // 장비창 아이템 아이콘
	public Item[] equipItemList;				// 장착된 장비 리스트
	public int selectedSlot;                    // 선택된 장비 슬롯

	public bool activated = false;
	#endregion

	#region Private Method
	private void Start()
	{
		thePlayerState = FindObjectOfType<PlayerStatManager>();
		theOOC = FindObjectOfType<OKOrCancel>();
		theInven = FindObjectOfType<Inventory>();
		text[0].text = "무기 1";
		text[1].text = "무기 2 : 사용 불가";
		text[2].text = "갑옷.";
	}
	private void Update()
	{
	if(inputKey)
	{
			if (activated)
			{
				selectedSlot = 0;
				ClearEquip();
				ShowEquip();

				if (Input.GetKeyDown(KeyCode.Return))
				{
					if (equipItemList[selectedSlot].itemID != 0)
					{
						inputKey = false;
						StartCoroutine(OOCCoroutine("장비", "취소"));
					}
				}
			}
		}
			else
			{
				ClearEquip();
				ShowEquip();
			}
	}
	private void EquipEffect(Item _item)
	{
		thePlayerState.atk += _item.atk;
		thePlayerState.def += _item.def;
		thePlayerState.recover_hp += _item.recover_hp;
	}
	private void TakeOffEffect(Item _item)
	{
		thePlayerState.atk -= _item.atk;
		thePlayerState.def -= _item.def;
		thePlayerState.recover_hp -= _item.recover_hp;
	}
	#endregion

	#region Public Method
	public void EquipItem(Item _item)
	{
		string temp = _item.itemID.ToString();
		temp = temp.Substring(0, 3);
		switch(temp)
		{
			case "200": //무기
				EquipItemCheck(WEAPON, _item);
				text[0].text = _item.itemName;
				img_slots[0].sprite = _item.itemIcon;
				break;
			case "201": //갑옷
				EquipItemCheck(ARMOR, _item);
				text[2].text = _item.itemName;
				img_slots[2].sprite = _item.itemIcon;
				break;
			case "202": // 총
				EquipItemCheck(GUN, _item);
				text[1].text = _item.itemName;
				img_slots[1].sprite = _item.itemIcon;
				break;
		}
		ShowEquip();
	}
	public void EquipItemCheck(int _count, Item _item)
	{
		if(equipItemList[_count].itemID == 0)
		{
			equipItemList[_count] = _item;
		}
		else
		{
			theInven.EquipToInventory(equipItemList[_count]);
			equipItemList[_count] = _item;
		}
		EquipEffect(_item);
	}
	public void ClearEquip()
	{
		Color color = img_slots[0].color;
		color.a = 0f;

		for(int i = 0; i < equipItemList.Length; i++)
		{
			img_slots[i].sprite = null;
			img_slots[i].color = color;
		}
	}
	public void ShowEquip()
	{
		Color color = img_slots[0].color;
		color.a = 1f;

		for(int i = 0; i < equipItemList.Length; i++)
		{
			if(equipItemList[i].itemID != 0)
			{
				img_slots[i].sprite = equipItemList[i].itemIcon;
				img_slots[i].color = color;
			}
		}
	}
	#endregion
	#region 코루틴
	IEnumerator OOCCoroutine(string _up, string _down)
	{
		Go_OOC.SetActive(true);
		theOOC.ShowTwoChoice(_up, _down);
		yield return new WaitUntil(() => !theOOC.activated);
		if (theOOC.GetResult())
		{
			theInven.EquipToInventory(equipItemList[selectedSlot]);
			TakeOffEffect(equipItemList[selectedSlot]);
			equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
			ClearEquip();
			ShowEquip();
		}
		inputKey = false;
		Go_OOC.SetActive(false);
	}
	#endregion
}
