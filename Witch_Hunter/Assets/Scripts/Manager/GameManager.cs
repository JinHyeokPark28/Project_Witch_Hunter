using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	#region Private Variable
	private static GameManager _GameManager = null;
	#endregion

	#region Public Variable
	public Text TextGold;

	public Text TextTime;
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
		get { return _GameManager; }
	}
	public void RecordTime(){

		float rec_Time = 1;

		rec_Time += Time.deltaTime;


	}
	public void KillPoints(){
	//	int KillWitchTime = 0;

	}
	public void Gold(){

		int Gold = 0;
		Gold += Random.Range(0, 21);

		TextGold.text = Gold.ToString();
	}
	#endregion

}
