using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnRegen : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void respawnItems()
    {
        print("respawn items");
        foreach(GameObject item in items)
        {
            item.GetComponent<MeshRenderer>().enabled = true;
            item.GetComponent<MeshCollider>().enabled = true;
        }
    }
}
