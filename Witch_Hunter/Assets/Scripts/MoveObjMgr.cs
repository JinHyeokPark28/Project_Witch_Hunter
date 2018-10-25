using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjMgr : MonoBehaviour
{
    public List<GameObject> objList = new List<GameObject>();
    public GameObject m_Player;
    public GameObject[] Monster;
    // Use this for initialization
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        objList.Add(GameObject.FindGameObjectWithTag("Player"));
        Monster = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            objList.Add(Monster[i]);
        }
        foreach (GameObject name in objList)
        {
            //name은 Gameobject가 가지고 있는 요소 이름
            //list의 모든 요소의 이름을 출력->제대로 들어갔음
            print(name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Player.GetComponent<PlayerController>().MapOn == true)
        {
            //speed=0으로 주기
            objList[0].GetComponent<PlayerController>().Speed = 0;
            for (int i = 0; i < objList.Count; i++)
            {
                if (objList[i].tag == "Enemy")
                {
                    objList[i].GetComponent<EnemyChaser>().Speed = 0;
                }
            }
        }
        else
        {
            objList[0].GetComponent<PlayerController>().Speed = 5;
            for (int i = 0; i < objList.Count; i++)
            {
                if (objList[i].tag == "Enemy")
                {
                    objList[i].GetComponent<EnemyChaser>().Speed = 2.5f;
                }
            }
        }


    }

}
