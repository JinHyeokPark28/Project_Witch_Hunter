using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//대화창 UI
//진혁씨 이거 손대지 마셈
public class TalkingTextParser : MonoBehaviour {
    public TextAsset TalkingTextCSV;
    public int TextHeightLength;   //한 세로줄에 셀 몇개 들어가있는지
    public int TextWidthLength;    //한 가로줄에 셀 몇개 들어가있는지
                                   // Use this for initialization
    public int StartIndex=-1;  //어디서 대화 시작할 건지
    public List<List<string>> data = new List<List<string>>();

    public List<Sprite> CharacterBigImage = new List<Sprite>();
    //캐릭터 대화 이미지 넣을 리스트
    public List<string> Option = new List<string>();
    public bool StartTalking;   //PlayerController에 의해 true되면 자식 오브젝트들 활성화
    #region 자식 오브젝트들
    private GameObject PlayerImage;
    public GameObject CharacterImage;
    public GameObject BlackImage;
    public GameObject CharacterName;
    public GameObject PlayerName;
    public GameObject CharacterText;
    #endregion
    void Start () {
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
        ManagingChildrenObjs();
	}
    void ManagingChildrenObjs()
    {
        if (StartTalking == true)
        {
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
            }
        }
        else
        {
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
    }
}
