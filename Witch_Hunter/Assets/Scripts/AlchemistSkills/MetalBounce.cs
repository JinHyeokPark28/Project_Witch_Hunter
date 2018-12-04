using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBounce : MonoBehaviour {
    public GameObject MyWitch;  //이 스킬 쓰는 마녀찾기(연금술마녀)
    public GameObject metalSlime;
    private int TimeNum;    //이걸로 rand돌려서 같이 나타나는 메탈바운스가 instantiate 줄 때 시간차 차이주려고
	// Use this for initialization
    IEnumerator RespawningSlim()
    {
        yield return new WaitForSeconds(TimeNum);
        while (true)
        {
            Instantiate(metalSlime, transform.position, Quaternion.Euler(0, 0, 0));
            yield return new WaitForSeconds(5);
        }
    }
	void Start () {
        TimeNum = Random.Range(0, 6);
        MyWitch = GameObject.FindGameObjectWithTag("Witch");
        StartCoroutine(RespawningSlim());
        if (metalSlime == null)
        {
            // Resources.Load:Loads an asset stored at path in a folder called Resources.
            //Resources폴더에 있는 것만 지원함..ㅋ
            metalSlime = Resources.Load(Application.dataPath + "/Prefabs/GahyunPrefabs/WitchSkillPrefabs/MetalSlime.prefab") as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (MyWitch.GetComponent<WitchClass>().isDead == true)
        {
                Destroy(this.gameObject);
            //마녀 죽으면 없어지도록
        }
	}
   
}
