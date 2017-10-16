using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public string name;
    public int id;
    public int maxStack;
    public int count;

    public Item(ItemOnPlace item)
    {
        name = item.name;
        id = item.id;
        maxStack = item.maxStack;
        count = item.count;
    }

    public Item(string name, int id, int maxStack, int count)
    {
        this.name = name;
        this.id = id;
        this.maxStack = maxStack;
        this.count = count;
    }
}
