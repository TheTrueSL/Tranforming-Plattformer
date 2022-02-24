using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    private void OnMouseEnter()
    {
        Application.Quit();
    }
}
