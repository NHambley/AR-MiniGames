using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int currentScore;

    bool reset;

	// Use this for initialization
	void Start () {
        gameStart = true;

        currentFrame = 0;
        attempt = 0;

        reset = false;

        pinVecs = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
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
                zVel = zVel * 5.0f;
                //Debug.Log(zVel);

                xVel = posEnd.x - posStart.x;
                xVel = xVel * 2.0f;
                //Debug.Log(xVel);
                roll = false;
                startRoll = false;
            }

            HoldVelocity();

            //Debug.Log("Velocity: " + bowlingBall.GetComponent<Rigidbody>().velocity);
        }

        if(bowlingBall.transform.position.y < -12.0f)
        {
            attempt++;
            CheckScore();
            bowlingBall.transform.position = new Vector3(0.0f, 0.5f, 1.0f);
            xVel = 0;
            zVel = 0;
            startRoll = true;
            if(attempt > 2 || currentScore == 10)
            {
                reset = true;
                currentScore = 0;
            }
        }

        if (reset)
        {
            foreach(GameObject p in pins)
            {
                Destroy(p);
            }

            SetPinVecs();
            SpawnPins();

            currentFrame++;
            attempt = 1;
            reset = false;
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

    void CheckScore()
    {
        currentScore = 0;
        foreach(GameObject p in pins)
        {
            if(p.transform.position.y < .3f)
            {
                currentScore++;
            }
        }
        Debug.Log("Score: " + currentScore);
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
