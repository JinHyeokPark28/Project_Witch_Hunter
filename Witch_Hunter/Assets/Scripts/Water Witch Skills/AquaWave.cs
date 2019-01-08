using UnityEngine;
using System.Collections;

public class AquaWave: MonoBehaviour
{
    public GameObject WaveManager;  //파도 관리하는 부모 오브젝트 받는 웨이브매니저 게임오브젝트
    private float speed = 10f;
    private bool MovingUp = true;
    //HaveToEnd=true면 함수 종료
    private bool HavetoEnd = false;
    private float y_pos = -11f;  //worldpositon
    public int MyNum=-1;
    //시작할 때 y 좌표값
    //충돌은 최소하로 하고싶고 다른 오브젝트를 안밀려나게 하고 싶어서 리지드바디 안붙이는게 나을것 같음
    //활성화 되자마자 바로 움직임!
    //파도 하나에 달린 스크립트
    //물의 마녀 스킬: 아쿠아 웨이브
    //움직이고 밑으로 다시 내려오면 비활성화


    // Use this for initialization
    #region 파도 방향 지정해주는 함수
    IEnumerator WaveMovingManager()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (MovingUp == false)
            {
                MovingUp = !MovingUp;
            }
            else if (MovingUp == true)
            {
                MovingUp = !MovingUp;
            }
            i++;
            if (i == 2)
            {
                //함수 두번 돌았으면->끝내기
                HavetoEnd = true;
                i = 0;
            }
        }
    }
    #endregion

    void Start()
    {
        WaveManager = this.gameObject.transform.parent.gameObject;
        transform.position = new Vector2(transform.position.x, y_pos);
        
    }
    private void OnEnable()
    {
        transform.position = new Vector2(transform.position.x, y_pos);
        StartCoroutine(WaveMovingManager());
    }
    private void OnDisable()
    {
        if (HavetoEnd == true)
        {
            HavetoEnd = false;
        }
        //transform.position = new Vector2(transform.position.x, y_pos);
    }
    // Update is called once per frame
    void Update()
    {
        if (HavetoEnd == false)
        {
            if (MovingUp == true)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime);
            }
        }
        if (HavetoEnd == true)
        {
            if (WaveManager.GetComponent<WaveManager>().IsLeftWave == true)
            {
                //부모 파도가 왼쪽에서 시작하는가?->YES->내가 맨 오른쪽(마지막)파도인가?
                if (this.gameObject == WaveManager.GetComponent<WaveManager>().WaveChildren[WaveManager.GetComponent<WaveManager>().WaveChildren.Count - 1])
                {
                    //만약 내가 웨이브 매니저의 마지막 자식 오브젝트라면(왼쪽이면 맨 마지막!!오른쪽이면 첫번째것!!)
                    if (WaveManager.GetComponent<WaveManager>().TurnOff == false)
                    {
                        WaveManager.GetComponent<WaveManager>().TurnOff = true;
                    }
                    //WaveManager에게 신호주기(스스로 비활성화 하라고)
                }
            }
            else
            {
                //부모 파도가 왼쪽에서 시작하는가?->NO->내가 맨 왼쪽(마지막)파도인가?
                if (this.gameObject == WaveManager.GetComponent<WaveManager>().WaveChildren[0])
                {
                    //만약 내가 웨이브 매니저의 마지막 자식 오브젝트라면(왼쪽이면 맨 마지막!!오른쪽이면 첫번째것!!)
                    if (WaveManager.GetComponent<WaveManager>().TurnOff == false)
                    {
                        WaveManager.GetComponent<WaveManager>().TurnOff = true;
                    }                    //WaveManager에게 신호주기(스스로 비활성화 하라고)
                }
            }
            this.gameObject.SetActive(false);
        }
    }
}
