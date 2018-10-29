using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	#region Private Variable
	private static GameManager _GameManager = null;
	#endregion

	#region Public Variable
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
	#endregion

}
