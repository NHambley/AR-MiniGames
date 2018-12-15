using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    private bool tracking;

    public Text text;

    GameObject mazeObj;

	// Use this for initialization
	void Start () {
        tracking = false;
        mazeObj = GameObject.Find("Maze");
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.touchCount > 0 && !tracking)
        {
            tracking = true;
            text.text = "FREEZE";
            mazeObj.transform.parent = this.gameObject.transform;
        }
        else if (Input.touchCount <= 0)
        {
            text.text = "";
            tracking = false;
            mazeObj.transform.parent = null;
        }

        if (tracking)
        {
            Vector3 rotation = mazeObj.transform.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            mazeObj.transform.rotation = Quaternion.Euler(rotation);

            Vector3 pos = mazeObj.transform.position;
            pos.y = 0;
            mazeObj.transform.position = pos;
        }
	}
}
