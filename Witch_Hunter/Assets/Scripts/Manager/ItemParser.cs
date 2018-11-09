using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemParser : MonoBehaviour
{
    //플레이어 인벤토리 스크립트로부터 어떤 아이템을 가졌는지 받아와야함->그럼 그 인덱스 정보 바탕으로 플레이어인벤토리스크립트
    //아이템 정보만을 담은 리스트
    //플레이어가 소유하고 있는지 아닌지 여부는 안달림
    public TextAsset ItemCSV;
    public string[] option;
    public string[,] data;
    public string[] TextArray;
    public string WholeText;
    public int CellLength;
    public int CellHeight;
    
    // Use this for initialization
    void Start()
    {
        if (ItemCSV != null)
        {
            WholeText = ItemCSV.text;
            TextArray = WholeText.Split('\n');
            CellHeight = TextArray[0].Split(',').Length;
            //가로 길이
            CellLength = TextArray.Length;
            GetOption(0);
            data = new string[CellLength, CellHeight];
            for (int i = 0; i < CellLength; i++)
            {
                for (int j = 0; j < CellHeight; j++)
                {
                    data[i, j] = TextArray[i].Split(',')[j];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetOption(int index)
    {
        //첫 줄만 읽어오도록 하는 함수
        option = TextArray[index].Split(',');
    }
}
