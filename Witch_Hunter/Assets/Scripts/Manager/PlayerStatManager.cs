using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour {
	#region Variable
	public static PlayerStatManager instance;

	public int hp;
	public int currentHP;

	public int atk;
	public int def;
	public int recover_hp;
	
	#endregion
	#region Method
	private void Start()
	{
		instance = this;
	}
	#endregion
}
