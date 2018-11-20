using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	#region Private Variable
	private static GameManager _GameManager = null;

	

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

}
