using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType{
   Yellow
}

public class PowerUp : MonoBehaviour
{
	PowerUpType type;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            PowerUpVerwalter walter = FindObjectOfType<PowerUpVerwalter>();
			if(walter != null)
				walter.Collect(type);
			Destroy(gameObject.transform.parent.gameObject);
        }
        
    }
}
