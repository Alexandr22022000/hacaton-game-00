using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {
    private float damagForWolf, damagForTrees;

    public void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10000);
    }

    public void SetDamag (float damagForWolf, float damagForTrees)
    {
        this.damagForWolf = damagForWolf;
        this.damagForTrees = damagForTrees;
    }

    void OnCollisionEnter(Collision c)
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().drag = 10000;

        if (c.gameObject.GetComponent<Wolf>() != null)
        {
            c.gameObject.GetComponent<Wolf>().SetDamag(damagForWolf);
        }

        if (c.gameObject.GetComponent<MyMapObject>() != null)
        {
            c.gameObject.GetComponent<MyMapObject>().SetDamag(damagForTrees);
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }
}
