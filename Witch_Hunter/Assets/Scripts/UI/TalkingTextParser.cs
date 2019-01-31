using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//대화창 UI
//진혁씨 이거 손대지 마셈
public class TalkingTextParser : MonoBehaviour {
    public TextAsset TalkingTextCSV;
    private int TextHeightLength;   //한 세로줄에 셀 몇개 들어가있는지
    private int TextWidthLength;    //한 가로줄에 셀 몇개 들어가있는지
                                   // Use this for initialization
    public int StartIndex=-1;  //어디서 대화 시작할 건지
    public List<List<string>> data = new List<List<string>>();

    public List<Sprite> CharacterBigImage = new List<Sprite>();
    //캐릭터 대화 이미지 넣을 리스트
    public List<string> Option = new List<string>();
    public bool StartTalking;   //PlayerController에 의해 true되면 자식 오브젝트들 활성화
  //  public bool ShowingOnce = false;
    //public bool TextShowingOnebyOne = false;
    //클릭하면 자동 true,false면 전체 텍스트 다 보여준것
    #region 자식 오브젝트들
    private GameObject PlayerImage;
    private GameObject CharacterImage;
    private GameObject BlackImage;
    private GameObject CharacterName;
    private GameObject PlayerName;
    private GameObject CharacterText;
    #endregion
    //IEnumerator ShowingTextSlowly()
    //{
    //    while(true)
    //    {
    //        for (int i = 0; i <= data[StartIndex][1].Length; i++)
    //        {
    //            print("index" + StartIndex);//->2부터 들어감
    //            CharacterText.GetComponent<Text>().text = data[StartIndex][1].Substring(0, i);
    //            if (i == data[StartIndex][1].Length)
    //            {
    //                break;
    //                //TextShowingOnebyOne = false;
    //            }
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }
    //}
    void Start () {
       
        if (StartIndex < 0)
        {
            StartIndex = 1;
        }
        TextHeightLength=TalkingTextCSV.text.Split('\n').Length;
        //TextArray 원소 하나마다 WholeText를 한 줄씩 쪼개서 넣어줌
        TextWidthLength = TalkingTextCSV.text.Split('\n')[0].Split(',').Length;

        PlayerImage = transform.Find("PlayerImage").gameObject;
        CharacterImage = transform.Find("CharacterImage").gameObject;
        BlackImage = transform.Find("BlackBackground").gameObject;
        CharacterName = transform.Find("CharacterName").gameObject;
        PlayerName = transform.Find("PlayerNAME").gameObject;
        CharacterText = transform.Find("CharacterText").gameObject;

        data.Clear();
        Option.Clear();
        for(int i = 0; i < TextWidthLength; i++)
        {
            Option.Add(TalkingTextCSV.text.Split('\n')[0].Split(',')[i]);
        }
        for(int i = 0; i < TextHeightLength; i++)
        {
            data.Add(new List<string>());
            for (int j = 0; j < TextWidthLength; j++)
            {
                data[i].Add(TalkingTextCSV.text.Split('\n')[i].Split(',')[j]);
            }
        }
        
    }

    // Update is called once per frame
    void Update () {
        if (StartTalking == true)
        {
            if (CharacterText.activeInHierarchy == true&&StartIndex<=TextHeightLength)
            {
             CharacterText.GetComponent<Text>().text = data[StartIndex][1];
            }
            if (data[StartIndex][0] != "Player")
            {
                CharacterName.GetComponent<Text>().text = data[StartIndex][0];
                PlayerImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                CharacterName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                PlayerName.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
                CharacterImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
            else
            {
                PlayerImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                PlayerName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                CharacterName.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
                CharacterImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        
        ManagingChildrenObjs();
        UpdatingDialogState();
	}
    #region 대화창 오브젝트들 관리
    void ManagingChildrenObjs()
    {
        #region 자식 오브젝트들 현재 활성화 상태 관리
        if (StartTalking == true)
        {
            if (data[StartIndex][0]== "WoundedSoldier")
            {
                CharacterImage.GetComponent<Image>().sprite = CharacterBigImage[0];
            }
            if (PlayerImage.activeInHierarchy == false)
            {
                PlayerImage.SetActive(true);
            }
            if (CharacterImage.activeInHierarchy == false)
            {
                CharacterImage.SetActive(true);
            }
            if (BlackImage.activeInHierarchy == false)
            {
                BlackImage.SetActive(true);
            }
            if (CharacterName.activeInHierarchy == false)
            {
                CharacterName.SetActive(true);
            }
            if (PlayerName.activeInHierarchy == false)
            {
                PlayerName.SetActive(true);
            }
            if (CharacterText.activeInHierarchy == false)
            {
                CharacterText.SetActive(true);
                //if (ShowingOnce == false)
                //{
                //    StartCoroutine(ShowingTextSlowly());
                //    ShowingOnce = true;
                //}
            }
        }
        else
        {
            //TextShowingOnebyOne = false;
            if (PlayerImage.activeInHierarchy == true)
            {
                PlayerImage.SetActive(false);
            }
            if (CharacterImage.activeInHierarchy == true)
            {
                CharacterImage.SetActive(false);
            }
            if (BlackImage.activeInHierarchy == true)
            {
                BlackImage.SetActive(false);
            }
            if (CharacterName.activeInHierarchy == true)
            {
                CharacterName.SetActive(false);
            }
            if (PlayerName.activeInHierarchy == true)
            {
                PlayerName.SetActive(false);
            }
            if (CharacterText.activeInHierarchy == true)
            {
                CharacterText.SetActive(false);
            }
        }
        #endregion
    }
    #endregion
    void UpdatingDialogState()
    {
        if (Input.GetKeyDown(KeyCode.Z) && StartTalking == true){
            if (Convert.ToBoolean(data[StartIndex][2]) != true)
            {
                StartIndex++;
                //StartCoroutine(ShowingTextSlowly());
                //if (TextShowingOnebyOne == false)
                //{
                //    TextShowingOnebyOne = true;
                //}
                //else
                //TextshowingOnebyone == true이면(글자 보여지고 있는 상태)
                //{
                //    z.input받으면 전체 다 보여주기
                //    TextShowingOnebyOne = false;
                //}
            }
            else
            {
                StartTalking = false;
            }
        }
    }
}
