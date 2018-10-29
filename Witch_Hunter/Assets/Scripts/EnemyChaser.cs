using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour {
    private Animator anim;
    public bool Chasing;
    public float Speed;
    public int Direct = 1;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Direction());
    }
    IEnumerator Direction()
    {
        while (true)
        {
            Direct *= -1;
            yield return new WaitForSeconds(2);
            Direct *= -1;
            yield return new WaitForSeconds(2);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Direct == -1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Direct == 1)
        {
            GetComponent<SpriteRenderer>().flipX = false;

        }
        if (Chasing == true)
        {
            anim.SetBool("Chase", true);
        }
        else
        {
            transform.Translate(Vector3.right * Direct * Speed * Time.deltaTime);
            anim.SetBool("Chase", false);
        }
    }
}
