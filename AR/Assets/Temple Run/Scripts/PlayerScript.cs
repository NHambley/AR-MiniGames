using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    int health = 3;
    
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        health -= 1;

        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
}
