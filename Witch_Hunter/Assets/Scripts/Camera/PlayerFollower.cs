using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {
    //카메라가 플레이어를 따라가게 하게끔 만드는 스크립트
    public GameObject Player;
    public Vector2 offset;
    public float moveSpeed;
    public BoxCollider2D boundBox;
    public bool isStop;
    public float ScreenWidth;
    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        //씬 크기를 알려주는 바운드 박스의 콜라이더를 불러옴->씬 크기 알수있음
       // boundBox = GameObject.FindGameObjectWithTag("Bounds").GetComponent<BoxCollider2D>();
        //현재 카메라 화면의 가로 길이 계산
        ScreenWidth = GetComponent<Camera>().orthographicSize / Screen.height * Screen.width;
    }

    // Update is called once per frame
    void LateUpdate() {
        //플레이어가 새로운 바운드에 접촉할 때바다 boundBox를 바꿔줌
        if (boundBox != null)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
            if (transform.position.x + ScreenWidth > boundBox.bounds.max.x)
            {
                transform.position = new Vector3(boundBox.bounds.max.x - ScreenWidth, transform.position.y, transform.position.z);
            }
            if (transform.position.x - ScreenWidth < boundBox.bounds.min.x)
            {
                transform.position = new Vector3(boundBox.bounds.min.x + ScreenWidth, transform.position.y, transform.position.z);
            }
            if (transform.position.y + GetComponent<Camera>().orthographicSize > boundBox.bounds.max.y)
            {
                transform.position = new Vector3(transform.position.x, boundBox.bounds.max.y - GetComponent<Camera>().orthographicSize, transform.position.z);
            }
            if (transform.position.y - GetComponent<Camera>().orthographicSize < boundBox.bounds.min.y)
            {
                transform.position = new Vector3(transform.position.x, boundBox.bounds.min.y + GetComponent<Camera>().orthographicSize, transform.position.z);
            }

        }
       
    }
}
       
  