using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathplane : MonoBehaviour
{
    private Vector3 startpos = new Vector3(1.11f, 8.13f, -1.44f);

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Deathplane")
        {
            Debug.Log("weg daa");
            player.transform.position = startpos;
        }
    }
}
