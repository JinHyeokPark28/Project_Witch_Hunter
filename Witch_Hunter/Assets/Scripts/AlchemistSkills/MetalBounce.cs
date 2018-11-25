using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBounce : MonoBehaviour {
    public GameObject MyWitch;  //이 스킬 쓰는 마녀찾기(연금술마녀)
    public GameObject metalSlime;
	// Use this for initialization
    IEnumerator RespawningSlim()
    {
        while (true)
        {
            Instantiate(metalSlime, transform.position, Quaternion.Euler(0, 0, 0));
            yield return new WaitForSeconds(5);
        }
    }
	void Start () {
        MyWitch = GameObject.FindGameObjectWithTag("Witch");
        StartCoroutine(RespawningSlim());
        if (metalSlime == null)
        {
            // Resources.Load:Loads an asset stored at path in a folder called Resources.
            //Resources폴더에 있는 것만 지원함..ㅋ
            //metalSlime = Resources.Load(Application.dataPath + "/Prefabs/GahyunPrefabs/WitchSkillPrefabs/MetalSlime.prefab") as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (MyWitch.GetComponent<WitchClass>().Phase == 3)
        {
            Destroy(this.gameObject);
            //단계(페이즈) 3일 경우 없어지도록
        }
	}
   
}
