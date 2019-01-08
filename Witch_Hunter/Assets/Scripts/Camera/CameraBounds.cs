using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라는 이 Bounds안에서만 움직일 수 있다
public class CameraBounds : MonoBehaviour {
    private BoxCollider2D bounds;
    private PlayerFollower theCamera;
	// Use this for initialization
	void Start () {
        bounds=GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<PlayerFollower>();
	}
	
	// Update is called once per frame
	
}
