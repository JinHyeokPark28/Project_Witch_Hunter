﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OOC : MonoBehaviour
{

	public GameObject up_Panel;
	public GameObject down_Panel;

	public Text up_Text;
	public Text down_Text;

	public bool activated;
	private bool keyInput;
	private bool result = true;


	// Use this for initialization
	void Start()
	{

	}

	public void Selected()
	{

		result = !result;

		if (result)
		{
			up_Panel.gameObject.SetActive(false);
			down_Panel.gameObject.SetActive(true);
		}
		else
		{
			up_Panel.gameObject.SetActive(true);
			down_Panel.gameObject.SetActive(false);
		}
	}
	public void ShowTwoChoice(string _upText, string _downText)
	{
		activated = true;
		result = true;
		up_Text.text = _upText;
		down_Text.text = _downText;
		up_Panel.gameObject.SetActive(false);
		down_Panel.gameObject.SetActive(true);

		StartCoroutine(ShowTwoChoiceCoroutine());
	}
	public bool GetResult(){
		return result;
	}
	IEnumerator ShowTwoChoiceCoroutine()
	{
		yield return new WaitForSeconds(0.01f);
		keyInput = true;
	}

	// Update is called once per frame
	void Update()
	{

		if (keyInput)
		{
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				Selected();
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				Selected();
			}
			else if (Input.GetKeyDown(KeyCode.Return))
			{
				keyInput = false;
				activated = false;
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				keyInput = false;
				activated = false;
				result = false;
			}
		}
	}
}
