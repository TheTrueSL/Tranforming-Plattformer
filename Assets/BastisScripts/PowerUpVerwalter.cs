using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PowerUpVerwalter : MonoBehaviour
{
    // Start is called before the first frame update

    public FirstPersonController player;
    
    [SerializeField]
    private GameObject MyCompass;
    [SerializeField]
    private GameObject Needle;

    public void Start()
    {
        Needle.SetActive(false);
    }
    public void Collect(PowerUpType type)
    {
        player.currentMax = (player.currentMax + 1) % 4; 
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
                MyCompass.SetActive(false);
                Needle.SetActive(true);
                break;
            case PowerUpType.Green:
                SceneManager.LoadScene("Game");
                break;

            default: break;
        }
           
    }
}
