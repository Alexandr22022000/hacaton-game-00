using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMapObject : MonoBehaviour {
    private float hp = 100f;

    public void SetDamag(float damag)
    {
        hp -= damag;

        if (hp <= 0f)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject spawnedObject = Instantiate(Resources.Load<GameObject>("ItemsOnPlace\\palka"), new Vector3(gameObject.transform.position.x, transform.position.y + 5, transform.position.z), transform.rotation);
                spawnedObject.GetComponent<ItemOnPlace>().SetDate(new Item("palka", 0, 5, 3));
            }

            Destroy(gameObject);
        }
    }
}
