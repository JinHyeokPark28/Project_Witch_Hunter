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
        boundBox = GameObject.FindGameObjectWithTag("Bounds").GetComponent<BoxCollider2D>();
        print("BoundMAx:" + boundBox.bounds.max);
        print("BoundMin:" + boundBox.bounds.min);
        print("CAMERAHalfSIze:" + GetComponent<Camera>().orthographicSize);
        print("CAmeraCenter:" + transform.position);
        ScreenWidth = GetComponent<Camera>().orthographicSize / Screen.height * Screen.width;
        print("ScreenWidth:" + ScreenWidth);
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        
        if (transform.position.x+ ScreenWidth > boundBox.bounds.max.x)
        {
            transform.position = new Vector3(boundBox.bounds.max.x -ScreenWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.x - ScreenWidth < boundBox.bounds.min.x)
        {
            transform.position = new Vector3(boundBox.bounds.min.x + ScreenWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.y + GetComponent<Camera>().orthographicSize > boundBox.bounds.max.y)
        {
            transform.position = new Vector3(transform.position.x, boundBox.bounds.max.y-GetComponent<Camera>().orthographicSize, transform.position.z);
        }
        if (transform.position.y - GetComponent<Camera>().orthographicSize < boundBox.bounds.min.y)
        {
            transform.position = new Vector3(transform.position.x, boundBox.bounds.min.y + GetComponent<Camera>().orthographicSize, transform.position.z);
        }
       
    }
    //cameraController에게 여기 Bounds가 있다고 알려줌
    
}
       
  