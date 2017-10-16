using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCalback : MonoBehaviour {
    private VeryMainScript veryMainScript;

	void Start () {
        veryMainScript = GameObject.Find("MainCamera").GetComponent<VeryMainScript>();
	}

    public void OnClickMainButton ()
    {
        veryMainScript.StartLevel();
    }
	
	public void OnClickChangeScene (bool isForwrd)
    {
        veryMainScript.ChangeScene(isForwrd);
    }
}
