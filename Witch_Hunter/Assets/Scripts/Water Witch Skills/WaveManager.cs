using UnityEngine;
using System.Collections;
using System.Collections.Generic;   //List를 사용하기 위한 라이브러리

//파도 하나하나씩 관리하는 오브젝트
//물의 마녀에 의해 랜덤값으로 왼쪽 오른쪽중 하나가 활성화된다
public class WaveManager : MonoBehaviour
{
   public List<GameObject> WaveChildren=new List<GameObject>();
    //정렬로 다 위치 맞게 정렬하기
    public bool TurnOff = false;    //마지막 파도까지 활성화 되었으면 false 
    public bool IsLeftWave = true;  //왼쪽에서 오는 파도면 true,오른쪽에서 오면 false
    private bool SetAll = false;    //정렬 되면 true
                                    // Use this for initialization
    #region 2초에 한번씩 자식 오브젝트인 파도  켜주는 함수

    IEnumerator ActivatingWave()
    {
        if (IsLeftWave == true)
        {
            //만약 왼쪽에서 시작하는 파도라면
            int WaveNum = 0;
            while (WaveNum < WaveChildren.Count)
            {
                //존재하는 자식 오브젝트 갯수까지만 돌려줌
                WaveChildren[WaveNum].SetActive(true);
                WaveChildren[WaveNum].GetComponent<AquaWave>().WaveManager = this.gameObject;
                if (WaveChildren[WaveNum].GetComponent<AquaWave>().MyNum == -1)
                {
                    WaveChildren[WaveNum].GetComponent<AquaWave>().MyNum = WaveNum + 1;
                }
                WaveNum++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            //오른쪽에서 시작하는 파도라면
            int WaveNum = WaveChildren.Count-1;
            while (WaveNum >=0)
            {
                //존재하는 자식 오브젝트 갯수까지만 돌려줌
                WaveChildren[WaveNum].SetActive(true);
                if (WaveChildren[WaveNum].GetComponent<AquaWave>().MyNum == -1)
                {
                    WaveChildren[WaveNum].GetComponent<AquaWave>().MyNum = WaveNum + 1;
                }
                WaveChildren[WaveNum].GetComponent<AquaWave>().WaveManager = this.gameObject;
                WaveNum--;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    #endregion
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        if (TurnOff == true)
        {
            //GameObject.FindObjectOfType<WitchClass>().WaveActive = false;
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchClass>().WaveActive = true;
        TurnOff = false;
        //자기 자식만 찾아야함
       
        if (SetAll == false)
        {
            foreach (Transform child in transform)
            {
                //모든 자식 오브젝트 찾는 명령문(foreach로 돌림)
                //transform.child로 찾아서 거기 붙어있는 gameobject를 Gameobject 리스트에 하나씩 넣어줌!!!
                WaveChildren.Add(child.gameObject);
            }
            //정렬 할당위해 길이와 함께 선언선언선언~~
            //퀵정렬은 나중에 해보기
            for (int i = 0; i < WaveChildren.Count - 1; i++)
            {
                //정렬 알고리즘 시작~~~~
                if (WaveChildren[i].transform.position.x > WaveChildren[i + 1].transform.position.x)
                {
                    Swap(i, i + 1);
                }
                if (i == WaveChildren.Count - 2)
                {
                    SetAll = true;
                }
            }
        }
        StartCoroutine(ActivatingWave());
    }
    private void OnDisable()
    {
        //비활성화 되었을 때 켜지는 함수
        if (TurnOff == true)
        {
            TurnOff = false;
            
        }
    }
    void Swap(int x,int y)
    {
        //퀵정렬에 쓰일 스왑함수
        GameObject forSwap;
        forSwap = WaveChildren[x];
        WaveChildren[x] = WaveChildren[y];
        WaveChildren[y] = forSwap;

    }
    
    
}
