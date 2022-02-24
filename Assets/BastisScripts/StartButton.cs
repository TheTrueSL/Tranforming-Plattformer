using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("hallo");
        SceneManager.LoadScene("Game");
    }
}
