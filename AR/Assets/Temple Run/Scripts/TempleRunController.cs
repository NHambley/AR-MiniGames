using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a tutorial on audio visualization in Unity. Look into using this immediately!!!!!
/// https://www.youtube.com/playlist?list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo
/// </summary>
public class TempleRunController : MonoBehaviour
{
    // keep track of the platforms the player "runs" on
    [SerializeField]
    GameObject platform;
    [SerializeField]
    GameObject obstacle;// an array of different types of obstacles that will come at the player
    List<GameObject> spawnedObs;

    GameObject[] platforms;

    float platformSpeed = 5.0f;


    bool gameplayStart = false;

    float obsSpawnTimer = 2.0f;
    float spawnTimerTrack = 2.0f;
	// Use this for initialization
	void Start ()
    {
        // wait for the user to tap the screen then spawn all three platforms and start spawning obstacles
        platforms = new GameObject[3];
        spawnedObs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if the player touches the screen for the first time set gameplaystart to true
		if(Input.touchCount > 0 && gameplayStart == false)
        {
            gameplayStart = true;

            // spawn the platforms
            platforms[0] = Instantiate(platform, new Vector3(0, -1, 10.5f), Quaternion.identity);
            platforms[1] = Instantiate(platform, new Vector3(0, -1, 31.5f), Quaternion.identity);
            platforms[2] = Instantiate(platform, new Vector3(0, -1, 52.5f), Quaternion.identity);
        }

        // check if the gameplay loop can start
        if(gameplayStart)
        {
            // gameplay loop
            MovePlatforms();
            SpawnObstacles();
            MoveObstacles();
        }
	}

    void MovePlatforms()
    {
        foreach (GameObject plat in platforms)
        {
            Vector3 pos = plat.transform.position;
            pos += -Vector3.forward * Time.deltaTime * platformSpeed;
            if(pos.z + 10.5f <= 0)// reset the platform  to the back of the line
            {
                pos = new Vector3(pos.x, pos.y, 52.5f);
            }
            plat.transform.position = pos;
        }
    }

    void SpawnObstacles()
    {
        // use the transform
        spawnTimerTrack -= Time.deltaTime;
        if (spawnTimerTrack <= 0)
        {
            GameObject temp;
            spawnTimerTrack = obsSpawnTimer;

            temp = Instantiate(obstacle, new Vector3(Random.Range(-2.5f, 2.5f), transform.position.y, transform.position.z), Quaternion.identity);
            // pick a random place on the plane to spawn an obstacle y position and z position at spawn are alwasy the same
            spawnedObs.Add(temp);
        }
    }

    void MoveObstacles()
    {
        foreach (GameObject obs in spawnedObs)
        {
            Vector3 pos = obs.transform.position;
            pos += -Vector3.forward * Time.deltaTime * platformSpeed;
            obs.transform.position = pos;
        }
    }
}
