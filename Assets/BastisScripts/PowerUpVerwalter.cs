using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpVerwalter : MonoBehaviour
{
    // Start is called before the first frame update
    private int yellowCount = 0;
    [SerializeField] private int yellowMin = 5;

    public FirstPersonController player;

    public void Update()
    {
        if (yellowCount >= yellowMin)
            YellowPowerUpActivated();
    }

    public void Collect(PowerUpType type)
    {
        if (type == PowerUpType.Yellow)
            yellowCount++;
    }

    private void YellowPowerUpActivated()
    {
        if (player != null)
            player.SetForm("Cat");
    }
}
