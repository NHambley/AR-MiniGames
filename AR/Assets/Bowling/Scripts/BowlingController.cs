using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlingController : MonoBehaviour {

    public GameObject lane;

    public GameObject pin;
    List<GameObject> pins;
    List<Vector3> pinVecs;

    public GameObject bowlingBall;
    Vector3 ballPos;

    public Camera cam;

    bool gameStart;

    bool startRoll;
    bool roll;
    float xVel;
    float zVel;
    Vector3 posStart;
    Vector3 posEnd;

    int currentFrame;
    int attempt;

    bool reset;

    int[][] score;

    bool gameOver;

    //Text component to display on gameover
    GameObject goText;

    GameObject frameText;

    // Use this for initialization
    void Start () {
        gameStart = true;

        currentFrame = 0;
        attempt = 0;

        reset = false;

        pinVecs = new List<Vector3>();

        score = new int[10][];
        for (int i = 0; i < 10; i++)
        {
            score[i] = new int[3];
        }

        gameOver = false;


        //Setting up Text
        goText = GameObject.Find("Text");
        goText.SetActive(false);

        frameText = GameObject.Find("FrameText");
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            if (gameStart)
            {
                bowlingBall = Instantiate(bowlingBall, new Vector3(0.0f, 0.5f, 1.0f), Quaternion.identity);
                roll = false;
                xVel = 0;
                zVel = 0;

                SetPinVecs();
                SpawnPins();

                gameStart = false;
                currentFrame = 1;
                attempt = 1;
                startRoll = true;
            }

            if (currentFrame != 0 && currentFrame <= 10 && attempt != 0)
            {
                if (Input.GetMouseButton(0) && startRoll)
                {
                    bowlingBall.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    ballPos = Input.mousePosition;
                    ballPos.z = 1.0f;
                    //Debug.Log(cam.ScreenToWorldPoint(ballPos));
                    bowlingBall.transform.position = cam.ScreenToWorldPoint(ballPos);
                    posEnd = cam.ScreenToWorldPoint(ballPos);
                    if (posStart == Vector3.zero)
                    {
                        posStart = cam.ScreenToWorldPoint(ballPos);
                    }
                    else if (posStart.y < posEnd.y)
                    {
                        //Debug.Log(posStart.y);
                        //Debug.Log(posEnd.y);
                        roll = true;
                    }
                }
                else if (Input.GetMouseButtonUp(0) && roll)
                {
                    zVel = posEnd.y - posStart.y;
                    zVel = zVel * 9.0f;
                    //Debug.Log(zVel);

                    xVel = posEnd.x - posStart.x;
                    xVel = xVel * 5.0f;
                    //Debug.Log(xVel);
                    roll = false;
                    startRoll = false;
                }

                HoldVelocity();

                //Debug.Log("Velocity: " + bowlingBall.GetComponent<Rigidbody>().velocity);
            }
            else
            {
                //Debug.Log("GAME OVER");
                gameOver = true;
                goText.SetActive(true);
            }

            if (bowlingBall.transform.position.y < -2.0f && attempt != -1)
            {
                bowlingBall.transform.position = new Vector3(0.0f, 0.5f, 1.0f);
                xVel = 0;
                zVel = 0;
                startRoll = true;

                int currentScore = 0;
                currentScore = CheckScore();

                if (attempt == 1 && currentScore == 10)
                {
                    score[currentFrame - 1][attempt - 1] = currentScore;
                    reset = true;
                }
                else if (attempt == 1 && currentScore != 10)
                {
                    score[currentFrame - 1][attempt - 1] = currentScore;
                }
                else if (attempt == 2)
                {
                    score[currentFrame - 1][attempt - 1] = currentScore;
                    reset = true;
                }

                attempt++;
            }

            if (reset)
            {
                foreach (GameObject p in pins)
                {
                    Destroy(p);
                }

                SetPinVecs();
                SpawnPins();

                //Debug.Log("Frame: " + currentFrame + " Score: " + score[currentFrame - 1][0] + " " + score[currentFrame - 1][1]);

                string scoreStr = "";
                for(int i  = 0; i < currentFrame; i++)
                {
                    if(i > 0)
                    {
                        scoreStr += "\n";
                    }
                    scoreStr += "Frame " + (i + 1) + ": " + score[i][0] + " " + score[i][1];
                }

                frameText.GetComponent<Text>().text = scoreStr;//"Frame " + currentFrame + "\nScore: " + score[currentFrame - 1][0] + " " + score[currentFrame - 1][1];

                currentFrame++;
                attempt = 1;
                reset = false;
            }

            if (gameOver)
            {
                foreach (GameObject p in pins)
                {
                    Destroy(p);
                }

                Destroy(bowlingBall);
            }
        }
    }

    void SetPinVecs()
    {
        pinVecs = new List<Vector3>();
        //frontmost pin
        pinVecs.Add(new Vector3(0, 1, 8));

        float spacing1 = 0.3f;

        //row 2
        pinVecs.Add(new Vector3(-spacing1, 0, spacing1) + pinVecs[0]);
        pinVecs.Add(new Vector3(spacing1, 0, spacing1) + pinVecs[0]);

        float spacing2 = spacing1 * 2;

        //row 3
        pinVecs.Add(new Vector3(-spacing2, 0, spacing2) + pinVecs[0]);
        pinVecs.Add(new Vector3(0, 0, spacing2) + pinVecs[0]);
        pinVecs.Add(new Vector3(spacing2, 0, spacing2) + pinVecs[0]);

        float spacing3 = spacing1 * 3;

        //row 4
        pinVecs.Add(new Vector3(-spacing3, 0, spacing3) + pinVecs[0]);
        pinVecs.Add(new Vector3(-spacing1, 0, spacing3) + pinVecs[0]);
        pinVecs.Add(new Vector3(spacing1, 0, spacing3) + pinVecs[0]);
        pinVecs.Add(new Vector3(spacing3, 0, spacing3) + pinVecs[0]);
    }

    void SpawnPins()
    {
        pins = new List<GameObject>();

        foreach(Vector3 v in pinVecs)
        {
            pins.Add(Instantiate(pin, v, Quaternion.Euler(-90.0f,0.0f,0.0f)));
        }
    }

    int CheckScore()
    {
        int currentScore = 0;
        foreach(GameObject p in pins)
        {
            if(p.transform.position.y < .3f)
            {
                currentScore++;
            }
        }
        //Debug.Log("Score: " + currentScore);
        return currentScore;
    }


    //Keep the vector velocity for the bowling ball, frictionless
    void HoldVelocity()
    {
        Vector3 vel = bowlingBall.GetComponent<Rigidbody>().velocity;
        vel.x = xVel;
        vel.z = zVel;
        bowlingBall.GetComponent<Rigidbody>().velocity = vel;
    }
}
