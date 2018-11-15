using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

	#region Private Vairable
	
	private InventorySlot[] slots;      // 인벤토리 슬롯
	private OKOrCancel theOOC;
	private DatabaseManager theDatabase;

	private List<Item> inventoryItemList;   // 플레이어가 소지한 아이템 리스트
	private List<Item> inventoryTabList;    // 선택한 탭에 따라 다르게 보여질 아이템 리스트

	private int selectedItem;               // 선택된 아이템
	private int selectedTab;                // 선택된 탭

	private bool activated;                 // 인벤토리 활성화시 true;
	private bool tabActivated;              // 탭 활성화시 true;
	private bool itemActivated;             // 아이템 활성화시 true;
	private bool stopKeyInput;              // 키입력 제한 (소비할 때 질의가 나옴, 그때 키입력 방지)
	private bool preventExec;               // 키입력 방지

	private WaitForSeconds waitTime = new WaitForSeconds(.01f);

	#endregion
	#region Public Vairable
	public static Inventory instance;
	public Text Description_Text;           // 부연 설명
	public string[] tabDescription;         // 탭 부연 설명

	public Transform Slots;                 // 슬롯 부모객체

	public GameObject Go_OOC;				// 선택지 활성화 및 비활성화
	public GameObject Go;                   // 인벤토리 활성화 및 비활성화
	public GameObject[] selectedTabImages;  // 아이템 위에 있을때 깜박거릴 패널
	#endregion
	#region Private Method
	private void Start()
	{
		instance = this;
		theDatabase = FindObjectOfType<DatabaseManager>();
		theOOC = FindObjectOfType<OKOrCancel>();
		inventoryItemList = new List<Item>();
		inventoryTabList = new List<Item>();
		slots = Slots.GetComponentsInChildren<InventorySlot>();
	}

	private void Update()
	{
		if (!stopKeyInput)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				activated = !activated;

				if (activated)
				{
					// 안움직이게 하기
					Go.SetActive(true);
					selectedTab = 0;
					tabActivated = true;
					itemActivated = false;
					ShowTab();
					StartCoroutine(SelectedTabEffectCoroutine());
				}
				else
				{
					StopAllCoroutines();
					Go.SetActive(false);
					tabActivated = false;
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
					}
				}

				else if (itemActivated)
				{
					if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						if (selectedItem < inventoryTabList.Count - 1)
							selectedItem += 2;
						else
							selectedItem %= 2;
						SelectedItem();
					}
					else if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						if (selectedItem > 1)
							selectedItem -= 2;
						else
							selectedItem = inventoryTabList.Count - 1 - selectedItem;
						SelectedItem();
					}
					else if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						if (selectedItem < inventoryTabList.Count - 1)
							selectedItem++;
						else
							selectedItem = 0;
						SelectedItem();

					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
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
							stopKeyInput = true;
							// 물약 및 무기 착용
							StartCoroutine(OOCCoroutine());
							transform.Find("Equipment_UI").gameObject.SetActive(true);
						}
						else if(selectedTab == 1)
						{
							// 물약 조합
						}
						else
						{
							transform.Find("Equipment_UI").gameObject.SetActive(false);
						}
					}
					else if (Input.GetKeyDown(KeyCode.Escape))
					{
						StopAllCoroutines();
						itemActivated = false;
						tabActivated = true ;
						ShowTab();
					}
				}			// 아이템 활성화시 키입력 처리
				if (Input.GetKeyUp(KeyCode.Return))	// 키 입력 중복 처리
					preventExec = false;

			}

		}
	}
	#endregion
	#region Public Method
	public void ShowItem()								// 아이템 활설화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
	{
		inventoryTabList.Clear();
		RemoveSlot();
		selectedItem = 0;

		switch (selectedTab)                            // 아이템 분류 / 인벤토리 리스트에 추가
		{
			case 0:
				for (int i = 0; i < inventoryItemList.Count; i++)
				{
					if (Item.ItemType.Use == inventoryItemList[i].itemType || Item.ItemType.Equip == inventoryItemList[i].itemType)
						inventoryTabList.Add(inventoryItemList[i]);
				}
				break;
			case 1:
				for (int i = 0; i < inventoryItemList.Count; i++)
				{
					if (Item.ItemType.ETC == inventoryItemList[i].itemType)
						inventoryTabList.Add(inventoryItemList[i]);
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
	public void SelectedItem()							// 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값 0으로 조정.
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
	public void ShowTab()								// 탭 활성화
	{
		RemoveSlot();
		SelectedTab();
	}
	public void SelectedTab()							//선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
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
	IEnumerator OOCCoroutine()
	{
		Go_OOC.SetActive(true);
		theOOC.ShowTwoChoice("사용", "취소");
		yield return new WaitUntil(() => !theOOC.activated);
		if (theOOC.GetResult())
		{
			for (int i = 0; i < inventoryItemList.Count; i++)
			{
				if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
				{
					theDatabase.UseItem(inventoryItemList[i].itemID);

					if (inventoryItemList[i].itemCount > 1)
						inventoryItemList[i].itemCount--;
					else
						inventoryItemList.RemoveAt(i);

					ShowItem();
					break;
				}
			}
		}
		stopKeyInput = false;
		Go_OOC.SetActive(false);
	}
	IEnumerator SelectedItemEffectCoroutine()			// 선택된 아이템 반짝임 효과
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
	IEnumerator SelectedTabEffectCoroutine()			// 선택된 탭 반작임 효과
	{
		while (tabActivated)
		{	
			Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
			while(color.a < 0.5f)
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
	public void RemoveSlot()							//인벤토리 슬롯 초기화
	{
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].RemoveItem();
			slots[i].gameObject.SetActive(false);
		}
	}
	public void GetAnItem(int _itemID, int _count = 1)
	{
		for(int i = 0; i < theDatabase.itemList.Count; i++)						// 데이터베이스 아이템 검색
		{
			if (_itemID == theDatabase.itemList[i].itemID)                      // 데이터베이스 아이템 발견
			{
				for (int j = 0; j < inventoryItemList.Count; j++)				// 소지품에 같은 아이템이 있는지  검색.
				{
					if (inventoryItemList[j].itemID == _itemID)					// 소지품에 같은 아이템이 있다. -> 소지량 증감
					{
						if(inventoryItemList[i].itemType == Item.ItemType.Use)
						{
							inventoryItemList[j].itemCount += _count;
						}
						else
						{
							inventoryItemList.Add(theDatabase.itemList[i]);
						}
						return;
					}
				}
				inventoryItemList.Add(theDatabase.itemList[i]);                 // 소지품에 해당 아이템 추가
				inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
				return;
			}
				
		}
		Debug.LogError("데이터베이스에 해당되는 ID를 가진 아이템이 존재하지 않습니다.");	//데이터베이스에 ID 없음
	}
	#endregion
}
