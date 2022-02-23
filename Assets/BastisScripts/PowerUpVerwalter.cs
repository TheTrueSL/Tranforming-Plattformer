using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PowerUpVerwalter : MonoBehaviour
{
    // Start is called before the first frame update

    public FirstPersonController player;

    public void Collect(PowerUpType type)
    {
        switch (type) {
            case PowerUpType.Yellow:
                player.RabbitUnlocked = true;
                player.SetForm(Form.Rabbit);
                break;
            case PowerUpType.Red:
                player.TigerUnlocked = true;
                player.SetForm(Form.Tiger);
                break;
            case PowerUpType.Blue:
                player.CraneUnlocked = true;
                player.SetForm(Form.Crane);
                break;
            case PowerUpType.Green:
                SceneManager.LoadScene("WinScreen");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("WinScreen"));
                break;

            default: break;
        }
           
    }
}
