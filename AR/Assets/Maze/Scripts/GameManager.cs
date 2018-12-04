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

	// Use this for initialization
	void Start () {
        score = 0;
        health = 0;

        mazeGen.GenerateMaze();
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
