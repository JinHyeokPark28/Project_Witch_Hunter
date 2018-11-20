using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallManager : MonoBehaviour {
	#region Private Variable
	private static HallManager _HallManager;

	private WitchManager _Witch;

	#endregion

	#region Public Variable
	#endregion

	#region Private Method
	private void Awake()
	{
		if (_HallManager == null) _HallManager = this;

		else					  Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

	}
	private void Start()
	{
		_Witch = FindObjectOfType<WitchManager>();
	}
	private void Update()
	{
		
	}
	#endregion

	#region Public Method
	public void CheckWitchType(WitchManager witch)
	{
		witch.CheckWitch(0);
	}
	public static HallManager GetHallManager
	{
		get { return _HallManager; }
	}
	#endregion

}
