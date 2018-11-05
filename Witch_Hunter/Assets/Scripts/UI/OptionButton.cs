using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour {
	#region Private Variable
	#endregion

	#region Public Variable
	public bool _CheckClick;

	public GameObject Inven;
	#endregion

	#region Private Method
	private void OnClick(){
		if(transform.Find("InventoryBtn")){
			GameObject.Find("InventoryUI").gameObject.SetActive(true);
		}
	}
	
	#endregion

	#region Public Method
	#endregion

}
