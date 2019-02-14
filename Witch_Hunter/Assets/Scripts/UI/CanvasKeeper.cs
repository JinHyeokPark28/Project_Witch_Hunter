using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasKeeper : MonoBehaviour {
    private static bool CanvasExists;
	// Use this for initialization
	void Start () {
        if (CanvasExists == false)
        {
            CanvasExists = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
