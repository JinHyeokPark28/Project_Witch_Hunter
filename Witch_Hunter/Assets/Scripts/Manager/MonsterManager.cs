using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class MonsterManager :MonoBehaviour {
    //몬스터, 마녀마다 CSV파일 따로있음
	#region Private Variable
	private static MonsterManager _MonsterManager = null;
	private int HP = 0;                                     // 몬스터 HP
	private int Attack = 0;									// 몬스터 공격력
    public int Stage;   //스테이지 넘버에 따라 스폰하는 몬스터 종류 달라짐
    //각 씬의 오브젝트로부터 씬 넘버 받아옴
    public bool getSceneNumber;
    //각 씬에서 씬 넘버 받아오면 true, 한씬 넘어가면 바로 false로 바뀜
	private enum Contidion { BURN, FROZEN, KNOCKBACK };
    #endregion

    #region public Variable

    public TextAsset MonsterCSV;
	public List<GameObject> NormalMonsterList = new List<GameObject>();
    public List<GameObject> WitchList = new List<GameObject>();
    //public TextAsset MonsterCsv;    //몬스터 CSV파일
    public List<Vector2> RespawnPoint=new List<Vector2>();  //매 스테이지마다 리스폰위치 갯수들이 틀리니까 리스트로 선언해야할듯
    public CSVParser parsing;
    #endregion

    #region Private Method
    private void Start()
	{
        parsing.MonsterCSV = this.MonsterCSV;
        parsing.Start();
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
    private void Update()
    {
        if (Stage != SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = false;
        }
        else if (Stage == SceneManager.GetActiveScene().buildIndex)
        {
            getSceneNumber = true;
        }
        if (getSceneNumber == false&& Stage != SceneManager.GetActiveScene().buildIndex)
        {
            RespawnPoint.Clear();
            Stage = SceneManager.GetActiveScene().buildIndex;
            //스테이지 바뀌면 리스폰 포인트 새로 받아옴
            //일반 몬스터 리젠
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("Respawn").Length; i++)
            {
                RespawnPoint.Add(GameObject.FindGameObjectsWithTag("Respawn")[i].transform.position);
                
                print("RespawnPoint:" + RespawnPoint[i]);
            }
            switch (Stage)
            {
                case 0:
                    print("Scene:" + Stage);
                    break;
                case 1:
                    break;
                case 2:
                    break;
               
            }
        }
       
    }
   
    private void m_Attack(){
	}
	#endregion

	#region public Method
	public static MonsterManager GetMonsterManager{
	get{ return _MonsterManager; }
	}
	#endregion

}
