using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라는 이 Bounds안에서만 움직일 수 있다
public class Bounds : MonoBehaviour {
    private BoxCollider2D bounds;
    private PlayerFollower theCamera;
	// Use this for initialization
	void Start () {
        bounds=GetComponent<BoxCollider2D>();
        //bounds = FindObjectOfType<BoxCollider2D>();
        //boundBox = GameObject.FindGameObjectWithTag("Bounds").GetComponent<BoxCollider2D>();

        //이것때문에 boundBox를 유니티에서 Bounds오브젝트로 설정했는데도 시작하자마자
        //boxcollider2d가 달린 모든 오브젝트들 가운데 자기가 마음대로 아무거나 찾아서 선택

        theCamera = FindObjectOfType<PlayerFollower>();
        //theCamera.SetBounds(bounds);
	}
	
	// Update is called once per frame
	
}
