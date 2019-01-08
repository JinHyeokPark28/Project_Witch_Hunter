using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MENUS {
	INVENTORY,
	EQUIPMENT,
	PLAYERSTAT,
	OPTION,
	MINIMAP,
};
public class MenuManager : MonoBehaviour
{
	#region Private Variable
	private bool Activated = false;
	#endregion

	#region Public Variable
	public MENUS e_Menus = MENUS.INVENTORY;
	public static MenuManager instance = null;

	public GameObject m_Inventory;
	public GameObject m_Equipment;
	public GameObject m_PlayerStat;
	public GameObject m_Option;
	public GameObject m_Minimap;
	#endregion

	#region Private Method
	private void Awake()
	{
		#region 싱글톤 설정
		if (instance == null)
		{
			instance = this;
		}
		else if(instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		#endregion
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.I))					// 인벤토리 창
		{
			Activated = !Activated;
			if(Activated == true)
			{
				m_Inventory.SetActive(true);
				m_PlayerStat.SetActive(false);
				m_Option.SetActive(false);
				m_Minimap.SetActive(false);
			}
			else if(Activated == false)
			{
				m_Inventory.SetActive(false);
				m_PlayerStat.SetActive(false);
				m_Option.SetActive(false);
				m_Minimap.SetActive(false);
			}
		}
		else if(Input.GetKeyDown(KeyCode.P))            // 플레이어 상태창
		{
			Activated = !Activated;
			if (Activated == true)
			{
				m_PlayerStat.SetActive(true);
				m_Inventory.SetActive(false);
				m_Option.SetActive(false);
				m_Minimap.SetActive(false);
			}
			else if (Activated == false)
			{
				m_PlayerStat.SetActive(false);
				m_Inventory.SetActive(false);
				m_Option.SetActive(false);
				m_Minimap.SetActive(false);
			}

		}
		else if(Input.GetKeyDown(KeyCode.Escape))       // 옵션창
		{
			Activated = !Activated;
			if (Activated == true)
			{
				m_Option.SetActive(true);
				m_PlayerStat.SetActive(false);
				m_Inventory.SetActive(false);
				m_Minimap.SetActive(false);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else if (Activated == false)
			{
				m_Option.SetActive(false);
				m_PlayerStat.SetActive(false);
				m_Inventory.SetActive(false);
				m_Minimap.SetActive(false);
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}

		}
		else if(Input.GetKeyDown(KeyCode.Tab))          // 미니맵창
		{
			Activated = !Activated;
			if (Activated == true)
			{
				m_Minimap.SetActive(true);
				m_PlayerStat.SetActive(false);
				m_Inventory.SetActive(false);
				m_Option.SetActive(false);
			}
			else if (Activated == false)
			{
				m_Minimap.SetActive(false);
				m_PlayerStat.SetActive(false);
				m_Inventory.SetActive(false);
				m_Option.SetActive(false);
			}

		}
	}
	#endregion

	#region Public Method
	public static MenuManager GetMenuManager
	{
		get { return instance; }
	}
	#endregion

}
