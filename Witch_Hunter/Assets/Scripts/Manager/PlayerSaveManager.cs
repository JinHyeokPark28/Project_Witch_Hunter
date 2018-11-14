using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSaveManager : MonoBehaviour {

    //세이브 없음->Player정보 csv파일에 있는 스탯 불러옴(게임 처음 시작할때)
    //세이브 있음->플레이어가 세이브 선택함->해당 세이브 저장된 csv파일 로부터 플레이어 정보 불러옴
    //세이브 저장->새로 저장->새로 csv만듬
    //->기존에 있던 세이브에 저장->기존에 있는 csv파일에 덮어씌우기(기존 파일 csv찾는것 필요!)
    //저장해야 할것:현재 player위치, 현재 플레이어가 착용하고 있는 장비, 현재 가지고 있는 장비,플레이어hp 최대량, 현재 플레이어 hp량
    //플레이어가 죽인 마녀 목록, 플레이어가 죽인 몬스터 수, 현재 스테이지(!!!), 플레이 타임
    //(새로 저장할 경우)저장할 csv이름, 저장하는 시각(?)/(기존 csv파일에 덮어씌우는 경우)기존 csv이름
    //메인메뉴->새로하기/불러오기->csv목록들(저장할 csv수 제한)
    #region Private Variable

    #endregion

    #region Public Variable
    public TextAsset PlayerInfoCSV; //게임을 처음 시작할 때 불러올 플레이어 정보
    public string[] option;
    public string[,] data;
    public string[] TextArray;
    //한 줄
    public string WholeText;
    public int CellLength;
    public int CellHeight;
    public GameObject Player;
    #endregion

    #region Private Method
    private bool IsNewGame; //true면 player기본 정보 담긴 csv로부터 플레이어 정보 불러오고/ 아니면 기존에 저장된 파일로부터 불러오기 하는거라서 false
    //메인 메뉴의 버튼으로부터 값 불러옴
    #endregion

    #region Public Method
    public void Start()
    {
        WholeText = PlayerInfoCSV.text;
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
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Player != null)
        {
            if (IsNewGame == true)
            {
                NewGame(1);
            }
        }
    }
    public void GetOption(int index)
    {
        //첫 줄만 읽어오도록 하는 함수
        option = TextArray[index].Split(',');

    }
    public void NewGame(int i)
    {
      //여기에 플레이어 새 정보 불러오기->CSV로 안해도 될듯?
    }
	#endregion

}
