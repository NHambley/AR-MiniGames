using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    TempleRunController controller;
    AudioPeer audioMgr;

    public float startScale, scaleMultiplier;
    public bool useBuffer;

    float timer1 = 0;
    float timer2 = 0;
    float timer3 = 0;
    float timer4 = 0;

    float timerTrack = 1f;
    [SerializeField]
    GameObject obstacle;// an array of different types of obstacles that will come at the player
    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<TempleRunController>();
        audioMgr = GameObject.Find("Audio-Obstacle Listener").GetComponent<AudioPeer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        timer3 -= Time.deltaTime;
        timer4 -= Time.deltaTime;
    }

    public void GenerateObstacles()
    {
        // for each pairing of bands spawn an obstacle if they hit a certain value
        if(audioMgr.freqBand[0] + audioMgr.freqBand[1] >= 0.5f && timer1 <= 0)// bottom right
        {
            GameObject temp = obstacle;
            timer1 = timerTrack;

            if (useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.bandBuffer[0] * scaleMultiplier) + startScale,
                                                        (audioMgr.bandBuffer[1] * scaleMultiplier) + startScale,
                                                        ((audioMgr.bandBuffer[0] + audioMgr.bandBuffer[1]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(temp, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.identity));
            }
            if (!useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.freqBand[0] * scaleMultiplier) + startScale,
                                                        (audioMgr.freqBand[1] * scaleMultiplier) + startScale,
                                                        ((audioMgr.freqBand[0] + audioMgr.freqBand[1]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(temp, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.identity));
            }
        }
        if (audioMgr.freqBand[2] + audioMgr.freqBand[3] >= 0.5f && timer2 <= 0)// bottom left
        {
            GameObject temp = obstacle;
            timer2 = timerTrack;

            if (useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.bandBuffer[2] * scaleMultiplier) + startScale,
                                                        (audioMgr.bandBuffer[3] * scaleMultiplier) + startScale,
                                                        ((audioMgr.bandBuffer[1] + audioMgr.bandBuffer[3]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity));
            }
            if (!useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.freqBand[2] * scaleMultiplier) + startScale,
                                                        (audioMgr.freqBand[3] * scaleMultiplier) + startScale,
                                                        ((audioMgr.freqBand[2] + audioMgr.freqBand[3]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity));
            }
        }
        if (audioMgr.freqBand[4] + audioMgr.freqBand[5] >= 0.6f && timer3 <= 0)// top right
        {
            GameObject temp = obstacle;
            timer3 = timerTrack;

            if (useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.bandBuffer[4] * scaleMultiplier) + startScale,
                                                        (audioMgr.bandBuffer[5] * scaleMultiplier) + startScale,
                                                        ((audioMgr.bandBuffer[4] + audioMgr.bandBuffer[5]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x + 1f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity));
            }
            if (!useBuffer)
            {
                temp.transform.localScale = new Vector3((audioMgr.freqBand[4] * scaleMultiplier) + startScale,
                                                        (audioMgr.freqBand[5] * scaleMultiplier) + startScale,
                                                        ((audioMgr.freqBand[4] + audioMgr.freqBand[5]) * scaleMultiplier) + startScale);
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x + 1f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity));
            }
        }
        if (audioMgr.freqBand[6] + audioMgr.freqBand[7] >= 0.5f && timer4 <= 0)// top left
        {
            GameObject temp = obstacle;
            timer4 = timerTrack;

            if (useBuffer)
            {
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x - 1f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity));
            }
            if (!useBuffer)
            {
                controller.spawnedObs.Add(Instantiate(obstacle, new Vector3(transform.position.x - 1f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity));
            }
        }
    }
}
