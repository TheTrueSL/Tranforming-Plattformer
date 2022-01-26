using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject modelElephant;
    [SerializeField]
    private GameObject modelRabbit;
    [SerializeField]
    private GameObject modelCat;
    [SerializeField]
    private GameObject modelBird;

    private GameObject currentModel;
    [SerializeField]
    private FirstPersonController player;
    // Start is called before the first frame update

    private void Start()
    {
        currentModel = modelElephant;
    }

    public void ChangeModelToRabbit() {
        Destroy(currentModel);
        currentModel = Instantiate(modelRabbit, transform.position, transform.rotation);
    }
}
