using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {
    public bool torch, shot;
    public float hp;
    private float timer;

    public void SetDamag (float damag)
    {
        GameObject.Find("Dead").GetComponent<Image>().color = new Color(100,0,0,.6f);
        timer = 0f;

        hp -= damag;

        if (hp <= 0f)
        {
            timer = -1f;
            gameObject.GetComponent<PlayerMove>().isDead = true;
            gameObject.GetComponent<Inventory>().isDead = true;
        }
        else
        {
            if (hp <= 30f)
            {

            }
            else
            {
                if (hp <= 60f)
                {
                    if (hp <= 80)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }
    }

    public void Update()
    {
        if (timer != -1)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                GameObject.Find("Dead").GetComponent<Image>().color = Color.clear;
            }
        }
    }
}
