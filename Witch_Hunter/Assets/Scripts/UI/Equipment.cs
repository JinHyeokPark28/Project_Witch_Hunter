using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Equipment : MonoBehaviour
{

	private const int WEAPON = 0, GUN = 1, ARMOR = 2;
	private Inventory theInven;
	public Text[] text;
	public Image[] img_slots;

	public Item[] equipItemList;
	public bool inputKey;
	public bool activated;


	// Use this for initialization
	void Start()
	{
		theInven = FindObjectOfType<Inventory>();
	}

	// Update is called once per frame
	void Update()
	{
		if (inputKey)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				activated = !activated;

				if(activated)
				{

				}
				else
				{

				}
			}

		}
	}
}
