using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//StreamReader클래스를 쓰기 위한 라이브러리

public abstract class  CSVParser : MonoBehaviour {
    #region 변수 목록
    public static CSVParser _CSV = null;
    public TextAsset MonsterCSV;
    public string[] option;
    public string[,] data;
    public string[] TextArray;
    //한 줄
    public string WholeText;
    public int CellLength;
    public int CellHeight;
    #endregion
    // Use this for initialization
    public  void Start() {
        WholeText = MonsterCSV.text;
        TextArray = WholeText.Split('\n');
        CellHeight = TextArray[0].Split(',').Length;
        //가로 길이
        CellLength = TextArray.Length;
        GetOption(0);
        data = new string[CellLength, CellHeight];
        for(int i = 0; i < CellLength; i++)
        {
            for(int j = 0; j < CellHeight; j++)
            {
                data[i, j] = TextArray[i].Split(',')[j];
            }
        }
        print(CellLength);
       
    }

    // Update is called once per frame
   public  void Update() {

    }
    public void GetOption(int index)
    {
        //첫 줄만 읽어오도록 하는 함수
        option = TextArray[index].Split(',');
        
    }
    public void MakingMonster(int i)
    {
        Monster[] M;
        M = new Monster[CellLength - i];
        for(int x = i; x < CellLength; x++)
        {
            int j = 0;
            M[j] = new Monster();   //이거 안해줘서 에러났음;;
            //클래스 꼭 하나씩 초기화 시키자;;
            M[j].index= Convert.ToInt32(data[x, 0]);
            M[j].name = data[x, 1];
            M[j].Attack = Convert.ToInt32(data[x, 2]);
            M[j].HP = Convert.ToInt32(data[x, 3]);
            M[j].IsBoss = Convert.ToBoolean(data[x, 4]);
           // print("NAME:" + M[j].name+","+"LEVEL"+ M[j].level+","+"ATTACK:"+ M[j].Attack);
            if (j < CellLength - i)
            {
                j++;
            }
            else
            {
                break;
            }
            
        }
       
    }
    public static CSVParser GetCSVParser
    {
        get { return _CSV; }
    }
   
}
public class Monster
{
    public int index;
    public string name;
    public int Attack;
    public int HP;
    public bool IsBoss;
}


