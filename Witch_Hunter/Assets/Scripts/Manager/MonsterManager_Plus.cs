using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class MonsterManager_Plus : MonoBehaviour
{
    //일일히 매니저 쪽에서 전체 보고 하는 것 보다는 
    //각 포인트 마다 스크립트 달아서 각 포인트마다 리스폰 하는게 나음
    //몬스터, 마녀마다 CSV파일 따로있음
    #region Private Variable
    private static MonsterManager_Plus _MonsterManager_Plus = null;
    private int Stage;   //스테이지 넘버에 따라 스폰하는 몬스터 종류 달라짐
    //각 씬의 오브젝트로부터 씬 넘버 받아옴
    private bool getSceneNumber;
    //각 씬에서 씬 넘버 받아오면 true, 한씬 넘어가면 바로 false로 바뀜
    private bool CreatingMonster = false;
    //리스폰 했으면 true
    private enum Contidion { BURN, FROZEN, KNOCKBACK };
    #endregion
    public int MonsterNumber = -1;   //랜덤이 아니라 정해놓고 만들고싶은 몬스터 리스트 넘버

    #region CSV 변수 목록
    public TextAsset MonsterCSV;
    public string[] option;
    public string[,] data;
    public string[] TextArray;
    //한 줄
    public string WholeText;
    public int CellLength;
    public int CellHeight;
    #endregion
    #region public Variable
    public List<GameObject> NormalMonsterList = new List<GameObject>();
    //public TextAsset MonsterCsv;    //몬스터 CSV파일
    //public List<Vector2> RespawnPoint = new List<Vector2>(); 
    //매 스테이지마다 리스폰위치 갯수들이 틀리니까 리스트로 선언해야할듯
                                                              //public CSVParser parsing;->이거 하려면 반드시 CSVPArser가 있는 객체가 필요함
    #endregion
   //리스폰 포인트마다 각자 몬스터 생성하는 걸로 바꾸기
        //NormalListMonster에 반드시 인덱스에 맞게 배치되어야함!!!
    public void Start()
    {
        WholeText = MonsterCSV.text;
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
        
        Stage = -1;
        //스테이지는 1부터 시작하기 때문에 강제로 모든 씬 초기화 하기 위해 -1로 잡아줌
        if (Stage != SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = false;
            Stage = SceneManager.GetActiveScene().buildIndex;
            
        }
        else if (Stage == SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = true;
        }
        MakingMonster(1);
        
    }
    public void Update()
    {
        if (getSceneNumber == false && Stage == SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = true;
        }
        #region 몬스터 생성함수
        if (CreatingMonster == false)
        {
            int RandNum = UnityEngine.Random.Range(0, NormalMonsterList.Count);
            if (getSceneNumber == true)
            {
                if (MonsterNumber == -1)
                {
                    //특정 몬스터 번호가 주어지지 않았다면(몬스터 인덱스1부터 존재)
                    Instantiate(NormalMonsterList[RandNum], new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 2), new Quaternion(0, 0, 0, 0));
                    CreatingMonster = true;
                }
                else
                {
                    //만약 그대로-1이라면
                    print("CREATE");
                    Instantiate(NormalMonsterList[MonsterNumber], new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 2), new Quaternion(0, 0, 0, 0));
                    CreatingMonster = true;
                }
            }
        }
        #endregion
    }


    #region CSV함수
    public void GetOption(int index)
    {
        //첫 줄만 읽어오도록 하는 함수
        option = TextArray[index].Split(',');

    }
    public void MakingMonster(int j)
    {
        //생성한 노말 몬스터 리스트만큼 만들기
        //==노말 몬스터 리스트에 넣어주기 
        //에러->첫번째로 생성한 리스폰 포인트 몬스터 빼고 나머지 인덱스2부터 정보 안들어감
        int x = 0;
        for(int i = j; i < NormalMonsterList.Count+1; i++)
        {
             //j=1
                if ((NormalMonsterList[x].GetComponent<MonsterAI_Moving>() != null)
                    && (NormalMonsterList[x].GetComponent<MonstersAI_FIXED>() == null))
                {
                    //움직일 수 있는 몬스터인 경우->그런 스크립트만 가지고 있는 경우
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().index = Convert.ToInt32(data[i, 0]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().MonsterName = data[i, 1];
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().attack = Convert.ToInt32(data[i, 3]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().HP = Convert.ToInt32(data[i, 2]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().MonsterType = Convert.ToInt32(data[i, 4]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().NormalSpeed = Convert.ToSingle(data[i, 5]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().attack = Convert.ToInt32(data[i, 6]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>()._AttackSpeed = Convert.ToSingle(data[i, 7]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>()._CheckDelay = Convert.ToSingle(data[i, 8]);
                    NormalMonsterList[x].GetComponent<MonsterAI_Moving>().GetInfo = true;
                }
                else if ((NormalMonsterList[x].GetComponent<MonstersAI_FIXED>() != null) &&
                    (NormalMonsterList[x].GetComponent<MonsterAI_Moving>() == null))
                {
                    //움직일 수 없는 고정형 몬스터 인 경우
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().index = Convert.ToInt32(data[i, 0]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().MonsterName = data[i, 1];
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[i, 3]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().HP = Convert.ToInt32(data[i, 2]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().MonsterType = Convert.ToInt32(data[i, 4]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().NormalSpeed = Convert.ToSingle(data[i, 5]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[i, 6]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>()._AttackSpeed = Convert.ToSingle(data[i, 7]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>()._CheckDelay = Convert.ToSingle(data[i, 8]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().Recon = Convert.ToBoolean(data[i, 10]);
                    NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().GetInfo = true;

                }
                if (x < NormalMonsterList.Count)
                {
                    x++;
                }
                else if (x >= NormalMonsterList.Count)
                {
                    break;
                }

          
        }

    }
    #endregion
    
    #region public Method
    public static MonsterManager_Plus GetMonsterManager
    {
        get { return _MonsterManager_Plus; }
    }

    #endregion
    //1.미리 게임오브젝트[]만든거 있음->인덱스 맞춰서 넣으려고
   //csv로부터 각각 monsterAI_FIXED에 맞는 정보 파싱->어디에 저장?->만들어진 몬스터 게임오브젝트의 스크립트에?
}
