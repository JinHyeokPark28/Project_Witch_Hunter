using UnityEngine;
using System.Collections;

//보물 상자에 달린 스크립트
public class TreasureBox : MonoBehaviour
{
	public static TreasureBox instance;
    public bool isOpen = false; //플레이어와 상호작용 하면 플레이어 스크립트 쪽에서 isOpen=true로 변환
    private bool Active = false;    //박스 열리면 true가 되는 변수
	public GameObject Item;
    public int coin;
    //보물상자에 붙어있는 스크립트
    private Animator anim;
    // Use this for initialization
    void Start()
    {
		instance = this;
        anim = this.gameObject.GetComponent<Animator>();
        coin = Random.Range(10, 50);
        print("COIN:" + coin);
		Item.SetActive(false);
    }
    IEnumerator DestroyBox()
    {
        Active = true;
        anim.SetBool("IsOpen", true);
        print("OPEN");
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
		Item.SetActive(true);
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
