using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//StreamReader클래스를 쓰기 위한 라이브러리

public  class  CSVParser : MonoBehaviour {
    #region 변수 목록
    public static CSVParser _CSV = null;
    public TextAsset MonsterCSV;
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
    public  void Start() {
        WholeText = MonsterCSV.text;
        //WholeText에 MonsterCSV에 있는 텍스트를 넣음
        TextArray = WholeText.Split('\n');
        //TextArray 원소 하나마다 WholeText를 한 줄씩 쪼개서 넣어줌
        CellLength = TextArray.Length;
        //셀이 한 세로줄에 몇 개 있는가?:TextArray가 몇 개나 있는지(전체 텍스트를 엔터로 나눈 길이)
        CellHeight = TextArray[0].Split(',').Length;
        //셀이 한 가로줄에 몇개 있는가?: TextArray의 한 줄을 콤마 기준으로 나눴을 때의 길이
        GetOption(0);
        //option에 맨 윗줄 한칸 씩 넣어주는 변수
        data = new string[CellLength, CellHeight];
        //data 변수 선언[세로 길이, 가로 길이]
        for(int i = 0; i < CellLength; i++)
        {
            //한칸씩 넣어줌
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

/*//Monster[] M;
        //M = new Monster[CellLength - i];
        List<MonstersAI_FIXED> M = new List<MonstersAI_FIXED>();
        //M = new List<MonsterClass>[CellLength - i];
        //M = new MonsterClass[CellLength - i];
        for (int x = i; x < CellLength; x++)
        {
            //x=1~7
            //j=모든 csv가로줄 길이-1->0~6(실제 cSV에서는 1~7)
            int j = 0;
            // M[j] = new Monster();   //이거 안해줘서 에러났음;;
            M.Add(new MonstersAI_FIXED());
            //모노비헤이비어 상속받은 클래스는 new 키워드를 사용하여 생성할 수 없습니다. 
            //해당 스크립트가 붙은 프리팹을 Instantiate를 사용하여 오브젝트 생성 형태로 만들어야 합니다 
            //M[j] = new MonstersAI_FIXED();
            //클래스 꼭 하나씩 초기화 시키자;;
            M[j].index = Convert.ToInt32(data[x, 0]);
            M[j].name = data[x, 1];
            M[j].attack = Convert.ToInt32(data[x, 2]);
            M[j].HP = Convert.ToInt32(data[x, 3]);
            M[j].MonsterType = Convert.ToInt32(data[x, 4]);
            print("index:" + M[j].index);
            //M[j].IsBoss = Convert.ToBoolean(data[x, 4]);
            // print("NAME:" + M[j].name+","+"LEVEL"+ M[j].level+","+"ATTACK:"+ M[j].Attack);
            print("COUNT:" + M.Count);

            if (j < CellLength - i) //CellLength:8-i(1)=7
            {
                j++;
            }
            else
            {
                break;
                */
