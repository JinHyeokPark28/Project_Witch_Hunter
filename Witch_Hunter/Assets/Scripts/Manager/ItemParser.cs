using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//StreamReader클래스를 쓰기 위한 라이브러리

public class ItemParser : MonoBehaviour
{
	#region 변수 목록
	public TextAsset Item_List;
	//텍스트파일 받는 MonsterCSV->여기다가 CSV넣으면 됨
	public string[] option;
	//CSV 옵션(맨 윗줄)받는 option변수
	public string[,] data;
	//콤마 기준으로 원소 하나마다 엑셀 한칸 씩 받는 data 2차원 배열
	public string[] TextArray;
	//엔터 기준으로 한줄 씩 받는 TextArray 배열
	public string WholeText;
	//CSV파일의 전체 텍스트 받는 스트링 변수
	public int CellLength;
	//셀이 한 세로 줄에 몇 칸이나 있는지)
	public int CellHeight;
	//셀이 한 가로 줄에 몇 칸이나 있는지)
	#endregion
	// Use this for initialization
	public void Start()
	{
		WholeText = Item_List.text;
		//WholeText에 MonsterCSV에 있는 텍스트를 넣음
		TextArray = WholeText.Split('\n');
		//TextArray 원소 하나마다 WholeText를 한 줄씩 쪼개서 넣어줌
		CellLength = TextArray.Length;
		//셀이 한 세로줄에 몇 개 있는가?:TextArray가 몇 개나 있는지(전체 텍스트를 엔터로 나눈 길이)
		CellHeight = TextArray[0].Split(',').Length;
		//셀이 한 가로줄에 몇개 있는가?: TextArray의 한 줄을 콤마 기준으로 나눴을 때의 길이
		data = new string[CellLength, CellHeight];
		//data 변수 선언[세로 길이, 가로 길이]
		for (int i = 0; i < CellLength; i++)
		{
			//한칸씩 넣어줌
			for (int j = 0; j < CellHeight; j++)
			{
				data[i, j] = TextArray[i].Split(',')[j];
			}
		}


	}

	public void GetOption(int index)
	{
		//첫 줄만 읽어오도록 하는 함수
		option = TextArray[index].Split(',');
	}

}
