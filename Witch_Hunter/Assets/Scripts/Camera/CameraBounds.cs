using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Bounds에 달린 스크립트->없어도 잘 되는데?사실상 카메라에 달린 PlayerFollower로 다 하고 있음
//카메라는 이 Bounds안에서만 움직일 수 있다
public class CameraBounds : MonoBehaviour {
    private BoxCollider2D bounds;
    private PlayerFollower theCamera;
	// Use this for initialization
	void Start () {
        bounds=this.gameObject.GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<PlayerFollower>();
	}
	
	// Update is called once per frame
	
}
