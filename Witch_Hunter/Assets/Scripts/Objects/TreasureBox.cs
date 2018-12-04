using UnityEngine;
using System.Collections;

public class TreasureBox : MonoBehaviour
{
    public bool isOpen = false;
    private bool Active = false;    //박스 열리면 true가 되는 변수
    public int coin;
    //보물상자에 붙어있는 스크립트
    private Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        coin = Random.Range(10, 50);
        print("COIN:" + coin);
    }
    IEnumerator DestroyBox()
    {
        Active = true;
        anim.SetBool("IsOpen", true);
        print("OPEN");
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isOpen == true)
        {
            if (Active == false)
            {
                StartCoroutine(DestroyBox());
            }
        }
    }
   
}
