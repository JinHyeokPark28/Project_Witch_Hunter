using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireFlame : MonoBehaviour {
    public GameObject smallFlame;
    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Sword") || (collision.gameObject.tag == "Bullet"))
        {
            if (Player.GetComponent<PlayerController>().IsAttacking == true)
            {

            }
        }
    }
}
