using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// 에셋스토어에서 임포팅한 Json.
using Boomlagoon.JSON;
using System;

[SerializeField]
public class GetItem
{
	public int ItemID;
	public string Name;
	public string Description;
	public double Stat;
	public int HP_Max;
	public string Type;
	public int Count;
}
public class ItemManager : MonoBehaviour {

	public static ItemManager instance = null;
	// json 파일의 경로 설정.
	// 경로를 가져올때 \ 대신에 역슬래쉬 / 를 사용한다.
	private const string path = "C://Users/vnddn/Documents/Project_Witch_Hunter/Assets/Resource/ItemList.Json";

	// GetItem을 리스트로 선언한 뒤 동적할당
	public List<GetItem> getItem = new List<GetItem>();

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		ReadJson();
	}
	public void ReadJson()
	{
		// Json경로 찾는 변수
		StreamReader itemReader = new StreamReader(path);
		// Json 파일을 가져오기
		string Reader = itemReader.ReadToEnd();

		// 아이템을 여러가지로 되어 있기 때문에 배열처리로 파일을 불러온다.
		JSONArray ItemArray = JSONArray.Parse(Reader);

		for (int i = 0; i < 20; i++)
		{
			GetItem item = new GetItem();

			item.ItemID = Convert.ToInt32(ItemArray[i].Obj["ItemID"].Number);
			item.Name = ItemArray[i].Obj["Name"].ToString();
			item.Description = ItemArray[i].Obj["Description"].ToString();
			item.Stat = Convert.ToDouble(ItemArray[i].Obj["Stat"].Number);
			item.HP_Max = Convert.ToInt32(ItemArray[i].Obj["HP_Max"].Number);
			item.Type = ItemArray[i].Obj["Type"].ToString();
			item.Count = Convert.ToInt32(ItemArray[i].Obj["Count"].Number);

			getItem.Add(item);
		}
	}

	// 아이템의 아이콘을 넣어줄 함수도 필요함.

}
