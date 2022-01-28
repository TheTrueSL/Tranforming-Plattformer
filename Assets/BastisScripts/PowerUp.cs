using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType{
   Yellow, Red, Blue
}

public class PowerUp : MonoBehaviour
{
	public PowerUpType type;
    [SerializeField]
    GameObject MyPointer;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            PowerUpVerwalter walter = FindObjectOfType<PowerUpVerwalter>();
			if(walter != null)
				walter.Collect(type);
            Destroy(MyPointer);
			Destroy(gameObject);
        }
        
    }
}
