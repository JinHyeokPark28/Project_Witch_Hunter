using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {

	private Item _Item;
	private bool isCheck = false;

	public GameObject SwordPanel;
	public GameObject ArmorPanel;
	public GameObject AmmoPanel;
	
	public Text Sword;
	public Image SwordImage;
	public Text Ammo;
	public Image AmmoImage;
	public Text Armor;
	public Image ArmorImage;
	public Text Attack;
	public Text SubAttack;
	public Text Defend;
	public static Equipment instance = null;
	private void Awake()
	{
		_Item = FindObjectOfType<Item>();
	}
	private void Start()
	{
		instance = this;
		Sword.text = "무기 1";
		Ammo.text = "사용불가";
		Armor.text = "갑옷";
	}
	private void TakeOff()
	{ 
		if(Input.GetMouseButton(0))
		{
			
		}
	}
}
