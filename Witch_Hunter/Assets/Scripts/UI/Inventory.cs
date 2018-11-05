using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

	// 오디오 매니저 추가하기
	// keysound
	// enter sound
	// cancel sound
	// open sound
	// beep sound

	#region Private Varibale
	private InventorySlot[] slots;          // 인벤토리 슬롯들

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
	public GameObject Player;
	public Text Description_Text;           // 부연 설명
	public string[] tabDescription; // 탭 부연설명

	public Transform tf;            // slot의 부모객체(Grid slot)

	public GameObject go; // 인벤토리 활성화 비활성화
	public GameObject[] selectedTabImages;		// 골라진 탭 이미지

	#endregion




	// Use this for initialization
	void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		inventoryItemList = new List<Item>();
		inventoryTabList = new List<Item>();
		slots = tf.GetComponentsInChildren<InventorySlot>();

		inventoryItemList.Add(new Item(10001, "빨간 포션", "체력을 어느정도 천천히 채워주는 물약", Item.ItemType.Use, 10));
		inventoryItemList.Add(new Item(10002, "하얀 포션", "체력을 단숨에 채워주는 물약", Item.ItemType.Use));
		inventoryItemList.Add(new Item(10003, "수습기사의 장검", "수습 기사 서약식때 받을수 있는 검", Item.ItemType.Equip));

	}
	#region Public Method
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
					if (Item.ItemType.Use == inventoryItemList[i].itemType) 
						inventoryTabList.Add(inventoryItemList[i]);
				}
				break;
			case 1:
				for (int i = 0; i < inventoryItemList.Count; i++)
				{
					if (Item.ItemType.Equip == inventoryItemList[i].itemType)
						inventoryTabList.Add(inventoryItemList[i]);
				}
				break;
			case 2:
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
			for (int i = 0; i < inventoryTabList.Count; i++){
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
			// Update is called once per frame
	void Update()
	{
		if (!stopKeyInput)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				activated = !activated;
				if (activated)
				{
					// 오디오 파일 넣기
					// 플레이어 못움직이게 하기
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
							if (selectedItem > 11)
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
								stopKeyInput = true;

							}
							else if (selectedItem == 1)
							{
								// 장비 장착
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
	}
}
#endregion