using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	#region 인벤토리 변수
	// 오디오 매니저 추가하기
	// keysound
	// enter sound
	// cancel sound
	// open sound
	// beep sound

	#region Private Varibale

	private OOC theOOC;

	private InventorySlot[] slots;          // 인벤토리 슬롯들

	private DatabaseManager theDatabase;

	private List<Item> inventoryItemList;       // 플레이어가 소지한 아이템 리스트
	private List<Item> inventoryTabList;        // 선택한 탭에 따라 보여질  리스트

	private int selectedItem;      //선택된 아이템
	private int selectedTab; // 선택된 탭

	private bool activated;         // 인벤토리 활성화시 true;
	private bool tabActivated;      // 탭 활성화시 true;
	private bool itemActivated;         // 아이템 활성화시 true;
	private bool stopKeyInput;          // 키입력 제한(아이템 사용시 키입력 방지)
	private bool prevenExec;            // 중복실행 제한


	private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
	#endregion


	#region Public Variable
	public static Inventory instance;
	public GameObject Player;
	public Text Description_Text;           // 부연 설명
	public string[] tabDescription; // 탭 부연설명

	public Transform tf;            // slot의 부모객체(Grid slot)

	public GameObject go; // 인벤토리 활성화 비활성화
	public GameObject go_OOC;               // 선택시 활성화 혹은 비활성화
	public GameObject[] selectedTabImages;      // 골라진 탭 이미지

	#endregion


	#endregion
	
	// Use this for initialization
	#region Start 함수
	void Start()
	{
		instance = this;
		Player = GameObject.FindGameObjectWithTag("Player");
		inventoryItemList = new List<Item>();
		inventoryTabList = new List<Item>();
		slots = tf.GetComponentsInChildren<InventorySlot>();
		theOOC = FindObjectOfType<OOC>();
		theDatabase = FindObjectOfType<DatabaseManager>();
	}
	#endregion

	#region Update 함수
	void Update()
	{
		#region 인벤토리
		if (!stopKeyInput)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				activated = !activated;
				if (activated && GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject.activeInHierarchy == false)
				{
					// 오디오 파일 넣기
					go.SetActive(true);
					selectedTab = 0;
					tabActivated = true;
					itemActivated = false;
					ShowTab();
				}
				else
				{
					// 오디오 파일 넣기

					StopAllCoroutines();
					go.SetActive(false);
					tabActivated = false;
					itemActivated = false;
				}
			}

			if (activated)
			{
				if (tabActivated)
				{
					if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						if (selectedTab < selectedTabImages.Length - 1)
							selectedTab++;
						else
							selectedTab = 0;
						// 오디오 파일 넣기
						SelectedTab();

					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						if (selectedTab > 0)
							selectedTab--;
						else
							selectedTab = selectedTabImages.Length - 1;

						SelectedTab();
						// 오디오 파일 넣기

					}
					else if (Input.GetKeyDown(KeyCode.Return))
					{
						Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
						color.a = 0.25f;
						selectedTabImages[selectedTab].GetComponent<Image>().color = color;
						itemActivated = true;
						tabActivated = false;
						prevenExec = true;
						ShowItem();
					}
				}       // 탭 활성화시 키입력 처리

				else if (itemActivated)
				{
					if (inventoryTabList.Count > 0)
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
							if (selectedItem > 9)
								selectedItem -= 2;
							else
								selectedItem = inventoryTabList.Count - 1 - selectedItem;
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
						else if (Input.GetKeyDown(KeyCode.RightArrow))
						{
							if (selectedItem < inventoryTabList.Count - 1)
								selectedItem++;
							else
								selectedItem = 0;
							SelectedItem();
						}
						else if (Input.GetKeyDown(KeyCode.Return) && !prevenExec)
						{
							if (selectedTab == 0)
							{

								StartCoroutine(OOCCoroutine("장착", "취소"));

							}
							else if (selectedTab == 1)
							{
								stopKeyInput = true;

								StartCoroutine(OOCCoroutine("장착", "취소"));
							}
							else
							{
								// 비프음 출력
							}
						}
					}

					if (Input.GetKeyDown(KeyCode.Escape))
					{
						StopAllCoroutines();
						itemActivated = false;
						tabActivated = true;
						ShowTab();
					}

				}       //아이템 활성화시 키입력 처리

				if (Input.GetKeyUp(KeyCode.Return)) // 중복 실행 방지
					prevenExec = false;
			}

		}
		#endregion

		

	}

	#endregion

	#region public 인벤토리 함수

	#region Public Method
	public void GetAnItem(int _itemID, int _count = 1)
	{
		for (int i = 0; i < theDatabase.itemList.Count; i++)    // 데이터베이스 아이템 검색
		{
			if (_itemID == theDatabase.itemList[i].itemID)  // 데이터베이스에 아이템 발견
			{
				for (int j = 0; j < inventoryItemList.Count; j++)   // 소지품 같은 아이템 확인
				{
					if (inventoryItemList[j].itemID == _itemID)     // 같은 아이템이 있다 -> 개수 증감
					{
						if (inventoryItemList[i].itemType == Item.ItemType.Use)
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
				inventoryItemList.Add(theDatabase.itemList[i]); // 소지품에 해당 아이템 추가
				inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
				return;
			}
		}
		Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재 하지 않음"); // 오류코드 
	}
	public void ShowTab()
	{
		RemoveSlot();
		SelectedTab();
	}
	public void RemoveSlot()
	{
		if (slots.Length != 0)
		{

			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].RemoveItem();
				slots[i].gameObject.SetActive(false);
			}
		}
	}       // 인벤토리 슬롯 초기화
	public void SelectedTab()
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
	}       // 탭 활성화
	IEnumerator SelectedTabEffectCoroutine()
	{
		while (tabActivated)
		{
			Color color = selectedTabImages[0].GetComponent<Image>().color;
			while (color.a < 0.5f)
			{
				color.a += 0.03f;
				selectedTabImages[selectedTab].GetComponent<Image>().color = color;
				yield return waitTime;
			}
			while (color.a > 0f)
			{
				color.a -= 0.03f;
				selectedTabImages[selectedTab].GetComponent<Image>().color = color;
				yield return waitTime;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}       // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
	public void ShowItem()
	{
		inventoryTabList.Clear();
		RemoveSlot();
		selectedItem = 0;
		switch (selectedTab)
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
		}      // 탭에 따른 아이템 분류. 그것을 인벤토리 탭 리스트에 추가.

		for (int i = 0; i < inventoryTabList.Count; i++)
		{
			slots[i].gameObject.SetActive(true);
			slots[i].Additem(inventoryTabList[i]);
		}     // 인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가

		SelectedItem();
	}               // 아이템 활성화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)

	public void SelectedItem()
	{
		StopAllCoroutines();
		if (inventoryTabList.Count > 0)
		{
			Color color = slots[0].selected_Item.GetComponent<Image>().color;
			color.a = 0f;
			for (int i = 0; i < inventoryTabList.Count; i++)
			{
				slots[i].selected_Item.GetComponent<Image>().color = color;
			}
			Description_Text.text = inventoryTabList[selectedItem].itemDescription;
			StartCoroutine(SelectedItemEffectCoroutine());
		}
		else
		{
			Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
		}
	}       //	선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정
	IEnumerator SelectedItemEffectCoroutine()
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
			while (color.a > 0f)
			{
				color.a -= 0.03f;
				slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
				yield return waitTime;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}       // 선택된 아이템 반짝임 효과
	IEnumerator OOCCoroutine(string _up, string _down)
	{
		stopKeyInput = true;

		go_OOC.SetActive(true);
		theOOC.ShowTwoChoice(_up, _down);
		yield return new WaitUntil(() => !theOOC.activated);
		if (theOOC.GetResult())
		{
			for (int i = 0; i < inventoryItemList[i].itemCount; i++)
			{
				if (inventoryItemList[i].itemID == inventoryItemList[selectedItem].itemID)
				{
					if (selectedTab == 0)
					{

						//아이템 습득 코드 적기

						if (inventoryItemList[i].itemCount > 1)
							inventoryItemList[i].itemCount--;
						else
							inventoryItemList.RemoveAt(i);

						ShowItem();
						break;
					}
					else if (selectedTab == 1)
					{
						inventoryItemList.RemoveAt(i);
						ShowItem();
						break;
					}
				}
			}
		}
		stopKeyInput = false;
		go_OOC.SetActive(false);
	}
	#endregion

	#endregion

}


