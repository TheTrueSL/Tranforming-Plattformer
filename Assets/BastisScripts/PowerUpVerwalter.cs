using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpVerwalter : MonoBehaviour
{
	public GameObject tiger;
	public GameObject elephant;
	public GameObject cow;
	public GameObject bird;

	private FirstPersonController active;

	public CompassBar compassBar;

	private void Start()
	{
		//active = Instantiate(elephant, new Vector3(22.1700001f,9.82999992f,-1.67119896f), new Quaternion(0,-0.747593343f,0,0.664156795f));
		//compassBar.ChangePlayer(active.transform);
	}

    public void Collect(PowerUpType type)
    {
	    if (type == PowerUpType.Yellow)
	    {
			//Vector3 pos = active.transform.position;
			//Destroy(active);
			//active = Instantiate(cow,pos, Quaternion.identity);
			//compassBar.ChangePlayer(active.transform);
			active.SetForm("Cat");
	    }
		else if(type == PowerUpType.Red)
	    {
		    Vector3 pos = active.transform.position;
		    Destroy(active);
		    //active = Instantiate(tiger,pos, Quaternion.identity);
		    compassBar.ChangePlayer(active.transform);
	    }
		else if(type == PowerUpType.Blue)
	    {
		    Vector3 pos = active.transform.position;
		    Destroy(active);
		    //active = Instantiate(bird,pos, Quaternion.identity);
		    compassBar.ChangePlayer(active.transform);
	    }
    }
}
