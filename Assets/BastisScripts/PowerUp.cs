using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType{
   Yellow, Red, Blue, Green
}

public class PowerUp : MonoBehaviour
{
	public PowerUpType type;
    [SerializeField]
    private GameObject MyPointer;

    private void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }

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
