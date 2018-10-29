using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {
    //카메라가 플레이어를 따라가게 하게끔 만드는 스크립트
    public GameObject Player;
    public Vector2 offset;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(Player.transform.position.x + offset.x,
            Player.transform.position.y + offset.y,transform.position.z);
        //z안 넣어주니까 강제적으로 0으로 맞춰짐
	}
}
