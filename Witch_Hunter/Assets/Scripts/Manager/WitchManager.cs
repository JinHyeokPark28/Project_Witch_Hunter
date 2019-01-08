using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class WitchManager : MonoBehaviour
{
    public int Stage;   //스테이지 넘버에 따라 스폰하는 몬스터 종류 달라짐
    //각 씬의 오브젝트로부터 씬 넘버 받아옴
    public bool getSceneNumber;
    //각 씬에서 씬 넘버 받아오면 true, 한씬 넘어가면 바로 false로 바뀜
    public bool CanGoFinal; //세 마녀를 다 쓰러뜨리면 true가 되어서 최종보스인 광기의 마녀를 만날수 있다;
    public int KillWitchNumber=0;    //특정 수 이상이면 최종 보스 만남 가능
	public S_Witch _Witch;
    #region CSV 변수 목록
    public TextAsset WitchCSV;
    public string[] option;
    public string[,] data;
    public string[] TextArray;
    //한 줄
    public string WholeText;
    public int CellLength;
    public int CellHeight;
    public GameObject Witch;
    public bool WitchExist; //마녀 오브젝트가 존재하는지
    public int NowStage;    //현재 스테이지 번호
    #endregion

    private enum Contidion
    {
        BURN, FROZEN, KNOCKBACK

    };
    public enum S_Witch
    {
        ALCHEMIST, WIND, WATER, FIRE

    };
    // Use this for initialization
    void Start()
    {
        //시작할 때 받는 씬번호=현재 열린 씬 번호
        NowStage = SceneManager.GetActiveScene().buildIndex;
        
        WholeText = WitchCSV.text;
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
        //마녀가 있는 스테이지에 돌입->마녀 받음
        //아니면 스테이지가 바뀌고 마녀 새로 받음
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NowStage != SceneManager.GetActiveScene().buildIndex)
        {
            WitchExist = false;
        }
        if (WitchExist == false)
        {
            if (Witch != null)
            {
                Witch = null;
            }
            if (GameObject.FindGameObjectWithTag("Witch") != null&&GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchClass>().getInfo==false)
            {
                Witch = GameObject.FindGameObjectWithTag("Witch");
                ReadingWitch(Witch.GetComponent<WitchClass>().index);
                NowStage = SceneManager.GetActiveScene().buildIndex;
            }
        }
        if (KillWitchNumber >= 4)   //광기의 마녀 뺀 다른 마녀들을 모두 죽였으면
        {
            CanGoFinal = true;  //광기의 마녀 단계 해금
        }
	}
	public void GetOption(int index)
    {
        //첫 줄만 읽어오도록 하는 함수
        option = TextArray[index].Split(',');
    }
	public void CheckWitch(S_Witch witch)
	{
		_Witch = witch;
	}
    public void ReadingWitch(int i)
    {
        if (Witch.GetComponent<WitchClass>() != null)
        {
            //현재 존재하는 마녀 오브젝트의 인덱스 번호로부터 csv파일에 해당 인덱스의 정보를 불러와 마녀 오브젝트에 넣어주는 함수
            Witch.GetComponent<WitchClass>().Name = data[i, 1]; //name과 다른 변수임!!Name은 스크립트상에서 새로 생성한 변수이름임!!
            Witch.GetComponent<WitchClass>().HP = Convert.ToInt32(data[i, 2]);
            Witch.GetComponent<WitchClass>().Attack = Convert.ToInt32(data[i, 3]);
            Witch.GetComponent<WitchClass>().Coin = Convert.ToInt32(data[i, 4]);
            Witch.GetComponent<WitchClass>().isFinalBoss = Convert.ToBoolean(data[i, 5]);
            Witch.GetComponent<WitchClass>().Stage_Location = Convert.ToInt32(data[i, 7]);
            Witch.GetComponent<WitchClass>().getInfo = true;
        }
    }
}