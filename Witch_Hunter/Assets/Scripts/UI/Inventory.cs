using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//System.DateTime.Now.Second
public class Inventory : MonoBehaviour
{
	#region Private Vairable
	private InventorySlot[] slots;      // 인벤토리 슬롯
	private OKOrCancel theOOC;
	private Equipment theEquip;
	private Item _item;

	private List<Item> inventoryItemList;   // 플레이어가 소지한 아이템 리스트
	private List<Item> inventoryTabList;    // 선택한 탭에 따라 다르게 보여질 아이템 리스트

	private int selectedItem;               // 선택된 아이템
	private int selectedTab;                // 선택된 탭

	private bool tabActivated;              // 탭 활성화시 true;
	private bool itemActivated;             // 아이템 활성화시 true;
	private bool stopKeyInput;              // 키입력 제한 (소비할 때 질의가 나옴, 그때 키입력 방지)
	private bool preventExec;               // 키입력 방지
	private string temp;

	private WaitForSeconds waitTime = new WaitForSeconds(.01f);

	#endregion
	#region Public Vairable

	public bool _isInvenOpen;
	public bool activated;                 // 인벤토리 활성화시 true;
	public static Inventory instance;
	public Text Description_Text;           // 부연 설명
	public string[] tabDescription;         // 탭 부연 설명

	public Transform Slots;                 // 슬롯 부모객체

