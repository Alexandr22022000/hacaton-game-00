using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public bool isDead = false;

    private Item[] inventory = new Item[12];
    private int activeItem = 0;
    private GameObject camera, spawnPoint, hand, itemInHand, activeImage, animationController, inventoryMenu;
    private Animator handAnimator;
    private bool inventoryIsOpen = false;
    private ParticleSystem particle;
    private float timer = -1;

    void Start ()
    {
        Cursor.visible = false;
        camera = GameObject.Find("Camera");
        spawnPoint = GameObject.Find("SpawnItem");
        hand = GameObject.Find("Hand");
        handAnimator = hand.GetComponent<Animator>();
        activeImage = GameObject.Find("ActiveItem");
        inventoryMenu = GameObject.Find("InventoryMenu");

        ItemUpdate();
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.LoadLevel(0);
        if (isDead) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = new Vector3(mousePosition.x + 55, mousePosition.y - 55, mousePosition.z);
        activeImage.transform.position = mousePosition;

        if (Input.GetKeyDown(KeyCode.E)) GetObject();
        if (Input.GetKeyDown(KeyCode.Mouse0)) OnHitStart();
        if (Input.GetKeyUp(KeyCode.Mouse0)) OnHitEnd();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryIsOpen)
            {
                Cursor.visible = false;
                Vector3 position = inventoryMenu.transform.position;
                inventoryMenu.transform.position = new Vector3(position.x, -65, 0);
            }
            else
            {
                Cursor.visible = true;
                Vector3 position = inventoryMenu.transform.position;
                inventoryMenu.transform.position = new Vector3(position.x, 200, 0);
            }
            inventoryIsOpen = !inventoryIsOpen;
        }


        if (timer != -1)
        {
            timer += Time.deltaTime;
            if (timer >= 1.5f) timer = -1;
        }
    }

    public void MigrateItem (int ferstId, int secondId)
    {
        if (inventory[secondId] != null && inventory[ferstId].maxStack != 1 && inventory[secondId].maxStack != 1 && inventory[ferstId].id == inventory[secondId].id)
        {
            int freeCount = inventory[secondId].maxStack - inventory[secondId].count;
            if (freeCount >= inventory[ferstId].count)
            {
                inventory[secondId].count += inventory[ferstId].count;
                inventory[ferstId] = null;
            }
            else
            {
                inventory[secondId].count = inventory[secondId].maxStack;
                inventory[ferstId].count -= freeCount;
            }
        }
        else
        {
            Item bufer = inventory[secondId];
            inventory[secondId] = inventory[ferstId];
            inventory[ferstId] = bufer;
        }

        ItemUpdate();
    }

    public bool CheeckItem (int id)
    {
        return inventory[id] != null;
    }



    private void OnHitStart ()
    {
        if (timer != -1) return;
        timer = 0f;

        handAnimator.SetBool("isHit", true);

        if (inventory[activeItem] != null)
        {
            if (inventory[activeItem].id == 1)
            {
                Transform spawn = GameObject.Find("SpawnAmmo").transform;
                GameObject ammo = Instantiate(Resources.Load<GameObject>("Any\\Ammo"), spawn.position, spawn.rotation);
                ammo.GetComponent<Ammo>().SetDamag(100f, 10f);

                ParticleSystem particle = GameObject.Find("Smoke").GetComponent<ParticleSystem>();
                particle.Play();
                particle.Emit(5);
            }
            else
            {
                itemInHand.GetComponent<Animator>().SetBool("isHit", true);

                if (inventory[activeItem].id == 0)
                {
                    Hit(30f, 30f);
                }
                else
                {
                    Hit(50f, 100f);
                }
            }
        }
        else
        {
            Hit(20f, 20f);
        }
    }

    private void Hit (float damagForWolws, float DamagForTrees)
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward * 0.000009999999f, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Wolf>() != null)
            {
                hit.collider.gameObject.GetComponent<Wolf>().SetDamag(damagForWolws);
            }

            if (hit.collider.gameObject.GetComponent<MyMapObject>() != null)
            {
                hit.collider.gameObject.GetComponent<MyMapObject>().SetDamag(DamagForTrees);
            }
        }
    }

    private void OnHitEnd ()
    {
        handAnimator.SetBool("isHit", false);

        if (inventory[activeItem] != null && inventory[activeItem].id != 1)
        {
            itemInHand.GetComponent<Animator>().SetBool("isHit", false);
        }
    }

    private void GetObject ()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward * 0.000009999999f, out hit))
        {
            if (hit.collider.gameObject.GetComponent<ItemOnPlace>() != null)
            {
                hit.collider.gameObject.GetComponent<ItemOnPlace>().GiveObject(this);
            }
        }
    }

    public void GiveObject (GameObject itemOnPlace)
    {
        ItemOnPlace item = itemOnPlace.GetComponent<ItemOnPlace>();

        if (inventory[activeItem] != null && inventory[activeItem].maxStack != 1 && item.maxStack != 1 && inventory[activeItem].id == item.id)
        {
            int freeCount = inventory[activeItem].maxStack - inventory[activeItem].count;
            if (freeCount >= item.count)
            {
                inventory[activeItem].count += item.count;
                Destroy(itemOnPlace);
            }
            else
            {
                inventory[activeItem].count = inventory[activeItem].maxStack;
                item.count -= freeCount; 
            }
        }
        else
        {
            if (inventory[activeItem] != null)
            {
                SpawnItem(inventory[activeItem]);
                inventory[activeItem] = null;
            }

            inventory[activeItem] = new Item(item);
            Destroy(itemOnPlace);
        }

        ItemUpdate();
    }

    private void ItemUpdate ()
    {
        if (itemInHand != null)
        {
            Destroy(itemInHand);
        }

        if (inventory[activeItem] != null)
        {
            itemInHand = Instantiate(Resources.Load<GameObject>("ItemsInHand\\" + inventory[activeItem].name), hand.transform.position, hand.transform.rotation);
            itemInHand.transform.parent = camera.transform;
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                GameObject.Find("Item-" + i).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemsImages\\" + inventory[i].name);
                GameObject.Find("Count-" + i).GetComponent<Text>().text = (inventory[i].maxStack == 1) ? "" : inventory[i].count + "";
            }
            else
            {
                GameObject.Find("Item-" + i).GetComponent<Image>().sprite = null;
                GameObject.Find("Count-" + i).GetComponent<Text>().text = "";
            }   
        }

        if (inventory[activeItem] == null)
        {
            handAnimator.SetBool("isShotgun", false);
            handAnimator.SetBool("isPalka", false);
        }
        else
        {
            if (inventory[activeItem].id == 1)
            {
                handAnimator.SetBool("isShotgun", true);
                handAnimator.SetBool("isPalka", false);
            }
            else
            {
                handAnimator.SetBool("isPalka", true);
                handAnimator.SetBool("isShotgun", false);
            }
        }
    }

    private void SpawnItem (Item item)
    {
        GameObject spawnedItem = Instantiate(Resources.Load<GameObject>("ItemsOnPlace\\" + inventory[activeItem].name), spawnPoint.transform.parent.position, spawnPoint.transform.parent.rotation);
        spawnedItem.GetComponent<ItemOnPlace>().SetDate(item);
    }
}
