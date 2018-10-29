using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPress : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Monsters;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Monsters = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
