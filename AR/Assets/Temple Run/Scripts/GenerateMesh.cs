using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    public Material mat;
    public void CreateShape(float x, float y, float z)
    {

        // create a new object based off of sound data
        Vector3[] vertices =
        {
                    new Vector3 (0, 0, 0),
                    new Vector3 (x/2, 0, 0),
                    new Vector3 (x/3, y/5, 0),
                    new Vector3 (0, y/2, 0),
                    new Vector3 (0, y/6, z/3),
                    new Vector3 (x/3, y/7, z/9),
                    new Vector3 (x/6, 0, z/3),
                    new Vector3 (0, 0, z/5),
        };

        int[] triangles =
        {
            0, 2, 1, //face front
		    0, 3, 2,
            2, 3, 4, //face top
		    2, 4, 5,
            1, 2, 5, //face right
		    1, 5, 6,
            0, 7, 4, //face left
		    0, 4, 3,
            5, 4, 7, //face back
		    5, 7, 6,
            0, 6, 7, //face bottom
		    0, 1, 6
        };

        transform.localScale = new Vector3(x, y, z);
        
        //edit the mesh of the object according to sound info
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshRenderer>().material = mat;
    }
}

