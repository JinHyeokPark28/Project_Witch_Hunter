using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class MonsterManager_Plus : CSVParser
{
    //몬스터, 마녀마다 CSV파일 따로있음
    #region Private Variable
    private static MonsterManager _MonsterManager = null;
  public int Stage;   //스테이지 넘버에 따라 스폰하는 몬스터 종류 달라짐
    //각 씬의 오브젝트로부터 씬 넘버 받아옴
    public bool getSceneNumber;
    //각 씬에서 씬 넘버 받아오면 true, 한씬 넘어가면 바로 false로 바뀜
    private enum Contidion { BURN, FROZEN, KNOCKBACK };
    #endregion

    #region public Variable
    private int RandomNum;  //랜덤하게 각 스테이지에서 생성할 몬스터 인덱스 변수
    public List<GameObject> NormalMonsterList = new List<GameObject>();
    public List<GameObject> WitchList = new List<GameObject>();
    //public TextAsset MonsterCsv;    //몬스터 CSV파일
    public List<Vector2> RespawnPoint = new List<Vector2>();  //매 스테이지마다 리스폰위치 갯수들이 틀리니까 리스트로 선언해야할듯
    //public CSVParser parsing;->이거 하려면 반드시 CSVPArser가 있는 객체가 필요함
    #endregion
    #region Private Method
    public  void Start()
    {
        //_CSV = CSVParser.GetCSVParser;
        //parsing.MonsterCSV = this.MonsterCSV;
        //parsing.Start();
        //csvParser는 담겨있는 거기 때문에 한번만 불러옴
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
    }
    public new void Update()
    {
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

                print("RespawnPoint:" + RespawnPoint[i]);
            }
            switch (Stage)
            {
                case 0:
                    RandomNum=UnityEngine.Random.Range(1, 3);
                    print("RanNum:" + RandomNum);
                    print("Scene:" + Stage);
                    break;
                case 1:
                    RandomNum = UnityEngine.Random.Range(1, 3);
                    print("RanNum:" + RandomNum);
                    print("Scene:" + Stage);
                    break;
                case 2:
                    RandomNum = UnityEngine.Random.Range(1, 3);
                    print("RanNum:" + RandomNum);
                    print("Scene:" + Stage);
                    break;

            }
        }

    }

    private void m_Attack()
    {
    }
    #endregion

    #region public Method
    public static MonsterManager GetMonsterManager
    {
        get { return _MonsterManager; }
    }
    #endregion

}
