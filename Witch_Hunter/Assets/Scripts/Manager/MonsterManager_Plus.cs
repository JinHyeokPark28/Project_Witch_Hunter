using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class MonsterManager_Plus : MonoBehaviour
{
    //몬스터, 마녀마다 CSV파일 따로있음
    #region Private Variable
    private static MonsterManager_Plus _MonsterManager_Plus = null;
  public int Stage;   //스테이지 넘버에 따라 스폰하는 몬스터 종류 달라짐
    //각 씬의 오브젝트로부터 씬 넘버 받아옴
    public bool getSceneNumber;
    //각 씬에서 씬 넘버 받아오면 true, 한씬 넘어가면 바로 false로 바뀜
    private enum Contidion { BURN, FROZEN, KNOCKBACK };
    #endregion
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
    private int RandomNum;  //랜덤하게 각 스테이지에서 생성할 몬스터 인덱스 변수
    public List<GameObject> NormalMonsterList = new List<GameObject>();
    //public TextAsset MonsterCsv;    //몬스터 CSV파일
    public List<Vector2> RespawnPoint = new List<Vector2>();  //매 스테이지마다 리스폰위치 갯수들이 틀리니까 리스트로 선언해야할듯
                                                              //public CSVParser parsing;->이거 하려면 반드시 CSVPArser가 있는 객체가 필요함
    #endregion
   
        //NormalListMonster에 반드시 인덱스에 맞게 배치되어야함!!!
    #region Private Method
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
        }
        else if (Stage == SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = true;
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        {
            for (int j = 0; j < GameObject.FindGameObjectsWithTag("Enemy").Length; j++)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>() != null)
                {
                    //AddComponent로 해서 에러났음 inputstringERror도 add로 해서 그랬음
                    //기존에 존재하는 몬스터가 가진 스크립트에 값을 넣어주는 것
                    int x = GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().index;
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().Name = data[x, 1];
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[x, 3]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().HP = Convert.ToInt32(data[x, 2]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().MonsterType = Convert.ToInt32(data[x, 4]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().NormalSpeed = Convert.ToSingle(data[x, 5]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[x, 6]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>()._AttackSpeed = Convert.ToSingle(data[x, 7]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>()._CheckDelay = Convert.ToSingle(data[x, 8]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().Recon = Convert.ToBoolean(data[x, 10]);
                    GameObject.FindGameObjectsWithTag("Enemy")[j].GetComponent<MonstersAI_FIXED>().GetInfo = true;

                }
            }
        }
        MakingMonster(1);
    }
    public void Update()
    {
        //미리 생성된 몬스터들에게 파싱
       
        if (Stage != SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = false;
        }
        else if (Stage == SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = true;
        }
        if (getSceneNumber == false && Stage != SceneManager.GetActiveScene().buildIndex)
        {
            RespawnPoint.Clear();
            Stage = SceneManager.GetActiveScene().buildIndex;
            //스테이지 바뀌면 리스폰 포인트 새로 받아옴
            //일반 몬스터 리젠
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Respawn").Length; i++)
            {
                RespawnPoint.Add(GameObject.FindGameObjectsWithTag("Respawn")[i].transform.position);
                
            }
            print("Scene:" + Stage);
            for (int i = 0; i < RespawnPoint.Count; i++)
            {
                RandomNum = UnityEngine.Random.Range(0, NormalMonsterList.Count);
                Instantiate(NormalMonsterList[RandomNum], new Vector3(RespawnPoint[i].x, RespawnPoint[i].y + 3, transform.position.z), transform.rotation);
            }
            
      
        }

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
            if (NormalMonsterList[x].GetComponent<MonstersAI_FIXED>()== null)
            {
                //스크립트 컴포넌트가 없다면
                NormalMonsterList[x].AddComponent<MonstersAI_FIXED>();
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().index = Convert.ToInt32(data[i, 0]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().Name = data[i, 1];
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[i, 3]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().HP = Convert.ToInt32(data[i, 2]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().MonsterType = Convert.ToInt32(data[i, 4]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().NormalSpeed = Convert.ToSingle(data[i, 5]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().attack = Convert.ToInt32(data[i, 6]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>()._AttackSpeed = Convert.ToSingle(data[i, 7]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>()._CheckDelay= Convert.ToSingle(data[i, 8]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().Recon = Convert.ToBoolean(data[i, 10]);

                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().GetInfo = true;
            }
            else if(NormalMonsterList[x].GetComponent<MonstersAI_FIXED>() != null)
            {
                //스크립트 컴포넌트가 있다면
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().index = Convert.ToInt32(data[i, 0]);
                NormalMonsterList[x].GetComponent<MonstersAI_FIXED>().Name = data[i, 1];
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
    private void m_Attack()
    {
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
