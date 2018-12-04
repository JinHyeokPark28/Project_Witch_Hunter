using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    private GameObject[] WaveChildren;
    //정렬로 다 위치 맞게 정렬하기
    private bool FindAllWaveChildren = false;   //자식 오브젝트들 다 위치에 맞게 찾았으면 true
    // Use this for initialization
    void Start()
    {
        WaveChildren = new GameObject[GameObject.FindGameObjectsWithTag("Waves").Length];
        //정렬 할당위해 길이와 함께 선언선언선언~~
        int pivotNum = (int)(WaveChildren.Length / 2);
        print("PIVOT:" + pivotNum);
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Waves").Length; i++)
        {
            //정렬 알고리즘 시작~~~~

        }
    }
    IEnumerator WaveActiver()
    {
        //자식오브젝트인 Wave들 하나씩 활성화 시켜주는 함수
        while (true)
        {
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
