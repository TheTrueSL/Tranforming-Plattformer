using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{

	[SerializeField] float power = 2f;	
    
	private void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.gameObject.tag == "Movable"){
			Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();
			Vector3 dir = hit.transform.position - transform.position;
			dir = new Vector3(dir.x, 0, dir.z);
			body.velocity = power*dir;
		}
	}
}
