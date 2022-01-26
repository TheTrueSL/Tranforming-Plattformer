using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationManager : MonoBehaviour {
    [SerializeField]
    private GameObject Ox;
    [SerializeField]
    private GameObject Rabbit;
    [SerializeField]
    private GameObject Tiger;
    [SerializeField]
    private GameObject Crane;
    [SerializeField]
    private GameObject current;

    private void Start()
    {
        //Ox.SetActive(false);
        //Rabbit.SetActive(false);
        //Tiger.SetActive(false);
        //Crane.SetActive(false);
    }

    public void TransFormInto(Form form)
    {
        current.SetActive(false);
        switch (form)
        {
            case Form.Ox:
                Ox.SetActive(true);
                current = Ox;
                break;
            case Form.Rabbit:
                Rabbit.SetActive(true);
                current = Rabbit;
                break;
            case Form.Tiger:
                Tiger.SetActive(true);
                current = Tiger;
                break;
            case Form.Crane:
                Crane.SetActive(true);
                current = Crane;
                break;
        }

    }       
}
