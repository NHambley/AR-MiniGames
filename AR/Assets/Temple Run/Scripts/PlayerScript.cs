using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    int health = 3;
    
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="obstacle")
        {
            Debug.Log("Hit");
            health -= 1;

            if (health == 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
