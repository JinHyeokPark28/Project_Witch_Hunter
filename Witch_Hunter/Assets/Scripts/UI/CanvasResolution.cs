using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Screen.SetResolution(Screen.width, Screen.width * 16/9, true);
	}
	
}
