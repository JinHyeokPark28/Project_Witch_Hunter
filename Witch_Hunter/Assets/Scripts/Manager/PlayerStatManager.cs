using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour {
	#region Variable
	public static PlayerStatManager instance;
    private Equipment theEquip;
	public int hp;
	public int currentHP;

	public int atk;
	public int def;
	//public int recover_hp;
	//public int WeaponType;
	//enum WeaponItem
	//{
	//	sword,
	//	bong,
	//	NonSelect,
	//}
	//WeaponItem WeaponTypeState = WeaponItem.NonSelect;
	//WeaponTypeState = (WeaponItem) WEaponcount;
	#endregion
	#region Method
	private void Start()
	{
        theEquip = GetComponent<Equipment>();
        instance = this;
    }
	private void Update()
	{

		//atk = theEquip.equipItemList[theEquip.selectedSlot].atk;
		//def = theEquip.equipItemList[theEquip.selectedSlot].def;
	}
	#endregion
}
