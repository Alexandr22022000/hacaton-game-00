using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnPlace : MonoBehaviour
{
    public string name;
    public int id;
    public int maxStack;
    public int count;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetDate (Item item)
    {
        name = item.name;
        id = item.id;
        maxStack = item.maxStack;
        count = item.count;
    }

    public void GiveObject(Inventory inventory)
    {
        inventory.GiveObject(gameObject);
    }
}
