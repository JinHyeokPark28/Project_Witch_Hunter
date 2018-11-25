using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    #region 함정 특성
    //함정은 HP없음
    public int Attack;  //함정 공격력
    public bool isActive;   //true면 작동된것!->일정 시간 지나면 false로 바뀜
    public float ResetTime=0;  //이 시간이 특정 값에 도달하면 isActive가 false로 바뀝니다.
    public GameObject MyAttackChild;    //평소엔 비활성화 되어있다가 trap이 true일 시 활성화 되어 날아가는 오브젝트
    public float ChildLife; //일정 시간 뒤 자식 오브젝트 제거
    public float ActiveTime;    //resetTime>ActvieTime이면 종료
    #endregion
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            //함정 관련함수
            ResetTime += Time.deltaTime;
            if (MyAttackChild != null)
            {
                //public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                //instantiate로 주어진 프리팹 생성
                Instantiate(MyAttackChild, transform.position, transform.rotation, this.gameObject.transform);
                Destroy(MyAttackChild,ChildLife);
                //GameObject clone = (GameObject)Instantiate (original, transform. ...
                //Destroy(clone, 1.0f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<PlayerStateUI>().NowHP -= Attack;
        }
    }
}
