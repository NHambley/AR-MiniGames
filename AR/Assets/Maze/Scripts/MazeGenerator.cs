using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    List<GameObject> mazeObjects;
    int[,] maze;

    const int MAZE_SIZE = 20;
    const int MAZE_STOPS = 5;

    const float MAZE_BLOCK_SIZE = 1f;
    const int MIN_NEW_PATH_LENGTH = 4;
    const int MAX_NEW_PATH_LENGTH = 10;
    const int NUM_NEW_PATHS = 20;

    public GameObject mazeBlockPrefab;
    public Transform mazeParent;
    public GameObject endBlockObject;

    public Transform playerPos;

    private List<Vector2> GeneratePath()
    {
        List<Vector2> path = new List<Vector2>();

        Vector2 startPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;

        bool pickTop = false;

        if (Random.Range(0, 2) == 0)
        {
            startPos = new Vector2(Random.Range(0, MAZE_SIZE - 1), 0);
            endPos = new Vector2(Random.Range(0, MAZE_SIZE - 1), MAZE_SIZE-1);
            pickTop = true;
        }   
        else
        {
            startPos = new Vector2(0, Random.Range(0, MAZE_SIZE - 1));
            endPos = new Vector2(MAZE_SIZE-1, Random.Range(0, MAZE_SIZE - 1));
        }

        int numOfStops = MAZE_STOPS;
        int spacing = Mathf.FloorToInt(MAZE_SIZE / (numOfStops + 1));

        List<Vector2> stops = new List<Vector2>();

        for (int i = 0; i < numOfStops; i++)
        {
            Vector2 stop;
            if (!pickTop)
            {
                stop = new Vector2((i + 1) * spacing, Random.Range(0, MAZE_SIZE - 1));
            }
            else
            {
                stop = new Vector2(Random.Range(0, MAZE_SIZE - 1), (i + 1) * spacing);
            }
            stops.Add(stop);
        }

        Vector2 curPos = startPos;
        Vector2 target = Vector2.zero;
        int targetIndex = 0;

        path.Add(startPos);

        do
        {
            if (target == Vector2.zero)
            {
                if (targetIndex >= stops.Count)
                {
                    target = endPos;
                }
                else
                {
                    target = stops[targetIndex];
                    targetIndex++;
                }
            }

            if (pickTop)
            {
                if (curPos.y != target.y)
                {
                    if (curPos.y < target.y)
                    {
                        curPos.y++;
                    }
                    else if (curPos.y > target.y)
                    {
                        curPos.y--;
                    }
                }
                else if (curPos.x != target.x)
                {
                    if (curPos.x < target.x)
                    {
                        curPos.x++;
                    }
                    else if (curPos.x > target.x)
                    {
                        curPos.x--;
                    }
                }
            }
            else
            {
                if (curPos.x != target.x)
                {
                    if (curPos.x < target.x)
                    {
                        curPos.x++;
                    }
                    else if (curPos.x > target.x)
                    {
                        curPos.x--;
                    }
                }
                else if (curPos.y != target.y)
                {
                    if (curPos.y < target.y)
                    {
                        curPos.y++;
                    }
                    else if (curPos.y > target.y)
                    {
                        curPos.y--;
                    }
                }
            }

            if (curPos == target)
            {
                target = Vector2.zero;
            }

            path.Add(curPos);

        } while (curPos != endPos);

        return path;
    }

    public void GenerateMaze()
    {
        maze = new int[MAZE_SIZE+4, MAZE_SIZE+4];

        List<Vector2> path = GeneratePath();


        for (int i = 0; i < path.Count; i++)
        {
            maze[(int)path[i].x + 2, (int)path[i].y + 2] = 1;
        }

        for (int i = 0; i < NUM_NEW_PATHS; i++)
        {
            int newPathLength = Random.Range(MIN_NEW_PATH_LENGTH, MAX_NEW_PATH_LENGTH);
            int pathPositionStartIndex = Random.Range(0, path.Count-1);
            Vector2 newPathPos = path[pathPositionStartIndex];

            int maxLoops = newPathLength*2;

            for (int j = 0; j < newPathLength; j++)
            {
                bool badValues = false;
                int direction = Random.Range(0, 3);

                Vector2 moveDir = Vector2.zero;

                switch(direction)
                {
                    case 0:
                        moveDir.x = -1;
                        break;
                    case 1:
                        moveDir.x = 1;
                        break;
                    case 2:
                        moveDir.y = -1;
                        break;
                    case 3:
                        moveDir.y = 1;
                        break;
                }

                Vector2 testPos = newPathPos + moveDir;
                if (testPos.x >= MAZE_SIZE || testPos.x < 0 || testPos.y >= MAZE_SIZE || testPos.y < 0) { badValues = true; }
                else if (maze[(int)testPos.x, (int)testPos.y] == 1) { badValues = true; }
                else
                {
                    newPathPos += moveDir;
                    maze[(int)newPathPos.x, (int)newPathPos.y] = 1;
                }


                if (badValues)
                {
                    j--;
                    maxLoops++;
                    if (maxLoops >= 10) { j += newPathLength; }
                }
            }
        }

        //clean-up

        for (int i = 0; i < MAZE_SIZE + 4; i++)
        {
            for (int j = 0; j < MAZE_SIZE + 4; j++)
            {
                if ((i == 0 || j == 0) && (maze[i, j] == 1))
                {
                    maze[i, j] = 0;
                }
            }
        }


        CreateObjects(path[0], path[path.Count-1]);
    }

    private void CreateObjects(Vector2 start, Vector2 end)
    {
        Vector2 startOffset = new Vector2((start.x - playerPos.position.x + 2) * MAZE_BLOCK_SIZE, (start.y - playerPos.position.z + 2) * MAZE_BLOCK_SIZE);
        for (int i = 0; i < MAZE_SIZE+4; i++)
        {
            for (int j = 0; j < MAZE_SIZE+4; j++)
            {
                if (maze[i, j] == 0)
                {
                    Vector3 pos = new Vector3((i * MAZE_BLOCK_SIZE) - startOffset.x, 0, (j * MAZE_BLOCK_SIZE) - startOffset.y);
                    GameObject block = Instantiate(mazeBlockPrefab, mazeParent);
                    block.transform.position = pos;
                }
            }
        }


        Vector3 endPos = new Vector3(((end.x + 2) * MAZE_BLOCK_SIZE) - startOffset.x, 0, ((end.y + 2) * MAZE_BLOCK_SIZE) - startOffset.y);

        GameObject endBlock = Instantiate(endBlockObject, mazeParent);
        endBlock.transform.position = endPos;
    }

    private void PrintMaze()
    {
        for (int i = 0; i < MAZE_SIZE; i++)
        {
            string line = "";
            for (int j = 0; j < MAZE_SIZE; j++)
            {
                line += maze[j, i] + " ";
            }
            Debug.Log(line);
        }
    }
}
