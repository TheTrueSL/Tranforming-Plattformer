using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{

    [SerializeField] float power = 2f;    
	private FirstPersonController fpc;

	private void Start(){
		fpc = gameObject.GetComponent<FirstPersonController>();
	}
    
    private void OnControllerColliderHit(ControllerColliderHit hit){
        if(fpc.currentForm == Form.Ox && hit.gameObject.tag == "Movable"){
            Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();
            Vector3 dir = hit.transform.position - transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            body.velocity = power*dir;
        }
    }
}
