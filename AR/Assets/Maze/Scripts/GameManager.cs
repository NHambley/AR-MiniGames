using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text healthText;
    public Text scoreText;

    private int score;
    private int health;

    public MazeGenerator mazeGen;
    public GameObject mazeObj;

    public Transform lastPosition;

	// Use this for initialization
	void Start () {
        score = 0;
        health = 0;

        mazeGen.GenerateMaze();
	}

    private void Update()
    {
        Vector3 pos = this.gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.W))
        {
            pos += this.gameObject.transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            pos -= this.gameObject.transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            pos -= this.gameObject.transform.right;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            pos += this.gameObject.transform.right;
        }

        Vector3 rot = this.gameObject.transform.rotation.eulerAngles;
        if (Input.GetKeyDown(KeyCode.E))
        {
            rot.y += 90;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            rot.y -= 90;
        }

        this.gameObject.transform.rotation = Quaternion.Euler(rot);
        this.gameObject.transform.position = pos;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "positionTrigger")
        {
            lastPosition = col.gameObject.transform;
            Debug.Log("UPDATED LAST TO: " + lastPosition);
        }
        else if (col.gameObject.tag == "end")
        {
            AddScore();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("hit a wall");
        }
    }

    public void AddScore()
    {
        score ++;
        scoreText.text = "Score: " + score;

        for (int i = 0; i < mazeObj.transform.childCount; i++)
        {
            Destroy(mazeObj.transform.GetChild(i));
        }

        mazeGen.GenerateMaze();
    }

    public void TakeDamage()
    {
        health--;

        healthText.text = "Health: " + health;

        if (health <= 0)
        {
            //die or something
        }
    }
}