	public GameObject Go_Combi;             // 조합창 활성화 및 비활성화.
	public GameObject Go_Equip;             // 장비창 활성화 및 비활성화.
	public GameObject Go_OOC;               // 선택지 활성화 및 비활성화
	public GameObject Go;                   // 인벤토리 활성화 및 비활성화
	public GameObject[] selectedTabImages;  // 아이템 위에 있을때 깜박거릴 패널
	#endregion
	#region Private Method
	private void Awake()
	{
		_item = GetComponent<Item>();
		instance = this;
		theOOC = FindObjectOfType<OKOrCancel>();
		inventoryItemList = new List<Item>();
		inventoryTabList = new List<Item>();
		slots = Slots.GetComponentsInChildren<InventorySlot>();
		theEquip = FindObjectOfType<Equipment>();
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.L))
		//{
		//	Debug.Log((_item == null) ? "NULL" : "N");
		//}

		if (!stopKeyInput)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				activated = !activated;

				if (activated == true)
				{
					// 안움직이게 하기
					//Time.timeScale = 0; // 알파값 코루틴이 안돔.
					Go.SetActive(true);
					selectedTab = 0;
					tabActivated = true;
					itemActivated = false;
					ShowTab();
					StartCoroutine(SelectedTabEffectCoroutine());
				}
				else if (activated == false)
				{
					//Time.timeScale = 1;
					StopAllCoroutines();
					Go.SetActive(false);
					tabActivated = false;
					itemActivated = false;
					OpenEquip(false);

				}

			}
			else if(Input.GetKeyDown(KeyCode.Escape))
			{
				activated = !activated;

				if(activated == false)
				{
					// 메뉴매니저에서 연동되서 다른 UI창 킬때 자동으로 false 시켜주기.
					tabActivated = false;
					itemActivated = false;
					OpenEquip(false);
					Go.SetActive(false);
				}
				else if(activated == true)
				{
					// 메뉴매니저에서 연동되서 다른 UI창 킬때 자동으로 false 시켜주기.
					tabActivated = true;
					itemActivated = false;
				}
			}

			if (activated)                          // 인벤토리가 활성화 되었을 경우
			{
				if (tabActivated)                   // 탭 부분이 활성화 되었을 경우
				{
					if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						if (selectedTab < selectedTabImages.Length - 1)
							selectedTab++;
						else
							selectedTab = 0;
						SelectedTab();
					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						if (selectedTab > 0)
							selectedTab--;
						else
							selectedTab = selectedTabImages.Length - 1;
						SelectedTab();
					}
					else if (Input.GetKeyDown(KeyCode.Return))
					{
						Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
						color.a = 0.25f;
						selectedTabImages[selectedTab].GetComponent<Image>().color = color;
						itemActivated = true;
						tabActivated = false;
						preventExec = true;
						ShowItem();
						if (selectedTab == 0)
						{
							OpenEquip(true);
							OpenCombination(false);
						}
						if (selectedTab == 1)
						{
							OpenEquip(false);
							OpenCombination(true);
						}
					}
				}

				else if (itemActivated)
				{
					if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						if (selectedItem < inventoryTabList.Count - 1)
							selectedItem += 2;
						else
							selectedItem %= 2;
						SelectedItem();
					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						if (selectedItem > 1)
							selectedItem -= 2;
						else
							selectedItem = inventoryTabList.Count - 1 - selectedItem;
						SelectedItem();
					}
					else if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						if (selectedItem < inventoryTabList.Count - 1)
							selectedItem++;
						else
							selectedItem = 0;
						SelectedItem();

					}
					else if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						if (selectedItem > 0)
							selectedItem--;
						else
							selectedItem = inventoryTabList.Count - 1;
						SelectedItem();
					}
					else if (Input.GetKeyDown(KeyCode.Return) && !preventExec)
					{
						if (selectedTab == 0)
						{
							// 물약 및 무기 착용
							StartCoroutine(OOCCoroutine("사용", "취소"));
						}
						else if (selectedTab == 1)
						{
							// 물약 조합
						}
						else
						{

						}
					}
					else if (Input.GetKeyDown(KeyCode.Escape))
					{
						StopAllCoroutines();
						itemActivated = false;
						tabActivated = true;
						OpenEquip(false);
						OpenCombination(false);
						ShowTab();
					}
				}           // 아이템 활성화시 키입력 처리

				if (Input.GetKeyUp(KeyCode.Return)) // 키 입력 중복 처리
					preventExec = false;
			}
		}
	}
	#endregion

	#region Public Method
	public void EquipToInventory(Item _item)            // 장비창에서 인벤토리로 옮김
	{
		inventoryItemList.Add(_item);
	}
	public void ShowItem()                              // 아이템 활성화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
	{
		inventoryTabList.Clear();
		RemoveSlot();
		selectedItem = 0;

		switch (selectedTab)                            // 아이템 분류 / 인벤토리 리스트에 추가
		{
			case 0:                                                 // 인벤토리 
				for (int i = 0; i < inventoryItemList.Count; i++)
				{
					if (_item.itemID == inventoryItemList[i].itemID)
					{
						temp = _item.itemID.ToString();
						temp.Substring(0, 1);
						switch (temp)	// 아이템 아이디 첫번째, 두번째에 따라서 구별한다.
						{
							case "10":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
							case "20":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
							case "30":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
							case "40":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
						}
					}
				}
				break;
			case 1:                                                 // 물약 조합
				for (int i = 0; i < inventoryItemList.Count; i++)
				{
					if (_item.itemID == inventoryItemList[i].itemID)
					{
						temp = _item.itemID.ToString();
						temp.Substring(0, 1);
						switch (temp) // 아이템 아이디 첫번째, 두번째에 따라서 구별한다.
						{
							case "40":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
							case "50":
								inventoryTabList.Add(inventoryItemList[i]);
								break;
						}
					}
				}
				break;
		}

		for (int i = 0; i < inventoryTabList.Count; i++)                // 인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가
		{
			slots[i].gameObject.SetActive(true);
			slots[i].Additem(inventoryTabList[i]);
		}
		SelectedItem();
	}
	public void SelectedItem()                          // 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값 0으로 조정.
	{
		StopAllCoroutines();
		if (inventoryTabList.Count > 0)
		{
			Color color = slots[0].selected_Item.GetComponent<Image>().color;
			color.a = 0f;
			for (int i = 0; i < inventoryTabList.Count; i++)
				slots[i].selected_Item.GetComponent<Image>().color = color;
			Description_Text.text = inventoryTabList[selectedItem].itemDescription;
			StartCoroutine(SelectedItemEffectCoroutine());
		}
		else
			Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
	}
	public void ShowTab()                               // 탭 활성화
	{
		RemoveSlot();
		SelectedTab();
	}
	public void SelectedTab()                           //선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
	{
		StopAllCoroutines();
		Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
		color.a = 0f;
		for (int i = 0; i < selectedTabImages.Length; i++)
		{
			selectedTabImages[i].GetComponent<Image>().color = color;
		}
		Description_Text.text = tabDescription[selectedTab];
		StartCoroutine(SelectedTabEffectCoroutine());

	}
	IEnumerator OOCCoroutine(string _up, string _down)                      // 사용 할지 말지 팝업창
	{

		stopKeyInput = true;

		Go_OOC.SetActive(true);
		theOOC.ShowTwoChoice(_up, _down);
		yield return new WaitUntil(() => !theOOC.activated);
		if (theOOC.GetResult())
		{
			for (int i = 0; i < inventoryItemList.Count; i++)               // 인벤토리 안에 있는 아이템 개수 불러오기
			{
				if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)       // 아이템리스트의 아이디와 인벤토리 탭안의 아이템 아이디 검사
				{
					if (selectedTab == 0)
					{
						_item.UseItem(inventoryItemList[i].itemID);           // 데이터베이스 아이템리스트에 있는 아이템을 검사하기

						if (inventoryItemList[i].itemID == 401 || inventoryItemList[i].itemID == 402 ||
						inventoryItemList[i].itemID == 403 || inventoryItemList[i].itemID == 404 ||
						inventoryItemList[i].itemID == 501 || inventoryItemList[i].itemID == 502)
						{
							if (inventoryItemList[i].itemCount > 1)
							{                                                    // 아이템이 1개보다 많으면 삭제
								inventoryItemList[i].itemCount--;
								ShowItem();
							}
							else
							{
								inventoryItemList.RemoveAt(i);                          // 아이템이 없으면 공란 생성
							}
							ShowItem();                                                 // 인벤토리 탭 활성화
							break;
						}
						else if (inventoryItemList[i].itemID != 401 || inventoryItemList[i].itemID != 402 ||
							inventoryItemList[i].itemID != 403 || inventoryItemList[i].itemID != 404 ||
							inventoryItemList[i].itemID != 501 || inventoryItemList[i].itemID != 502)
						{
							theEquip.EquipItem(inventoryItemList[i]);                   // 장비한 아이템 삭제
							inventoryItemList.RemoveAt(i);
							ShowItem();
							break;
						}

					}
				}
			}
		}
		stopKeyInput = false;
		Go_OOC.SetActive(false);
	}
	IEnumerator SelectedItemEffectCoroutine()           // 선택된 아이템 반짝임 효과
	{
		while (itemActivated)
		{
			Color color = slots[0].GetComponent<Image>().color;
			while (color.a < 0.5f)
			{
				color.a += 0.03f;
				slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
				yield return waitTime;
			}
			while (color.a > 0)
			{
				color.a -= 0.03f;
				slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
				yield return waitTime;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}
	IEnumerator SelectedTabEffectCoroutine()            // 선택된 탭 반작임 효과
	{
		while (tabActivated)
		{
			Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
			while (color.a < 0.5f)
			{
				color.a += 0.03f;
				selectedTabImages[selectedTab].GetComponent<Image>().color = color;
				yield return waitTime;
			}
			while (color.a > 0)
			{
				color.a -= 0.03f;
				selectedTabImages[selectedTab].GetComponent<Image>().color = color;
				yield return waitTime;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}
	public void RemoveSlot()                            //인벤토리 슬롯 초기화
	{
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].RemoveItem();
			slots[i].gameObject.SetActive(false);
		}
	}
	public void GetAnItem(int _itemID, int _count = 1)
	{
		for (int i = 0; i < _item.ItemList.Length; i++)                    // 데이터베이스 아이템 검색
		{
			if (_itemID == _item.ItemList[i].itemID)                      // 데이터베이스 아이템 발견
			{
				for (int j = 0; j < inventoryItemList.Count; j++)               // 소지품에 같은 아이템이 있는지  검색.
				{
					if (inventoryItemList[j].itemID == _itemID)                 // 소지품에 같은 아이템이 있다. -> 소지량 증감
					{
						if (inventoryItemList[i].itemID == 401 || inventoryItemList[i].itemID == 402 ||
							inventoryItemList[i].itemID == 403 || inventoryItemList[i].itemID == 404 ||
							inventoryItemList[i].itemID == 501 || inventoryItemList[i].itemID == 502)
						{
							inventoryItemList[j].itemCount += _count;
						}
						else if (inventoryItemList[i].itemID == 101 || inventoryItemList[i].itemID != 402 ||
								inventoryItemList[i].itemID != 403 || inventoryItemList[i].itemID != 404 ||
								inventoryItemList[i].itemID != 501 || inventoryItemList[i].itemID != 502)
						{
							inventoryItemList.Add(_item.ItemList[i]);
						}
						return;
					}
				}
				inventoryItemList.Add(_item.ItemList[i]);                 // 소지품에 해당 아이템 추가
				inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
				return;
			}

		}
		Debug.LogError("데이터베이스에 해당되는 ID를 가진 아이템이 존재하지 않습니다.");  //데이터베이스에 ID 없음
	}
	public void OpenEquip(bool check)
	{
		Go_Equip.SetActive(check);
	}
	public void OpenCombination(bool check)
	{
		Go_Combi.SetActive(check);
	}
	#endregion

}