using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {
    private string save = "";
    private string[] objects = new string[] { "ItemsOnPlace\\palka", "ItemsOnPlace\\shotgun", "ItemsOnPlace\\tapor", "ObjectsOnMap\\treeA", "ObjectsOnMap\\treeA", "ObjectsOnMap\\bush", "ObjectsOnMap\\cobblestone 1", "NPC\\wolf" };

	void Start ()
    {
        save = VeryMainScript.levelData;
        int[] ids, xPoss, yPoss;
        List<int> bufer = new List<int>();

        int i1 = 0, i2;
		while (i1 != -1)
        {
            i2 = save.IndexOf(";", i1);

            bufer.Add(int.Parse(save.Substring(i1, i2 - i1)));

            i1 = ((i2 + 1) == save.Length) ? -1 : i2 + 1;
        }

        int[] array = bufer.ToArray();
        ids = new int[array.Length / 3];
        xPoss = new int[array.Length / 3];
        yPoss = new int[array.Length / 3];

        for (int key = 0, i = 0; key < array.Length; i++)
        {
            ids[i] = array[key];
            key++;
            xPoss[i] = array[key];
            key++;
            yPoss[i] = array[key];
            key++;
        }

        for (int i = 0; i < ids.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(xPoss[i], 2000, yPoss[i]), Vector3.down * 1000000, out hit))
            {
                Instantiate(Resources.Load<GameObject>(objects[ids[i]]), hit.point, new Quaternion());
            }
        }
	}
}
