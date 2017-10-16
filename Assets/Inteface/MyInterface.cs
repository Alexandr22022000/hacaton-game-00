using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInterface : MonoBehaviour {
    private GameObject activeItem;
    private Inventory inventory;
    public int activeItemId = -1;

	void Start ()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        activeItem = GameObject.Find("ActiveItem");
	}

    public void OnClicItem (int id)
    {
        activeItemId = activeItem.GetComponent<MyInterface>().activeItemId;
        if (activeItemId == -1)
        {
            if (inventory.CheeckItem(id))
            {
                activeItem.GetComponent<Image>().enabled = true;
                activeItem.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                activeItem.GetComponent<MyInterface>().activeItemId = id;
            }
        }
        else
        {
            inventory.MigrateItem(activeItemId, id);
            activeItem.GetComponent<MyInterface>().activeItemId = -1;
            activeItem.GetComponent<Image>().enabled = false;
        }
    }
}
