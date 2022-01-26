using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpVerwalter : MonoBehaviour
{
    // Start is called before the first frame update

    public FirstPersonController player;

    public void Collect(PowerUpType type)
    {
        if (type == PowerUpType.Yellow)
            player.SetForm("Cat");
		else if(type == PowerUpType.Red)
			player.SetForm("SuperCat");
		else if(type == PowerUpType.Blue)
			player.SetForm("Bird");
    }
}
