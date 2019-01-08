using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	#region Private Variable
	private static GameManager _GameManager = null;
   
	//private MonsterManager_Plus _MonsterManager = null;
	public ItemParser _ItemParser = null;


    public Text Gold;                                           // 골드 텍스트
	#endregion

	#region Public Variable
	public bool _CheckGold = false; 
	public bool _isDead = false;

	public int m_Gold = 0;										// 몬스터 골드 드랍

	public int w_Gold = 0;										// 마녀 골드 드랍
	#endregion

	#region Private Method
	private void Awake() {
		if (_GameManager == null)
			_GameManager = this;
		else if (_GameManager != null)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		//if (_MenuManager == null)
		//    _MenuManager.transform.Find("MenuManager").GetComponent<MenuManager>();
		//if (_MonsterManager == null)
		//    _MonsterManager = transform.Find("EnemyManager").GetComponent<MonsterManager_Plus>();
		if (_ItemParser == null)
			_ItemParser = GameObject.Find("GameManager").GetComponent<ItemParser>();

		//// 마우스 커서 잠금
		//Cursor.lockState = CursorLockMode.Locked;
		//// 마우스 커서 안보이게 하기
		//Cursor.visible = false;
    }
	#endregion

     #region Public Method
     public static GameManager GetGameManager{
      get{ return _GameManager; }
     }

	public void m_GetGold(int value)                          // 몬스터한테 얻는 돈
	{
		m_Gold += value;


		if (_CheckGold == true)
		{
			GameObject.Find("Canvas").gameObject.transform.Find("Coin").gameObject.SetActive(true);
		}
		if (_CheckGold == false)
		{
			GameObject.Find("Canvas").gameObject.transform.Find("Coin").gameObject.SetActive(false);
		}
		Gold.text = "G" + value.ToString("N0");

		print("_CheckGold + " + value);
	}
	/*public void w_GetGold(int value){                                    // 마녀에게서 얻는 돈
	

		m_Gold += value;

		Gold.text ="G" + w_Gold.ToString("N0");
	}*/
	#endregion


    //public MonsterManager_Plus monsterManager
    //{ get { return _MonsterManager; } }

    //public void Test()
    //{
    //    MonsterManager_Plus temp;

    //    temp = GameManager.GetGameManager.monsterManager;
        
    //    temp.
    //}

}
